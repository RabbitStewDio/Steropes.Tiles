using Steropes.Tiles.Renderer;

namespace Steropes.Tiles.Sample.Shared
{
    public interface IRenderCallbackFactory<TRenderParameter, TTile>
    {
        public IRenderCallback<TTile, TContext> CreateRenderer<TContext>(IRenderingFactoryConfig<TTile> tileSetSource, TRenderParameter p);
    }
}
