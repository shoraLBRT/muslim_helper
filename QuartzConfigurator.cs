//using Microsoft.Extensions.Logging.Console;
//using Quartz;
//using Quartz.Impl;
//using Quartz.Logging;
//using Telegram.Bot;

//namespace muslim_helper
//{
//    internal class QuartzConfigurator
//    {
//        TimeSpan timeOffset;

//        ClosestNamazFinder closestNamazFinder = new();
//        ReminderHandler reminderHandler = new();

//        public async Task RemindStarter()
//        {
//            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

//            StdSchedulerFactory factory = new StdSchedulerFactory();

//            IScheduler scheduler = await factory.GetScheduler();

//            await scheduler.Start();

//            IJobDetail job = JobBuilder.Create<HelloJob>().WithIdentity("job1", "group1").Build();

//            ITrigger trigger = TriggerBuilder.Create()
//                .WithIdentity("trigger1", "group1")
//                .StartNow()
//                .WithSimpleSchedule(x =>x.WithInterval(closestNamazFinder.TestTimeSpanGetter().Result).WithRepeatCount(1))
//                .Build();

//            await scheduler.ScheduleJob(job, trigger);
//            await Task.Delay(TimeSpan.FromSeconds(60));

//            await scheduler.Shutdown();

//            Console.WriteLine("Press Any Key");
//            Console.ReadKey();
//        }
//        private TimeSpan TimeOffsetToClosestNamaz()
//        {
//            timeOffset = closestNamazFinder.GetTimeOffsetToClosestNamaz().Result.ToTimeSpan();
//            return timeOffset;
//        }
//    }

//    internal class ConsoleLogProvider : ILogProvider
//    {
//        public Logger GetLogger(string name)
//        {
//            return (level, func, exception, parameters) =>
//                {
//                    if (level >= LogLevel.Info && func != null)
//                    {
//                        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
//                    }
//                    return true;
//                };
//        }

//        IDisposable ILogProvider.OpenMappedContext(string key, object value, bool destructure)
//        {
//            throw new NotImplementedException();
//        }

//        IDisposable ILogProvider.OpenNestedContext(string message)
//        {
//            throw new NotImplementedException();
//        }
//    }
//    public class HelloJob : IJob
//    {
//        ReminderHandler reminderHandler = new();
//        public async Task Execute(IJobExecutionContext context)
//        {
//            await reminderHandler.HandleMessage();
//        }
//    }
//}