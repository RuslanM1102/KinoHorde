using System.Threading.Tasks;
using KinopoiskParser;
using System;


namespace ConsoleDebug
{
    public partial class Program
    {
        static void Main(string[] args)
        {
            //string result = await TestParser.GetMovies("Техасская резня");
            Console.WriteLine(SeleniumParser.GetPage().GetAttribute("innerHTML"));
        }
    }
}