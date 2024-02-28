using muslim_helper.DAL;
using muslim_helper.Entites;

namespace muslim_helper.TaskTracking
{
    internal class TaskTrackingHandler
    {
        MuslimHelperDBContext dbContext;
        public TaskTrackingHandler(MuslimHelperDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task ActivateTracking(long chatid, bool state)
        {
            UsersTable? user = dbContext.UsersTables.Where(p => p.Chatid == chatid).FirstOrDefault();
            user!.TaskTracking = state;
            await dbContext.SaveChangesAsync();
            Console.WriteLine(@$"для юзера {user.Username} с chatID - {user.Chatid}, переменная трекинга намазов установлена как {state} ");
        }
        private void TaskCreating()
        {

        }
    }
}
