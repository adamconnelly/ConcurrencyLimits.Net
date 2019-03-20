using ConcurrencyLimits.Net.Core;
using ConcurrencyLimits.Net.Core.Limiters;
using Moq;
using Xunit;

namespace ConcurrencyLimits.Net.Tests.Core.Limiters
{
    public class ListenerTests
    {
        [Fact]
        public void Success_ReleasesToken()
        {
            // Arrange
            var limiter = new Mock<ILimiter>();
            var listener = new Listener(limiter.Object);

            // Act
            listener.Success();

            // Assert
            limiter.Verify(l => l.Release());
        }
    }
}