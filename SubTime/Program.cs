using System;
using SubTime.Contracts;
using SubTime.Factories;

namespace SubTime
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = args[0];
            int seconds = Convert.ToInt32(args[1]);

            //string fileName = "Dorm.Daze.2003.1080p.BluRay.srt";
            //int seconds = 18;

            var factory = new TranslatorFactory();
            ITranslator translator = factory.GetTranslator(fileName);
            translator?.Translate(fileName, seconds);
        }
    }
}
