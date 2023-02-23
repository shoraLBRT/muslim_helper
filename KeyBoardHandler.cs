using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace muslim_helper
{
    internal class KeyBoardHandler
    {
        public async Task MainKeyBoard(ITelegramBotClient botClient, Message msg)
        {
            ReplyKeyboardMarkup keyboardMarkup = new(new[]
            {
                    new KeyboardButton[] {"Время намазов на сегодня","Ближайший намаз", "Время конкретного намаза"},
                    new KeyboardButton[] {"Напоминания о намазах", "Аят дня", "Обновить аяты"}
                })
            {
                ResizeKeyboard = true
            };
            await botClient.SendTextMessageAsync(msg.Chat.Id, "Что делаем дальше?", replyMarkup: keyboardMarkup);

            return;
        }
        public async Task NamazesKeyBoard(ITelegramBotClient botClient, Message msg)
        {
            ReplyKeyboardMarkup keyboard = new ReplyKeyboardMarkup(new[]
                {
                    new KeyboardButton[] { "Фаджр 🌅", "Восход 🌄", "Зухр 🏙" },
                    new KeyboardButton[] { "Аср 🌁", "Магриб 🌇", "Иша 🌃" },
                    new KeyboardButton[] { "Вернуться 🔙" }
                })
            {
                ResizeKeyboard = true
            };
            await botClient.SendTextMessageAsync(msg.Chat.Id, "Выбери необходимый: ", replyMarkup: keyboard);
        }

    }
}
