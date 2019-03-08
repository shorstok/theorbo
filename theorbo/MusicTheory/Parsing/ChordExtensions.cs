using System;
using System.Collections.Generic;
using System.Globalization;
using Sprache;
using theorbo.MusicTheory.Domain;

namespace theorbo.MusicTheory.Parsing
{
    public static class ChordExtensions
    {
        public static Parser<IEnumerable<char>> MajParser = Parse.String("maj");

        private static readonly Parser<ExtensionBase> BaseExtensionParser =
            (from maj7 in MajParser.Optional()
                from degree in Parse.Digit.AtLeastOnce().Text()
                select new ExtensionBase(int.Parse(degree, CultureInfo.InvariantCulture), !maj7.IsEmpty,null)).
            Or(
                from maj7 in MajParser
                select ExtensionBase.Maj7);

        private static readonly Parser<Accidental> ExtensionAccidentalParser =
            Parse.String("+").Or(Parse.String("#")).Select(s => Accidental.Sharp)
                .XOr(Parse.String("-").Or(Parse.String("b")).Select(s => Accidental.Flat));

        private static readonly Parser<Extension.ExtensionKind> ActionTypeParser =
            Parse.String("add").Or(Parse.String("/").Then(v=>Parse.Digit.AtLeastOnce().Preview().Select(s=>s.Get()))).      //'add9' or case when '/9' for adding chord extension
                Select(s => Extension.ExtensionKind.Add)
                .XOr(Parse.String("omit").Select(s => Extension.ExtensionKind.Omit));

        private static readonly Parser<Extension> ChordExtensionItemParser =
            OptionallyParenthesis(
                from action in ActionTypeParser.Optional() //add | omit
                from accidental in ExtensionAccidentalParser.Optional() // '#' '+' '-' 'b'
                from degree in Parse.Digit.AtLeastOnce().Text() //degree number
                select new Extension(int.Parse(degree,
                        CultureInfo.InvariantCulture),
                    accidental.IsEmpty
                        ? Accidental.None
                        : accidental.Get(),
                    action.IsEmpty
                        ? Extension.ExtensionKind.Add
                        : action.Get()));

        public static readonly Parser<Tuple<ExtensionBase, IEnumerable<Extension>>> ExtensionParser =
            from start in BaseExtensionParser.Optional().Named("base") // matches start, like 'maj7' in Cmaj7add3omit1
            from extensions in ChordExtensionItemParser.Many() //matches extensions, like add3omit1 in Cmaj7add3omit1
            select Tuple.Create(start.IsEmpty ? ExtensionBase.Default : start.Get(), extensions);

        /// <summary>
        ///     Matches extension in parenthesis or without them
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static Parser<T> OptionallyParenthesis<T>(Parser<T> source)
        {
            return (from left in Parse.Char('(')
                from srcRef in Parse.Ref(() => source)
                from right in Parse.Char(')')
                select srcRef).XOr(source);
        }


        public struct ExtensionBase
        {
            public ExtensionBase(int degrees, bool isMaj, bool? hasThird)
            {
                IsMaj = isMaj;
                HasThird = hasThird ?? degrees != 5;     //Fifth chord means there is no third
                Degrees = degrees;
            }

            public bool IsMaj { get; }
            public int Degrees { get; }
            public bool HasThird { get; }

            public static ExtensionBase Default => new ExtensionBase(5, isMaj: false,hasThird: true);
            public static ExtensionBase Maj7 =>new ExtensionBase(7, isMaj: true,hasThird: true);
            public static ExtensionBase Powerchord  =>new ExtensionBase(5, isMaj: false,hasThird: false);

            public override string ToString()
            {
                return $"{(IsMaj ? "Maj" : String.Empty)}{Degrees}";
            }
        }

        public struct Extension
        {
            public enum ExtensionKind
            {
                Add,
                Omit
            }

            public Extension(int degree, Accidental accidental, ExtensionKind kind)
            {
                Kind = kind;
                Accidental = accidental;
                Degree = degree;
            }

            public ExtensionKind Kind { get; }
            public Accidental Accidental { get; }
            public int Degree { get; }

            public override string ToString()
            {
                return $"{Degree}({Accidental}, {Kind})";
            }
        }
    }
}