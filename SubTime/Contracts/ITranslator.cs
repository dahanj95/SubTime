using SubTime.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SubTime.Contracts
{
    public abstract class ITranslator
    {
        public string Pattern { get; protected set; }
        public string ParseExactPattern { get; protected set; }

        public void Translate(string fileName, int seconds)
        {
            var times = GetAllTimes(fileName);
            AdjustTimesBySeconds(ref times, seconds);
            WriteChangesToFile(fileName, times);
        }

        private IEnumerable<Time> GetAllTimes(string pathToSubtitle)
        {
            string content = File.ReadAllText(pathToSubtitle);
            var matches = Regex.Matches(content, Pattern);
            var result = new List<Time>();
            if (matches.Count == 0)
                return result;
            foreach (Match match in matches)
            {
                string m1 = match.Groups[1].Value;
                string m2 = match.Groups[2].Value;

                var dt0 = DateTime.ParseExact(m1, ParseExactPattern, CultureInfo.CurrentCulture);
                var dt1 = DateTime.ParseExact(m2, ParseExactPattern, CultureInfo.CurrentCulture);

                Time time = new Time
                {
                    TimeBegin = dt0,
                    TimeEnd = dt1
                };

                result.Add(time);
            }

            return result;
        }

        private void AdjustTimesBySeconds(ref IEnumerable<Time> times, int seconds)
        {
            foreach (Time time in times)
            {
                time.TimeBegin = time.TimeBegin.AddSeconds(seconds);
                time.TimeEnd = time.TimeEnd.AddSeconds(seconds);
            }
        }

        private void WriteChangesToFile(string pathToSubtitle, IEnumerable<Time> times)
        {
            string text = File.ReadAllText(pathToSubtitle, Encoding.GetEncoding("windows-1255"));
            var matches = Regex.Matches(text, Pattern);
            for (int i = 0; i < matches.Count; i++)
            {
                Time time = times.ElementAt(i);
                text = text.Replace(
                    matches[i].Value,
                    $"{time.TimeBegin.ToString(ParseExactPattern)} --> {time.TimeEnd.ToString(ParseExactPattern)}"
                );
            }

            File.WriteAllText($"new_{pathToSubtitle}", text, Encoding.GetEncoding("windows-1255"));
        }
    }
}