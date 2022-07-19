using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System.Collections.Generic;

[assembly: CommandClass(typeof(FINN.CAD.Commands.FlowChartCommand))]
namespace FINN.CAD.Commands
{
    public class FlowChartCommand
    {
        private const double TitleTextHeight = 5;

        public static Dictionary<string, List<Models.ProcessNode>> Data { get; set; }

        private static Dictionary<string, List<Models.ProcessNode>> PromptInput()
        {
            var acDoc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            Data = new Dictionary<string, List<Models.ProcessNode>>();

            PromptKeywordOptions pKeyOpts = new PromptKeywordOptions("")
            {
                Message = "\nEnter an option "
            };
            pKeyOpts.Keywords.Add("ADD");
            pKeyOpts.Keywords.Add("FINISH");
            pKeyOpts.Keywords.Add("ESC");
            pKeyOpts.AllowNone = false;

            var r = acDoc.Editor.GetKeywords(pKeyOpts);
            while (r.Status == PromptStatus.OK)
            {
                if (r.StringResult == "ESC") return null;
                if (r.StringResult == "FINISH")
                {
                    break;
                };

                var opt1 = new PromptStringOptions("Please enter group name...");
                var r1 = acDoc.Editor.GetString(opt1);
                var node = ProcessNodeCommand.PromptInput();

                if (Data.TryGetValue(r1.StringResult, out var nodes))
                {
                    nodes.Add(node);
                }
                else
                {
                    Data.Add(r1.StringResult, new List<Models.ProcessNode>() { node });
                }

                r = acDoc.Editor.GetKeywords(pKeyOpts);
            }
            if (r.Status == PromptStatus.Cancel) return null;

            return Data;
        }
        /// <summary>
        /// Create a legacy block using specified data. 
        /// If data is not declared before, a user prompt will prompt to let user give the information.
        /// </summary>
        [CommandMethod("FINN", "gnrt", CommandFlags.Modal)]
        public void Generate()
        {
            var acDoc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;

            // prompt user input if Sample is null
            Data = Data ?? PromptInput();
            if (Data == null) return;

            // insert node block
            var acDatabase = acDoc.Database;

            using (var acTrans = acDatabase.TransactionManager.StartTransaction())
            {
                foreach (var group in Data)
                {
                    var ids = new List<ObjectId>();

                    for (int i = 0; i < group.Value.Count; i++)
                    {
                        ProcessNodeCommand.Data = group.Value[i];
                        var id = ProcessNodeCommand.InnerExecute();

                        ids.Add(id);
                    }

                    // get ids by name
                    ProcessNodeGroupCommand.Data = new Models.ProcessNodeGroup()
                    {
                        Name = group.Key,
                        Items = ids.ToArray()
                    };
                    ProcessNodeGroupCommand.InnerExecute();

                    var acBlockTable = acTrans.GetObject(acDatabase.BlockTableId, OpenMode.ForRead) as BlockTable;
                }
                acTrans.Commit();
            }

            // clear sample
            Data = null;
        }
    }
}