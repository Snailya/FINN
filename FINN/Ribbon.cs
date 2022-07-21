using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Drawing;
using System;
using FINN.CAD;
using FINN.COMMON.Dtos;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using System.Text;
using FINN.COMMON.Constants;
using FINN.Models;

namespace FINN
{
    public partial class Ribbon
    {
        private Microsoft.Office.Interop.Excel.Application _excel;

        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {
            _excel = Globals.ThisAddIn.Application;
        }

        private IEnumerable<Station> Read()
        {
            var stations = new List<Station>();

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
                    var value =
                        $"{usedRange.Cells[rowIndex, 2].Text}{usedRange.Cells[rowIndex, 3].Text}: {usedRange.Cells[rowIndex, colIndex].Text}";
                    opertaionMeta.Add(value);
                }

                var length = usedRange.Cells[rowCount - 1, colIndex].Value2;
                var width = usedRange.Cells[rowCount, colIndex].Value2;
                var shapeMeta = new ShapeMeta() { Length = length, Width = width };

                var station = new Station()
                {
                    Name = name,
                    Color = color,
                    ProcessParameters = opertaionMeta,
                    ShapeMeta = shapeMeta
                };
                stations.Add(station);
            }

            return stations;
        }

        private static void Execute(string commandName, string jsonText)
        {
            // write json text into a file and pass the file path as the command parameter
            var filePath = Path.GetTempFileName();
            File.WriteAllText(filePath, jsonText, Encoding.UTF8);

            var assemblyPath = @"C:\\Users\\snailya\\source\\repos\\FINN\\FINN\\bin\\Debug\\FINN.CAD.dll";
            CADHelper.LoadInProcessAssembly(assemblyPath);

            var command = $"(command \"{commandName}\" \"{filePath.ToBase64()}\") \r";
            CADHelper.Execute(command);
        }

        private void btnStation_Click(object sender, RibbonControlEventArgs e)
        {
            Execute(Command.CREATE_STATION_SILENT, JsonConvert.SerializeObject(Read().First().ToDto()));
        }

        private void btnGroup_Click(object sender, RibbonControlEventArgs e)
        {
            var stations = Read();
            var color = stations.First().Color;

            Execute(Command.CREATE_STATION_GROUP_SILENT,
                JsonConvert.SerializeObject(stations.Where(x => x.Color == color).ToDto(color.ToString())));
        }

        private void btnLine_Click(object sender, RibbonControlEventArgs e)
        {
            Execute(Command.CREATE_STATION_LINE_SILENT, JsonConvert.SerializeObject(Read().ToDto()));
        }
    }
}