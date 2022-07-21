using Newtonsoft.Json;

namespace FINN.COMMON.Dtos
{
	public class StationDto
	{
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonProperty("body")]
		public string Body { get; set; }

		[JsonProperty("xLength")]
		public double XLength { get; set; }
		[JsonProperty("yLength")]
		public double YLength { get; set; }
	}
}
