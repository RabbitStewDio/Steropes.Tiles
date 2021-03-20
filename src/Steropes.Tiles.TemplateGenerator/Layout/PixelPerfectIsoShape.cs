using System;
using System.Collections.Generic;
using System.Drawing;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.TemplateGenerator.Layout
{
    public class PixelPerfectIsoShape : IShape
    {
        public Point Top { get; }
        public Point Left { get; }
        public Point Bottom { get; }
        public Point Right { get; }

        public PixelPerfectIsoShape(Point top, Point left, Point bottom, Point right)
        {
            this.Top = top;
            this.Left = left;
            this.Bottom = bottom;
            this.Right = right;
        }

        public void Draw(Graphics g, Pen pen)
        {
            g.DrawLine(pen, Left.X + 1, Left.Y - 1, Top.X, Top.Y);
            g.DrawLine(pen, Right.X - 1, Right.Y - 1, Top.X - 1, Top.Y);

            g.DrawLine(pen, Left.X + 1, Left.Y, Bottom.X, Bottom.Y);
            g.DrawLine(pen, Right.X - 1, Right.Y, Bottom.X - 1, Bottom.Y);
        }

        public IShape ToHighlight()
        {
            return new PixelPerfectIsoShape(new Point(Top.X, Top.Y + 2),
                                            new Point(Left.X + 4, Left.Y),
                                            new Point(Bottom.X, Bottom.Y - 2),
                                            new Point(Right.X - 4, Right.Y));
        }

        Point MidPoint(Point p1, Point p2)
        {
            return new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
        }

        public List<Point> GetHighlightFor(NeighbourIndex idx)
        {
            var points = new List<Point>();
            switch (idx)
            {
                case NeighbourIndex.North:
                    points.Add(new Point(Top.X - 1, Top.Y));
                    points.Add(new Point(Right.X - 1, Right.Y - 1));
                    break;
                case NeighbourIndex.NorthEast:
                    points.Add(MidPoint(new Point(Right.X - 1, Right.Y - 1),
                                        new Point(Top.X - 1, Top.Y)));
                    points.Add(new Point(Right.X - 1, Right.Y - 1));
                    points.Add(new Point(Right.X - 1, Right.Y));
                    points.Add(MidPoint(new Point(Right.X - 1, Right.Y),
                                        new Point(Bottom.X - 1, Bottom.Y)));
                    break;
                case NeighbourIndex.East:
                    points.Add(new Point(Right.X - 1, Right.Y));
                    points.Add(new Point(Bottom.X - 1, Bottom.Y));
                    break;
                case NeighbourIndex.SouthEast:
                    points.Add(MidPoint(new Point(Right.X - 1, Right.Y),
                                        new Point(Bottom.X - 1, Bottom.Y)));
                    points.Add(new Point(Bottom.X - 1, Bottom.Y));
                    points.Add(new Point(Bottom.X, Bottom.Y));
                    points.Add(MidPoint(new Point(Left.X + 1, Left.Y),
                                        new Point(Bottom.X, Bottom.Y)));
                    break;
                case NeighbourIndex.South:
                    points.Add(new Point(Bottom.X, Bottom.Y));
                    points.Add(new Point(Left.X + 1, Left.Y));
                    break;
                case NeighbourIndex.SouthWest:
                    points.Add(MidPoint(new Point(Left.X + 1, Left.Y),
                                        new Point(Bottom.X, Bottom.Y)));
                    points.Add(new Point(Left.X + 1, Left.Y));
                    points.Add(new Point(Left.X + 1, Left.Y - 1));
                    points.Add(MidPoint(new Point(Left.X + 1, Left.Y - 1),
                                        new Point(Top.X, Top.Y)));
                    break;
                case NeighbourIndex.West:
                    points.Add(new Point(Left.X + 1, Left.Y - 1));
                    points.Add(new Point(Top.X, Top.Y));
                    break;
                case NeighbourIndex.NorthWest:
                    points.Add(MidPoint(new Point(Left.X + 1, Left.Y - 1),
                                        new Point(Top.X, Top.Y)));
                    points.Add(new Point(Top.X, Top.Y));
                    points.Add(new Point(Top.X - 1, Top.Y));
                    points.Add(MidPoint(new Point(Right.X - 1, Right.Y - 1),
                                        new Point(Top.X - 1, Top.Y)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(idx), idx, null);
            }

            return points;
        }
    }
}
