using Avalonia.Media;
using SkiaSharp;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;
using System;
using System.Collections.Generic;

namespace Steropes.Tiles.TemplateGen.Models.Rendering.Shapes
{
    public class PixelPerfectIsoShape : IShape
    {
        public IntPoint Top { get; }
        public IntPoint Left { get; }
        public IntPoint Bottom { get; }
        public IntPoint Right { get; }

        public PixelPerfectIsoShape(IntPoint top, IntPoint left, IntPoint bottom, IntPoint right)
        {
            this.Top = top;
            this.Left = left;
            this.Bottom = bottom;
            this.Right = right;
        }

        public void Draw(SKCanvas g, Color pen)
        {
            g.DrawRasterLine(pen, Left.X, Left.Y - 1, Top.X - 1, Top.Y);
            g.DrawRasterLine(pen, Right.X, Right.Y - 1, Top.X, Top.Y);
            
            g.DrawRasterLine(pen, Left.X, Left.Y, Bottom.X - 1, Bottom.Y);
            g.DrawRasterLine(pen, Right.X, Right.Y, Bottom.X, Bottom.Y);
        }

        public IShape ToHighlight()
        {
            return new PixelPerfectIsoShape(new IntPoint(Top.X, Top.Y + 2),
                                            new IntPoint(Left.X + 4, Left.Y),
                                            new IntPoint(Bottom.X, Bottom.Y - 2),
                                            new IntPoint(Right.X - 4, Right.Y));
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
                    points.Add(new IntPoint(Top.X - 1, Top.Y));
                    points.Add(new IntPoint(Right.X - 1, Right.Y - 1));
                    break;
                case NeighbourIndex.NorthEast:
                    points.Add(MidPoint(new IntPoint(Right.X - 1, Right.Y - 1),
                                        new IntPoint(Top.X - 1, Top.Y)));
                    points.Add(new IntPoint(Right.X - 1, Right.Y - 1));
                    points.Add(new IntPoint(Right.X - 1, Right.Y));
                    points.Add(MidPoint(new IntPoint(Right.X - 1, Right.Y),
                                        new IntPoint(Bottom.X - 1, Bottom.Y)));
                    break;
                case NeighbourIndex.East:
                    points.Add(new IntPoint(Right.X - 1, Right.Y));
                    points.Add(new IntPoint(Bottom.X - 1, Bottom.Y));
                    break;
                case NeighbourIndex.SouthEast:
                    points.Add(MidPoint(new IntPoint(Right.X - 1, Right.Y),
                                        new IntPoint(Bottom.X - 1, Bottom.Y)));
                    points.Add(new IntPoint(Bottom.X - 1, Bottom.Y));
                    points.Add(new IntPoint(Bottom.X, Bottom.Y));
                    points.Add(MidPoint(new IntPoint(Left.X + 1, Left.Y),
                                        new IntPoint(Bottom.X, Bottom.Y)));
                    break;
                case NeighbourIndex.South:
                    points.Add(new IntPoint(Bottom.X, Bottom.Y));
                    points.Add(new IntPoint(Left.X + 1, Left.Y));
                    break;
                case NeighbourIndex.SouthWest:
                    points.Add(MidPoint(new IntPoint(Left.X + 1, Left.Y),
                                        new IntPoint(Bottom.X, Bottom.Y)));
                    points.Add(new IntPoint(Left.X + 1, Left.Y));
                    points.Add(new IntPoint(Left.X + 1, Left.Y - 1));
                    points.Add(MidPoint(new IntPoint(Left.X + 1, Left.Y - 1),
                                        new IntPoint(Top.X, Top.Y)));
                    break;
                case NeighbourIndex.West:
                    points.Add(new IntPoint(Left.X + 1, Left.Y - 1));
                    points.Add(new IntPoint(Top.X, Top.Y));
                    break;
                case NeighbourIndex.NorthWest:
                    points.Add(MidPoint(new IntPoint(Left.X + 1, Left.Y - 1),
                                        new IntPoint(Top.X, Top.Y)));
                    points.Add(new IntPoint(Top.X, Top.Y));
                    points.Add(new IntPoint(Top.X - 1, Top.Y));
                    points.Add(MidPoint(new IntPoint(Right.X - 1, Right.Y - 1),
                                        new IntPoint(Top.X - 1, Top.Y)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(idx), idx, null);
            }

            return points;
        }
    }
}
