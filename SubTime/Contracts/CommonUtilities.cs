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
    public abstract class CommonUtilities
    {
        protected virtual List<Duration> ReadTimeListByPattern(string filePath, string pattern, string parseExactPattern)
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

                var dt0 = DateTime.ParseExact(m1, parseExactPattern, CultureInfo.CurrentCulture);
                var dt1 = DateTime.ParseExact(m2, parseExactPattern, CultureInfo.CurrentCulture);

                Duration time = new Duration
                {
                    TimeBegin = dt0,
                    TimeEnd = dt1
                };

                result.Add(time);
            }

            return result;
        }

        protected virtual void AdjustDurationListByTime(ref List<Duration> durationList, int seconds)
        {
            foreach (Duration duration in durationList)
            {
                var time = TimeSpan.FromSeconds(seconds);

                duration.TimeBegin = duration.TimeBegin.Add(time);
                duration.TimeEnd = duration.TimeEnd.Add(time);
            }
        }

        protected virtual void WriteTimeListToFileByPattern(string filePath, List<Duration> times, string pattern, string format)
        {
            string text = File.ReadAllText(filePath, Encoding.GetEncoding("windows-1255"));
            var matches = Regex.Matches(text, pattern);

            for (int i = 0; i < matches.Count; i++)
            {
                Duration time = times.ElementAt(i);
                text = text.Replace(
                    matches[i].Value,
                    $"{time.TimeBegin.ToString(format)} --> {time.TimeEnd.ToString(format)}"
                );
            }

            File.WriteAllText($"new_{filePath}", text, Encoding.GetEncoding("windows-1255"));
        }
    }
}
