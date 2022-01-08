using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;

namespace W2D.WordTheme
{
	public class WordThemeDictionary : IWordDictionary
	{
		private const string DicEntryName = "dictionary.txt";
		private const string RemovedEntryName = "removed.txt";

		private readonly ModelDictionary innerDictionary;
		private readonly Dictionary<string, string> data;

		private WordThemeDictionary(string label)
		{
			this.data = new Dictionary<string, string>()
			{
				{ RemovedEntryName, string.Empty },
			};
			innerDictionary = new ModelDictionary()
			{
				libelle = label,
				identifier = Guid.NewGuid(),
				version = "2",
				dm = DateTime.Now,
				listAssoWT = new LinkedList<ModelWordTheme>(),
				ltheme = new LinkedList<ModelTheme>(),
				lword = new LinkedList<ModelWord>(),
				listWordThemeAssociation = new LinkedList<ModelWordThemeAssociation>(
					new ModelWordThemeAssociation[] {
						new ModelWordThemeAssociation()
						{
							idTheme = -1,
							idWord = -1,
						}
					}),
			};
		}

		private WordThemeDictionary(Dictionary<string, string> data)
		{
			this.data = data;
			innerDictionary = data.TryGetValue(DicEntryName, out var body)
				? JsonConvert.DeserializeObject<ModelDictionary>(body)
				: new ModelDictionary();
		}

		public static WordThemeDictionary CreateNew(string label)
		{
			return new WordThemeDictionary(label);
		}

		public static WordThemeDictionary ReadFromFile(string path)
		{
			using (var file = File.OpenRead(path))
			using (var zip = new ZipArchive(file, ZipArchiveMode.Read))
			{
				Dictionary<string, string> data = new Dictionary<string, string>();

				foreach (var entry in zip.Entries)
				{
					using (StreamReader sr = new StreamReader(entry.Open()))
					{
						data.Add(entry.Name, sr.ReadToEnd());
					}
				}

				return new WordThemeDictionary(data);
			}
		}

		public void WriteToFile(string path)
		{
			using (var file = File.OpenWrite(path))
			using (var zip = new ZipArchive(file, ZipArchiveMode.Create))
			{
				data[DicEntryName] = JsonConvert.SerializeObject(
					innerDictionary,
					new JsonSerializerSettings()
					{
						NullValueHandling = NullValueHandling.Ignore,
						DateTimeZoneHandling = DateTimeZoneHandling.Utc,
					});
				foreach (var entry in data)
				{
					ZipArchiveEntry zipEntry = zip.CreateEntry(entry.Key);
					using (StreamWriter writer = new StreamWriter(zipEntry.Open()))
					{
						writer.Write(entry.Value);
					}
				}
			}
		}

		public void AddWord(string topicName, string wordText, string translationText)
		{
			var topic = innerDictionary.ltheme.SingleOrDefault(x => x.l == topicName);
			var word = innerDictionary.lword.SingleOrDefault(x => x.m == wordText);
			var link = word != null
				? innerDictionary.listAssoWT.SingleOrDefault(x => x.w == word.id)
				: null;

			if (topic == null)
			{
				topic = new ModelTheme()
				{
					id = innerDictionary.ltheme
						.Select(x => x.id)
						.DefaultIfEmpty()
						.Max() + 1,
					uid = Guid.NewGuid(),
					dm = DateTime.Now,
					l = topicName,
				};
				innerDictionary.ltheme.AddLast(topic);
			}

			if (word == null)
			{
				word = new ModelWord()
				{
					id = innerDictionary.lword
						.Select(x => x.id)
						.DefaultIfEmpty()
						.Max() + 1,
					uid = Guid.NewGuid(),
					dc = DateTime.Now,
				};
				innerDictionary.lword.AddLast(word);
			}
			word.dm = DateTime.Now;
			word.m = wordText;
			word.t = translationText;

			if (link == null)
			{
				link = new ModelWordTheme();
				innerDictionary.listAssoWT.AddLast(link);
			}
			link.t = topic.id;
			link.w = word.id;
		}

		public void RemoveWord(string word)
		{
			
		}
	}
}
