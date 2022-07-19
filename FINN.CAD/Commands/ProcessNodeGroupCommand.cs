using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using System.Linq;
using FINN.CAD.Models;

[assembly: CommandClass(typeof(FINN.CAD.Commands.ProcessNodeGroupCommand))]
namespace FINN.CAD.Commands
{
	public class ProcessNodeGroupCommand
	{
		private const double NameTextHeight = 5;

		public static ProcessNodeGroup Data { get; set; }

		private static ProcessNodeGroup PromptInput()
		{
			var acDoc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;

			var opt1 = new PromptStringOptions("Please enter group name...");
			var r1 = acDoc.Editor.GetString(opt1);

			var opt2 = new PromptSelectionOptions();
			var r2 = acDoc.Editor.GetSelection(opt2);
			if (r2.Status != PromptStatus.OK) return null;

			return new ProcessNodeGroup() { Name = r1.StringResult, Items = r2.Value.GetObjectIds() };
		}

		public static void InnerExecute()
		{
			var acDoc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
			var acDatabase = acDoc.Database;

			using (var acTrans = acDatabase.TransactionManager.StartTransaction())
			{
				var acBlockTable = acTrans.GetObject(acDatabase.BlockTableId, OpenMode.ForRead) as BlockTable;

				var entities = Data.Items.Select(i => acTrans.GetObject(i, OpenMode.ForRead) as Entity);
				double minX = entities.Min(e => e.GeometricExtents.MinPoint.X);
				double minY = entities.Min(e => e.GeometricExtents.MinPoint.Y);
				double maxX = entities.Max(e => e.GeometricExtents.MaxPoint.X);
				double maxY = entities.Max(e => e.GeometricExtents.MaxPoint.Y);

				var acBlockTableRecord = acTrans.GetObject(acBlockTable[BlockTableRecord.ModelSpace],
								OpenMode.ForWrite) as BlockTableRecord;

				var acText = Util.CreateText(new Point2d(minX, maxY), NameTextHeight, Data.Name);
				acBlockTableRecord.AppendEntity(acText);
				acText.Dispose();

				var acRect = Util.CreateRectangle(new Point2d(minX, minY), new Point2d(maxX, maxY));
				acBlockTableRecord.AppendEntity(acRect);
				acRect.Dispose();

				acTrans.Commit();
			}
		}

		/// <summary>
		/// Group specificied entityes inside a rectangle. 
		/// If data is not declared before, a user prompt will prompt to let user give the information.
		/// </summary>
		[CommandMethod("FINN", "grp", CommandFlags.Modal)]
		public void Execute()
		{
			// prompt user input if Sample is null
			Data = Data ?? PromptInput();
			if (Data == null) return;

			InnerExecute();

			Data = null;
		}
	}
}
