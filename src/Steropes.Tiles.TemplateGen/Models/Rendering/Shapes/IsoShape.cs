using Avalonia.Media;
using SkiaSharp;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;
using System;
using System.Collections.Generic;

namespace Steropes.Tiles.TemplateGen.Models.Rendering
{
    public class IsoShape : IShape
    {
        public IntPoint Top { get; }
        public IntPoint Left { get; }
        public IntPoint Bottom { get; }
        public IntPoint Right { get; }

        public IsoShape(IntPoint top, IntPoint left, IntPoint bottom, IntPoint right)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public void Draw(SKCanvas g, Color pen)
        {
            g.DrawLine(pen, Left, Top);
            g.DrawLine(pen, Right, Top);
            g.DrawLine(pen, Right, Bottom);
            g.DrawLine(pen, Left, Bottom);
        }

        public IShape ToHighlight()
        {
            var hy = 2;
            var width = Math.Max(1, Right.X - Left.X + 1);
            var height = Math.Max(1, Bottom.X - Top.X + 1);
            var hx = width * hy / height;

            var top = new IntPoint(Top.X, Top.Y + hy);
            var left = new IntPoint(Left.X + hx, Left.Y);
            var bottom = new IntPoint(Bottom.X, Bottom.Y - hy);
            var right = new IntPoint(Right.X - hx, Right.Y);
            return new IsoShape(top, left, bottom, right);
        }

        IntPoint MidPoint(IntPoint p1, IntPoint p2)
        {
            return new IntPoint((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }

        public List<IntPoint> GetHighlightFor(NeighbourIndex idx)
        {
            var points = new List<IntPoint>();
            switch (idx)
            {
                case NeighbourIndex.North:
                    points.Add(Top);
                    points.Add(Right);
                    break;
                case NeighbourIndex.NorthEast:
                    points.Add(MidPoint(Right, Top));
                    points.Add(Right);
                    points.Add(MidPoint(Right, Bottom));
                    break;
                case NeighbourIndex.East:
                    points.Add(Right);
                    points.Add(Bottom);
                    break;
                case NeighbourIndex.SouthEast:
                    points.Add(MidPoint(Right, Bottom));
                    points.Add(Bottom);
                    points.Add(MidPoint(Left, Bottom));
                    break;
                case NeighbourIndex.South:
                    points.Add(Bottom);
                    points.Add(Left);
                    break;
                case NeighbourIndex.SouthWest:
                    points.Add(MidPoint(Left, Bottom));
                    points.Add(Left);
                    points.Add(MidPoint(Left, Top));
                    break;
                case NeighbourIndex.West:
                    points.Add(Left);
                    points.Add(Top);
                    break;
                case NeighbourIndex.NorthWest:
                    points.Add(MidPoint(Left, Top));
                    points.Add(Top);
                    points.Add(MidPoint(Right, Top));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(idx), idx, null);
            }

            return points;
        }
    }
}
