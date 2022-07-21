namespace FINN.COMMON.Dtos
{
	public static class Extensions
	{
		/// <summary>
		/// Convert the object file path into base64 string to escape escape characters which can't be accept as AutoCAD command line input.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string ToBase64(this string path)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(path);
			return System.Convert.ToBase64String(plainTextBytes);
		}

		/// <summary>
		/// Convert back base64 file path to readable path string.
		/// </summary>
		/// <param name="base64EncodedData"></param>
		/// <returns></returns>
		public static string FromBase64(this string base64EncodedData)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}
	}
}
