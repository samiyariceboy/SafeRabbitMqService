using Taday.Corelibrary.Common.Shared;

namespace Test_HealthHub_Api
{
    public class RabbitMqOption : RabbitMq
    {
        public string Route1 { get; set; } = default!;
        public string Route2 { get; set; } = default!;
        public string Route3 { get; set; } = default!;
    }
}