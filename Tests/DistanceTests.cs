﻿using System;
using System.Text.Json;
using Units;
using Xunit;

namespace Tests
{
    public class DistanceTests
    {
        /// <summary>
        /// Show that converting distance in one unit of length equals the same real distance expressed in a different unit of length
        /// </summary>
        /// <param name="baseDistanceInput"></param>
        /// <param name="baseUnitInput"></param>
        /// <param name="targetDistanceInput"></param>
        /// <param name="targetUnitInput"></param>
        [Theory]
        [InlineData(10, "mm", 1, "cm")]
        [InlineData(100, "cm", 1, "m")]
        [InlineData(1000, "mm", 1, "m")]
        [InlineData(1000, "m", 1, "km")]
        [InlineData(12, "i", 1, "ft")]
        [InlineData(3, "ft", 1, "yd")]
        [InlineData(1609.344, "m", 1, "mi")]
        [InlineData(1, "cm", 10, "mm")]
        [InlineData(1, "m", 100, "cm")]
        [InlineData(1, "m", 1000, "mm")]
        [InlineData(1, "km", 1000, "m")]
        [InlineData(1, "ft", 12, "i")]
        [InlineData(1, "yd", 3, "ft")]
        [InlineData(1, "mi", 1609.344, "m")]
        public void ConvertedDistancesAreEqual(
            decimal baseDistanceInput, string baseUnitInput, 
            decimal targetDistanceInput, string targetUnitInput
            )
        {
            Length.TryParseUnit(baseUnitInput, out var @base);
            var baseDistance = Distance.Create(baseDistanceInput, @base);

            Length.TryParseUnit(targetUnitInput, out var targetUnit);
            var targetDistance = baseDistance.ConvertTo(targetUnit);

            Assert.Equal(baseDistance.ToString(), $"{baseDistanceInput}{baseUnitInput}");
            Assert.Equal(baseDistanceInput, baseDistance.Value);

            Assert.Equal(targetDistance.ToString(), $"{targetDistanceInput}{targetUnitInput}");
            Assert.Equal(targetDistanceInput, targetDistance.Value);
            
            Assert.Equal(baseDistance, targetDistance);
        }

        /// <summary>
        /// Rudimentary verification that Distance.Equals does not always return true
        /// </summary>
        [Fact]
        public void CheckNotEquals()
        {
            Length.TryParseUnit("mm", out var @base);
            var baseDistance = Distance.Create(9m, @base);

            Length.TryParseUnit("cm", out var targetUnit);
            var targetDistance = Distance.Create(9m, targetUnit);

            Assert.Equal("9mm", baseDistance.ToString());
            Assert.Equal("9cm", targetDistance.ToString());
            Assert.NotEqual(baseDistance, targetDistance);
        }
    }
}
