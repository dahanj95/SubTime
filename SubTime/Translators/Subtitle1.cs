using SubTime.Contracts;
using SubTime.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SubTime.Translators
{
    public class Subtitle1 : CommonUtilities, ISubtitle
    {
        public string Pattern { get; } = "(\\d{2}:\\d{2}:\\d{2}.\\d{3}) --> (\\d{2}:\\d{2}:\\d{2}.\\d{3})";
        public string ParseExactPattern { get; } = "HH:mm:ss.fff";

        public bool IsCompatible(string content)
        {
            return Regex.Match(content, Pattern).Success;
        }

        public void SyncTime(string fileName, int timeBySeconds)
        {
            var timeList = ReadTimeListByPattern(fileName, Pattern, ParseExactPattern);
            AdjustDurationListByTime(ref timeList, timeBySeconds);
            WriteTimeListToFileByPattern(fileName, timeList);
        }

        private void WriteTimeListToFileByPattern(string filePath, List<Duration> times)
        {
            string text = File.ReadAllText(filePath, Encoding.GetEncoding("windows-1255"));
            var matches = Regex.Matches(text, Pattern);

            for (int i = 0; i < matches.Count; i++)
            {
                Duration time = times.ElementAt(i);
                text = text.Replace(
                    matches[i].Value,
                    $"{time.TimeBegin.ToString(ParseExactPattern)} --> {time.TimeEnd.ToString(ParseExactPattern)}"
                );
            }

            File.WriteAllText($"new_{filePath}", text, Encoding.GetEncoding("windows-1255"));
        }
    }
}
