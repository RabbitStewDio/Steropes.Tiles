using System.Drawing;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace Steropes.Tiles.TemplateGenerator.Test.Painter
{
  public static class ColorComparison
  {
    public class ColorAssertions : ReferenceTypeAssertions<Color, ColorAssertions>
    {
      public ColorAssertions(Color subject)
      {
        Subject = subject;
      }

      protected override string Identifier => "color";

      public AndConstraint<ColorAssertions> BeSameColor(Color other, string because = "", params object[] becauseArgs)
      {
        Execute.Assertion.ForCondition(Subject.ToArgb() == other.ToArgb()).BecauseOf(because, becauseArgs).FailWith("Expected color to have the same ARGB value as {0}{reason}", other);
        return new AndConstraint<ColorAssertions>(this);
      }
    }

    public static ColorAssertions Should(this Color a)
    {
      return new ColorAssertions(a);
    }
  }
}