using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sprache;
using theorbo.MusicTheory.Domain;
using theorbo.MusicTheory.Parsing;

namespace theorbo.tests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void ShouldParseBasicDegrees()
        {
            var basicDegrees = new Dictionary<string, ValueTuple<Accidental, int, KnownChordKind>>
            {
                ["#IVm"] = (Accidental.Sharp,4,KnownChordKind.Min),
                ["#II"] = (Accidental.Sharp,2,KnownChordKind.Maj),
                ["bii"] = (Accidental.Flat,2,KnownChordKind.Min),
                ["ii"] = (Accidental.None,2,KnownChordKind.Min),
                ["iiM"] = (Accidental.None,2,KnownChordKind.Maj),
                ["bbiii"] = (Accidental.DoubleFlat,3,KnownChordKind.Min),
                ["bbV-"] = (Accidental.DoubleFlat,5,KnownChordKind.Min),
                ["bb3-"] = (Accidental.DoubleFlat,3,KnownChordKind.Min),
                ["bb3"] = (Accidental.DoubleFlat,3,KnownChordKind.Maj),
                ["#7-"] = (Accidental.Sharp,7,KnownChordKind.Min),
                ["##8lyd"] = (Accidental.DoubleSharp,8,KnownChordKind.Lyd),
            };

            var shouldFail = new[]
            {
                "IVmM",
                "b#III",
                "bb#ii",
                "iiM-"
            };

            //Parse all valid degree notation cases and 

            foreach (var item in basicDegrees)
            {
                var degree =  Degrees.BaseDegreePraser.TryParse(item.Key);
                
                Assert.That(degree.WasSuccessful && degree.Remainder.AtEnd, Is.True, $"Parsing of `{item}` failed: {degree.Message}");
                Assert.That(degree.Value, Is.EqualTo(item.Value),$"`{item.Key}` misparsed: {degree.Value} != {item.Value}");
            }

            //Make sure these wont get full valid match

            foreach (var item in shouldFail)
            {
                var degree =  Degrees.BaseDegreePraser.TryParse(item);

                Assert.That(degree.WasSuccessful && degree.Remainder.AtEnd, Is.False, $"Parsing of `{item}` succeeded but had to fail. Got {degree}");
            }
        }
    }
}
