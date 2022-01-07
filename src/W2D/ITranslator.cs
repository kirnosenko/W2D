using System;

namespace W2D
{
	public interface ITranslator
	{
		public string Translate(string word, string langFrom, string langTo);
	}
}
