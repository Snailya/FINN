using Autodesk.AutoCAD.Geometry;

namespace FINN.CAD.Utilities
{
    internal static class Extensions
    {
        internal static Point2d To2d(this Point3d point)
        {
            return new Point2d(point.X, point.Y);
        }

        internal static Point3d To3d(this Point2d point)
        {
            return new Point3d(point.X, point.Y, 0);
        }
    }
}