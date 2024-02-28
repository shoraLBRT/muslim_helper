using System.Configuration;
using muslim_helper.Reminder;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace muslim_helper
{
    internal class TelegramBotConnecter
    {
        private static ITelegramBotClient bot = new TelegramBotClient(ConfigurationManager.ConnectionStrings["telegramBotToken"].ConnectionString);

        BotErrorHandler botErrorHandler;
        BotUpdateHandler botUpdateHandler;
        ResponseHandler responseHandler;
        BackgroundReminderLauncher backgroundReminderLauncher;
        public TelegramBotConnecter(BotUpdateHandler botUpdateHandler, BotErrorHandler botErrorHandler, ResponseHandler responseHandler, BackgroundReminderLauncher backgroundReminderLauncher)
        {
            this.botUpdateHandler = botUpdateHandler;
            this.botErrorHandler = botErrorHandler;
            this.responseHandler = responseHandler;
            responseHandler.ProvideBotClient(bot);
            this.backgroundReminderLauncher = backgroundReminderLauncher;
            backgroundReminderLauncher.ReminderForCloseNamaz();
            TelegramBotConneceting();
        }

        private async Task TelegramBotConneceting()
        {
            Console.WriteLine("запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var recieverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };

            bot.StartReceiving(
                botUpdateHandler.HandleUpdateAsync,
                botErrorHandler.HandlerErrorAsync,
                recieverOptions,
                cancellationToken
                );
            Console.ReadLine();
        }

        public ITelegramBotClient GetBotClient()
        {
            return bot;
        }
    }
}
