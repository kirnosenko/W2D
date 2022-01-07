using System;

namespace W2D.WordTheme
{
	public class ModelWord
	{
		public int id { get; set; }
		public Guid uid { get; set; }
		public string m { get; set; }
		public string t { get; set; }
		public string i { get; set; }
		public DateTime dc { get; set; }
		public DateTime dm { get; set; }
		public int tm { get; set; }
	}
}
