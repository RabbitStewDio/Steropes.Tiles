using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Steropes.Tiles.MonogameDemo.GameData.Strategy.Model;
using Steropes.Tiles.MonogameDemo.GameData.Util;

namespace Steropes.Tiles.MonogameDemo.GameData.Strategy
{
  public class MapReader
  {
    readonly Dictionary<char, byte> terrainsByCharId;
    readonly Dictionary<char, byte> resourcesByCharId;
    readonly Dictionary<char, IRoadType> roadsByCharId;
    readonly Dictionary<char, Improvements> improvementByCharId;
    readonly IRoadType river;

    public MapReader(StrategyGameRules rules)
    {
      this.terrainsByCharId = rules.TerrainTypes.ToIndexDict(t => t.AsciiId);
      this.resourcesByCharId = rules.TerrainResourceTypes.ToIndexDict(r => r.AsciiId);
      this.roadsByCharId = rules.RoadTypes.Where(r => !r.River).ToDict(r => r.AsciiId);
      this.improvementByCharId = rules.TerrainImprovementTypes.ToDict(r => r.AsciiId);
      this.river = rules.Roads.River;
    }

    public TerrainMap Map { get; set; }

    public void ReadTerrain(TextReader r, int ox = 0, int y = 0, int w = 0, int h = 0)
    {
      void ReadTerrainLine(char c, int x, int targetY)
      {
        if (terrainsByCharId.TryGetValue(c, out byte t))
        {
          var v = Map[x, targetY];
          v.TerrainIdx = t;
          Map[x, targetY] = v;
        }
        else
        {
          throw new ArgumentException();
        }
      }

      Read(r, ReadTerrainLine, ox, y, w, h);
    }

    ReadLine CreateImprovementHandler<T>(Dictionary<char, T> dict) where T : ITerrainExtra
    {
      void ReadHandler(char c, int x, int targetY)
      {
        if (dict.TryGetValue(c, out T t))
        {
          var v = Map[x, targetY];
          v.Improvement = v.Improvement.AddExtra(t);
          Map[x, targetY] = v;
        }
      }

      return ReadHandler;
    }

    public void ReadRoads(TextReader r, int ox = 0, int y = 0, int w = 0, int h = 0)
    {
      Read(r, CreateImprovementHandler(roadsByCharId), ox, y, w, h);
    }

    public void ReadImprovement(TextReader r, int ox = 0, int y = 0, int w = 0, int h = 0)
    {
      Read(r, CreateImprovementHandler(improvementByCharId), ox, y, w, h);
    }

    public void ReadRivers(TextReader r, int ox = 0, int y = 0, int w = 0, int h = 0)
    {
      void ReadRiver(char c, int x, int targetY)
      {
        var v = Map[x, targetY];
        if (c != ' ')
        {
          v.Improvement = v.Improvement.AddExtra(river);
        }
        Map[x, targetY] = v;
      }

      Read(r, ReadRiver, ox, y, w, h);
    }

    public void ReadResources(TextReader r, int ox = 0, int y = 0, int w = 0, int h = 0)
    {
      void ReadResource(char c, int x, int targetY)
      {
        if (resourcesByCharId.TryGetValue(c, out byte t))
        {
          var v = Map[x, targetY];
          v.Resources = t;
          Map[x, targetY] = v;
        }
      }

      Read(r, ReadResource, ox, y, w, h);
    }

    void Read(TextReader r, ReadLine handler, int x = 0, int y = 0, int w = 0, int h = 0)
    {
      var line = r.ReadLine();
      var targetY = y;


      var maxH = Math.Max(0, h > 0 ? h : Map.Height - y) + y;
      while (line != null && targetY < maxH)
      {
        var maxX = CalculateMaxLength(line, w);
        for (var idx = 0; idx < maxX; idx += 1)
        {
          handler(line[idx], x + idx, targetY);
        }
        targetY += 1;
        line = r.ReadLine();
      }
    }

    delegate void ReadLine(char c, int x, int targetY);

    static int CalculateMaxLength(string line, int width)
    {
      var maxX = line.Length;
      if (width > 0)
      {
        maxX = Math.Min(maxX, width);
      }
      return maxX;
    }
  }

  public static class MapReaderExtensions
  {
    public static string Strip(this string text, char barrier = '|')
    {
      var nl = Environment.NewLine;
      var sr = new StringReader(text);
      var line = sr.ReadLine();
      var result = new StringBuilder();
      var firstLine = true;
      while (line != null)
      {
        if (!firstLine)
        {
          result.Append(nl);
        }

        int idx = line.IndexOf(barrier);
        if (idx >= 0)
        {
          result.Append(line.Substring(idx + 1));
          firstLine = false;
        }
        else if (result.Length == 0 && line.Length == 0)
        {
          // skip leading empty lines.
        }
        else
        {
          result.Append(line);
          firstLine = false;
        }

        line = sr.ReadLine();
      }
      return result.ToString();
    }
  }
}