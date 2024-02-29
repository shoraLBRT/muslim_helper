using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace muslim_helper.Keyboards
{
    internal class KeyBoardHandler
    {
        public async Task MainKeyBoard(ITelegramBotClient botClient, Message msg)
        {
            ReplyKeyboardMarkup keyboardMarkup = new(new[]
            {
                    ["Намаз совершен","Случайный аят"],
                    ["Время намазов","Ближайший намаз"],
                    ["Отключить напоминания о намазах"],
                    new KeyboardButton[] {"Установить напоминания о намазах"}
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
                    ["Аср 🌁", "Магриб 🌇", "Иша 🌃"],
                    ["Вернуться 🔙"]
                })
            {
                ResizeKeyboard = true
            };
            await botClient.SendTextMessageAsync(msg.Chat.Id, "Выбери необходимый: ", replyMarkup: keyboard);
        }
    }
}
