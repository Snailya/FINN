using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;

namespace FINN.CAD.Models
{
    public interface IColorRegion
    {
        DBObjectCollection Region { get; }
        Color Color { get; }
    }
}