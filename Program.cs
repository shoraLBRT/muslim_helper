using Microsoft.Extensions.DependencyInjection;

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
                .AddTransient<ReminderHandler>()
                .AddTransient<IncomingMessageHandler>()
                .AddTransient<InlineKeyboardHandler>()
                .AddTransient<KeyBoardHandler>()
                .AddTransient<DataBase>()
                .AddTransient<NamazTimesParsing>()
                .AddTransient<NamazTimesData>()
                .AddTransient<TaskTrackingHandler>()
                .AddTransient<NotificationHandler>()
                .AddTransient<UsersConfigurationHandler>()
                .AddScoped<MuslimHelperDBContext>();

            using var serviceProvider = services.BuildServiceProvider();
            TelegramBotConnecter? botConnecter = serviceProvider.GetRequiredService<TelegramBotConnecter>();
            ReminderHandler reminderHandler = serviceProvider.GetRequiredService<ReminderHandler>();
        }

    }
}