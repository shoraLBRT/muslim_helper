using muslim_helper.Keyboards;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace muslim_helper
{
    internal class BotUpdateHandler
    {
        IncomingMessageHandler messageHandler;
        InlineKeyboardHandler inlineKeyboard;

        public BotUpdateHandler(IncomingMessageHandler messageHandler, InlineKeyboardHandler inlineKeyboard)
        {
            this.messageHandler = messageHandler;
            this.inlineKeyboard = inlineKeyboard;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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
    }
}
