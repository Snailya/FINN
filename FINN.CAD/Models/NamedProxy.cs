using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace FINN.CAD.Models
{
    internal abstract class NamedProxy : BaseProxy
    {
        protected NamedProxy(Point3d location)
        {
            Location = location;
        }

        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public Point3d Location { get; }
    }
}