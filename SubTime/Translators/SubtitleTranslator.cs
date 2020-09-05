using SubTime.Contracts;

namespace SubTime.Translators
{
    public class SubtitleTranslator : ITranslator
    {
        public SubtitleTranslator()
        {
            Pattern = "(\\d{2}:\\d{2}:\\d{2},\\d{3}) --> (\\d{2}:\\d{2}:\\d{2},\\d{3})";
            ParseExactPattern = "HH:mm:ss,fff";
        }
    }
}
