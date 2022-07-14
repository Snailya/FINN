using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System;

namespace FINN
{
	public partial class Ribbon
	{
		private Application _excel;

		private void Ribbon_Load(object sender, RibbonUIEventArgs e)
		{
			_excel = Globals.ThisAddIn.Application;
		}


		private void btnGenerate_Click(object sender, RibbonControlEventArgs e)
		{
			var nodes = new List<ProcessNode>();

			var usedRange = ((Worksheet)_excel.ActiveSheet).UsedRange;
			var rowCount = usedRange.Rows.Count;
			var colCount = usedRange.Columns.Count;

			for (int colIndex = 4; colIndex < colCount + 1; colIndex++)
			{
				var name = usedRange.Cells[1, colIndex].Text;

				Color? color = null;
				if (usedRange.Cells[1, colIndex].Interior.Color is double colorValue)
				{
					color = Color.FromArgb(Convert.ToInt32(colorValue));
				}

				var opertaionMeta = new List<string>();
				for (int rowIndex = 2; rowIndex < rowCount - 1; rowIndex++)
				{
					var value = $"{usedRange.Cells[rowIndex, 2].Text}{usedRange.Cells[rowIndex, 3].Text}: {usedRange.Cells[rowIndex, colIndex].Text}";
					opertaionMeta.Add(value);
				}

				var length = usedRange.Cells[rowCount - 1, colIndex].Value2;
				var width = usedRange.Cells[rowCount, colIndex].Value2;
				var shapeMeta = new ShapeMeta(length, width);

				var node = new ProcessNode()
				{
					Name = name,
					Color = color,
					OpertaionMeta = opertaionMeta,
					ShapeMeta = shapeMeta
				};
				nodes.Add(node);
			}

			Debugger.Break();
		}
	}
}
