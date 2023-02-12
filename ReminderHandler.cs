using System.Security.Cryptography;
using System.Xml.Linq;

namespace muslim_helper
{
    internal class ReminderHandler
    {
        static NamazTimesParsing namazTimes = new();
        static TimeOnly[] namazDictionaryInTimeFormat = new TimeOnly[6];
        static Dictionary<string, string> namazDictionary = namazTimes.ReturnNamazDictionary();

        public static async Task<string> ClosestNamaz()
        {
            await FormDictionaryInTimeFormat();
            TimeOnly currentTime = TimeOnly.Parse("12:00:00");
                //TimeOnly.FromDateTime(DateTime.Now);

            for (int i = 0; i < namazDictionaryInTimeFormat.Length; i++)
            {
                TimeOnly firstTime = namazDictionaryInTimeFormat[i];
                TimeOnly secondTime;
                if (i + 1 >= namazDictionaryInTimeFormat.Length)
                    secondTime = namazDictionaryInTimeFormat[1];
                else secondTime = namazDictionaryInTimeFormat[i + 1];
                if (currentTime.IsBetween(namazDictionaryInTimeFormat[i], namazDictionaryInTimeFormat[i+1]))
                {
                    Console.WriteLine($"{currentTime} - текущее время, проверяю возможность нахождения между временем {namazDictionaryInTimeFormat[i]} и {namazDictionaryInTimeFormat[i+1]} ");
                    return "Ближайший намаз - " + namazDictionaryInTimeFormat[i + 1];
                }
            }
            return "Непредвиденная ошибка в поиске ближайшего намаза";
        }
        private static Task FormDictionaryInTimeFormat()
        {
            int index = 0;
            foreach (var concreteTime in namazDictionary)
            {
                namazDictionaryInTimeFormat.SetValue(TimeOnly.Parse(concreteTime.Value), index + 1);
                Console.WriteLine(TimeOnly.Parse(concreteTime.Value));
            }
            return Task.CompletedTask;
        }
    }
}