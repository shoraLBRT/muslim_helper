using Newtonsoft.Json;
using Telegram.Bot;

namespace muslim_helper
{
    internal class BotErrorHandler
    {
        public async Task HandlerErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            // someText
            Console.WriteLine(JsonConvert.SerializeObject(exception));
        }
    }
}
