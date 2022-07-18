using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FINN.CAD
{
	internal static class Extensions
	{
		public static Models.ProcessNode ToProcessNode(this FINN.ProcessNode node, double x, double y)
		{
			return new Models.ProcessNode()
			{
				Title = node.Name,
				Body = string.Join("\n", node.OpertaionMeta),

				XLength = node.ShapeMeta.Length,
				YLength = node.ShapeMeta.Width,

				Position = new Autodesk.AutoCAD.Geometry.Point2d(x, y)
			};
		}
	}
}

