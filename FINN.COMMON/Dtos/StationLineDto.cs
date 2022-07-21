using Newtonsoft.Json;

namespace FINN.COMMON.Dtos
{
	public class StationLineDto
	{
		[JsonProperty("groups")]
		public StationGroupDto[] Groups { get; set; }
	}
}
