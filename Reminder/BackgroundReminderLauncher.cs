using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace muslim_helper.Reminder
{
    public class BackgroundReminderLauncher
    {
        public async Task ReminderForCloseNamaz()
        {
            IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<BackgroundReminderService>();
                    services.AddSingleton<IBackgroundReminderService, BackgroundReminderService>();
                })
                .Build();


            await host.RunAsync();
        }

    }
}