using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.Renderer;
using Steropes.Tiles.Sample.Shared.Strategy.Model;
using Steropes.UI.Components;
using Steropes.UI.Widgets;
using Steropes.UI.Widgets.Container;


namespace Steropes.Tiles.MonogameDemo.GameData.Strategy
{
    /// <summary>
    ///  A specialised renderer that draws the city bar using a UI framework.
    ///  Some tasks are simply ill suited as tile based rendering.
    /// </summary>
    public class CityBarRenderer : IRenderCallback<ISettlement, Nothing>
    {
        readonly IRendererControl viewport;
        readonly Group parent;
        readonly Stack<CityBarWidget> pool;
        readonly Dictionary<MapCoordinate, CityBarWidget> widgetsByMapPosition;

        int epoch;
        DoublePoint offset;

        public CityBarRenderer(Group parent,
                               IRendererControl viewport)
        {
            this.viewport = viewport ?? throw new ArgumentNullException(nameof(viewport));
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            this.pool = new Stack<CityBarWidget>();
            this.widgetsByMapPosition = new Dictionary<MapCoordinate, CityBarWidget>();
        }

        public void StartDrawing()
        {
            unchecked
            {
                epoch += 1;
            }

            offset = viewport.CalculateTileOffset();
        }

        public void StartLine(int logicalLine, ContinuousViewportCoordinates screen)
        { }

        public void Draw(ISettlement tile,
                         Nothing context,
                         SpritePosition pos,
                         ContinuousViewportCoordinates screenLocation)
        {
            if (widgetsByMapPosition.TryGetValue(tile.Location, out CityBarWidget w))
            {
                w.Epoch = epoch;
                w.UpdateSettlementData();
                w.Anchor = Reposition(w, screenLocation);
            }
            else
            {
                w = pool.Count != 0 ? pool.Pop() : new CityBarWidget(parent.UIStyle);
                w.Epoch = epoch;
                w.Settlement = tile;
                w.UpdateSettlementData();
                parent.Add(w);
                w.Anchor = Reposition(w, screenLocation);
                widgetsByMapPosition[tile.Location] = w;
            }
        }

        AnchoredRect Reposition(IVisualContent w, ContinuousViewportCoordinates screenPos)
        {
            var ds = w.DesiredSize;
            var renderPos = screenPos.ToPixels(viewport.TileSize) + offset;
            return AnchoredRect.CreateTopLeftAnchored((int)(renderPos.X - ds.Width / 2), (int)(renderPos.Y - ds.Height * 2));
        }

        public void EndLine(int logicalLine, ContinuousViewportCoordinates screen)
        { }

        public void FinishedDrawing()
        {
            foreach (var pair in widgetsByMapPosition)
            {
                if (pair.Value.Epoch != epoch)
                {
                    parent.Remove(pair.Value);
                    pool.Push(pair.Value);
                }
            }

            foreach (var widget in pool)
            {
                widgetsByMapPosition.Remove(widget.Settlement.Location);
            }

            parent.Arrange(parent.LayoutRect);
        }

        public class CityBarWidget : InternalContentWidget<DockPanel>
        {
            ISettlement settlement;
            readonly Image sizeIcon;
            readonly Label nameLabel;
            readonly Label productionLabel;
            readonly Label productionTurns;
            readonly Label growthTurns;

            public ISettlement Settlement
            {
                get { return settlement; }
                set
                {
                    settlement = value;
                    UpdateSettlementData();
                }
            }

            public int Epoch { get; set; }

            public void UpdateSettlementData()
            {
                if (settlement == null)
                {
                    sizeIcon.Texture = null;
                    nameLabel.Text = "";
                    productionLabel.Text = "";
                    productionTurns.Text = "";
                    growthTurns.Text = "";
                }
                else
                {
                    nameLabel.Text = settlement.Name;
                    productionLabel.Text = "Fireworks";
                    productionTurns.Text = "-";
                    growthTurns.Text = settlement.Location.X.ToString();
                }
            }

            public CityBarWidget(IUIStyle style) : base(style)
            {
                sizeIcon = new Image(style);
                nameLabel = new Label(style);
                productionLabel = new Label(style);
                productionTurns = new Label(style);
                growthTurns = new Label(style);

                var center = new Grid(style);
                center.RowConstraints.Add(LengthConstraint.Auto);
                center.RowConstraints.Add(LengthConstraint.Auto);
                center.ColumnConstraints.Add(LengthConstraint.Auto);
                center.ColumnConstraints.Add(LengthConstraint.Relative(20));
                center.Add(nameLabel, new Point(0, 0));
                center.Add(growthTurns, new Point(1, 0));
                center.Add(productionLabel, new Point(0, 1));
                center.Add(productionTurns, new Point(1, 1));

                this.InternalContent = new DockPanel(style, false);
                InternalContent.Add(sizeIcon, DockPanelConstraint.Left);
                InternalContent.Add(center, DockPanelConstraint.Right);
            }

            public override string NodeType => nameof(CityBarWidget);
        }
    }
}
