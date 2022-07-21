using Autodesk.AutoCAD.Geometry;

namespace FINN.CAD.Models
{
    internal abstract class BlockProxy : NamedProxy
    {
        protected BlockProxy(Point3d location) : base(location)
        {
        }
    }
}