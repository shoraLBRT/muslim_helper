using Telegram.Bot;
using Telegram.Bot.Types;

namespace muslim_helper
{
    internal class IncomingMessageHandler
    {
        KeyBoardHandler keyBoardHandler;
        InlineKeyboardHandler inlineKeyboard;
        NamazTimesData namazTimes;
        AyatParsingHandler ayatParsing;
        ClosestNamazFinder closestNamazFinder;
        DataBase dataBase;
        public IncomingMessageHandler(KeyBoardHandler keyBoardHandler, InlineKeyboardHandler inlineKeyboard,
            AyatParsingHandler ayatParsing, ClosestNamazFinder closestNamazFinder, DataBase dataBase, NamazTimesData namazTimes)
        {
            this.keyBoardHandler = keyBoardHandler;
            this.inlineKeyboard = inlineKeyboard;
            this.ayatParsing = ayatParsing;
            this.namazTimes = namazTimes;
            this.closestNamazFinder = closestNamazFinder;
            this.dataBase = dataBase;
        }

        Random numberOfAyat = new Random();

        public static async Task BotAnswer(ITelegramBotClient botClient, long chatID, string answerText)
        {
            if (chatID > 0)
                await botClient.SendTextMessageAsync(chatID, answerText);
        }
        public async Task HandleMessage(ITelegramBotClient botClient, Update update)
        {
            var msg = update.Message;
            if (msg.Text != null)
            {
                string messageText = msg.Text.ToLower();
                int num = numberOfAyat.Next(0, 7450);

                switch (messageText)
                {
                    case "/start":
                        await botClient.SendTextMessageAsync(msg.Chat, "Здравствуй! Бот преназначен для небольшой помощи мусульманам с выполнением своих основных обязанностей.\n" +
                        "для получения информации о возможностях бота, отправь мне команду /info");
                        await botClient.SendTextMessageAsync(msg.Chat, "Так же можно воспользоваться клавиатурой /keyboard");
                        dataBase.AddUserIntoDB(msg.Chat.Username, msg.Chat.FirstName, msg.Chat.Id);
                        break;
                    case "/info":
                        await botClient.SendTextMessageAsync(msg.Chat,
                            "/start - для начала работы с ботом \n" +
                            "/info - для получения информации о командах бота \n" +
                            "/keyboard - для открытия клавиатуры управления ботом \n" +
                            "/namaztimes или упоминание в любом тексте слов <время намаза> - предоставит обновлённое время всех намазов \n" +
                            "/setreminder - поставить напоминание на ближайший намаз");
                        break;
                    case "ближайший намаз":
                        await botClient.SendTextMessageAsync(msg.Chat, await closestNamazFinder.GetInfoAboutClosestNamaz());
                        break;
                    case "время намазов" or "/namaztimes":
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
                    case "случайный аят":
                        await botClient.SendTextMessageAsync(msg.Chat, "Случайный аят из Корана\n➖➖➖➖➖➖➖\n☪" + await ayatParsing.GetAyatFromNumber(num) + "\n➖➖➖➖➖➖➖\n" + num);
                        break;
                    case "обновить аяты":
                        await botClient.SendTextMessageAsync(msg.Chat, "Аяты успешно обновлены");
                        break;
                    case "вернуться 🔙":
                        await keyBoardHandler.MainKeyBoard(botClient, msg);
                        break;
                    case "/keyboard":
                        await keyBoardHandler.MainKeyBoard(botClient, msg);
                        break;
                    case "намаз совершен":
                        await keyBoardHandler.NamazesKeyBoard(botClient, msg);
                        break;
                    case "напоминания о намазах":
                        await inlineKeyboard.HandleInlineKeyBoard(botClient, msg);
                        break;
                    case "тест":
                        await botClient.SendTextMessageAsync(msg.Chat, closestNamazFinder.GetTimeOffsetToClosestNamaz().Result.ToString());
                        break;
                    case "/setreminder" or "установить напоминания о намазах":
                        dataBase.SetNamazNotificationForUser(msg.Chat.Id, true);
                        await botClient.SendTextMessageAsync(msg.Chat, "Уведомления о намазах <b>активированы</b> ✅", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                        await botClient.SendTextMessageAsync(msg.Chat, await closestNamazFinder.GetInfoAboutClosestNamaz());
                        break;
                    case "/offreminder" or "отключить напоминания о намазах":
                        dataBase.SetNamazNotificationForUser(msg.Chat.Id, false);
                        await botClient.SendTextMessageAsync(msg.Chat, "Уведомления о намазах <b>отключены</b>.❌", parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                        break;
                }
            }
            return;
        }
    }
}
