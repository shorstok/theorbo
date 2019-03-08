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
                
                Assert.That(degree.WasSuccessful && degree.Remainder.AtEnd, Is.True, $"Parsing of `{item.Key}` failed: {degree.Message}");
                Assert.That(degree.Value, Is.EqualTo(item.Value),$"`{item.Key}` misparsed: {degree.Value} != {item.Value}");
            }

            //Make sure these wont get full valid match

            foreach (var item in shouldFail)
            {
                var degree =  Degrees.BaseDegreePraser.TryParse(item);

                Assert.That(degree.WasSuccessful && degree.Remainder.AtEnd, Is.False, $"Parsing of `{item}` succeeded but had to fail. Got {degree}");
            }
        }

        [Test]
        public void ShouldParseDegreesWithExtensions()
        {
            var testData = new Dictionary<string, Degrees.ParsedDegree>
            {
                ["bVmaj7"] = new Degrees.ParsedDegree(Accidental.Flat, 5, KnownChordKind.Maj, ChordExtensions.ExtensionBase.Maj7, new List<ChordExtensions.Extension>()),
                ["III5"] = new Degrees.ParsedDegree(Accidental.None, 3, KnownChordKind.Maj, ChordExtensions.ExtensionBase.Powerchord, new List<ChordExtensions.Extension>()),
                ["bVmaj7/9"] = new Degrees.ParsedDegree(Accidental.Flat, 5, KnownChordKind.Maj, ChordExtensions.ExtensionBase.Maj7,new[]
                {
                    new ChordExtensions.Extension(9,Accidental.None, ChordExtensions.Extension.ExtensionKind.Add),
                }),
                ["#ivmaj"] = new Degrees.ParsedDegree(Accidental.Sharp, 4, KnownChordKind.Min, ChordExtensions.ExtensionBase.Maj7, new List<ChordExtensions.Extension>()),
                ["#IVm(b9)add13"] = new Degrees.ParsedDegree(Accidental.Sharp,
                    4,
                    KnownChordKind.Min,
                    ChordExtensions.ExtensionBase.Default,
                    new[]
                    {
                        new ChordExtensions.Extension(9,Accidental.Flat, ChordExtensions.Extension.ExtensionKind.Add),
                        new ChordExtensions.Extension(13,Accidental.None, ChordExtensions.Extension.ExtensionKind.Add),
                    }),
                ["biiM9#13+5b5"] = new Degrees.ParsedDegree(Accidental.Flat,
                    2,
                    KnownChordKind.Maj,
                    new ChordExtensions.ExtensionBase(9,false,true), 
                    new[]
                    {
                        new ChordExtensions.Extension(13,Accidental.Sharp, ChordExtensions.Extension.ExtensionKind.Add),
                        new ChordExtensions.Extension(5,Accidental.Sharp, ChordExtensions.Extension.ExtensionKind.Add),
                        new ChordExtensions.Extension(5,Accidental.Flat, ChordExtensions.Extension.ExtensionKind.Add),
                    }),                    
                ["#iM13#13-3b5/v"] = new Degrees.ParsedDegree(Accidental.Sharp,
                    1,
                    KnownChordKind.Maj,
                    new ChordExtensions.ExtensionBase(13,false,true), 
                    new[]
                    {
                        new ChordExtensions.Extension(13,Accidental.Sharp, ChordExtensions.Extension.ExtensionKind.Add),
                        new ChordExtensions.Extension(3,Accidental.Flat, ChordExtensions.Extension.ExtensionKind.Add),
                        new ChordExtensions.Extension(5,Accidental.Flat, ChordExtensions.Extension.ExtensionKind.Add),
                    }, Tuple.Create(5,KnownChordKind.Min)),
                ["bi5#11-3omitb5/V"] = new Degrees.ParsedDegree(Accidental.Flat,
                    1,
                    KnownChordKind.Min,
                    ChordExtensions.ExtensionBase.Powerchord, 
                    new[]
                    {
                        new ChordExtensions.Extension(11,Accidental.Sharp, ChordExtensions.Extension.ExtensionKind.Add),
                        new ChordExtensions.Extension(3,Accidental.Flat, ChordExtensions.Extension.ExtensionKind.Add),
                        new ChordExtensions.Extension(5,Accidental.Flat, ChordExtensions.Extension.ExtensionKind.Omit),
                    }, Tuple.Create(5,KnownChordKind.Maj))
            };
            
            //Parse all valid degree notation cases 

            foreach (var item in testData)
            {
                var degree =  Degrees.DegreePraser.TryParse(item.Key);
                
                Assert.That(degree.WasSuccessful, Is.True, $"Parsing of `{item.Key}` failed: {degree.Message}");
                Assert.That(degree.Remainder.AtEnd, Is.True, $"Parsing of `{item.Key}` didnt consume all input");
                Assert.That(degree.Value, Is.EqualTo(item.Value),$"`{item.Key}` misparsed: {degree.Value} != {item.Value}");
            }

        }      
        
        [Test]
        public void ShouldParseChords()
        {
            var testData = new Dictionary<string, Chords.ParsedChord>
            {
                ["Bbmaj7"] = new Chords.ParsedChord(NoteValue.B, Accidental.Flat, KnownChordKind.Maj, ChordExtensions.ExtensionBase.Maj7, new ChordExtensions.Extension[0], null),
                ["Bbmaj7/9"] = new Chords.ParsedChord(NoteValue.B,
                    Accidental.Flat,
                    KnownChordKind.Maj,
                    ChordExtensions.ExtensionBase.Maj7,
                    new[]
                    {
                        new ChordExtensions.Extension(9,
                            Accidental.None,
                            ChordExtensions.Extension.ExtensionKind.Add),
                    },
                    null),
                ["A#7/F"] = new Chords.ParsedChord(NoteValue.A,
                    Accidental.Sharp,
                    KnownChordKind.Maj,
                    new ChordExtensions.ExtensionBase(7,false,true), 
                    new ChordExtensions.Extension[0], 
                    Tuple.Create(NoteValue.F, Accidental.None)),
                ["Ebm5#13-3omitb5/F#"] = new Chords.ParsedChord(NoteValue.E,
                    Accidental.Flat,
                    KnownChordKind.Min,
                    ChordExtensions.ExtensionBase.Powerchord,
                    new[]
                    {
                        new ChordExtensions.Extension(13,Accidental.Sharp, ChordExtensions.Extension.ExtensionKind.Add),
                        new ChordExtensions.Extension(3,Accidental.Flat, ChordExtensions.Extension.ExtensionKind.Add),
                        new ChordExtensions.Extension(5,Accidental.Flat, ChordExtensions.Extension.ExtensionKind.Omit),
                    },
                    Tuple.Create(NoteValue.F, Accidental.Sharp)),
            };
            
            //Parse all valid degree notation cases 

            foreach (var item in testData)
            {
                var chord =  Chords.ChordPraser.TryParse(item.Key);
                
                Assert.That(chord.WasSuccessful, Is.True, $"Parsing of `{item.Key}` failed: {chord.Message}");
                Assert.That(chord.Remainder.AtEnd, Is.True, $"Parsing of `{item.Key}` didnt consume all input");
                Assert.That(chord.Value, Is.EqualTo(item.Value),$"`{item.Key}` misparsed: {chord.Value} != {item.Value}");
            }

        }
    }
}
