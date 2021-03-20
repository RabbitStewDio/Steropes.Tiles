using Steropes.Tiles.Renderer;
using Steropes.Tiles.Sample.Shared;

namespace Steropes.Tiles.Unit2D.Demo.Components
{
    public interface IRenderCallbackFactory<TRenderParameter, TTile>
    {
        public IRenderCallback<TTile, TContext> CreateRenderer<TContext>(IRenderingFactoryConfig<TTile> tileSetSource, TRenderParameter p);
    }
}
