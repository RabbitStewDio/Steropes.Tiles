using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Steropes.Tiles.Monogame
{
    public static class RenderStateUtil
    {
        public struct GraphicsState
        {
            readonly GraphicsDevice device;
            RasterizerState RasterizerState { get; }
            Rectangle ScissorRectangle { get; }

            public GraphicsState(GraphicsDevice device)
            {
                this.device = device;
                this.RasterizerState = device.RasterizerState;
                this.ScissorRectangle = device.ScissorRectangle;
                this.BlendState = device.BlendState;
            }

            BlendState BlendState { get; }

            public void RestoreState()
            {
                device.RasterizerState = RasterizerState;
                device.ScissorRectangle = ScissorRectangle;
                device.BlendState = BlendState;
            }
        }

        public static GraphicsState SaveState(this GraphicsDevice d)
        {
            return new GraphicsState(d);
        }

        public static RasterizerState Copy(this RasterizerState state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            return new RasterizerState()
            {
                ScissorTestEnable = state.ScissorTestEnable,
                CullMode = state.CullMode,
                DepthBias = state.DepthBias,
                SlopeScaleDepthBias = state.SlopeScaleDepthBias,
                DepthClipEnable = state.DepthClipEnable,
                FillMode = state.FillMode,
                MultiSampleAntiAlias = state.MultiSampleAntiAlias
            };
        }
    }
}
