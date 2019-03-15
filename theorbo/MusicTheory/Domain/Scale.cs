using System;
using System.Linq;
using Topshelf.Runtime.Windows;

namespace theorbo.MusicTheory.Domain
{
    public class Scale
    {
        public Note Root { get; }
        public Interval[] Tones { get; }

        public Scale(Interval[] tones, Note root = null)
        {
            Tones = tones;
            Root = root;
        }

        public static Scale FromSteps(params int[] steps)
        {
            return new Scale(steps.Select(Interval.FromSemitones).ToArray());
        }

        public Interval GetScaleDegree(int value, Accidental accidental)
        {
            if(value < 1)
                throw new ArgumentOutOfRangeException(nameof(value));

            value = value % (Tones.Length + 1)-1;

            if (value == 0)            
                return Interval.Unity.ApplyAccidental(accidental);

            var delta = Tones.Take(value).Sum(s => s.Semitones);

            return Interval.FromSemitones(delta + Note.AccidentalToSemitones(accidental));
        }

        public Scale RotateLeft(int steps)
        {
            steps = steps % Tones.Length;
            Interval[] buffer = new Interval[steps];
            
            var result = (Interval[])Tones.Clone();

            Array.Copy(result, buffer, steps);
            Array.Copy(result, steps, result, 0, result.Length - steps);
            Array.Copy(buffer, 0, result, result.Length - steps, steps);

            return new Scale(result,Root);
        }
    }
}