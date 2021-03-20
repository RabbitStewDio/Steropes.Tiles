using System.Collections.Generic;
using System.Drawing;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
    public interface IShape
    {
        Point Top { get; }
        Point Left { get; }
        Point Bottom { get; }
        Point Right { get; }

        void Draw(Graphics g, Pen pen);
        IShape ToHighlight();
        List<Point> GetHighlightFor(NeighbourIndex idx);
    }
}
