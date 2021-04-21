using SkiaSharp;

namespace Steropes.Tiles.TemplateGen.Models.Rendering
{
    public interface ITilePainter
    {
        void Draw(SKCanvas g, TextureTile tile);
    }
}
