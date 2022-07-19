using Autodesk.AutoCAD.DatabaseServices;

namespace FINN.CAD.Models
{
	public class ProcessNodeGroup
	{
		public string Name { get; set; }
		public ObjectId[] Items { get; set; }
	}
}
