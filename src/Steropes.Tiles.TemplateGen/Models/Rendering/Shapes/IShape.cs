using Avalonia.Media;
using SkiaSharp;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;
using System.Collections.Generic;

namespace Steropes.Tiles.TemplateGen.Models.Rendering.Shapes
{
    public interface IShape
    {
        IntPoint Top { get; }
        IntPoint Left { get; }
        IntPoint Bottom { get; }
        IntPoint Right { get; }

        void Draw(SKCanvas g, Color pen);
        IShape ToHighlight();
        List<IntPoint> GetHighlightFor(NeighbourIndex idx);
    }
}
