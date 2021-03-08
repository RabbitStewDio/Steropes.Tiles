using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.Plotter;

namespace Steropes.Tiles.Test.ScreenMapping
{
  public abstract class ScreenMapperTestBase
  {
    protected abstract RenderType RenderType { get; }
    protected MapToScreenMapper MapToScreen;
    protected ContinuousMapToScreenMapper MapToScreenCon;
    protected ScreenToMapMapper ScreenToMap;
    protected ContinuousScreenToMapMapper ScreenToMapCon;

    [SetUp]
    public void SetUp()
    {
      MapToScreen = ScreenCoordinateMapping.CreateMapToScreenMapper(RenderType);
      MapToScreenCon = ScreenCoordinateMapping.CreateToContinuousMapToScreenMapper(RenderType);
      ScreenToMap = ScreenCoordinateMapping.CreateToMapMapper(RenderType);
      ScreenToMapCon = ScreenCoordinateMapping.CreateToContinuousMapMapper(RenderType);
    }

    public MapCoordinate MapCoordinate(int x, int y)
    {
      return new MapCoordinate(x, y);
    }

    public DoublePoint MapPosition(double x, double y)
    {
      return new DoublePoint(x, y);
    }

    public ScreenMapperCase<MapCoordinate, ViewportCoordinates> ViewCoordinate(int x, int y)
    {
      return new ScreenMapperCase<MapCoordinate, ViewportCoordinates>(new ViewportCoordinates(x, y));
    }

    public ScreenMapperCase<DoublePoint, ContinuousViewportCoordinates> ContViewCoordinate(double x, double y)
    {
      return new ScreenMapperCase<DoublePoint, ContinuousViewportCoordinates>(new ContinuousViewportCoordinates(x, y));
    }

    [Test]
    public void ValidateViewToMap()
    {
      foreach (var screenMapperCase in DirectlyEquivalent)
      {
        var mc = ScreenToMap(screenMapperCase.ViewCoordinates.X, screenMapperCase.ViewCoordinates.Y);
        mc.Should().Be(screenMapperCase.MapCoordinate, "the view coordinate was {0}", screenMapperCase.ViewCoordinates);
      }

      foreach (var screenMapperCase in Fractional)
      {
        var mc = ScreenToMap(screenMapperCase.ViewCoordinates.X, screenMapperCase.ViewCoordinates.Y);
        mc.Should().Be(screenMapperCase.MapCoordinate, "the fractional view coordinate was {0}", screenMapperCase.ViewCoordinates);
      }
    }

    [Test]
    public void ValidateMapToView()
    {
      foreach (var screenMapperCase in DirectlyEquivalent)
      {
        var vc = MapToScreen(screenMapperCase.MapCoordinate.X, screenMapperCase.MapCoordinate.Y);
        vc.Should().Be(screenMapperCase.ViewCoordinates, "the map coordinate was {0}", screenMapperCase.MapCoordinate);
      }
    }

    [Test]
    public void ValidateContViewToMap()
    {
      foreach (var screenMapperCase in ContDirectlyEquivalent)
      {
        var mc = ScreenToMapCon(screenMapperCase.ViewCoordinates.X, screenMapperCase.ViewCoordinates.Y);
        mc.Should().Be(screenMapperCase.MapCoordinate, "the view coordinate was {0}", screenMapperCase.ViewCoordinates);
      }

      foreach (var screenMapperCase in ContFractional)
      {
        var mc = ScreenToMapCon(screenMapperCase.ViewCoordinates.X, screenMapperCase.ViewCoordinates.Y);
        mc.Should().Be(screenMapperCase.MapCoordinate, "the fractional view coordinate was {0}", screenMapperCase.ViewCoordinates);
      }
    }

    [Test]
    public void ValidateContMapToView()
    {
      foreach (var screenMapperCase in ContDirectlyEquivalent)
      {
        var vc = MapToScreenCon(screenMapperCase.MapCoordinate.X, screenMapperCase.MapCoordinate.Y);
        vc.Should().Be(screenMapperCase.ViewCoordinates, "the map coordinate was {0}", screenMapperCase.MapCoordinate);
      }
    }

    protected IEnumerable<ScreenMapperCase<DoublePoint, ContinuousViewportCoordinates>> 
      Transform(IEnumerable<ScreenMapperCase<MapCoordinate, ViewportCoordinates>> t)
    {
      foreach (var c in t)
      {
        var vc = c.ViewCoordinates;
        var mc = c.MapCoordinate;
        yield return ContViewCoordinate(vc.X, vc.Y).IsSameAs(new DoublePoint(mc.X, mc.Y));
      }
    }

    public abstract IEnumerable<ScreenMapperCase<MapCoordinate, ViewportCoordinates>> DirectlyEquivalent { get; }
    public abstract IEnumerable<ScreenMapperCase<MapCoordinate, ViewportCoordinates>> Fractional { get; }

    public abstract IEnumerable<ScreenMapperCase<DoublePoint, ContinuousViewportCoordinates>> ContDirectlyEquivalent { get; }
    public abstract IEnumerable<ScreenMapperCase<DoublePoint, ContinuousViewportCoordinates>> ContFractional { get; }
  }
}