using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace muslim_helper
{
    internal class InlineKeyboardHandler
    {
        public async Task HandleInlineKeyBoard(ITelegramBotClient botClient, Message msg)
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
{
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Фаджр 🌅", callbackData : "callbackButton1"),
                            InlineKeyboardButton.WithCallbackData("Восход 🌄", callbackData : "callbackButton2"),
                            InlineKeyboardButton.WithCallbackData("Зухр 🏙", callbackData : "callbackButton3")
                        }, new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Аср 🌁", callbackData: "callbackButton4"),
                            InlineKeyboardButton.WithCallbackData("Магриб 🌇", callbackData: "callbackButton5"),
                            InlineKeyboardButton.WithCallbackData("Иша 🌃", callbackData: "callbackButton6")
                        }
                    });
            await botClient.SendTextMessageAsync(msg.Chat.Id, "Выберите интересующий вас намаз", replyMarkup: inlineKeyboard);
        }
        public async Task HandleCallBackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, $"Ты выбрал: {callbackQuery.Data} \n К сожалению функционал напоминаний еще не готов, однако в скором времени всё будет доработано");
            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Нажата кнопка " + callbackQuery.Data);
            return;
        }
    }
}
