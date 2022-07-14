namespace FINN
{
	public class Shape<T> where T : class
	{
		public Shape(T userData)
		{
			UserData = userData;
		}

		/// <summary>
		/// Used for locating shape block in other software
		/// </summary>
		public ShapeTransform ShapeTransform { get; set; }

		/// <summary>
		/// Used for define shape color
		/// </summary>
		public FillFormat FillFormat { get; set; }

		/// <summary>
		/// User defined custom data, most of them are read from excel
		/// </summary>
		public T UserData { get; private set; }
	}
}
