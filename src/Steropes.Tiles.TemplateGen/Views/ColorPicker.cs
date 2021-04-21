using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Media;
using Steropes.Tiles.TemplateGen.ColorPicker;
using System;
using System.Globalization;

namespace Steropes.Tiles.TemplateGen.Views
{
    public class ColorPicker : TemplatedControl
    {
        class HsvValueConverters
        {
            public IValueConverter Value1Converter { get; } = new HueConverter();

            public IValueConverter Value2Converter { get; } = new SaturationConverter();

            public IValueConverter Value3Converter { get; } = new ValueConverter();

            public IValueConverter Value4Converter { get; } = new AlphaConverter();
        }

        public static readonly StyledProperty<double> Value1Property =
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Value1));

        public static readonly StyledProperty<double> Value2Property =
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Value2));

        public static readonly StyledProperty<double> Value3Property =
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Value3));

        public static readonly StyledProperty<double> Value4Property =
            AvaloniaProperty.Register<ColorPicker, double>(nameof(Value4));

        public static readonly StyledProperty<Color> ColorProperty =
            AvaloniaProperty.Register<ColorPicker, Color>(nameof(Color));

        Canvas? colorCanvas;
        Thumb? colorThumb;
        Canvas? hueCanvas;
        Thumb? hueThumb;
        Canvas? alphaCanvas;
        Thumb? alphaThumb;
        bool updating;
        bool captured;
        readonly HsvValueConverters converters = new HsvValueConverters();

        public ColorPicker()
        {
            this.GetObservable(Value1Property).Subscribe(_ => OnValueChange());
            this.GetObservable(Value2Property).Subscribe(_ => OnValueChange());
            this.GetObservable(Value3Property).Subscribe(_ => OnValueChange());
            this.GetObservable(Value4Property).Subscribe(_ => OnValueChange());
            this.GetObservable(ColorProperty).Subscribe(_ => OnColorChange());
        }

        public double Value1
        {
            get { return GetValue(Value1Property); }
            set { SetValue(Value1Property, value); }
        }

        public double Value2
        {
            get { return GetValue(Value2Property); }
            set { SetValue(Value2Property, value); }
        }

        public double Value3
        {
            get { return GetValue(Value3Property); }
            set { SetValue(Value3Property, value); }
        }

        public double Value4
        {
            get { return GetValue(Value4Property); }
            set { SetValue(Value4Property, value); }
        }

        public Color Color
        {
            get { return GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            if (colorCanvas != null)
            {
                colorCanvas.PointerPressed -= ColorCanvas_PointerPressed;
                colorCanvas.PointerReleased -= ColorCanvas_PointerReleased;
                colorCanvas.PointerMoved -= ColorCanvas_PointerMoved;
            }

            if (colorThumb != null)
            {
                colorThumb.DragDelta -= ColorThumb_DragDelta;
            }

            if (hueCanvas != null)
            {
                hueCanvas.PointerPressed -= HueCanvas_PointerPressed;
                hueCanvas.PointerReleased -= HueCanvas_PointerReleased;
                hueCanvas.PointerMoved -= HueCanvas_PointerMoved;
            }

            if (hueThumb != null)
            {
                hueThumb.DragDelta -= HueThumb_DragDelta;
            }

            if (alphaCanvas != null)
            {
                alphaCanvas.PointerPressed -= AlphaCanvas_PointerPressed;
                alphaCanvas.PointerReleased -= AlphaCanvas_PointerReleased;
                alphaCanvas.PointerMoved -= AlphaCanvas_PointerMoved;
            }

            if (alphaThumb != null)
            {
                alphaThumb.DragDelta -= AlphaThumb_DragDelta;
            }

            colorCanvas = e.NameScope.Find<Canvas>("PART_ColorCanvas");
            colorThumb = e.NameScope.Find<Thumb>("PART_ColorThumb");
            hueCanvas = e.NameScope.Find<Canvas>("PART_HueCanvas");
            hueThumb = e.NameScope.Find<Thumb>("PART_HueThumb");
            alphaCanvas = e.NameScope.Find<Canvas>("PART_AlphaCanvas");
            alphaThumb = e.NameScope.Find<Thumb>("PART_AlphaThumb");

            if (colorCanvas != null)
            {
                colorCanvas.PointerPressed += ColorCanvas_PointerPressed;
                colorCanvas.PointerReleased += ColorCanvas_PointerReleased;
                colorCanvas.PointerMoved += ColorCanvas_PointerMoved;
            }

            if (colorThumb != null)
            {
                colorThumb.DragDelta += ColorThumb_DragDelta;
            }

            if (hueCanvas != null)
            {
                hueCanvas.PointerPressed += HueCanvas_PointerPressed;
                hueCanvas.PointerReleased += HueCanvas_PointerReleased;
                hueCanvas.PointerMoved += HueCanvas_PointerMoved;
            }

            if (hueThumb != null)
            {
                hueThumb.DragDelta += HueThumb_DragDelta;
            }

            if (alphaCanvas != null)
            {
                alphaCanvas.PointerPressed += AlphaCanvas_PointerPressed;
                alphaCanvas.PointerReleased += AlphaCanvas_PointerReleased;
                alphaCanvas.PointerMoved += AlphaCanvas_PointerMoved;
            }

            if (alphaThumb != null)
            {
                alphaThumb.DragDelta += AlphaThumb_DragDelta;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);
            OnColorChange();
            return size;
        }

        bool IsTemplateValid()
        {
            return colorCanvas != null
                   && colorThumb != null
                   && hueCanvas != null
                   && hueThumb != null
                   && alphaCanvas != null
                   && alphaThumb != null;
        }

        double Clamp(double val, double min, double max)
        {
            return Math.Min(Math.Max(val, min), max);
        }

        void MoveThumb(Canvas? canvas, Thumb? thumb, double x, double y)
        {
            if (canvas != null && thumb != null)
            {
                double left = Clamp(x, 0, canvas.Bounds.Width);
                double top = Clamp(y, 0, canvas.Bounds.Height);
                Canvas.SetLeft(thumb, left);
                Canvas.SetTop(thumb, top);
            }
        }

        T Convert<T>(IValueConverter converter, T value, T range)
        {
            return (T)converter.Convert(value, typeof(T), range, CultureInfo.CurrentCulture);
        }

        T ConvertBack<T>(IValueConverter converter, T value, T range)
        {
            return (T)converter.ConvertBack(value, typeof(T), range, CultureInfo.CurrentCulture);
        }

        double GetValue1Range() => hueCanvas?.Bounds.Height ?? 0.0;

        double GetValue2Range() => colorCanvas?.Bounds.Width ?? 0.0;

        double GetValue3Range() => colorCanvas?.Bounds.Height ?? 0.0;

        double GetValue4Range() => alphaCanvas?.Bounds.Width ?? 0.0;

        void UpdateThumbsFromColor()
        {
            ColorHelpers.FromColor(Color, out double h, out double s, out double v, out double a);
            double hueY = Convert(converters.Value1Converter, h, GetValue1Range());
            double colorX = Convert(converters.Value2Converter, s, GetValue2Range());
            double colorY = Convert(converters.Value3Converter, v, GetValue3Range());
            double alphaX = Convert(converters.Value4Converter, a, GetValue4Range());
            MoveThumb(hueCanvas, hueThumb, 0, hueY);
            MoveThumb(colorCanvas, colorThumb, colorX, colorY);
            MoveThumb(alphaCanvas, alphaThumb, alphaX, 0);
        }

        void UpdateThumbsFromValues()
        {
            double hueY = Convert(converters.Value1Converter, Value1, GetValue1Range());
            double colorX = Convert(converters.Value2Converter, Value2, GetValue2Range());
            double colorY = Convert(converters.Value3Converter, Value3, GetValue3Range());
            double alphaX = Convert(converters.Value4Converter, Value4, GetValue4Range());
            MoveThumb(hueCanvas, hueThumb, 0, hueY);
            MoveThumb(colorCanvas, colorThumb, colorX, colorY);
            MoveThumb(alphaCanvas, alphaThumb, alphaX, 0);
        }

        void UpdateValuesFromThumbs()
        {
            double hueY = Canvas.GetTop(hueThumb);
            double colorX = Canvas.GetLeft(colorThumb);
            double colorY = Canvas.GetTop(colorThumb);
            double alphaX = Canvas.GetLeft(alphaThumb);
            Value1 = ConvertBack(converters.Value1Converter, hueY, GetValue1Range());
            Value2 = ConvertBack(converters.Value2Converter, colorX, GetValue2Range());
            Value3 = ConvertBack(converters.Value3Converter, colorY, GetValue3Range());
            Value4 = ConvertBack(converters.Value4Converter, alphaX, GetValue4Range());
            Color = ColorHelpers.FromHSVA(Value1, Value2, Value3, Value4);
        }

        void UpdateColorFromThumbs()
        {
            double hueY = Canvas.GetTop(hueThumb);
            double colorX = Canvas.GetLeft(colorThumb);
            double colorY = Canvas.GetTop(colorThumb);
            double alphaX = Canvas.GetLeft(alphaThumb);
            double h = ConvertBack(converters.Value1Converter, hueY, GetValue1Range());
            double s = ConvertBack(converters.Value2Converter, colorX, GetValue2Range());
            double v = ConvertBack(converters.Value3Converter, colorY, GetValue3Range());
            double a = ConvertBack(converters.Value4Converter, alphaX, GetValue4Range());
            Color = ColorHelpers.FromHSVA(h, s, v, a);
        }

        void OnValueChange()
        {
            if (updating == false && IsTemplateValid())
            {
                updating = true;
                UpdateThumbsFromValues();
                UpdateValuesFromThumbs();
                UpdateColorFromThumbs();
                updating = false;
            }
        }

        void OnColorChange()
        {
            if (updating == false && IsTemplateValid())
            {
                updating = true;
                UpdateThumbsFromColor();
                UpdateValuesFromThumbs();
                UpdateColorFromThumbs();
                updating = false;
            }
        }

        void ColorCanvas_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var position = e.GetPosition(colorCanvas);
            updating = true;
            MoveThumb(colorCanvas, colorThumb, position.X, position.Y);
            UpdateValuesFromThumbs();
            UpdateColorFromThumbs();
            updating = false;
            captured = true;
        }

        void ColorCanvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (captured)
            {
                captured = false;
            }
        }

        void ColorCanvas_PointerMoved(object? sender, PointerEventArgs e)
        {
            if (captured)
            {
                var position = e.GetPosition(colorCanvas);
                updating = true;
                MoveThumb(colorCanvas, colorThumb, position.X, position.Y);
                UpdateValuesFromThumbs();
                UpdateColorFromThumbs();
                updating = false;
            }
        }

        void ColorThumb_DragDelta(object? sender, VectorEventArgs e)
        {
            double left = Canvas.GetLeft(colorThumb);
            double top = Canvas.GetTop(colorThumb);
            updating = true;
            MoveThumb(colorCanvas, colorThumb, left + e.Vector.X, top + e.Vector.Y);
            UpdateValuesFromThumbs();
            UpdateColorFromThumbs();
            updating = false;
        }

        void HueCanvas_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var position = e.GetPosition(hueCanvas);
            updating = true;
            MoveThumb(hueCanvas, hueThumb, 0, position.Y);
            UpdateValuesFromThumbs();
            UpdateColorFromThumbs();
            updating = false;
            captured = true;
        }

        void HueCanvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (captured)
            {
                captured = false;
            }
        }

        void HueCanvas_PointerMoved(object? sender, PointerEventArgs e)
        {
            if (captured)
            {
                var position = e.GetPosition(hueCanvas);
                updating = true;
                MoveThumb(hueCanvas, hueThumb, 0, position.Y);
                UpdateValuesFromThumbs();
                UpdateColorFromThumbs();
                updating = false;
            }
        }

        void HueThumb_DragDelta(object? sender, VectorEventArgs e)
        {
            double top = Canvas.GetTop(hueThumb);
            updating = true;
            MoveThumb(hueCanvas, hueThumb, 0, top + e.Vector.Y);
            UpdateValuesFromThumbs();
            UpdateColorFromThumbs();
            updating = false;
        }

        void AlphaCanvas_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var position = e.GetPosition(alphaCanvas);
            updating = true;
            MoveThumb(alphaCanvas, alphaThumb, position.X, 0);
            UpdateValuesFromThumbs();
            UpdateColorFromThumbs();
            updating = false;
            captured = true;
        }

        void AlphaCanvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            if (captured)
            {
                captured = false;
            }
        }

        void AlphaCanvas_PointerMoved(object? sender, PointerEventArgs e)
        {
            if (captured)
            {
                var position = e.GetPosition(alphaCanvas);
                updating = true;
                MoveThumb(alphaCanvas, alphaThumb, position.X, 0);
                UpdateValuesFromThumbs();
                UpdateColorFromThumbs();
                updating = false;
            }
        }

        void AlphaThumb_DragDelta(object? sender, VectorEventArgs e)
        {
            double left = Canvas.GetLeft(alphaThumb);
            updating = true;
            MoveThumb(alphaCanvas, alphaThumb, left + e.Vector.X, 0);
            UpdateValuesFromThumbs();
            UpdateColorFromThumbs();
            updating = false;
        }
    }
}
