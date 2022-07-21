using FINN.COMMON.Models;
using System.Collections.Generic;
using System.Linq;
using FINN.Models;

namespace FINN.COMMON.Dtos
{
	public static class Extensions
	{
		private static readonly double Gutter = 10;

		internal static StationDto ToDto(this Station node)
		{
			return new StationDto()
			{
				Title = node.Name,
				Body = string.Join("\n", node.ProcessParameters),

				XLength = node.ShapeMeta.Length,
				YLength = node.ShapeMeta.Width,
			};
		}

		internal static StationGroupDto ToDto(this IEnumerable<Station> stations, string name)
		{
			return new StationGroupDto()
			{
				Name = name,
				Stations = stations.Select(s =>
				{
					var dto = s.ToDto();
					return dto;
				}).ToArray()
			};
		}

		internal static StationLineDto ToDto(this IEnumerable<Station> stations)
		{
			return new StationLineDto()
			{
				Groups = stations.GroupBy(s => s.Color).Select(g => {
					return new StationGroupDto()
					{
						Name = g.Key.ToString(),
						Stations = g.Select(s =>
						{
							var dto = s.ToDto();
							return dto;
						}).ToArray()
					};
				}).ToArray()
			};
		}
	}
}

