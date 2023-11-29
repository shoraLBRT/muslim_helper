namespace muslim_helper
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            TelegramBotConnecter botConnecter = new();

            NamazTimesParsing timesParsing = new();
            AyatParsingHandler ayatParsing = new();
            ReminderHandler reminder = new();

            await ayatParsing.FormAyatDictionary();
            Console.WriteLine(await timesParsing.DoTheParsing());
            reminder.ReminderForCloseNamaz();
            await botConnecter.TelegramBotConneceting();
        }

    }
}