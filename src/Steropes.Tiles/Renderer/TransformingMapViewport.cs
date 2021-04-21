using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.Plotter;

namespace Steropes.Tiles.Renderer
{
    public class TransformingMapViewport : IMapViewport
    {
        readonly MapViewport parent;
        Matrix3 cachedTransformation;
        Matrix3 cachedReverseTransformation;
        double rotation;
        ContinuousViewportCoordinates translation;
        bool cachedTransformationValid;

        public TransformingMapViewport(MapViewport parent)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            this.parent.PropertyChanged += OnParentPropertyChanged;
        }

        public ContinuousViewportCoordinates Translation
        {
            get { return translation; }
            set
            {
                if (value.Equals(translation))
                {
                    return;
                }

                translation = value;
                cachedTransformationValid = false;
                OnPropertyChanged();
            }
        }

        public double Rotation
        {
            get { return rotation; }
            set
            {
                if (value.Equals(rotation))
                {
                    return;
                }

                rotation = value;
                cachedTransformationValid = false;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RotationDeg));
            }
        }

        public double RotationDeg
        {
            get { return rotation * 180.0 / Math.PI; }
            set
            {
                Rotation = value * Math.PI / 180.0;
            }
        }

        public RenderType RenderType
        {
            get { return parent.RenderType; }
        }


        public MapCoordinate CenterPointInMapCoordinates
        {
            get { return parent.CenterPointInMapCoordinates; }
            set { parent.CenterPointInMapCoordinates = value; }
        }

        public ContinuousViewportCoordinates CenterPoint
        {
            get
            {
                EnsureValid();
                var cp = parent.CenterPoint;
                return cachedTransformation.Transform(cp);
            }
            set
            {
                EnsureValid();
                parent.CenterPoint = cachedReverseTransformation.Transform(value);
            }
        }

        public IntInsets Overdraw
        {
            get { return parent.Overdraw; }
            set { parent.Overdraw = value; }
        }

        public IntDimension SizeInTiles
        {
            get { return parent.SizeInTiles; }
            set { parent.SizeInTiles = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnParentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        void EnsureValid()
        {
            if (!cachedTransformationValid)
            {
                cachedReverseTransformation = Matrix3.Multiply(Matrix3.Translate(-translation.X, -translation.Y),
                                                               Matrix3.Rotation(-rotation));

                cachedTransformation = Matrix3.Multiply(Matrix3.Rotation(rotation),
                                                        Matrix3.Translate(translation.X, translation.Y));

                cachedTransformationValid = true;
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            EnsureValid();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ContinuousViewportCoordinates CenterPointOffset
        {
            get
            {
                EnsureValid();
                return cachedTransformation.Transform(parent.CenterPointOffset);
            }
        }

        public DoublePoint ScreenPositionToMapPosition(in ContinuousViewportCoordinates screenPosition)
        {
            EnsureValid();
            return parent.ScreenPositionToMapPosition(cachedTransformation.Transform(screenPosition));
        }

        public MapCoordinate ScreenPositionToMapCoordinate(in ViewportCoordinates screenPosition)
        {
            EnsureValid();
            return parent.ScreenPositionToMapCoordinate(cachedTransformation.Transform(screenPosition));
        }

        public ContinuousViewportCoordinates MapPositionToScreenPosition(in DoublePoint mapPosition)
        {
            EnsureValid();
            return cachedReverseTransformation.Transform(parent.MapPositionToScreenPosition(mapPosition));
        }

        public ViewportCoordinates MapPositionToScreenPosition(in MapCoordinate mapPosition)
        {
            EnsureValid();
            return cachedReverseTransformation.Transform(parent.MapPositionToScreenPosition(mapPosition));
        }

        public IntDimension RenderedArea => MapViewportBaseCalculations.RenderedAreaOf(RenderInsets);

        public IntInsets RenderInsets
        {
            get { return parent.RenderInsets; }
        }
    }
}
