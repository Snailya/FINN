using Autodesk.AutoCAD.Runtime;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System.Diagnostics;
using FINN.CAD.Models;
using FINN.CAD.Utilities;

[assembly: CommandClass(typeof(FINN.CAD.Commands.TestCommand))]

namespace FINN.CAD.Commands
{
    internal class TestCommand
    {
        public class TestClass
        {
            [JsonProperty("name")] public string Name { get; set; }
        }

        [CommandMethod("FINN", "test", CommandFlags.Modal)]
        public void Test()
        {
            var blockReferenceId = ObjectId.Null;
            var acDb = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Database;

            var acBlockTableRecord = new BlockTableRecord()
            {
                Name = "Test"
            };
            var acMText = new MText()
            {
                Contents = "Test",
                Location = new Point3d(0, 0, 0)
            };
            acBlockTableRecord.AppendEntity(acMText);
            Debug.WriteLine($"[Test] - acMText - {acMText.ObjectId}");

            using (var acTrans = acDb.TransactionManager.StartTransaction())
            {
                var acBlockTable = acTrans.GetObject(acDb.BlockTableId, OpenMode.ForWrite) as BlockTable;
                var id = acBlockTable.Add(acBlockTableRecord);
                Debug.WriteLine($"[Test] - acBlockTableRecord - {acBlockTableRecord.ObjectId}");
                acBlockTableRecord.Dispose();
                acMText.Dispose();

                var acModelSpaceRecord =
                    acTrans.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                var acBlockReference = new BlockReference(new Point3d(0, 0, 0), id);
                acModelSpaceRecord.AppendEntity(acBlockReference);
                Debug.WriteLine($"[Test] - acBlockReference - {acBlockReference.ObjectId}");
                acBlockReference.Dispose();
                acModelSpaceRecord.Dispose();

                acTrans.Commit();
            }

            using (var acTrans = acDb.TransactionManager.StartTransaction())
            {
                var acBlockTable = acTrans.GetObject(acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                var acModelSpaceRecord =
                    acTrans.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
            }
        }

        [CommandMethod("FINN", "test2", CommandFlags.Modal)]
        public void Test2()
        {
            var acDb = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Database;

            var acBlockTableRecord = new BlockTableRecord()
            {
                Name = "Test2"
            };

            using (var acTrans = acDb.TransactionManager.StartTransaction())
            {
                var acBlockTable = acTrans.GetObject(acDb.BlockTableId, OpenMode.ForWrite) as BlockTable;


                acBlockTable.Add(acBlockTableRecord);
                Debug.WriteLine($"[Test2] - acBlockTableRecord - {acBlockTableRecord.ObjectId}");

                var acMText = new MText()
                {
                    Contents = "Test2",
                    Location = new Point3d(0, 0, 0)
                };
                acBlockTableRecord.AppendEntity(acMText);
                Debug.WriteLine($"[Test2] - acMText - {acMText.ObjectId}");
                acMText.Dispose();

                var acModelSpaceRecord =
                    acTrans.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                var acBlockReference = new BlockReference(new Point3d(0, 0, 0), acBlockTableRecord.ObjectId);
                acBlockTableRecord.Dispose();

                acModelSpaceRecord.AppendEntity(acBlockReference);
                Debug.WriteLine($"[Test2] - acBlockReference - {acBlockReference.ObjectId}");
                acBlockReference.Dispose();
                acModelSpaceRecord.Dispose();

                acTrans.Commit();
            }

            using (var acTrans = acDb.TransactionManager.StartTransaction())
            {
                var acBlockTable = acTrans.GetObject(acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                var acModelSpaceRecord =
                    acTrans.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
            }
        }

        [CommandMethod("FINN", "test3", CommandFlags.Modal)]
        public void Test3()
        {
            var acDb = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Database;

            using (var acTrans = acDb.TransactionManager.StartTransaction())
            {
                var acBlockTable = acTrans.GetObject(acDb.BlockTableId, OpenMode.ForWrite) as BlockTable;

                var acBlockTableRecord = new BlockTableRecord()
                {
                    Name = "Test3"
                };


                var acMText = new MText()
                {
                    Contents = "Test3",
                    Location = new Point3d(0, 0, 0)
                };
                acBlockTableRecord.AppendEntity(acMText);
                Debug.WriteLine($"[Test3] - acMText - {acMText.ObjectId}");

                acBlockTable.Add(acBlockTableRecord);
                Debug.WriteLine($"[Test3] - acBlockTableRecord - {acBlockTableRecord.ObjectId}");

                var acModelSpaceRecord =
                    acTrans.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                var acBlockReference = new BlockReference(new Point3d(0, 0, 0), acBlockTableRecord.ObjectId);
                acModelSpaceRecord.AppendEntity(acBlockReference);
                Debug.WriteLine($"[Test3] - acBlockReference - {acBlockReference.ObjectId}");


                acMText.Dispose();
                acBlockTableRecord.Dispose();
                acBlockReference.Dispose();

                acTrans.Commit();
            }

            using (var acTrans = acDb.TransactionManager.StartTransaction())
            {
                var acBlockTable = acTrans.GetObject(acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                var acModelSpaceRecord =
                    acTrans.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
            }
        }

        [CommandMethod("FINN", "test4", CommandFlags.Modal)]
        public void Test4()
        {
            var acDb = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Database;

            using (var acTrans = acDb.TransactionManager.StartTransaction())
            {
                var acMText = new MText()
                {
                    Contents = "Test4",
                    Location = new Point3d(0, 0, 0)
                };

                var acBlockTable = acTrans.GetObject(acDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                var acModelSpaceRecord =
                    acTrans.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                acModelSpaceRecord.AppendEntity(acMText);
                Debug.WriteLine($"[Test4] - acMText - {acMText.ObjectId}");
                acMText.Dispose();

                acTrans.Commit();
            }

            using (var acTrans = acDb.TransactionManager.StartTransaction())
            {
                var acBlockTable = acTrans.GetObject(acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                var acModelSpaceRecord =
                    acTrans.GetObject(acBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
            }
        }

        [CommandMethod("FINN", "test5", CommandFlags.Modal)]
        public void Test5()
        {
            var cm = new ContextManager();
            var group = new Fence(new Point3d(0, 0, 0), new Point3d(5, 5, 0), "test5");
            cm.Add(group);
            cm.SaveChanges();
        }

        [CommandMethod("FINN", "test6", CommandFlags.Modal)]
        public void Test6()
        {
            var cm = new ContextManager();
            var group = new Fence(new Point3d(0, 0, 0), new Point3d(5, 5, 0), "test6");
            cm.Add(group);
            var station = new Station(new Point3d(0, 0, 0), "test6", "test6");
            cm.Add(station);
            cm.SaveChanges();
        }

        [CommandMethod("FINN", "test7", CommandFlags.Modal)]
        public void Test67()
        {
            var cm = new ContextManager();


            var station = new Station(new Point3d(0, 0, 0), "s1", "s1b");
            cm.Add(station);

            Debug.WriteLine(station.GeoExtents.MaxPoint.X);
            var station2 = new Station(station.GeoExtents.MaxPoint, "s2", "s2b");
            cm.Add(station2);

            var group = new Fence(station.GeoExtents.MinPoint, station2.GeoExtents.MaxPoint, "group");
            cm.Add(group);

            cm.SaveChanges();
        }
        
        [CommandMethod("TextExtentsTest2")]
        public static void TextWidth() {
            var ed = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager
                .MdiActiveDocument.Editor;
            var ts = new Autodesk.AutoCAD.GraphicsInterface.TextStyle();
            ts.FromTextStyleTableRecord("Standard");
            ts.TextSize = 25;
            ed.WriteMessage("AP " + ts.ExtentsBox("AP", true, false, null).MaxPoint.X);
            ed.WriteMessage("\nAPPLE " + ts.ExtentsBox("APPLE", true, false, null).MaxPoint.X);
            ed.WriteMessage("\nAPPLESAUCE " + ts.ExtentsBox("APPLESAUCE", true, false, null).MaxPoint.X);
        }
    }
}