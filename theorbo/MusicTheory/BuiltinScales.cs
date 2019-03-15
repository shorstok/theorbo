using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using theorbo.MusicTheory.Domain;

namespace theorbo.MusicTheory
{
    public static class BuiltinScales
    {
        public static Scale Major { get; } = Scale.FromSteps(2,2,1,2,2,2,1);
        public static Scale Ionian { get; } = Major;
        public static Scale Dorian { get; } = Major.RotateLeft(1);
        public static Scale Phrygian { get; } = Major.RotateLeft(2);
        public static Scale Lydian { get; } = Major.RotateLeft(3);
        public static Scale Mixolydian { get; } = Major.RotateLeft(4);
        public static Scale Aeolian { get; } = Major.RotateLeft(5);
        public static Scale Locrian { get; } = Major.RotateLeft(6);
    }
}
