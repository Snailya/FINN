using Autodesk.AutoCAD.Geometry;

namespace FINN.CAD.Models
{
    internal abstract class GroupProxy : NamedProxy
    {
        protected GroupProxy(Point3d location) : base(location)
        {
        }
    }
}