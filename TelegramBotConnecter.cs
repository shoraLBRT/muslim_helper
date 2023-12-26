using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace muslim_helper
{
    internal class TelegramBotConnecter
    {
        IncomingMessageHandler messageHandler;
        InlineKeyboardHandler inlineKeyboard;
        public TelegramBotConnecter(IncomingMessageHandler messageHandler, InlineKeyboardHandler inlineKeyboard)
        {
            this.messageHandler = messageHandler;
            this.inlineKeyboard = inlineKeyboard;
            TelegramBotConneceting();
        }

        private static ITelegramBotClient bot = new TelegramBotClient("5873562774:AAHe8-_x--2tVwPYSrCYfq-PTwJDodqbKFA");
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
                HandleUpdateAsync,
                HandlerErrorAsync,
                recieverOptions,
                cancellationToken
                );

            Console.ReadLine();
        }
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(JsonConvert.SerializeObject(update));

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery && update != null)
            {
                await inlineKeyboard.HandleCallBackQuery(botClient, update.CallbackQuery);
            }

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message && update.Message != null)
            {
                await messageHandler.HandleMessage(botClient, update);
            }
        }
        private static async Task HandlerErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // someText
            Console.WriteLine(JsonConvert.SerializeObject(exception));
        }

        public static async Task<ITelegramBotClient> GetBotClient()
        {
            return bot;
        }
    }
}
