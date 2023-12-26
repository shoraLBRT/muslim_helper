using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkerService1;

namespace muslim_helper
{
    internal class ReminderHandler
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
