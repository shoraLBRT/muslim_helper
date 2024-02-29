using muslim_helper.AyahOfTheDay;
using muslim_helper.DAL;
using muslim_helper.Keyboards;
using muslim_helper.NamazTimes;
using muslim_helper.Notifications;
using muslim_helper.TaskTracking;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace muslim_helper
{
    internal class IncomingMessageHandler : IResponsible
    {
        KeyBoardHandler keyBoardHandler;
        NamazTimesData namazTimes;
        AyatParsingHandler ayatParsing;
        ClosestNamazFinder closestNamazFinder;
        TaskTrackingHandler taskTrackingHandler;
        NotificationHandler notificationHandler;
        UsersConfigurationHandler usersConfigurationHandler;
        ResponseHandler responseHandler;
        public IncomingMessageHandler(KeyBoardHandler keyBoardHandler,
            AyatParsingHandler ayatParsing, ClosestNamazFinder closestNamazFinder, NamazTimesData namazTimes,
            TaskTrackingHandler taskTrackingHandler, NotificationHandler notificationHandler, UsersConfigurationHandler usersConfigurationHandler, 
            ResponseHandler responseHandler)
        {
            this.keyBoardHandler = keyBoardHandler;
            this.ayatParsing = ayatParsing;
            this.namazTimes = namazTimes;
            this.closestNamazFinder = closestNamazFinder;
            this.taskTrackingHandler = taskTrackingHandler;
            this.notificationHandler = notificationHandler;
            this.usersConfigurationHandler = usersConfigurationHandler;
            this.responseHandler = responseHandler;
        }

        Random numberOfAyat = new Random();

        public async Task SendResponse(long chatID, string answerText, ParseMode? parseMode = default)
        {
            await responseHandler.SendSimpleMessage(chatID, answerText, parseMode);
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
                        await SendResponse(msg.Chat.Id, "Здравствуй! Бот преназначен для небольшой помощи мусульманам с выполнением своих основных обязанностей.\n" +
                        "для получения информации о возможностях бота, отправь мне команду /info");
                        await SendResponse(msg.Chat.Id, "Так же можно воспользоваться клавиатурой /keyboard");
                        usersConfigurationHandler.AddUserIntoDB(msg.Chat.Username!, msg.Chat.FirstName!, msg.Chat.Id);
                        break;
                    case "/info":
                        await SendResponse(msg.Chat.Id,
                            "/start - для начала работы с ботом \n" +
                            "/info - для получения информации о командах бота \n" +
                            "/keyboard - для открытия клавиатуры управления ботом \n" +
                            "/namaztimes или упоминание в любом тексте слов <время намаза> - предоставит обновлённое время всех намазов \n" +
                            "/setreminder - поставить напоминание на ближайший намаз \n" +
                            "/setnamaztracking - отслеживать выполнение намазов");
                        break;
                    case "ближайший намаз":
                        await SendResponse(msg.Chat.Id, await closestNamazFinder.GetInfoAboutClosestNamaz());
                        break;
                    case "время намазов" or "/namaztimes":
                        string allNamaz = namazTimes.ShowAllNamazes();
                        await SendResponse(msg.Chat.Id, allNamaz);
                        break;
                    case "фаджр 🌅" or "/fadjr":
                        await SendResponse(msg.Chat.Id, "Время намаза Фаджр: " + namazTimes.ShowConcreteNamaz("Фаджр"));
                        break;
                    case "восход 🌄" or "/sunrise":
                        await SendResponse(msg.Chat.Id, "Время восхода: " + namazTimes.ShowConcreteNamaz("Восход"));
                        break;
                    case "зухр 🏙" or "/zuhr":
                        await SendResponse(msg.Chat.Id, "Время намаза Зухр: " + namazTimes.ShowConcreteNamaz("Зухр"));
                        break;
                    case "аср 🌁" or "/asr":
                        await SendResponse(msg.Chat.Id, "Время намаза Аср: " + namazTimes.ShowConcreteNamaz("Аср"));
                        break;
                    case "магриб 🌇" or "/magrib":
                        await SendResponse(msg.Chat.Id, "Время намаза Магриб: " + namazTimes.ShowConcreteNamaz("Магриб"));
                        break;
                    case "иша 🌃" or "/isha":
                        await SendResponse(msg.Chat.Id, "Время намаза Иша: " + namazTimes.ShowConcreteNamaz("Иша"));
                        break;
                    case "случайный аят":
                        await SendResponse(msg.Chat.Id, "Случайный аят из Корана\n➖➖➖➖➖➖➖\n☪" + await ayatParsing.GetAyatFromNumber(num) + "\n➖➖➖➖➖➖➖\n" + num);
                        break;
                    case "обновить аяты":
                        await SendResponse(msg.Chat.Id, "Аяты успешно обновлены");
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
                    case "/setreminder" or "установить напоминания о намазах":
                        notificationHandler.SetNotification(msg.Chat.Id, true);
                        await SendResponse(msg.Chat.Id, "Уведомления о намазах <b>активированы</b> ✅", parseMode: ParseMode.Html);
                        await SendResponse(msg.Chat.Id, await closestNamazFinder.GetInfoAboutClosestNamaz());
                        break;
                    case "/offreminder" or "отключить напоминания о намазах":
                        notificationHandler.SetNotification(msg.Chat.Id, false);
                        await SendResponse(msg.Chat.Id, "Уведомления о намазах <b>отключены</b>.❌", parseMode: ParseMode.Html);
                        break;
                    case "/setnamaztracking":
                        await taskTrackingHandler.ActivateTracking(msg.Chat.Id, true);
                        await SendResponse(msg.Chat.Id, closestNamazFinder.GetTimeOffsetToClosestNamaz().Result.ToString());
                        break;
                    case "/geonamaz":
                        await LocationGetter.GetLocationFromUser(botClient,update);
                        break;


                }
            }
            if (msg.Location != null)
            {
                await LocationGetter.GetLocationFromUser(botClient, update);
            }
            return;
        }
    }
}
