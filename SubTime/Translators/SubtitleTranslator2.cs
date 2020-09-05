using SubTime.Contracts;

namespace SubTime.Translators
{
    public class SubtitleTranslator2 : ITranslator
    {
        public SubtitleTranslator2()
        {
            Pattern = "(\\d{2}:\\d{2}:\\d{2}.\\d{3}) --> (\\d{2}:\\d{2}:\\d{2}.\\d{3})";
            ParseExactPattern = "HH:mm:ss.fff";
        }
    }
}
