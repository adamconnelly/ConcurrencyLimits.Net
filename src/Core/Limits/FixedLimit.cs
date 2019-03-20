namespace ConcurrencyLimits.Net.Core.Limits
{
    public class FixedLimit : ILimit
    {
        public FixedLimit(int limit)
        {
            Limit = limit;
        }

        public int Limit { get; }
    }
}