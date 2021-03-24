using SubTime.Contracts;
using SubTime.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SubTime.Subtitles
{
    public class Subtitle2 : CommonUtilities, ISubtitle
    {
        public string Pattern => "{(\\d+)}{(\\d+)}";

        public string ParseExactPattern => "FFFFFF";

        public bool IsCompatible(string content)
        {
            return Regex.Match(content, Pattern).Success;
        }

        public void SyncTime(string fileName, int timeBySeconds)
        {
            var timeList = ReadTimeListByPattern(fileName, Pattern, ParseExactPattern);
            AdjustDurationListByTime(ref timeList, timeBySeconds);
            WriteTimeListToFileByPattern(fileName, timeList, Pattern, ParseExactPattern);
        }

        protected override List<Duration> ReadTimeListByPattern(string filePath, string pattern, string parseExactPattern)
        {
            string content = File.ReadAllText(filePath);
            var matches = Regex.Matches(content, pattern);
            var result = new List<Duration>();

            if (matches.Count == 0)
                return result;

            foreach (Match match in matches)
            {
                string m1 = match.Groups[1].Value;
                string m2 = match.Groups[2].Value;

                double d1 = double.Parse(m1);
                double d2 = double.Parse(m2);

                var dt0 = TimeSpan.FromMilliseconds(d1);
                var dt1 = TimeSpan.FromMilliseconds(d2);

                Duration time = new Duration
                {
                    TimeBegin = dt0,
                    TimeEnd = dt1
                };

                result.Add(time);
            }

            return result;
        }

        protected override void WriteTimeListToFileByPattern(string filePath, List<Duration> times, string pattern, string format)
        {
            string text = File.ReadAllText(filePath, Encoding.GetEncoding("windows-1255"));
            var matches = Regex.Matches(text, pattern);

            for (int i = 0; i < matches.Count; i++)
            {
                Duration time = times.ElementAt(i);
                text = text.Replace(
                    matches[i].Value,
                    $"{{{time.TimeBegin.TotalMilliseconds}}}{{{time.TimeEnd.TotalMilliseconds}}}"
                );
            }

            File.WriteAllText($"new_{filePath}", text, Encoding.GetEncoding("windows-1255"));
        }
    }
}
