using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using FINN.CAD.Utilities;

namespace FINN.CAD.Models
{
    internal sealed class Fence : BaseProxy, IColorRegion
    {
        public string Label { get; }
        public Point3d MinPoint { get; }
        public Point3d MaxPoint { get; }

        public Fence(Point3d minPoint, Point3d maxPoint, string label)
        {
            MinPoint = minPoint;
            MaxPoint = maxPoint;
            Label = label;

            Initialize();
        }

        public Fence(IEnumerable<Station> stations, string label)
        {
            // compute the min and max point
            var extents = new Extents3d();
            foreach (var station in stations)
            {
                extents.AddExtents(station.GeoExtents);
            }

            MinPoint = extents.MinPoint;
            MaxPoint = extents.MaxPoint;
            Label = label;

            Initialize();
        }

        private void Initialize()
        {
            var rect = new Polyline();
            rect.AddVertexAt(0, MinPoint.To2d(), 0, 0, 0);
            rect.AddVertexAt(1, new Point2d(MaxPoint.X, MinPoint.Y), 0, 0, 0);
            rect.AddVertexAt(2, MaxPoint.To2d(), 0, 0, 0);
            rect.AddVertexAt(3, new Point2d(MinPoint.X, MaxPoint.Y), 0, 0, 0);
            rect.Closed = true;
            ChildrenObjects.Add(rect);

            var text = new DBText { Position = new Point3d((MinPoint.X+MaxPoint.X)/2, MaxPoint.Y, 0), TextString = Label };
            ChildrenObjects.Add(text);

            Region.Add(rect);
        }

        public DBObjectCollection Region { get; } = new DBObjectCollection();
        public Color Color { get; }
    }
}