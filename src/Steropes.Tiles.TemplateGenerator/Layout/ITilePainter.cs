using System.Drawing;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
    public interface ITilePainter
    {
        void Draw(Graphics g, TextureTile tile);
    }
}
