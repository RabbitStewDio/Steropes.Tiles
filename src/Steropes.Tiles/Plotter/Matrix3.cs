using System;
using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles.Plotter
{
  /// <summary>
  ///   Simple matrix operation to support operations on the viewport's center.
  ///   Does only support translate and rotate in its direct API, as scale or
  ///   shear or other transformations dont really make sense in this context.
  /// </summary>
  public struct Matrix3
  {
    public static readonly Matrix3 Identity = new Matrix3(1, 0, 0,
                                                          0, 1, 0);

    public Matrix3(double m11,
                   double m12,
                   double m13,
                   double m21,
                   double m22,
                   double m23)
    {
      M11 = m11;
      M12 = m12;
      M13 = m13;
      M21 = m21;
      M22 = m22;
      M23 = m23;
    }

    public double M11 { get; }

    public double M12 { get; }

    public double M13 { get; }

    public double M21 { get; }

    public double M22 { get; }

    public double M23 { get; }

    public double M31
    {
      get { return 0; }
    }

    public double M32
    {
      get { return 0; }
    }

    public double M33
    {
      get { return 1; }
    }

    public static Matrix3 Translate(double x, double y)
    {
      return new Matrix3(1, 0, x, 0, 1, y);
    }

    static double Cleanse(double val)
    {
      return Math.Round(val, 10);
    }

    public static Matrix3 Rotation(double radians)
    {
      var cos = Cleanse(Math.Cos(radians));
      var sin = Cleanse(Math.Sin(radians));
      return new Matrix3(cos, sin, 0,
                         -sin, cos, 0);
    }
/*
    public Point Transform(Point position)
    {
      return Transform(position.X, position.Y);
    }
    */
    public ContinuousViewportCoordinates Transform(ContinuousViewportCoordinates position)
    {
      return new ContinuousViewportCoordinates(position.X * M11 + position.Y * M12 + M13,
                                               position.X * M21 + position.Y * M22 + M23);
    }

    public ViewportCoordinates Transform(ViewportCoordinates position)
    {
      var x = position.X * M11 + position.Y * M12 + M13;
      var y = position.X * M21 + position.Y * M22 + M23;
      return new ViewportCoordinates((int) Math.Round(x), (int) Math.Round(y));
    }

    /*
    public Point Transform(double x, double y)
    {
      return new Point(x * M11 + y * M12 + M13,
                       x * M21 + y * M22 + M23);
    }
    */
    public static Matrix3 Multiply(Matrix3 matrix1, Matrix3 matrix2)
    {
      var m11 = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31;
      var m12 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32;
      var m13 = matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33;
      var m21 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31;
      var m22 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32;
      var m23 = matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33;
      return new Matrix3(m11, m12, m13, m21, m22, m23);
    }
  }
}