using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SubTime.Models;

namespace SubTime
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = args[0];
            int seconds = Convert.ToInt32(args[1]);

            var times = GetAllTimes(fileName);
            AdjustTimesBySeconds(ref times, seconds);
            WriteChangesToFile(fileName, times);
        }

        static IEnumerable<Time> GetAllTimes(string pathToSubtitle)
        {
            string content = File.ReadAllText(pathToSubtitle);
            var matches = Regex.Matches(content, "(\\d{2}:\\d{2}:\\d{2},\\d{3}) --> (\\d{2}:\\d{2}:\\d{2},\\d{3})");
            var result = new List<Time>();
            if (matches.Count == 0)
                return result;
            foreach (Match match in matches)
            {
                string m1 = match.Groups[1].Value;
                string m2 = match.Groups[2].Value;

                var dt0 = DateTime.ParseExact(m1, "HH:mm:ss,fff", CultureInfo.CurrentCulture);
                var dt1 = DateTime.ParseExact(m2, "HH:mm:ss,fff", CultureInfo.CurrentCulture);

                Time time = new Time
                {
                    TimeBegin = dt0,
                    TimeEnd = dt1
                };

                result.Add(time);
            }

            return result;
        }

        static void AdjustTimesBySeconds(ref IEnumerable<Time> times, int seconds)
        {
            foreach (Time time in times)
            {
                time.TimeBegin = time.TimeBegin.AddSeconds(seconds);
                time.TimeEnd = time.TimeEnd.AddSeconds(seconds);
            }
        }

        static void WriteChangesToFile(string pathToSubtitle, IEnumerable<Time> times)
        {
            string text = File.ReadAllText(pathToSubtitle, Encoding.GetEncoding("windows-1255"));
            var matches = Regex.Matches(text, "(\\d{2}:\\d{2}:\\d{2},\\d{3}) --> (\\d{2}:\\d{2}:\\d{2},\\d{3})");
            for (int i = 0; i < matches.Count; i++)
            {
                if (Regex.IsMatch(matches[i].Value, "(\\d{2}:\\d{2}:\\d{2},\\d{3}) --> (\\d{2}:\\d{2}:\\d{2},\\d{3})"))
                {
                    Time time = times.ElementAt(i);
                    text = text.Replace(
                        matches[i].Value, 
                        $"{time.TimeBegin.ToString("HH:mm:ss,fff")} --> {time.TimeEnd.ToString("HH:mm:ss,fff")}"
                    );
                }
            }

            File.WriteAllText($"new_{pathToSubtitle}", text, Encoding.GetEncoding("windows-1255"));
        }
    }
}
