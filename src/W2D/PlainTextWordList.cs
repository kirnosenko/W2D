using System;
using System.Collections.Generic;
using System.IO;

namespace W2D
{
	public class PlainTextWordList : List<string>
	{
		public static PlainTextWordList ReadFromFile(string path)
		{
			PlainTextWordList result = new PlainTextWordList();

			foreach (var line in File.ReadAllLines(path))
			{
				result.Add(line);
			}

			return result;
		}
	}
}
