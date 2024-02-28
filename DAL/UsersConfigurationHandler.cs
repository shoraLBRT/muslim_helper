using muslim_helper.Entites;

namespace muslim_helper.DAL
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
