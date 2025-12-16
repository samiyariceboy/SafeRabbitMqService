using RabbitMQ.Client;
using Test_HealthHub_Api.Dtos;
using Taday.Corelibrary.Common.Shared;
using Taday.Corelibrary.Infrastucture.Services;

namespace Test_HealthHub_Api
{
    public class RabbitMqService(IAppLogger<RabbitMqBaseService> logger,
          ConnectionFactory connectionFactory, RabbitMqOption options)
        : RabbitMqBaseService(logger, connectionFactory, options), IRabbitMqService
    {
        private readonly RabbitMqOption config = options;

        protected async override Task Subscribe()
        {
            await SubscribeQueue(config.Route1, ConsumerHandler);
        }

        public Task PublishRandomMessage(RandomMessage message, CancellationToken cancellationToken)
        {
            return PublishAsync(message, config.Route1);
        }

        public async Task<bool> ConsumerHandler(byte[] data)
        {
            try
            {
                await Task.Delay(1500);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}