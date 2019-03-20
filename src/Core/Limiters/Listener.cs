namespace ConcurrencyLimits.Net.Core.Limiters
{
    public class Listener : IListener
    {
        private readonly ILimiter limiter;

        public Listener(ILimiter limiter)
        {
            this.limiter = limiter;
        }

        public void Success()
        {
            this.limiter.Release();
        }
    }
}