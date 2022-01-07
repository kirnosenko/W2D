using System;
using System.Collections.Generic;

namespace W2D.WordTheme
{
	public class ModelDictionary
	{
		public string libelle { get; set; }
		public Guid identifier { get; set; }
		public int version { get; set; }
		public DateTime dm { get; set; }
		public LinkedList<ModelWordTheme> listAssoWT { get; set; }
		public LinkedList<ModelTheme> ltheme { get; set; }
		public LinkedList<ModelWord> lword { get; set; }
		public LinkedList<ModelWordThemeAssociation> listWordThemeAssociation { get; set; }
	}
}
