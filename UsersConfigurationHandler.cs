using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace muslim_helper
{
    internal class UsersConfigurationHandler
    {
        MuslimHelperDBContext dbContext;
        public UsersConfigurationHandler(MuslimHelperDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void AddUserIntoDB(string username, string firstname, long chatid)
        {
            if (dbContext.UsersTables.Any(p => p.Chatid == (int)chatid))
                Console.WriteLine("такой пользователь уже имеется");
            else
            {
                dbContext.UsersTables.Add(new UsersTable { Username = username, FirstName = firstname, Chatid = (int)chatid });
                dbContext.SaveChanges();
                Console.WriteLine("данные о пользователе загружены в базу данных");
            }
        }
    }
}
