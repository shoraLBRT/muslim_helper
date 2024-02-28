namespace muslim_helper.NamazTimes
{
    internal class ClosestNamazFinder
    {
        public ClosestNamazFinder(NamazTimesData namazTimesData)
        {
            namazDictionary = namazTimesData.ReturnNamazDictionary();
        }

        static Dictionary<string, TimeOnly> namazDictionaryInTimeFormat = new();
        private Dictionary<string, string> namazDictionary;

        static TimeOnly timeOffset;

        KeyValuePair<string, TimeOnly> closestNamazTime;
        KeyValuePair<string, TimeOnly> currentNamazTime;

        bool closestNamazTimeIsFajr = false;

        TimeOnly currentTime = TimeOnly.MinValue;

        public async Task<KeyValuePair<string, TimeOnly>> GetClosestNamaz()
        {
            await FindClosestNamaz();
            return closestNamazTime;
        }
        public async Task<TimeOnly> FindClosestNamaz()
        {
            namazDictionaryInTimeFormat.Clear();
            await FormDictionaryInTimeFormat();
            currentTime = TimeOnly.FromDateTime(DateTime.Now);

            for (int i = 0; i < namazDictionaryInTimeFormat.Count; i++)
            {
                currentNamazTime = namazDictionaryInTimeFormat.ElementAt(i);
                timeOffset = TimeOnly.MinValue;

                if (i + 1 >= namazDictionaryInTimeFormat.Count)
                {
                    closestNamazTime = namazDictionaryInTimeFormat.ElementAt(0);
                    closestNamazTimeIsFajr = true;
                    return closestNamazTime.Value;
                }
                else closestNamazTime = namazDictionaryInTimeFormat.ElementAt(i + 1);

                if (!closestNamazTimeIsFajr && currentTime.IsBetween(currentNamazTime.Value, closestNamazTime.Value))
                {
                    return closestNamazTime.Value;
                }
            }
            return TimeOnly.MinValue;
        }

        public async Task<string> GetInfoAboutClosestNamaz()
        {
            await FindClosestNamaz();
            await GetTimeOffsetToClosestNamaz();
            return $@"текущее время {currentTime}, скоро {closestNamazTime}, до него осталось {timeOffset}";
        }
        public async Task<TimeOnly> GetTimeOffsetToClosestNamaz()
        {
            await FindClosestNamaz();// обновляем информацию о ближайшем намазе
            timeOffset = TimeOnly.MinValue;// выявляем возможный эксепшен передачей минимального значения

            if (!closestNamazTimeIsFajr && currentTime.IsBetween(currentNamazTime.Value, closestNamazTime.Value))
            {
                timeOffset = TimeOnly.FromTimeSpan(closestNamazTime.Value.ToTimeSpan().Subtract(currentTime.ToTimeSpan()));
            }

            // обработка варианта, если ближайший намаз - Фаджр
            if (closestNamazTimeIsFajr)
            {
                if (currentTime >= TimeOnly.Parse("16:00"))
                {
                    timeOffset = TimeOnly.Parse(TimeOnly.MaxValue
                        .Add(-currentTime.ToTimeSpan())
                        .Add(closestNamazTime.Value.ToTimeSpan())
                        .ToString());
                }
                else timeOffset = closestNamazTime.Value.Add(-currentTime.ToTimeSpan());
            }
            return timeOffset;
        }
        private Task FormDictionaryInTimeFormat()
        {
            foreach (var concreteTime in namazDictionary)
            {
                namazDictionaryInTimeFormat.Add(concreteTime.Key, TimeOnly.Parse(concreteTime.Value));
            }
            return Task.CompletedTask;
        }
    }
}
