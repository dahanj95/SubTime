using SubTime.Contracts;
using System.Text.RegularExpressions;

namespace SubTime.Subtitles
{
    public class Subtitle1 : CommonUtilities, ISubtitle
    {
        public string Pattern { get; } = "(\\d{2}:\\d{2}:\\d{2}.\\d{3}) --> (\\d{2}:\\d{2}:\\d{2}.\\d{3})";
        public string ParseExactPattern { get; } = "hh\\:mm\\:ss.fff";

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
    }
}
