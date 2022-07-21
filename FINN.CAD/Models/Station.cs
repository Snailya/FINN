using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using FINN.CAD.Utilities;

namespace FINN.CAD.Models
{
    internal sealed class Station : BlockProxy
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public double XLength { get; set; }
        public double YLength { get; set; }

        public Station(Point3d location, string title, string body) : base(location)
        {
            Body = body;
            Title = title;

            Initialize(false);
        }

        public Station(Point3d location, string title, string body, double xLength, double yLength,
            bool userDefinedShape = false) : base(location)
        {
            Body = body;
            Title = title;
            XLength = xLength;
            YLength = yLength;

            Initialize(userDefinedShape);
        }

        public void Initialize(bool userDefinedShape)
        {
            var extents = new Extents3d();
            // draw the title
            var title = new DBText() { Position = Location, TextString = Title, TextStyleId = Util.GetFontStyleId("Standard"), ColorIndex = 3};
            ChildrenObjects.Add(title);
            extents.AddExtents(title.GeometricExtents);

            // draw the body
            var body = new MText()
            {
                Location = extents.MinPoint, 
                Contents = Body,
                TextStyleId = Util.GetFontStyleId("Standard"),
            };
            ChildrenObjects.Add(body);
            extents.AddExtents(body.GeometricExtents);


            // draw the rectangle around
            if (userDefinedShape)
            {
                var acTitleRect = Util.PrepareRect(title.GeometricExtents.MinPoint.To2d(),
                    new Point2d(XLength, 0));
                ChildrenObjects.Add(acTitleRect);
                Util.LogGeometricExtents(nameof(acTitleRect), acTitleRect);

                var acBodyRect = Util.PrepareRect(title.GeometricExtents.MinPoint.To2d(),
                    new Point2d(XLength, YLength));
                ChildrenObjects.Add(acBodyRect);
                Util.LogGeometricExtents(nameof(acBodyRect), acBodyRect);
            }
            else
            {

                var acRect = Util.PrepareRect(extents.MinPoint.To2d(),
                    extents.MaxPoint.To2d());
                ChildrenObjects.Add(acRect);
                extents.AddExtents(acRect.GeometricExtents);
            }
        }
    }
}