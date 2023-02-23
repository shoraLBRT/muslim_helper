namespace muslim_helper
{
    internal class ClosestNamazFinder
    {
        static NamazTimesParsing namazTimes = new();
        static Dictionary<string, TimeOnly> namazDictionaryInTimeFormat = new();
        static Dictionary<string, string> namazDictionary = namazTimes.ReturnNamazDictionary();

        public static async Task<string> ClosestNamaz()
        {
            namazDictionaryInTimeFormat.Clear();
            await FormDictionaryInTimeFormat();
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            bool secondTimeIsFajr = false;
            TimeOnly timeOffset;

            for (int i = 0; i < namazDictionaryInTimeFormat.Count; i++)
            {
                var firstTime = namazDictionaryInTimeFormat.ElementAt(i);
                var secondTime = namazDictionaryInTimeFormat.ElementAt(i);
                if (i + 1 >= namazDictionaryInTimeFormat.Count)
                {
                    secondTime = namazDictionaryInTimeFormat.ElementAt(0);
                    secondTimeIsFajr = true;
                }
                else secondTime = namazDictionaryInTimeFormat.ElementAt(i + 1);

                if (!secondTimeIsFajr && currentTime.IsBetween(firstTime.Value, secondTime.Value))
                {
                    timeOffset = TimeOnly.Parse(secondTime.Value.ToTimeSpan().Subtract(currentTime.ToTimeSpan()).ToString());
                }
                if (secondTimeIsFajr)
                {
                    timeOffset = TimeOnly.Parse(TimeOnly.MaxValue.ToTimeSpan().Subtract(currentTime.ToTimeSpan()).Add(secondTime.Value.ToTimeSpan()).ToString());
                }
                if (timeOffset != TimeOnly.MinValue)
                    return $"{currentTime} - текущее время, сейчас время намаза {firstTime} \n" +
                        $"Ближайший намаз - {secondTime} до него осталось {timeOffset.Hour} часов {timeOffset.Minute} минут";
            }
            return "Непредвиденная ошибка в поиске ближайшего намаза";
        }
        private static Task FormDictionaryInTimeFormat()
        {
            foreach (var concreteTime in namazDictionary)
            {
                namazDictionaryInTimeFormat.Add(concreteTime.Key, TimeOnly.Parse(concreteTime.Value));
            }
            return Task.CompletedTask;
        }
    }
}
