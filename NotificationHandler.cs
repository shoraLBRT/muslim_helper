using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace muslim_helper
{
    internal class NotificationHandler
    {
        MuslimHelperDBContext dbContext;
        public NotificationHandler(MuslimHelperDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void SetNotification(long userID, bool state)
        {
            UsersTable? user = dbContext.UsersTables.Where(p=>p.Chatid == userID).FirstOrDefault();
            user!.NamazNotification = state;
            dbContext.Update(user);
            Console.WriteLine(@$"для юзера {user.Username} с chatID - {user.Chatid}, переменная напоминаний установлена как {state} ");
        }
        //public void SetNamazNotificationForUser(long chatid, bool state)
        //{
        //    openConnection();
        //    SqlCommand setNamazNotification = new($@"UPDATE users_table SET namaz_notification = '{state}' WHERE chatid = '{chatid}'", sqlConnection);
        //    setNamazNotification.ExecuteNonQuery();
        //    closeConnection();
        //}
    }
}
