using System;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace muslim_helper
{
    internal class IncomingMessageHandler
    {
        KeyBoardHandler keyBoardHandler = new();
        InlineKeyboardHandler inlineKeyboard = new();
        AyatParsingHandler ayatParsing = new();
        NamazTimesParsing namazTimes = new();
        Random numberOfAyat = new Random();


        public async Task HandleMessage(ITelegramBotClient botClient, Update update)
        {
            var msg = update.Message;
            string messageText = update.Message.Text.ToLower();
            int num = numberOfAyat.Next(0, 400);

            switch (messageText)
            {
                case "/start":
                    await botClient.SendTextMessageAsync(msg.Chat, "Здравствуй! Бот преназначен для небольшой помощи мусульманам с выполнением своих основных обязанностей.\n" +
                    "для получения информации о возможностях бота, отправь мне команду /info");
                    await botClient.SendTextMessageAsync(msg.Chat, "Так же можно воспользоваться клавиатурой /keyboard");
                    break;

                case "/info":
                    await botClient.SendTextMessageAsync(msg.Chat,
                        "/start - для начала работы с ботом \n" +
                        "/info - для получения информации о командах бота \n" +
                        "/keyboard - для открытия клавиатуры управления ботом \n" +
                        "/namaztimes или упоминание в любом тексте слов <время намаза> - предоставит обновлённое время всех намазов \n" +
                        "/fadjr - получение времени Фаджр намаза \n" +
                        "/sunrise - время восхода(конец времени Фаджр намаза \n" +
                        "/zuhr - время Зухр \n" +
                        "/asr - время Аср \n" +
                        "/magrib - время Магриб \n" +
                        "/isha - время Иша");
                    break;
                case "ближайший намаз":
                    Console.WriteLine(await ReminderHandler.ClosestNamaz());
                    break;
                case "время намазов на сегодня" or "/namaztimes":
                    string allNamaz = namazTimes.ShowAllNamazes();
                    await botClient.SendTextMessageAsync(msg.Chat, allNamaz);
                    break;
                case "фаджр 🌅" or "/fadjr":
                    await botClient.SendTextMessageAsync(msg.Chat, "Время намаза Фаджр: " + namazTimes.ShowConcreteNamaz("Фаджр"));
                    break;
                case "восход 🌄" or "/sunrise":
                    await botClient.SendTextMessageAsync(msg.Chat, "Время восхода: " + namazTimes.ShowConcreteNamaz("Восход"));
                    break;
                case "зухр 🏙" or "/zuhr":
                    await botClient.SendTextMessageAsync(msg.Chat, "Время намаза Зухр: " + namazTimes.ShowConcreteNamaz("Зухр"));
                    break;
                case "аср 🌁" or "/asr":
                    await botClient.SendTextMessageAsync(msg.Chat, "Время намаза Аср: " + namazTimes.ShowConcreteNamaz("Аср"));
                    break;
                case "магриб 🌇" or "/magrib":
                    await botClient.SendTextMessageAsync(msg.Chat, "Время намаза Магриб: " + namazTimes.ShowConcreteNamaz("Магриб"));
                    break;
                case "иша 🌃" or "/isha":
                    await botClient.SendTextMessageAsync(msg.Chat, "Время намаза Иша: " + namazTimes.ShowConcreteNamaz("Иша"));
                    break;
                case "аят дня":
                    await botClient.SendTextMessageAsync(msg.Chat, "Аят дня на сегодня\n➖➖➖➖➖➖➖\n☪" + await ayatParsing.GetAyatFromNumber(num) + "\n➖➖➖➖➖➖➖\n" + num);
                    break;
                case "обновить аяты":
                    await ayatParsing.FormAyatDictionary();
                    await botClient.SendTextMessageAsync(msg.Chat, "Аяты успешно обновлены");
                    break;
                case "вернуться 🔙":
                    await keyBoardHandler.MainKeyBoard(botClient, msg);
                    break;
            }
            return;
        }
        public async Task KeyboardsHandler(ITelegramBotClient botClient, Update update)
        {
            var msg = update.Message;
            string messageText = update.Message.Text.ToLower();
            if (messageText == "/keyboard")
            {
                await keyBoardHandler.MainKeyBoard(botClient, msg);
                return;
            }
            if (messageText == "время конкретного намаза")
            {
                await keyBoardHandler.NamazesKeyBoard(botClient, msg);
                return;
            }
            if (messageText.Contains("напоминания о намазах"))
            {
                await inlineKeyboard.HandleInlineKeyBoard(botClient, msg);
                return;
            }
            return;
        }
    }
}
