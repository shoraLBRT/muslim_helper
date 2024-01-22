using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace muslim_helper
{
    interface IResponsible
    {
        Task SendResponse(long chatid, string answerText, ParseMode? parseMode = default);
    }
    internal class ResponseHandler
    {
        private static ITelegramBotClient _botClient;
        public void ProvideBotClient(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }
        public async Task SendSimpleMessage(long chatID, string answerText, ParseMode? parseMode = default)
        {
            if (chatID > 0)
                await _botClient.SendTextMessageAsync(chatID, answerText, parseMode );
        }
    }
}