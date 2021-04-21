namespace Steropes.Tiles.TexturePack
{
    public interface IContentLoader<TRawTexture>
    {
        TRawTexture LoadTexture(string name);
    }
}
