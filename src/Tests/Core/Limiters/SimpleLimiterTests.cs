using ConcurrencyLimits.Net.Core.Limiters;
using ConcurrencyLimits.Net.Core.Limits;
using Xunit;

namespace ConcurrencyLimits.Net.Tests.Core.Limiters
{
    public class SimpleLimiterTests
    {
        [Fact]
        public void Inflight_IsInitializedToZero()
        {
            // Arrange
            var limiter = new SimpleLimiter(new FixedLimit(10));

            // Act
            var inFlight = limiter.InFlight;

            // Assert
            Assert.Equal(0, inFlight);
        }

        [Fact]
        public void Acquire_IncrementsInFlightCount()
        {
            // Arrange
            var limiter = new SimpleLimiter(new FixedLimit(10));

            // Act
            limiter.Acquire();
            limiter.Acquire();

            // Assert
            Assert.Equal(2, limiter.InFlight);
        }

        [Fact]
        public void Acquire_ReturnsTrueWhenLimitHasNotBeenReached()
        {
            // Arrange
            var limiter = new SimpleLimiter(new FixedLimit(10));

            // Act
            var (canProceed, listener) = limiter.Acquire();

            // Assert
            Assert.True(canProceed);
        }

        [Fact]
        public void Acquire_ReturnsFalseWhenLimitHasBeenReached()
        {
            // Arrange
            var limiter = new SimpleLimiter(new FixedLimit(2));

            // Act
            limiter.Acquire();
            limiter.Acquire();
            var (canProceed, listener) = limiter.Acquire();

            // Assert
            Assert.False(canProceed);
        }

        [Fact]
        public void Release_DecrementsInFlight()
        {
            // Arrange
            var limiter = new SimpleLimiter(new FixedLimit(10));
            limiter.Acquire();
            limiter.Acquire();

            // Act
            limiter.Release();

            // Assert
            Assert.Equal(1, limiter.InFlight);
        }
    }
}