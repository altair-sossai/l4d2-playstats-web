using L4D2PlayStats.Core.GameInfo.Services;

namespace L4D2PlayStats.Web.BackgroundServices;

public class PlayerConnectionInfoBackgroundService(IPlayerConnectionInfoService playerConnectionInfoService, ILogger<PlayerConnectionInfoBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
            try
            {
                var now = DateTimeOffset.UtcNow;
                var nextExecution = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, TimeSpan.Zero).AddDays(1);

                await Task.Delay(nextExecution - now, stoppingToken);

                var expirationDate = DateTimeOffset.UtcNow.AddMonths(-6);
                await playerConnectionInfoService.DeleteExpiredAsync(expirationDate, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred while deleting expired player connection information.");
            }
    }
}