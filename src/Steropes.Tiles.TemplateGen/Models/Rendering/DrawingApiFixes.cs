using Avalonia.Media;
using Serilog;
using SkiaSharp;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.TemplateGen.Models.Prefs;
using System;
using System.Collections.Generic;
using System.IO;

namespace Steropes.Tiles.TemplateGen.Models.Rendering
{
    public static class DrawingApiFixes
    {
        public static readonly ILogger Logger = SLog.ForContext(typeof(DrawingApiFixes));
        
        public static MemoryStream Write(this SKBitmap bitmap, SKEncodedImageFormat fmt = SKEncodedImageFormat.Png, int quality = 100)
        {
            var memStream = new MemoryStream();
            using var wstream = new SKManagedWStream(memStream);

            bitmap.Encode(wstream, fmt, quality);
            memStream.Position = 0;
            return memStream;
        }

        public static void DrawRectangle(this SKCanvas c, Color color, IntRect rectangle)
        {
            using var paint = new SKPaint()
            {
                Color = new SKColor(color.ToUint32()),
                IsAntialias = false,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1
            };
            c.DrawRect(new SKRect(rectangle.X, rectangle.Y, rectangle.X + rectangle.Width - 1, rectangle.Y + rectangle.Height - 1), paint);
        }

        public static void FillRectangle(this SKCanvas c, Color color, IntRect rectangle)
        {
            using var paint = new SKPaint()
            {
                Color = new SKColor(color.ToUint32()),
                IsAntialias = false,
                Style = SKPaintStyle.Fill
            };
            c.DrawRect(new SKRect(rectangle.X, rectangle.Y, rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height), paint);
        }

        public static void DrawLine(this SKCanvas g, Color color, float x1, float y1, float x2, float y2)
        {
            using var paint = new SKPaint()
            {
                Color = new SKColor(color.ToUint32()),
                IsAntialias = false,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1,
                StrokeCap = SKStrokeCap.Round
            };

            Logger.Verbose("DrawLine ({P1}) -> ({P2})", new DoublePoint(x1, y1), new DoublePoint(x2, y2));
            //g.DrawLine(x1 - 0.5f, y1 - 0.5f, x2 - 0.5f, y2 - 0.5f, paint);
            g.DrawLine(new SKPoint(x1, y1), new SKPoint(x2, y2), paint);
        }

        public static void DrawRasterLine(this SKCanvas g, Color color, IntPoint p1, IntPoint p2) => DrawRasterLine(g, color, p1.X, p1.Y, p2.X, p2.Y);

        public static void DrawRasterLine(this SKCanvas g, Color color, float x1, float y1, float x2, float y2)
        {
            using var paint = new SKPaint()
            {
                Color = new SKColor(color.ToUint32()),
                IsAntialias = false,
                Style = SKPaintStyle.Fill,
                StrokeWidth = 0,
                StrokeCap = SKStrokeCap.Round
            };


            g.DrawRasterLine(paint, x1, y1, x2, y2);
        }

        static void DrawLineLow(SKCanvas g, SKPaint paint, float x1, float y1, float x2, float y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            var incr = 1;
            if (dy < 0)
            {
                incr = -1;
                dy = -dy;
            }

            var delta = (2 * dy) - dx;
            var y = y1;
            for (var x = x1; x <= x2; x += 1)
            {
                DrawPixel(g, paint, x, y);
                if (delta > 0)
                {
                    y += incr;
                    delta += 2 * (dy - dx);
                }
                else
                {
                    delta += 2 * dy;
                }
            }
        }

        static void DrawLineHigh(SKCanvas g, SKPaint paint, float x1, float y1, float x2, float y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            var incr = 1;
            if (dx < 0)
            {
                incr = -1;
                dx = -dx;
            }

            var delta = (2 * dx) - dy;
            var x = x1;
            for (var y = y1; y <= y2; y += 1)
            {
                g.DrawPixel(paint, x, y);
                if (delta > 0)
                {
                    x += incr;
                    delta += 2 * (dx - dy);
                }
                else
                {
                    delta += 2 * dx;
                }
            }
        }

        public static void DrawPixel(this SKCanvas g, Color color, int x1, int y1)
        {
            g.DrawRectangle(color, new IntRect(x1, y1, 1, 1));
        }
        
        static void DrawPixel(this SKCanvas g, SKPaint paint, float x1, float y1)
        {
            // cheesy way of drawing a single pixel.
            g.DrawRect(x1, y1, 1, 1, paint);
        }

        static void DrawRasterLine(this SKCanvas g, SKPaint paint, float x1, float y1, float x2, float y2)
        {
            if (Math.Abs(y2 - y1) < Math.Abs(x2 - x1))
            {
                if (x1 > x2)
                {
                    DrawLineLow(g, paint, x2, y2, x1, y1);
                }
                else
                {
                    DrawLineLow(g, paint, x1, y1, x2, y2);
                }
            }
            else
            {
                if (y1 > y2)
                {
                    DrawLineHigh(g, paint, x2, y2, x1, y1);
                }
                else
                {
                    DrawLineHigh(g, paint, x1, y1, x2, y2);
                }
            }
        }


        public static void DrawLine(this SKCanvas g, Color color, IntPoint p1, IntPoint p2)
        {
            DrawLine(g, color, p1.X, p1.Y, p2.X, p2.Y);
        }

        public static IntDimension MeasureText(this Font font, string title, float lineWidth)
        {
            using var paint = new SKPaint()
            {
                Typeface = font.ToSkia(),
                TextSize = font.Size,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            var bounds = new SKRect();
            paint.MeasureText(title, ref bounds);

            var width = 0f;
            var height = 0f;
            while (true)
            {
                var offset = paint.BreakText(title, lineWidth, out var measuredWidth, out _);
                width = Math.Max(width, measuredWidth);
                height += bounds.Height;
                if (offset == 0)
                {
                    break;
                }

                title = title.Substring((int)offset);
            }

            return new IntDimension((int)Math.Ceiling(width), (int)Math.Ceiling(height));
        }

        public static void DrawTextLines(this SKCanvas g, string title, Font font, Color brush, IntPoint textOffset, int lineWidth)
        {
            using var paint = new SKPaint()
            {
                Typeface = font.ToSkia(),
                TextSize = font.Size,
                Color = new SKColor(brush.ToUint32()),
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            while (true)
            {
                var offset = paint.BreakText(title, lineWidth, out _, out var measuredText);
                g.DrawText(measuredText, new SKPoint(textOffset.X, textOffset.Y + paint.TextSize), paint);
                textOffset = new IntPoint(textOffset.X, textOffset.Y + (int)paint.TextSize);

                if (offset == 0)
                {
                    break;
                }

                title = title.Substring((int)offset);
            }
        }

        public static void DrawGeometry(this SKCanvas g, Color color, List<IntPoint> p)
        {
            using var paint = new SKPaint()
            {
                Color = new SKColor(color.ToUint32()),
                IsAntialias = false,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1
            };

            for (var i = 1; i < p.Count; i++)
            {
                var prev = p[i - 1];
                var curr = p[i];
                Logger.Verbose("Draw Polygon {P1} -> {P2}", prev, curr);
                g.DrawRasterLine(color, prev, curr);
            }
        }
    }
}
