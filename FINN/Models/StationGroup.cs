using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FINN.Models;

namespace FINN.COMMON.Models
{
	internal class StationGroup
	{
		public string Name { get; internal set; }
		public IEnumerable<Station> Stations { get; internal set; }
	}
}
