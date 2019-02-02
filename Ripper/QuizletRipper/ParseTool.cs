using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace QuizletRipper
{
	public class ParseTool
	{
		public void NormalizeElementNames(ref HtmlNode htmlBody)
		{
			htmlBody.Descendants().ToList().ForEach(x => { x.Name = x.Name.ToLower(); });
		}

		public bool IsThere(string html, string thisHTML)
		{
			return Regex.IsMatch(html, thisHTML, RegexOptions.IgnoreCase | RegexOptions.Multiline);
		}

		public string Grab(string html, string thisHTML)
		{
			Match m = Regex.Match(html, thisHTML, RegexOptions.IgnoreCase | RegexOptions.Multiline);
			return m.Value;
		}

		public string DeleteAll(string html, string replaceMe, string withThis = "")
		{
			return Regex.Replace(html, replaceMe, withThis, RegexOptions.IgnoreCase | RegexOptions.Multiline);
		}

		public string Delete(string html, string replaceMe, string withThis = "")
		{
			Regex rgx = new Regex(replaceMe, RegexOptions.IgnoreCase | RegexOptions.Multiline);
			return rgx.Replace(html, withThis, 1).Trim();
		}

		public string StripChars(string str)
		{
			Regex rgx = new Regex("[^a-zA-Z0-9 -'\"&./]");
			str = rgx.Replace(str, "");
			return str;
		}

	}
}
