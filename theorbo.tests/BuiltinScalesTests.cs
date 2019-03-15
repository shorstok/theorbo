using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sprache;
using theorbo.MusicTheory;
using theorbo.MusicTheory.Domain;
using theorbo.MusicTheory.Parsing;

namespace theorbo.tests
{
    [TestFixture]
    public class BuiltinScalesTests
    {
        private class ModeTestCase
        {
            public Scale Scale { get; }
            public int SourceDegree { get; }
            public Accidental SourceAccidental { get; }
            public Note TestNote { get; }

            public ModeTestCase(Scale scale, int sourceDegree, Accidental sourceAccidental, Note testNote)
            {
                Scale = scale;
                SourceDegree = sourceDegree;
                SourceAccidental = sourceAccidental;
                TestNote = testNote;
            }
        }

        private readonly ModeTestCase[] _modeTestCasses = {
            /* major (ionian) tests */
            new ModeTestCase(BuiltinScales.Ionian,1,Accidental.None,new Note(NoteValue.C)),
            new ModeTestCase(BuiltinScales.Ionian,2,Accidental.None,new Note(NoteValue.D)),
            new ModeTestCase(BuiltinScales.Ionian,3,Accidental.None,new Note(NoteValue.E)),
            new ModeTestCase(BuiltinScales.Ionian,4,Accidental.None,new Note(NoteValue.F)),
            new ModeTestCase(BuiltinScales.Ionian,5,Accidental.None,new Note(NoteValue.G)),
            new ModeTestCase(BuiltinScales.Ionian,6,Accidental.None,new Note(NoteValue.A)),
            new ModeTestCase(BuiltinScales.Ionian,7,Accidental.None,new Note(NoteValue.B)),

            /* dorian tests */
            new ModeTestCase(BuiltinScales.Dorian,1,Accidental.None,new Note(NoteValue.C)),
            new ModeTestCase(BuiltinScales.Dorian,2,Accidental.None,new Note(NoteValue.D)),
            new ModeTestCase(BuiltinScales.Dorian,3,Accidental.None,new Note(NoteValue.E,Accidental.Flat)),
            new ModeTestCase(BuiltinScales.Dorian,4,Accidental.None,new Note(NoteValue.F)),
            new ModeTestCase(BuiltinScales.Dorian,5,Accidental.None,new Note(NoteValue.G)),
            new ModeTestCase(BuiltinScales.Dorian,6,Accidental.None,new Note(NoteValue.A)),
            new ModeTestCase(BuiltinScales.Dorian,7,Accidental.None,new Note(NoteValue.B,Accidental.Flat)),

            /* Phrygian tests */
            new ModeTestCase(BuiltinScales.Phrygian,1,Accidental.None,new Note(NoteValue.C)),
            new ModeTestCase(BuiltinScales.Phrygian,2,Accidental.None,new Note(NoteValue.D,Accidental.Flat)),
            new ModeTestCase(BuiltinScales.Phrygian,3,Accidental.None,new Note(NoteValue.E,Accidental.Flat)),
            new ModeTestCase(BuiltinScales.Phrygian,4,Accidental.None,new Note(NoteValue.F)),
            new ModeTestCase(BuiltinScales.Phrygian,5,Accidental.None,new Note(NoteValue.G)),
            new ModeTestCase(BuiltinScales.Phrygian,6,Accidental.None,new Note(NoteValue.A,Accidental.Flat)),
            new ModeTestCase(BuiltinScales.Phrygian,7,Accidental.None,new Note(NoteValue.B,Accidental.Flat)),

            /* Lydian tests */
            new ModeTestCase(BuiltinScales.Lydian,1,Accidental.None,new Note(NoteValue.C)),
            new ModeTestCase(BuiltinScales.Lydian,2,Accidental.None,new Note(NoteValue.D)),
            new ModeTestCase(BuiltinScales.Lydian,3,Accidental.None,new Note(NoteValue.E)),
            new ModeTestCase(BuiltinScales.Lydian,4,Accidental.None,new Note(NoteValue.F, Accidental.Sharp)),
            new ModeTestCase(BuiltinScales.Lydian,5,Accidental.None,new Note(NoteValue.G)),
            new ModeTestCase(BuiltinScales.Lydian,6,Accidental.None,new Note(NoteValue.A)),
            new ModeTestCase(BuiltinScales.Lydian,7,Accidental.None,new Note(NoteValue.B)),

            /* mixolydian tests */
            new ModeTestCase(BuiltinScales.Mixolydian,1,Accidental.None,new Note(NoteValue.C)),
            new ModeTestCase(BuiltinScales.Mixolydian,2,Accidental.None,new Note(NoteValue.D)),
            new ModeTestCase(BuiltinScales.Mixolydian,3,Accidental.None,new Note(NoteValue.E)),
            new ModeTestCase(BuiltinScales.Mixolydian,4,Accidental.None,new Note(NoteValue.F)),
            new ModeTestCase(BuiltinScales.Mixolydian,5,Accidental.None,new Note(NoteValue.G)),
            new ModeTestCase(BuiltinScales.Mixolydian,6,Accidental.None,new Note(NoteValue.A)),
            new ModeTestCase(BuiltinScales.Mixolydian,7,Accidental.None,new Note(NoteValue.B,Accidental.Flat)),
            
            /* Aeolian tests */
            new ModeTestCase(BuiltinScales.Aeolian,1,Accidental.None,new Note(NoteValue.C)),
            new ModeTestCase(BuiltinScales.Aeolian,2,Accidental.None,new Note(NoteValue.D)),
            new ModeTestCase(BuiltinScales.Aeolian,3,Accidental.None,new Note(NoteValue.E, Accidental.Flat)),
            new ModeTestCase(BuiltinScales.Aeolian,4,Accidental.None,new Note(NoteValue.F)),
            new ModeTestCase(BuiltinScales.Aeolian,5,Accidental.None,new Note(NoteValue.G)),
            new ModeTestCase(BuiltinScales.Aeolian,6,Accidental.None,new Note(NoteValue.A,Accidental.Flat)),
            new ModeTestCase(BuiltinScales.Aeolian,7,Accidental.None,new Note(NoteValue.B,Accidental.Flat)),

            /* Locrian tests */
            new ModeTestCase(BuiltinScales.Locrian,1,Accidental.None,new Note(NoteValue.C)),
            new ModeTestCase(BuiltinScales.Locrian,2,Accidental.None,new Note(NoteValue.D, Accidental.Flat)),
            new ModeTestCase(BuiltinScales.Locrian,3,Accidental.None,new Note(NoteValue.E, Accidental.Flat)),
            new ModeTestCase(BuiltinScales.Locrian,4,Accidental.None,new Note(NoteValue.F)),
            new ModeTestCase(BuiltinScales.Locrian,5,Accidental.None,new Note(NoteValue.G, Accidental.Flat)),
            new ModeTestCase(BuiltinScales.Locrian,6,Accidental.None,new Note(NoteValue.A,Accidental.Flat)),
            new ModeTestCase(BuiltinScales.Locrian,7,Accidental.None,new Note(NoteValue.B,Accidental.Flat)),

            /* extra tests with accidentals */
            new ModeTestCase(BuiltinScales.Mixolydian,7,Accidental.Sharp,new Note(NoteValue.B)),
            new ModeTestCase(BuiltinScales.Mixolydian,7,Accidental.Flat,new Note(NoteValue.A)),
            new ModeTestCase(BuiltinScales.Mixolydian,7,Accidental.DoubleSharp,new Note(NoteValue.C).UpAnOctave()),
            new ModeTestCase(BuiltinScales.Mixolydian,7,Accidental.DoubleFlat,new Note(NoteValue.A,Accidental.Flat)),
        };

        [Test]
        public void ShouldKnowNotesInBuiltinModes()
        {        
            //basic checks

            Assert.That(BuiltinScales.Mixolydian.GetScaleDegree(1, Accidental.None).ApplyTo(new Note(NoteValue.C)),
                Is.EqualTo(new Note(NoteValue.C)));

            //check that transpose works for another root

            Assert.That(BuiltinScales.Mixolydian.GetScaleDegree(1, Accidental.None).ApplyTo(new Note(NoteValue.A)),
                Is.EqualTo(new Note(NoteValue.A)));

            //check all steps

            foreach (var step in _modeTestCasses)
            {
                Assert.That(step.Scale.GetScaleDegree(step.SourceDegree, step.SourceAccidental).
                        ApplyTo(new Note(NoteValue.C)),
                    Is.EqualTo(step.TestNote), $"Failed on {step.Scale}, note {step.TestNote}/{step.SourceAccidental}");
            }

        }
    }
}