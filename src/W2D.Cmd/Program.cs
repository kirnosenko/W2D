using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using W2D.WordTheme;

namespace W2D.Cmd
{
	class Program
	{
		static void Main(string[] args)
		{
            var dic = WordThemeDictionary.CreateNew("Google English 10K");
            var words = PlainTextWordList.ReadFromFile(
                "../../../../../words/google-10000-english.txt");
            ImportWords(dic, words, "all");
            dic.WriteToFile(
                "../../../../../dictionaries/eng10K.wt");
            
            Console.ReadLine();
		}

        static void ImportWords(WordThemeDictionary dic, IEnumerable<string> words, string topic)
        {
            foreach (var word in words)
            {
                dic.AddWord(topic, word, string.Empty);
            }
        }

        static string Translate(string word)
        {
            var toLanguage = "ru";
            var fromLanguage = "en";
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(word)}";
            var webClient = new WebClient
            {
                Encoding = System.Text.Encoding.UTF8
            };
            var result = webClient.DownloadString(url);
            try
            {
                result = result.Substring(4, result.IndexOf("\"", 4, StringComparison.Ordinal) - 4);
                return result;
            }
            catch
            {
                return "Error";
            }
        }
    }
}
