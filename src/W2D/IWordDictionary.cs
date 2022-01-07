using System;

namespace W2D
{
	public interface IWordDictionary
	{
		public void AddWord(string topic, string word, string translation);
		public void RemoveWord(string word);
	}
}
