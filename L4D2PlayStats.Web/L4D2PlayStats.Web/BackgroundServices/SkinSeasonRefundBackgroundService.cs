using L4D2PlayStats.Core.Skins;
using L4D2PlayStats.Core.Skins.Services;

namespace L4D2PlayStats.Web.BackgroundServices;

public class SkinSeasonRefundBackgroundService(ISkinShopService skinShopService, ILogger<SkinSeasonRefundBackgroundService> logger) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(10);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await skinShopService.RefundSeasonAsync(SkinSeason.Current.Start, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred while refunding the skin purchases of the previous ranking season.");
            }

            try
            {
                await Task.Delay(Interval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }
}