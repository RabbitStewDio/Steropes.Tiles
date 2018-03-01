using System;

namespace Steropes.Tiles.MonogameDemo.Util
{
  public class MathX
  {
    public const float PI = (float)Math.PI;

    static readonly float[] cosx;

    static MathX()
    {
      cosx = new float[1001];
      for (int i = 0; i < 1001; i += 1)
      {
        float v = i / 1000f;
        float ft = v * PI;
        float f = (float)((1 - Math.Cos(ft)) * 0.5f);

        cosx[i] = f;
      }
    }

    public static float Interpolate(float a, float b, float v)
    {
      int idx = Clamp(FloorToInt(Math.Abs(v) * 1000), 0, 1000);
      float f = cosx[idx] * Math.Sign(v);
      return a * (1 - f) + b * f;
    }

    public static float Clamp(float value, float min, float max)
    {
      if (value < min)
      {
        value = min;
      }
      else
      {
        if (value > max)
        {
          value = max;
        }
      }
      return value;
    }

    public static int Clamp(int value, int min, int max)
    {
      if (value < min)
      {
        value = min;
      }
      else
      {
        if (value > max)
        {
          value = max;
        }
      }
      return value;
    }

    public static int FloorToInt(float f)
    {
      return (int)Math.Floor(f);
    }


  }
}