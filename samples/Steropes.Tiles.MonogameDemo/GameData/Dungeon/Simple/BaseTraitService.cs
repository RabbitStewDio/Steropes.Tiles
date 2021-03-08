using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Steropes.Tiles.MonogameDemo.GameData.Dungeon.Model;
using Steropes.Tiles.MonogameDemo.Util;

namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon.Simple
{
  public class BaseTraitService<TItem>
  {
    readonly Dictionary<TItem, TraitRegistry> traits;
    readonly TraitCollection<IUpdateableTrait> updateableTraits;
    readonly Action<IUpdateableTrait, GameTime> updateAction;

    public BaseTraitService()
    {
      this.traits = new Dictionary<TItem, TraitRegistry>();
      this.updateAction = UpdateAction;
      this.updateableTraits = new TraitCollection<IUpdateableTrait>();
    }

    public void AddTrait<T>(TItem item, T trait) where T : IItemTrait
    {
      if (!traits.TryGetValue(item, out TraitRegistry tr))
      {
        tr = new TraitRegistry();
        traits[item] = tr;
      }

      updateableTraits.Add(trait);
      tr.Add(typeof(T), trait);
    }

    protected void RemoveAllTraits(TItem item)
    {
      if (traits.TryGetValue(item, out TraitRegistry tr))
      {
        tr.ForEachTrait((t, c) => updateableTraits.Remove(t), 0);
        traits.Remove(item);
      }
    }

    public void RemoveTrait<T>(TItem item) where T : IItemTrait
    {
      if (traits.TryGetValue(item, out TraitRegistry tr))
      {
        if (tr.Lookup(typeof(T), out T value))
        {
          updateableTraits.Remove(value);
        }

        tr.Remove<T>();
        if (tr.IsEmpty())
        {
          traits.Remove(item);
        }
      }
    }

    public bool TraitFor<T>(TItem c, out T trait) where T : IItemTrait
    {
      if (traits.TryGetValue(c, out TraitRegistry tr))
      {
        return tr.Lookup(typeof(T), out trait);
      }

      throw new ArgumentException($"No entry exists for {c}");
    }

    void UpdateAction(IUpdateableTrait v, GameTime time)
    {
      v.Update(time);
    }

    public void Update(GameTime time)
    {
      updateableTraits.ForAll(updateAction, time);
    }
  }

}