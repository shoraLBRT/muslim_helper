namespace muslim_helper.Reminder
{
    internal interface IBackgroundReminderService
    {
        async Task ExecuteAsync(CancellationToken stoppingToken) { }
    }
}
