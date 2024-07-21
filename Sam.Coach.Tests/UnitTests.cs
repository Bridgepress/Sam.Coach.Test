using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Sam.Coach.Exceptions.Forbidden403;
using Xunit;

namespace Sam.Coach.Tests
{
    public class UnitTests
    {
        private readonly ILongestRisingSequenceFinder _longestRisingSequenceFinder;

        public UnitTests()
        {
            var serviceProvider = Startup.Configure().BuildServiceProvider();
            _longestRisingSequenceFinder = serviceProvider.GetRequiredService<ILongestRisingSequenceFinder>();
        }

        [Theory]
        [InlineData(new[] { 4, 3, 5, 8, 5, 0, 0, -3 }, new[] { 4, 6, -3, 3, 7, 9 }, new[] { 4, 6, -3, 3, 7, 9 })]
        [InlineData(new[] { 9, 6, 4, 5, 2, 0 }, new[] { 4, 6, -3, 3, 7, 9 }, new[] { 4, 6, -3, 3, 7, 9 })]
        [InlineData(new[] { 10, 22, 9, 33, 21, 50, 41, 60, 80 }, new[] { 3, 10, 2, 1, 20 }, new[] { 10, 22, 9, 33, 21, 50, 41, 60, 80 })]
        [InlineData(new[] { 1, 3, 2, 3, 4, 1, 5, 6 }, new[] { 7, 8, 9 }, new[] { 1, 3, 2, 3, 4, 1, 5, 6 })]
        [InlineData(new[] { 4, 3, 5, 8, 5, 0, 0, -3 }, new[] { 4, 6, int.MinValue, 3, 7, int.MaxValue }, new[] { 4, 6, int.MinValue, 3, 7, int.MaxValue })]
        public async Task Can_Find_Longer(IEnumerable<int> data, IEnumerable<int> another_data, IEnumerable<int> expected)
        {
            var actual = await _longestRisingSequenceFinder.Find(data, another_data);
            actual.Should().Equal(expected);
        }

        [Theory]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 5, 4, 3, 2, 1 }, typeof(LongestIncrementEqualException), "The longest increment in the two arrays is equal to")]
        [InlineData(new[] { -3, 3, 7, 9, 2, 0 }, new[] { 4, 6, -3, 3, 7, 9 }, typeof(LongestIncrementEqualException), "The longest increment in the two arrays is equal to")]
        public async Task Can_Find_Longer_Handle_Error(IEnumerable<int> data, IEnumerable<int> another_data, Type exceptionType, string message)
        {
            try
            {
                var actual = await _longestRisingSequenceFinder.Find(data, another_data);
            }
            catch (Exception e)
            {
                Assert.True(e.GetType() == exceptionType);
                Assert.Equal(e.Message, message);
            }
        }
    }
}