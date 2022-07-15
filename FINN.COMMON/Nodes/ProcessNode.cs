using System.Collections.Generic;
using System.Drawing;

namespace FINN
{

	public class ProcessNode
	{
		public string Name { get; set; }
		public Color? Color { get; set; }
		public List<string> OpertaionMeta { get; set; }
		public ShapeMeta ShapeMeta { get; set; }

		public Shape<ProcessNode> ToShape()
		{
			var shape = new Shape<ProcessNode>(this);

			// initial shape transform
			if (ShapeMeta != null)
			{
				// pay attention that: visio shape's width = length in Excel; visio shape's height = width in Excel
				shape.ShapeTransform.Width = ShapeMeta.Length;
				shape.ShapeTransform.Height = ShapeMeta.Width;
			}
			else
			{
				//  consider compute a default value
			}

			// initial fillformat
			shape.FillFormat.Fill = Color;

			return shape;
		}
	}
}
