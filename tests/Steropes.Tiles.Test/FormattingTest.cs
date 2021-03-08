using System;
using NUnit.Framework;

namespace Steropes.Tiles.Test
{
  public class FormattingTest
  {
    [Test]
    public void TestFormatStrings()
    {
      var formatString = ProduceFormatString();
      Console.WriteLine(formatString);
      Console.WriteLine(formatString, 1, 2, 3, 4);
    }

    string ProduceFormatString(string tagFormat = null, string format = null)
    {
      var formatString = tagFormat ?? "nw{0}ne{1}se{2}sw{3}";
      format = format ?? "{0}_{1}";

      return string.Format(format, "{{0}}", formatString);
    }
  }
}