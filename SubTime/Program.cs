using SubTime.Contracts;
using SubTime.Factories;

namespace SubTime
{
    class Program
    {
        static void Main(string[] args)
        {
            //string fileName = args[0];
            //int seconds = Convert.ToInt32(args[1]);

            string fileName = "Alex and Emma (2003).sub";
            int seconds = 5;

            var subtitleFactory = new SubtitleFactory();
            ISubtitle subtitle = subtitleFactory.GetSubtitle(fileName);
            subtitle?.SyncTime(fileName, seconds);
        }
    }
}
