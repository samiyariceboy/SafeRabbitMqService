namespace Test_HealthHub_Api
{
    public class PublisherState
    {
        public bool IsEnabled { get; private set; } = true;

        public void Start() => IsEnabled = true;
        public void Stop() => IsEnabled = false;
    }
}
