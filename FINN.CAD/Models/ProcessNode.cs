using Autodesk.AutoCAD.Geometry;

namespace FINN.CAD.Models
{
    public class ProcessNode
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public double XLength { get; set; }
        public double YLength { get; set; }

        public Point2d Position { get; set; }
    }
}
