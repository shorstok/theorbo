using NUnit.Framework;
using theorbo.MusicTheory;
using theorbo.MusicTheory.Domain;

namespace theorbo.tests
{
    [TestFixture]
    public class IntervalTests
    {
        private class IntervalTestCase
        {
            public IntervalTestCase(Interval.Quality quality, int value, int testSemitoneCount)
            {
                Quality = quality;
                Value = value;
                TestSemitoneCount = testSemitoneCount;
            }

            public Interval.Quality Quality { get; }
            public int Value { get; }

            public int TestSemitoneCount { get; }
        }

        /// <summary>
        /// Using known intervals as test cases
        /// </summary>
        private readonly IntervalTestCase[] _testCases =
        {
            //min/maj/perfect

            new IntervalTestCase(Interval.Quality.Perfect,1,0), 
            new IntervalTestCase(Interval.Quality.Minor,2,1), 
            new IntervalTestCase(Interval.Quality.Major,2,2), 
            new IntervalTestCase(Interval.Quality.Minor,3,3), 
            new IntervalTestCase(Interval.Quality.Major,3,4), 
            new IntervalTestCase(Interval.Quality.Perfect,4,5),            
            new IntervalTestCase(Interval.Quality.Perfect,5,7), 
            new IntervalTestCase(Interval.Quality.Minor,6,8), 
            new IntervalTestCase(Interval.Quality.Major,6,9), 
            new IntervalTestCase(Interval.Quality.Minor,7,10), 
            new IntervalTestCase(Interval.Quality.Major,7,11), 
            new IntervalTestCase(Interval.Quality.Perfect,8,12), 

            // aug/dim

            new IntervalTestCase(Interval.Quality.Diminished,2,0), 
            new IntervalTestCase(Interval.Quality.Augmented,1,1), 
            new IntervalTestCase(Interval.Quality.Diminished,3,2), 
            new IntervalTestCase(Interval.Quality.Augmented,2,3), 
            new IntervalTestCase(Interval.Quality.Diminished,4,4), 
            new IntervalTestCase(Interval.Quality.Augmented,3,5),             
            new IntervalTestCase(Interval.Quality.Diminished,5,6), 
            new IntervalTestCase(Interval.Quality.Augmented,4,6),           
            new IntervalTestCase(Interval.Quality.Diminished,6,7), 
            new IntervalTestCase(Interval.Quality.Augmented,5,8), 
            new IntervalTestCase(Interval.Quality.Diminished,7,9), 
            new IntervalTestCase(Interval.Quality.Augmented,6,10), 
            new IntervalTestCase(Interval.Quality.Diminished,8,11), 
            new IntervalTestCase(Interval.Quality.Augmented,7,12), 
        };

        /*
         *
           Number of
           semitones	Minor, major,
           or perfect intervals	Short	Augmented or
           diminished intervals	Short	Widely used
           alternative names	Short	Audio
           0	Perfect unison[5][b]	P1	Diminished second	d2			About this soundPlay (help·info)
           1	Minor second	m2	Augmented unison[5][b]	A1	Semitone,[c] half tone, half step	S	About this soundPlay (help·info)
           2	Major second	M2	Diminished third	d3	Tone, whole tone, whole step	T	About this soundPlay (help·info)
           3	Minor third	m3	Augmented second	A2			About this soundPlay (help·info)
           4	Major third	M3	Diminished fourth	d4			About this soundPlay (help·info)
           5	Perfect fourth	P4	Augmented third	A3			About this soundPlay (help·info)
           6			Diminished fifth	d5	Tritone[a]	TT	About this soundPlay (help·info)
           Augmented fourth	A4
           7	Perfect fifth	P5	Diminished sixth	d6			About this soundPlay (help·info)
           8	Minor sixth	m6	Augmented fifth	A5			About this soundPlay (help·info)
           9	Major sixth	M6	Diminished seventh	d7			About this soundPlay (help·info)
           10	Minor seventh	m7	Augmented sixth	A6			About this soundPlay (help·info)
           11	Major seventh	M7	Diminished octave	d8			About this soundPlay (help·info)
           12	Perfect octave	P8	Augmented seventh	A7			About this soundPlay (help·info)
           
         *
         */


        [Test]
        public void ShouldCalculateIntervalByQuality()
        {
            foreach (var testCase in _testCases)
            {
                Assert.That(Interval.FromQuality(testCase.Quality,testCase.Value).Semitones, Is.EqualTo(testCase.TestSemitoneCount), $"Interval {testCase.Quality}{testCase.Value} built incorrectly");
            }
        }
    }
}