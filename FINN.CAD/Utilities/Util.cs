using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace FINN.CAD.Utilities
{
    internal static class Util
    {
        internal static Dictionary<string, ObjectId> FontStyles = new Dictionary<string, ObjectId>();

        /// <summary>
        /// Create a block reference at specified position 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="blockTableRecord"></param>
        /// <returns></returns>
        internal static BlockReference PrepareBlockReference(Point3d position, ObjectId blockTableRecord)
        {
            return new BlockReference(position, blockTableRecord);
        }

        internal static Line PrepareLine()
        {
            return new Line();
        }

        internal static Circle PrepareCircle()
        {
            return new Circle();
        }

        /// <summary>
        /// Create a Rectangle by specify the corners of the rectangle.
        /// </summary>
        /// <param name="minPoint"></param>
        /// <param name="maxPoint"></param>
        /// <returns>The object represents the specified rectangle, object should be disposed manually.
        /// </returns>
        internal static Polyline PrepareRect(Point2d minPoint, Point2d maxPoint)
        {
            var acRectangle = new Polyline();

            acRectangle.AddVertexAt(0, minPoint, 0, 0, 0);
            acRectangle.AddVertexAt(1, new Point2d(maxPoint.X, minPoint.Y), 0, 0, 0);
            acRectangle.AddVertexAt(2, maxPoint, 0, 0, 0);
            acRectangle.AddVertexAt(3, new Point2d(minPoint.X, maxPoint.Y), 0, 0, 0);
            acRectangle.Closed = true;

            return acRectangle;
        }

        /// <summary>
        /// Create a Text by specify the insertion point and text height.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="textHeight"></param>
        /// <param name="text"></param>
        /// <returns>The object with specified content string, object should be disposed manually.
        /// </returns>
        internal static DBText PrepareDbText(Point3d position, double textHeight, string text)
        {
            // cancel the operation if the content is empty
            if (string.IsNullOrEmpty(text)) return null;

            var acText = new DBText
            {
                Position = position,
                Height = textHeight,
                TextString = text,
            };

            return acText;
        }

        /// <summary>
        /// Create a MText by specify the corners of the region.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="content"></param>
        /// <returns>The object with specified content string, object should be disposed manually.
        /// </returns>
        internal static MText PrepareMText(Point3d minPoint, Point3d maxPoint, string content)
        {
            // cancel the operation if the content is empty
            if (string.IsNullOrEmpty(content)) return null;

            var acMText = new MText
            {
                Location = new Point3d(minPoint.X, maxPoint.Y, 0),
                Contents = content,
                Width = maxPoint.X - minPoint.X
            };

            return acMText;
        }

        internal static void LogGeometricExtents(string name, Entity entity)
        {
            Debug.WriteLine(
                $"[{name}] minPoint: {entity.GeometricExtents.MinPoint} maxPoint: {entity.GeometricExtents.MaxPoint}");
        }

        internal static ObjectId GetFontStyleId(string styleName)
        {
            if (FontStyles.TryGetValue(styleName, out var id)) return id;
            
            var acDb = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager
                .MdiActiveDocument.Database;
            using (var acTrans = acDb.TransactionManager.StartTransaction())
            {
                var acTextStyleTable =
                    acTrans.GetObject(acDb.TextStyleTableId, OpenMode.ForRead) as TextStyleTable;
                FontStyles.Add(styleName, acTextStyleTable[styleName]);
                return acTextStyleTable[styleName];
            }
        }

        internal static Point2d GetTextExtents(string value, string textStyleName = "Standard")
        {
            var ts = new Autodesk.AutoCAD.GraphicsInterface.TextStyle();
            ts.FromTextStyleTableRecord(textStyleName);
            return ts.ExtentsBox(value, true, false, null).MaxPoint;
        }
    }
}