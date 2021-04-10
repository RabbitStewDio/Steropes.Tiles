using Serilog;
using Steropes.Tiles.DataStructures;
using System;
using System.Text;

namespace Steropes.Tiles.TemplateGen.Models.Rendering
{
    public class ArrangeNode<T>
    {
        static readonly ILogger Logger = SLog.ForContext<ArrangeNode<T>>();

        ArrangeNode<T>? bottom;
        ArrangeNode<T>? left;
        public T? Content { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }
        int width;
        int height;
        public int SpaceAvailableX { get; }
        public int SpaceAvailableY { get; }

        public ArrangeNode(int width, int height)
        {
            this.SpaceAvailableX = width;
            this.SpaceAvailableY = height;
        }

        public ArrangeNode(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            this.SpaceAvailableX = width;
            this.SpaceAvailableY = height;
        }

        public override string ToString()
        {
            return
                $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(SpaceAvailableX)}: {SpaceAvailableX}, {nameof(SpaceAvailableY)}: {SpaceAvailableY}, {nameof(width)}: {width}, {nameof(height)}: {height} : {Content}";
        }

        public void Apply(Action<ArrangeNode<T>> a)
        {
            if (Content == null)
            {
                return;
            }

            a(this);
            left?.Apply(a);
            bottom?.Apply(a);
        }

        public ArrangeNode<T>? FindInsertPosition(IntDimension size)
        {
            if (Content == null)
            {
                if (Fits(size))
                {
                    Logger.Verbose("Choosing <empty node>");
                    return this;
                }

                Logger.Verbose("Rejecting <empty node>");
                return null;
            }

            Logger.Verbose("Choosing .. {Content}", Content);

            var leftPos = left?.FindInsertPosition(size);
            var bottomPos = bottom?.FindInsertPosition(size);
            if (leftPos == null)
            {
                Logger.Verbose("Choosing bottom by default for {Content}", Content);
                return bottomPos;
            }

            if (bottomPos == null)
            {
                Logger.Verbose("Choosing left by default for {Content}", Content);
                return leftPos;
            }

            var weightLeft = leftPos.ComputeWeight(size);
            var weightBottom = bottomPos.ComputeWeight(size);
            if (weightLeft <= weightBottom)
            {
                Logger.Verbose("Choosing left for {Content}", Content);
                return leftPos;
            }

            Logger.Verbose("Choosing bottom for {Content}", Content);
            return bottomPos;
        }

        double ComputeWeight(IntDimension s)
        {
            return Math.Pow(X + s.Width, 2) + Math.Pow(Y + s.Height, 2);
        }

        void ApplyInsert(IntDimension size, T c)
        {
            if (c == null)
            {
                throw new ArgumentNullException();
            }

            if (Content != null)
            {
                throw new InvalidOperationException();
            }

            if (!Fits(size))
            {
                throw new InvalidOperationException();
            }

            Content = c;
            width = size.Width;
            height = size.Height;

            var s = this.Split(size);
            left = s.Item1;
            bottom = s.Item2;
        }

        public bool Insert(IntDimension size, T c)
        {
            Logger.Verbose("Start Insert {Content}", c);

            try
            {
                var insertPos = FindInsertPosition(size);
                if (insertPos != null)
                {
                    insertPos.ApplyInsert(size, c);
                    return true;
                }

                return false;
            }
            finally
            {
                Logger.Verbose("Done Insert {Content}", c);
            }
        }


        bool Fits(IntDimension c)
        {
            if (c.Height > SpaceAvailableY)
            {
                return false;
            }

            if (c.Width > SpaceAvailableX)
            {
                return false;
            }

            return true;
        }

        public string Print()
        {
            var sb = new StringBuilder();
            Print(0, sb);
            return sb.ToString();
        }
        
        void Print(int indent, StringBuilder b)
        {
            b.AppendLine("".PadLeft(indent) + this);
            if (left != null)
            {
                b.AppendLine("".PadLeft(indent) + "Left:");
                left.Print(indent + 5, b);
            }

            if (bottom != null)
            {
                b.AppendLine("".PadLeft(indent) + "Bottom:");
                bottom.Print(indent + 5, b);
            }
        }
    }

    public static class ArrangeNode
    {
        static readonly ILogger Logger = SLog.ForContext(typeof(ArrangeNode));
        
        public static (ArrangeNode<T>, ArrangeNode<T>) Split<T>(this ArrangeNode<T> n, IntDimension size)
        {
            var hspace = n.SpaceAvailableX - size.Width;
            var vspace = n.SpaceAvailableY - size.Height;

            // estimate the available area 
            //var estH = hspace * n.SpaceAvailableY;
            //var estV = vspace * n.SpaceAvailableX;
            var estH = n.X + size.Width;
            var estV = n.Y + size.Height;
            if (estH >= estV)
            {
                Logger.Verbose("Splitting vertical for {EstH} , {EstV}", estH, estV);
                // vertical split 
                //
                //   content  |  Left
                //   ----------------
                //   Bottom
                //
                var left = new ArrangeNode<T>(n.X + size.Width, n.Y, hspace, size.Height);
                var bottom = new ArrangeNode<T>(n.X, n.Y + size.Height, n.SpaceAvailableX, vspace);
                return (left, bottom);
            }
            else
            {
                Logger.Verbose("Splitting horizontal for {EstH} , {EstV}", estH, estV);
                // horizontal split
                //
                //   content  |  Left
                //   ---------|
                //   Bottom   | 
                //
                var left = new ArrangeNode<T>(n.X + size.Width, n.Y, hspace, n.SpaceAvailableY);
                var bottom = new ArrangeNode<T>(n.X, n.Y + size.Height, size.Width, vspace);
                return (left, bottom);
            }
        }
    }
}
