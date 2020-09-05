using SubTime.Contracts;
using SubTime.Translators;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SubTime.Factories
{
    public class TranslatorFactory
    {
        private readonly List<ITranslator> translators;

        public TranslatorFactory()
        {
            translators = new List<ITranslator>
            {
                new SubtitleTranslator(),
                new SubtitleTranslator2()
            };
        }

        public ITranslator GetTranslator(string fileName)
        {
            string key = File.ReadAllLines(fileName)[1];

            foreach (ITranslator translator in translators)
            {
                if (Regex.IsMatch(key, translator.Pattern))
                {
                    return translator;
                }
            }

            return null;
        }
    }
}
