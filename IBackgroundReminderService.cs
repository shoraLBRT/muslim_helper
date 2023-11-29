namespace muslim_helper
{
    internal interface IBackgroundReminderService
    {
        async Task ExecuteAsync(CancellationToken stoppingToken) { }
    }
}
