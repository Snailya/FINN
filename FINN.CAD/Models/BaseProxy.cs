using Autodesk.AutoCAD.DatabaseServices;

namespace FINN.CAD.Models
{
    internal abstract class BaseProxy
    {
        public readonly ObjectIdCollection Children = new ObjectIdCollection();
        public readonly DBObjectCollection ChildrenObjects = new DBObjectCollection();

        public Extents3d GeoExtents
        {
            get
            {
                var extents = new Extents3d();
                foreach (Entity entity in ChildrenObjects)
                {
                    extents.AddExtents(entity.GeometricExtents);
                }

                return extents;
            }
        }

        ~BaseProxy()
        {
            ChildrenObjects.Dispose();
        }
    }
}