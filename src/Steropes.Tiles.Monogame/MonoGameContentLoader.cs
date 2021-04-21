using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Steropes.Tiles.TexturePack;

namespace Steropes.Tiles.Monogame
{
    public class MonoGameContentLoader : IContentLoader<XnaRawTexture>
    {
        readonly ContentManager mgr;
        readonly string basePath;

        public MonoGameContentLoader(ContentManager mgr, string basePath = null)
        {
            this.mgr = mgr;
            this.basePath = basePath;
        }

        public XnaRawTexture LoadTexture(string name)
        {
            string path;
            if (basePath != null)
            {
                path = $"{basePath}/{name}";
            }
            else
            {
                path = name;
            }

            return new XnaRawTexture(path, mgr.Load<Texture2D>(path));
        }
    }
}
