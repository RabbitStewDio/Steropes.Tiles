using System;

namespace Steropes.Tiles.MonogameDemo.GameData.Strategy.Model
{
  /// <summary>
  ///  Defines a resource in Civ style. 
  /// </summary>
  public struct Resources
  {
    public int Food { get; set; }
    public int Production { get; set; }
    public int Trade { get; set; }

    public Resources(int food, int production, int trade)
    {
      Food = food;
      Production = production;
      Trade = trade;
    }

    public static Resources operator +(Resources r1, Resources r2)
    {
      return new Resources(r1.Food + r2.Food, r1.Production + r2.Production, r1.Trade + r2.Trade);
    }

    public static Resources operator *(Resources r1, ResourcesBoost r2)
    {
      return new Resources((int) Math.Round(r1.Food * (1 + r2.Food)),
                           (int) Math.Round(r1.Production * (1 + r2.Production)),
                           (int) Math.Round(r1.Trade * (1 + r2.Trade)));
    }
  }

  /// <summary>
  ///  Defines a resource in Civ style. 
  /// </summary>
  public struct ResourcesBoost
  {
    public float Food { get; set; }
    public float Production { get; set; }
    public float Trade { get; set; }

    public ResourcesBoost(float food, float production, float trade)
    {
      Food = food;
      Production = production;
      Trade = trade;
    }
  }

  public static class ResourcesExt
  {
    public static Resources Resource(int food, int production, int trade)
    {
      return new Resources(food, production, trade);
    }

    public static Resources Prod(int production)
    {
      return new Resources(0, production, 0);
    }

    public static Resources Food(int food)
    {
      return new Resources(food, 0, 0);
    }

    public static Resources Trade(int trade)
    {
      return new Resources(0, 0, trade);
    }

    public static ResourcesBoost Boost(float food, float production, float trade)
    {
      return new ResourcesBoost(food, production, trade);
    }

    public static ResourcesBoost ProdBoost(float production)
    {
      return new ResourcesBoost(0, production, 0);
    }

    public static ResourcesBoost FoodBoost(float food)
    {
      return new ResourcesBoost(food, 0, 0);
    }

    public static ResourcesBoost TradeBoost(float trade)
    {
      return new ResourcesBoost(0, 0, trade);
    }
  }
}