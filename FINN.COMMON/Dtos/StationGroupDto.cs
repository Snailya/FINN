using Newtonsoft.Json;

namespace FINN.COMMON.Dtos
{
	public class StationGroupDto
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("stations")]
		public StationDto[] Stations { get; set; }
	}
}
