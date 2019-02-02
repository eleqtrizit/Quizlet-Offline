using HtmlAgilityPack;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace QuizletRipper
{
	public class FlashCard
	{
		public string question { get; set; }
		public string answer { get; set; }
		public string user { get; set; }
		public string pageTitle { get; set; }
		public string link { get; set; }
	}


	public class Bot
	{
		private readonly ParseTool _parse = new ParseTool();
		private List<FlashCard> _flashCards = new List<FlashCard>();

		public void GatherCardPages(string path)
		{
			string bigJson = "";
			var nodes = ProcessFiles(ProcessDirectory(path));
			foreach (var node in nodes)
			{
				GetCards(node);
			}

			var json = JsonConvert.SerializeObject(_flashCards, Formatting.Indented);
			var js = "var json = " + json + ";";
			System.IO.File.WriteAllText(path + @"\quizlet.json", json);
			System.IO.File.WriteAllText(path + @"\quizlet.js", js);
		}

		private void GetCards(HtmlNode node)
		{
			if (node == null)
			{
				return;
			}

			var pageTitle = node.SelectSingleNode("//h1[contains(@class, 'UIHeading UIHeading--one')]").InnerHtml;
			var user = node.SelectSingleNode("//span[contains(@class, 'UserLink-username')]").InnerHtml;
			var link = node.SelectNodes("//a[contains(@class, 'UILink UILink--inverted')]")
				.Where(x => x.GetAttributeValue("href", null).Contains("flash-card")).ToList()[0].GetAttributeValue("href",null);

			var classes = node.SelectNodes("//span[contains(@class, 'TermText notranslate lang-en')]");
			int i = 0;
			while (i < classes.Count-1)
			{
				var flash = new FlashCard
				{
					question = classes[i++].InnerHtml,
					answer = classes[i++].InnerHtml,
					pageTitle = pageTitle,
					user = user,
					link = link
				};
				_flashCards.Add(flash);

			}
		}

		private string[] ProcessDirectory(string targetDirectory)
		{
			string[] fileEntries = new string[]{};

			if (Directory.Exists(targetDirectory))
			{
				// Process the list of files found in the directory.

				fileEntries = Directory.GetFiles(targetDirectory);
				
			}

			return fileEntries;
		}

		private List<HtmlNode> ProcessFiles(string[] fileEntries)
		{
			var nodes = new List<HtmlNode>();
			foreach (var fileName in fileEntries)
			{
				Console.WriteLine(fileName);
				var doc = new HtmlDocument();
				doc.Load(fileName);
				var node = doc.DocumentNode.SelectSingleNode("//body");
				nodes.Add(node);
			}

			return nodes;
		}
	}


	class Program
	{
		static void Main(string[] args)
		{
			Bot bot = new Bot();
			Console.WriteLine("This program rips quizlet data.\n\nTo rip data, please save copies of all the quizlet pages to a directory on your disk.\n\nThe returned file will be a JS file for use with the bundled index.html page, plus a plain JSON file for your own use.\n\nThis program was working as of 02.02.19.\n\n");
			Console.WriteLine("Enter the directory path...");
			Console.Write("=>");
			var path = Console.ReadLine();
			bot.GatherCardPages(path);
			Console.WriteLine("\n\nDone.\nFile written to " + path + @"\quizlet.json");
			Console.WriteLine("File written to " + path + @"\quizlet.js");
			Console.WriteLine("Press any key to exit.");
			Console.Read();
		}
	}
}
