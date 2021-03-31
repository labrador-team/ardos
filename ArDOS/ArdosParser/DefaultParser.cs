using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;
using ArDOS.Model;
using ArDOS.Parser.Exceptions;
using ArDOS.Parser.Actions;

namespace ArDOS.Parser
{
    public partial class WindowsParser
    {
        public const RegexOptions DEFAULT_RE_OPTS = RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace;
        public static ArdosMenu Parse(string input)
        {
            var sections = Regex.Split(input, @"\n^-*$\n", DEFAULT_RE_OPTS).Select(x => x.Split(new [] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)).ToArray();

            // Parse title
            var titleSection = sections[0];
            var titleItem = ParseItem(sections[0][0], 0);

            // Parse sections
            var contentSections = sections[1..];
            var items = ParseSections(contentSections);

            return new ArdosMenu(titleItem.Text, titleItem.Image, items);
        }

        protected static ArdosItem[][] ParseSections(string[][] lines)
        {
            return lines.Select(ParseSection).ToArray();
        }

        protected static ArdosItem[][] ParseSections(string input)
        {
            return ParseSections(Regex.Split(input, @"\n^-*$\n", DEFAULT_RE_OPTS).Select(x => x.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)).ToArray());
        }

        protected static ArdosItem[] ParseSection(string[] lines)
        {
            var items = new List<ArdosItem>();
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].TrimStart().StartsWith("--"))
                {
                    if (items.Count == 0) throw new ParsingException(i, "cannot find parent sub-menu");

                    var submenuLines = lines[i].TrimStart()[2..];
                    while (lines[++i].TrimStart().StartsWith("--") && i < lines.Length)
                        submenuLines += "\n" + lines[i].TrimStart()[2..];
                    i--;
                    items[^1] = new ArdosSubMenu(items[^1], ParseSections(submenuLines));
                }
                else
                {
                    var item = ParseItem(lines[i], i);

                    // Set alternative / menu item
                    try
                    {
                        if (item.Alternate) items[^1].AlternativeItem = item;
                        else items.Add(item);
                    }
                    catch (FormatException e)
                    {
                        throw new ParsingException(i, e.Message);
                    }
                }
            }
            return items.ToArray();
        }

        protected static ArdosItem ParseItem(string line, int lineNumber)
        {
            // Separate text and attributes
            var match = Regex.Match(line, @"^(?<text>.*?) (?:\| \s* (?<attrs>.*) \s* )?$", DEFAULT_RE_OPTS);
            if (!match.Success) throw new ParsingException(lineNumber, "cannot recognize line format");
            var text = match.Groups["text"].Value;

            // Separate attributes
            var attrMatches = Regex.Matches(match.Groups["attrs"].Value, "(?:^|\\b)(?<name>\\w+)=(?<quote>[\"']?)(?<value>.+?)(\\k<quote>)(?:\\s+|$)", DEFAULT_RE_OPTS);
            var attrs = new Dictionary<string, string>(attrMatches.Select<Match, KeyValuePair<string, string>>(attrMatch => new(attrMatch.Groups["name"].Value, attrMatch.Groups["value"].Value)));

            // Create new item
            string key;
            ArdosItem item;
            try
            {
                item = new ArdosItem(text, !attrs.ContainsKey("trim") || bool.Parse(attrs["trim"]))
                {
                    Ansi = !attrs.ContainsKey("ansi") || bool.Parse(attrs["ansi"]),
                    Color = attrs.ContainsKey("color") ? ColorTranslator.FromHtml(attrs["color"]) : Color.Black,
                    Dropdown = !attrs.ContainsKey("dropdown") || bool.Parse(attrs["dropdown"]),
                    Emojize = !attrs.ContainsKey("emojize") || bool.Parse(attrs["emojize"]),
                    Font = new Font(attrs.ContainsKey("font") ? attrs["font"] : "Segoe UI",
                                                attrs.ContainsKey("size") ? float.Parse(attrs["size"]) : 11),
                    Image = attrs.ContainsKey(key = "image") || attrs.ContainsKey(key = "templateImage") ? Image.FromStream(new MemoryStream(Convert.FromBase64String(attrs[key]))) : null,
                    Length = attrs.ContainsKey("length") ? int.Parse(attrs["length"]) : 0,
                    Unescape = !attrs.ContainsKey("unescape") || bool.Parse(attrs["unescape"]),
                    UseMarkup = !attrs.ContainsKey("useMarkup") || bool.Parse(attrs["useMarkup"]),
                    Refresh = attrs.ContainsKey("refresh") && bool.Parse(attrs["refresh"]),
                    Alternate = attrs.ContainsKey("alternate") && bool.Parse(attrs["alternate"])
                };
            }
            catch (FormatException e)
            {
                throw new ParsingException(lineNumber, e.Message);
            }

            // Set Actions
            var actions = new List<IAction>();
            if (attrs.ContainsKey("href"))
                actions.Add(new WindowsHrefAction(new Uri(attrs["href"])));
            if (attrs.ContainsKey("bash"))
            {
                var args = new List<string>();
                for (int i = 1; attrs.ContainsKey($"param{i}"); i++)
                    args.Add(attrs[$"param{i}"]);
                actions.Add(new WindowsCMDAction(attrs["bash"], args.ToArray(), attrs.ContainsKey("terminal") && bool.Parse(attrs["terminal"])));
            }
            item.Actions = actions.ToArray();

            return item;
        }
    }
}
