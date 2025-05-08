using GPX_Analytic.Lib;

namespace GPX_Analytic.Exe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string pathToDirectory = @""; //enter the directory path here

            var trackInfos = GpxProcessor.GetTrackInfosFromDirectory(pathToDirectory);

            foreach (var track in trackInfos)
            {
                Console.WriteLine(track);
            }
        }
    }
}