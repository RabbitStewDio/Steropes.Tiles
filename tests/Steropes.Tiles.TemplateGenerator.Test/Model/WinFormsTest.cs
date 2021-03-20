using FluentAssertions;
using NUnit.Framework;
using System.Windows.Forms;

namespace Steropes.Tiles.TemplateGenerator.Test.Model
{
    public class WinFormsTest
    {
        [Test]
        public void NumericUpDownBehaviourTest()
        {
            var nu = new NumericUpDown();
            nu.Text = "";

            nu.Value = 10;

            nu.Text.Should().Be("10");
            nu.Value.Should().Be(10);
        }
    }
}
