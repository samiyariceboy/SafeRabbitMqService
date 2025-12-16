using Test_HealthHub_Api.Dtos;

namespace Test_HealthHub_Api
{
    public interface IRabbitMqService
    {
        public void Start(CancellationToken stoppingToken);
        Task PublishRandomMessage(RandomMessage message, CancellationToken cancellationToken);
    }
}
