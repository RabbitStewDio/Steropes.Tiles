using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.Plotter;

namespace Steropes.Tiles.Renderer
{
    /// <summary>
    ///   Manages the mapping between screen coordinates and tiles. All units given are in "tile-units".
    ///   A tile unit of 1 is the width or height of a single tile.
    /// </summary>
    public class MapViewport : IMapViewport
    {
        readonly ContinuousScreenToMapMapper contMapToMapMapper;
        readonly ContinuousMapToScreenMapper contMapToScreenMapper;
        readonly ScreenToMapMapper mapToMapMapper;
        readonly MapToScreenMapper mapToScreenMapper;
        ContinuousViewportCoordinates centerPoint;
        IntInsets overdraw;
        IntInsets renderInsets;
        IntDimension size;

        public MapViewport(RenderType renderType)
        {
            RenderType = renderType;
            overdraw = new IntInsets(0, 0, 0, 0);
            mapToScreenMapper = ScreenCoordinateMapping.CreateMapToScreenMapper(renderType);
            contMapToScreenMapper = ScreenCoordinateMapping.CreateToContinuousMapToScreenMapper(renderType);
            mapToMapMapper = ScreenCoordinateMapping.CreateToMapMapper(renderType);
            contMapToMapMapper = ScreenCoordinateMapping.CreateToContinuousMapMapper(renderType);
        }

        /// <summary>
        ///   Use this to define an rendering area that is larger than the visible
        ///   portion of the map. The overdraw can be defined separately for each
        ///   direction. If your tile is anchored on the bottom of the texture
        ///   (and therefore covers other tiles above it, like a large tree does),
        ///   define an 'bottom' overflow large enough to guarantee the rendering
        ///   of such tiles.
        /// </summary>
        /// <para>
        ///   Sometimes tiles render beyond their grid boundaries, for instance when
        ///   they display larger objects that cover a lot of screen space (like
        ///   large trees, huge monsters etc). If those tiles are rendered just beyond the
        ///   edge of the visible space, parts of the graphics will be visible on screen.
        ///   If you don't overdraw sufficiently, it will look as if the tile just
        ///   pops up randomly when the user scrolls.
        /// </para>
        /// <para>
        ///   Choose the overdraw to be 2 times the largest overdraw (one for each edge in
        ///   each direction).  So if you have a standard tile set size of 32px, and the
        ///   largest tile  (lets say a huge tree) has a width of 96px, then your overdraw
        ///   needs  to be 'Ceil((96/2)/32)' to ensure that that huge tree tile is rendered
        ///   if there is a chance that it might be partially visible on screen.
        /// </para>
        public IntInsets Overdraw
        {
            get { return overdraw; }
            set
            {
                if (value.Equals(overdraw))
                {
                    return;
                }

                overdraw = value;
                RefreshViewport();
                OnPropertyChanged();
            }
        }

        public RenderType RenderType { get; }

        public ContinuousViewportCoordinates CenterPoint
        {
            get { return centerPoint; }
            set
            {
                if (value.Equals(centerPoint))
                {
                    return;
                }

                centerPoint = value;
                RefreshViewport();
                OnPropertyChanged();
            }
        }

        public IntDimension SizeInTiles
        {
            get { return size; }
            set
            {
                if (value.Equals(size))
                {
                    return;
                }

                size = value;
                RefreshViewport();
                OnPropertyChanged();
            }
        }

        public IntInsets RenderInsets
        {
            get { return renderInsets; }
            private set
            {
                if (value.Equals(renderInsets))
                {
                    return;
                }

                renderInsets = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RenderedArea));
            }
        }

        public IntDimension RenderedArea
        {
            get { return MapViewportBaseCalculations.RenderedAreaOf(RenderInsets); }
        }

        public MapCoordinate CenterPointInMapCoordinates
        {
            get { return mapToMapMapper(centerPoint.X, centerPoint.Y); }
            set { CenterPoint = MapPositionToScreenPosition(value); }
        }

        public ViewportCoordinates MapPositionToScreenPosition(MapCoordinate c)
        {
            return mapToScreenMapper(c.X, c.Y);
        }

        public ContinuousViewportCoordinates CenterPointOffset
        {
            get
            {
                var cpX = CenterPoint.X;
                var cpY = CenterPoint.Y;

                // Map coordinates are always integers. For a smooth view we need a way to interpolate
                // positions. However, the IsoStaggered type has no linear coordinates on the map, so
                // we cannot simply draw a straight line from one tile to the other - we have to roundtrip
                // into the map and back and check the difference.
                var centreViewCoords = CenterPointInMapCoordinates;
                var sc = MapPositionToScreenPosition(centreViewCoords);

                var deltaX = Math.Round(cpX - sc.X, 9);
                var deltaY = Math.Round(cpY - sc.Y, 9);
                return new ContinuousViewportCoordinates(deltaX, deltaY);
            }
        }

        public ContinuousViewportCoordinates MapPositionToScreenPosition(DoublePoint mapPosition)
        {
            return contMapToScreenMapper(mapPosition.X, mapPosition.Y);
        }

        public DoublePoint ScreenPositionToMapPosition(ContinuousViewportCoordinates screenPosition)
        {
            return contMapToMapMapper(screenPosition.X, screenPosition.Y);
        }

        public MapCoordinate ScreenPositionToMapCoordinate(ViewportCoordinates screenPosition)
        {
            return mapToMapMapper(screenPosition.X, screenPosition.Y);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void RefreshViewport()
        {
            RenderInsets = MapViewportBaseCalculations.EnsureViewportValid(SizeInTiles, Overdraw);
            ;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
