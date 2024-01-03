using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace muslim_helper
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
