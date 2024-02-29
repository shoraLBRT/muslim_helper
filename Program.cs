using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using muslim_helper.AyahOfTheDay;
using muslim_helper.DAL;
using muslim_helper.Keyboards;
using muslim_helper.NamazTimes;
using muslim_helper.Notifications;
using muslim_helper.Reminder;
using muslim_helper.TaskTracking;

namespace muslim_helper
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddSingleton<TelegramBotConnecter>()
                .AddTransient<AyatParsingHandler>()
                .AddTransient<ClosestNamazFinder>()
                .AddTransient<IncomingMessageHandler>()
                .AddTransient<InlineKeyboardHandler>()
                .AddTransient<KeyBoardHandler>()
                .AddTransient<NamazTimesParsing>()
                .AddTransient<NamazTimesData>()
                .AddTransient<TaskTrackingHandler>()
                .AddTransient<NotificationHandler>()
                .AddTransient<UsersConfigurationHandler>()
                .AddTransient<ResponseHandler>()
                .AddTransient<BotErrorHandler>()
                .AddTransient<BotUpdateHandler>()
                .AddScoped<MuslimHelperDBContext>()
                .AddTransient<BackgroundReminderLauncher>()
                .AddTransient<BackgroundReminderService>()
                .AddHostedService<BackgroundReminderService>()
                .AddSingleton<IBackgroundReminderService, BackgroundReminderService>();



            using var serviceProvider = services.BuildServiceProvider();
            //BackgroundReminderLauncher backgroundReminderLauncher = serviceProvider.GetRequiredService<BackgroundReminderLauncher>();
            //await backgroundReminderLauncher.ReminderForCloseNamaz();
            TelegramBotConnecter? botConnecter = serviceProvider.GetRequiredService<TelegramBotConnecter>();

        }
    }
}