using SubTime.Contracts;
using SubTime.Subtitles;
using System.Collections.Generic;
using System.IO;

namespace SubTime.Factories
{
    public class SubtitleFactory
    {
        private readonly List<ISubtitle> _subtitles;

        public SubtitleFactory()
        {
            _subtitles = new List<ISubtitle>
            {
                new Subtitle0(),
                new Subtitle1(),
                new Subtitle2()
            };
        }

        public ISubtitle GetSubtitle(string fileName)
        {
            var content = File.ReadAllText(fileName);

            foreach (ISubtitle subtitle in _subtitles)
            {
                if (subtitle.IsCompatible(content))
                {
                    return subtitle;
                }
            }

            return null;
        }
    }
}
