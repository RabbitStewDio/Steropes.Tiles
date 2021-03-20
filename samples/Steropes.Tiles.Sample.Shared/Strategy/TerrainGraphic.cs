using System;
using System.Collections.Generic;

namespace Steropes.Tiles.Demo.Core.GameData.Strategy
{
    public class TerrainGraphic
    {
        public TerrainGraphic(string tag, bool blendLayer, string blendGraphicOverride = null)
        {
            GraphicTag = tag ?? throw new ArgumentNullException();
            DrawInBlendLayer = blendLayer;
            BlendGraphicOverride = blendGraphicOverride;
            MatchRule = new Dictionary<int, RenderLayerDefinition>();
        }

        public IReadOnlyDictionary<int, RenderLayerDefinition> MatchRule { get; private set; }
        public string GraphicTag { get; }

        public bool DrawInBlendLayer { get; set; }
        public string BlendGraphicOverride { get; }

        public TerrainGraphic WithMatchRule(int layer,
                                            TerrainMatchType matchType,
                                            string tag,
                                            string self = null,
                                            params string[] with)
        {
            if (layer < 0)
            {
                throw new ArgumentException();
            }

            MatchRule = new Dictionary<int, RenderLayerDefinition>((IDictionary<int, RenderLayerDefinition>)MatchRule)
            {
                [layer] = new RenderLayerDefinition(matchType, tag, self, new List<string>(with))
            };
            return this;
        }

        public TerrainGraphic WithMatch(int layer, string self)
        {
            if (layer < 0)
            {
                throw new ArgumentException();
            }

            MatchRule = new Dictionary<int, RenderLayerDefinition>((IDictionary<int, RenderLayerDefinition>)MatchRule)
            {
                [layer] = new RenderLayerDefinition(TerrainMatchType.None, null, self, new List<string>())
            };
            return this;
        }

        public string GetBlendGraphicFor(int layer)
        {
            if (!string.IsNullOrEmpty(BlendGraphicOverride))
            {
                return BlendGraphicOverride;
            }

            if (MatchRule.TryGetValue(layer, out RenderLayerDefinition d))
            {
                return d.Tag;
            }

            return null;
        }
    }
}
