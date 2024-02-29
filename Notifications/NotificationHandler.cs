using muslim_helper.DAL;
using muslim_helper.Entites;
using Telegram.Bot.Types.Enums;

namespace muslim_helper.Notifications
{
    internal class NotificationHandler : IResponsible
    {
        MuslimHelperDBContext dbContext;
        ResponseHandler responseHandler;
        public NotificationHandler(MuslimHelperDBContext dbContext, ResponseHandler responseHandler)
        {
            this.dbContext = dbContext;
            this.responseHandler = responseHandler;
        }

        public void SetNotification(long userID, bool state)
        {
            UsersTable? user = dbContext.UsersTables.Where(p => p.Chatid == userID).FirstOrDefault();
            user!.NamazNotification = state;
            dbContext.Update(user);
            Console.WriteLine(@$"для юзера {user.Username} с chatID - {user.Chatid}, переменная напоминаний установлена как {state} ");
        }
        public async Task SendNotificationToMembersAsync(string answerText)
        {
            var mustNotified = dbContext.UsersTables.Where(u => u.NamazNotification == true);
            foreach (var member in mustNotified)
            {
                await SendResponse(member.Chatid, answerText);
            }
        }
        public async Task SendResponse(long chatId, string answerText, ParseMode? parseMode = default)
        {
            await responseHandler.SendSimpleMessage(chatId, answerText);
        }
    }
}
