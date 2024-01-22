using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using muslim_helper;

namespace WorkerService1
{
    internal class BackgroundReminderService : BackgroundService, IBackgroundReminderService
    {
        private readonly ILogger<BackgroundReminderService> _logger;
        private ClosestNamazFinder namazFinder;
        NotificationHandler notificationHandler;

        public BackgroundReminderService(ILogger<BackgroundReminderService> logger, ClosestNamazFinder namazFinder, NotificationHandler notificationHandler)
        {
            _logger = logger;
            this.namazFinder = namazFinder;
            this.notificationHandler = notificationHandler;
        }

        private KeyValuePair<string, TimeOnly> _closestNamaz;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            TimeOnly currentTime;
            bool reminder60done = false;
            bool reminder15done = false;
            bool reminder1done = false;
            await Console.Out.WriteLineAsync("reminder executed");

            using (var timer = new PeriodicTimer(TimeSpan.FromSeconds(10)))
            {
                await Console.Out.WriteLineAsync("Обновление таймера");
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    currentTime = TimeOnly.FromDateTime(DateTime.Now);
                    var timeOffset = TimeSpan.Parse(namazFinder.GetTimeOffsetToClosestNamaz().Result.ToString());
                    _closestNamaz = await namazFinder.GetClosestNamaz();
                    if (timeOffset <= TimeSpan.FromMinutes(5))
                    {
                        if (timeOffset <= TimeSpan.FromMinutes(3))
                        {
                            await Console.Out.WriteLineAsync("<15min");
                            if (timeOffset <= TimeSpan.FromMinutes(1))
                            {
                                await Console.Out.WriteLineAsync("<1min");
                                if (!reminder1done)
                                {
                                    if (_closestNamaz.Key == "Восход")
                                        await NoticeMembers(@$"наступило время {_closestNamaz}. Ближайшие 10-15 минут - нежелательны для совершения намаза!");
                                    else
                                        await NoticeMembers(@$"наступило время намаза {_closestNamaz}. Помни, что наилучшее деяние - намаз совершенный в начале его времени!");
                                    reminder1done = true;
                                }
                            }
                            else
                            {
                                if (!reminder15done)
                                {
                                    await NoticeMembers(@$"осталось 15 минут до {_closestNamaz}. Если ты еще не совершил прошлый намаз - поспеши!");
                                    reminder15done = true;
                                }
                            }
                        }
                        if (!reminder60done)
                        {
                            await NoticeMembers(@$"осталось {timeOffset.Minutes} минут(а) до {_closestNamaz}.");
                            reminder60done = true;
                        }

                    }
                    else if (reminder15done || reminder60done || reminder1done)
                    {
                        reminder60done = false; reminder15done = false; reminder1done = false;
                    }
                    if (currentTime > _closestNamaz.Value && _closestNamaz.Key != "Фаджр")
                    {
                        await Console.Out.WriteLineAsync("время намаза вышло");
                        reminder15done = false;
                        reminder60done = false;
                        reminder1done = false;
                    }
                    if (currentTime > _closestNamaz.Value && _closestNamaz.Key == "Фаджр")
                    {
                        await Console.Out.WriteLineAsync("следующий намаз Фаджр");
                    }
                }
            }
        }
        private async Task NoticeMembers(string text)
        {
            await notificationHandler.SendNotificationToMembersAsync(text);
        }
    }
}