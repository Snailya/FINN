using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System.Linq;
using System;

namespace FINN.CAD
{
    internal class Util
    {
        /// <summary>
        /// Create a Rectangle by specify the corners of the rectangle.
        /// </summary>
        /// <param name="firstCorner"></param>
        /// <param name="secondCorner"></param>
        /// <returns>The polyline object represents the specified rectangle, object should be disposed manually.</returns>
        public static Polyline CreateRectangle(Point2d firstCorner, Point2d secondCorner)
        {
            var acRectangle = new Polyline();

            acRectangle.AddVertexAt(0, firstCorner, 0, 0, 0);
            acRectangle.AddVertexAt(1, new Point2d(secondCorner.X, firstCorner.Y), 0, 0, 0);
            acRectangle.AddVertexAt(2, secondCorner, 0, 0, 0);
            acRectangle.AddVertexAt(3, new Point2d(firstCorner.X, secondCorner.Y), 0, 0, 0);
            acRectangle.Closed = true;

            return acRectangle;
        }

        /// <summary>
        /// Create a Text by specify the insertion point and text height.
        /// </summary>
        /// <param name="insertionPoint"></param>
        /// <param name="textHeight"></param>
        /// <param name="text"></param>
        /// <returns>The DBText object with specified content string, object should be disposed manually.</returns>
        public static DBText CreateText(Point2d insertionPoint, double textHeight, string text)
        {
            // cancel the operation if the content is empty
            if (string.IsNullOrEmpty(text)) return null;

            var acText = new DBText
            {
                Position = new Point3d(insertionPoint.X, insertionPoint.Y, 0),
                Height = textHeight,
                TextString = text
            };

            return acText;
        }

        /// <summary>
        /// Create a MText by specify the corners of the region.
        /// </summary>
        /// <param name="firstCorner"></param>
        /// <param name="secondCorner"></param>
        /// <param name="content"></param>
        /// <returns>The MText object with specified content string, object should be disposed manually.</returns>
        public static MText CreateMText(Point2d firstCorner, Point2d secondCorner, string content)
        {
            // cancel the operation if the content is empty
            if (string.IsNullOrEmpty(content)) return null;

            // decide the location
            var points = new Point2d[] { firstCorner, secondCorner };
            var location = new Point3d(points.Select(p => p.X).Min(), points.Select(p => p.Y).Max(), 0);

            var acMText = new MText
            {
                Location = location,
                Width = Math.Abs(secondCorner.X - firstCorner.X),
                Contents = content
            };

            return acMText;
        }
    }
}
