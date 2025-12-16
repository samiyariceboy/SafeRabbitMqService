
using Test_HealthHub_Api.Dtos;
using Taday.Corelibrary.Common.Shared;

namespace Test_HealthHub_Api
{
    public class PublisherService
        (
            PublisherState state,
            IRabbitMqService rabbitMqService,
            IAppLogger<PublisherService> logger) 
        : BackgroundService
    {
        private const int BatchSize = 50;
        private static readonly TimeSpan BatchDelay = TimeSpan.FromSeconds(1);
        private static readonly TimeSpan Interval = TimeSpan.FromSeconds(25);

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            rabbitMqService.Start(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!state.IsEnabled)
                {
                    await Task.Delay(500, stoppingToken);
                    continue;
                }

                for (int i = 1; i <= BatchSize; i++)
                {
                    if (!state.IsEnabled)
                    {
                        break;
                    }

                    var message = new RandomMessage
                    {
                        Id = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow
                    };

                    await rabbitMqService.PublishRandomMessage(
                        message, cancellationToken: stoppingToken
                    );

                    logger.Log(LogLevel.Warning, "message sent successfully with message: {message} and Number: {number}", nameof(ExecuteAsync),
                        message.Id, i);

                    if (i < BatchSize)
                    {
                        await Task.Delay(BatchDelay, stoppingToken);
                        continue;
                    }

                }

                logger.Log(LogLevel.Error, "Wait send message {Interval} second Delay!", nameof(ExecuteAsync),
                        Interval);

                await Task.Delay(Interval, stoppingToken);
            }
        }
    }
}