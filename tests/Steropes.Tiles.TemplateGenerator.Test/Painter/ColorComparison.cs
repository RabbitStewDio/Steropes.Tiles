using Avalonia.Media;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using SkiaSharp;

namespace Steropes.Tiles.TemplateGenerator.Test.Painter
{
    public static class ColorComparison
    {
        public static ColorAssertions Should(this Color a)
        {
            return new ColorAssertions(a);
        }

        public static SkiaColorAssertions Should(this SKColor a)
        {
            return new SkiaColorAssertions(a);
        }

        public class ColorAssertions : ReferenceTypeAssertions<Color, ColorAssertions>
        {
            public ColorAssertions(Color subject)
            {
                Subject = subject;
            }

            protected override string Identifier => "color";

            public AndConstraint<ColorAssertions> BeSameColor(Color other, string because = "", params object[] becauseArgs)
            {
                Execute.Assertion.ForCondition(Subject.ToUint32() == other.ToUint32()).BecauseOf(because, becauseArgs).FailWith("Expected color {0} to have the same ARGB value as {1}{reason}", Subject, other);
                return new AndConstraint<ColorAssertions>(this);
            }
        }
        
        public class SkiaColorAssertions : ReferenceTypeAssertions<SKColor, SkiaColorAssertions>
        {
            public SkiaColorAssertions(SKColor subject)
            {
                Subject = subject;
            }

            protected override string Identifier => "color";

            public AndConstraint<SkiaColorAssertions> BeSameColor(Color other, string because = "", params object[] becauseArgs)
            {
                Execute.Assertion.ForCondition((uint)Subject == other.ToUint32()).BecauseOf(because, becauseArgs).FailWith("Expected color {0} to have the same ARGB value as {1}{reason}", Subject, other);
                return new AndConstraint<SkiaColorAssertions>(this);
            }
        }
    }
}
