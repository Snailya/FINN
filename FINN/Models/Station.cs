using System.Collections.Generic;
using System.Drawing;

namespace FINN.Models
{
	internal class Station
	{
		public string Name { get; internal set; }
		public Color? Color { get; internal set; }
		public List<string> ProcessParameters { get; internal set; }
		public ShapeMeta ShapeMeta { get;  internal set; }
	}

	internal class ShapeMeta
	{
		public double Length { get; set; }
		public double Width { get; set; }
	}
}
