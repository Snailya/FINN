using System;
using System.Linq;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;

[assembly: CommandClass(typeof(FINN.CAD.ProcessNodeCommand))]
namespace FINN.CAD
{
    public class ProcessNodeCommand
    {
        private const double TitleTextHeight = 5;

        public static Models.ProcessNode Data { get; set; }

        private static Entity[] InnerCreate(double xLength, double yLength, string title, string body)
        {
            // create title block
            var titleRect = Util.CreateRectangle(new Point2d(0, 0), new Point2d(xLength, -TitleTextHeight));
            var titleText = Util.CreateText(new Point2d(0, -TitleTextHeight), TitleTextHeight, title);

            // create body block
            var bodyRect = Util.CreateRectangle(new Point2d(0, -TitleTextHeight), new Point2d(xLength, -yLength));
            var bodyText = Util.CreateMText(new Point2d(0, -TitleTextHeight), new Point2d(xLength, -yLength), body);

            return new Entity[] { titleRect, bodyRect, titleText, bodyText };
        }

        /// <summary>
        /// Create a legacy block using specified data. If data is not declared before, a user prompt will prompt to let user give the information.
        /// </summary>
        [CommandMethod("createlegacy")]
        public void CreateLegacy()
        {
            var acDoc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;

            // prompt user input if Sample is null
            if (Data == null)
            {
                var opt1 = new PromptPointOptions("Please specify the first corner...");
                var r1 = acDoc.Editor.GetPoint(opt1);
                if (r1.Status != PromptStatus.OK) return;

                var opt2 = new PromptCornerOptions("Please specify the second corner...", r1.Value);
                var r2 = acDoc.Editor.GetCorner(opt2);
                if (r2.Status != PromptStatus.OK) return;

                var opt3 = new PromptStringOptions("Please input the title...");
                var r3 = acDoc.Editor.GetString(opt3);
                if (r3.Status != PromptStatus.OK) return;

                var opt4 = new PromptStringOptions("Please input the body...");
                var r4 = acDoc.Editor.GetString(opt4);
                if (r4.Status != PromptStatus.OK) return;

                Data = new Models.ProcessNode()
                {
                    Title = r3.StringResult,
                    Body = r4.StringResult,

                    XLength = Math.Abs(r1.Value.X - r2.Value.X),
                    YLength = Math.Abs(r1.Value.Y - r2.Value.Y),

                    Position = new Point2d(r1.Value.X, r1.Value.Y),
                };
            }

            // insert node block
            var acDatabase = acDoc.Database;

            using (var acTrans = acDatabase.TransactionManager.StartTransaction())
            {
                var acBlockTable = acTrans.GetObject(acDatabase.BlockTableId, OpenMode.ForRead) as BlockTable;

                // find or create the block
                ObjectId acBlockTableId = ObjectId.Null;
                if (acBlockTable.Has(Data.Title))
                {
                    acBlockTableId = acBlockTable[Data.Title];
                }
                else
                {
                    acTrans.GetObject(acDatabase.BlockTableId, OpenMode.ForWrite);

                    using (var acBlockTableRecord = new BlockTableRecord())
                    {
                        acBlockTableRecord.Name = Data.Title;
                        acBlockTableId = acBlockTable.Add(acBlockTableRecord);

                        var entities = InnerCreate(Data.XLength, Data.YLength, Data.Title, Data.Body);
                        entities.ToList().ForEach(e =>
                        {
                            acBlockTableRecord.AppendEntity(e);
                            e.Dispose();
                        });
                    }
                }

                // insert the block into the current space
                if (acBlockTableId != ObjectId.Null)
                {
                    using (BlockReference acBlkRef = new BlockReference(new Point3d(Data.Position.X, Data.Position.Y, 0), acBlockTableId))
                    {
                        BlockTableRecord acCurSpaceBlkTblRec;
                        acCurSpaceBlkTblRec = acTrans.GetObject(acDatabase.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                        acCurSpaceBlkTblRec.AppendEntity(acBlkRef);
                        acTrans.AddNewlyCreatedDBObject(acBlkRef, true);
                    }
                }

                acTrans.Commit();
            }

            // clear sample
            Data = null;
        }
    }
}