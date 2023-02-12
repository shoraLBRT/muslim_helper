using System.ComponentModel;
using System.Xml.XPath;

namespace muslim_helper
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            TelegramBotConnecter botConnecter = new();
            NamazTimesParsing timesParsing = new();
            AyatParsingHandler ayatParsing = new();


            await ayatParsing.FormAyatDictionary();
            await timesParsing.DoTheParsing();
            await botConnecter.TelegramBotConneceting();


        }
    }
}