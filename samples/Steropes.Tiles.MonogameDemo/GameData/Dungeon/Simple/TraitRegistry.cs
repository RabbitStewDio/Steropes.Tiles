using System;
using System.Collections;
using System.Collections.Generic;

namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon.Simple
{
  class TraitRegistry: IEnumerable<KeyValuePair<Type, object>>
  {
    readonly Dictionary<Type, object> traits;

    public TraitRegistry()
    {
      traits = new Dictionary<Type, object>();
    }

    public bool Lookup<T>(Type t, out T value)
    {
      if (traits.TryGetValue(t, out object raw))
      {
        value = (T) raw;
        return true;
      }
      else
      {
        value = default(T);
        return false;
      }
    }

    public void Put<T>(T value)
    {
      traits[typeof(T)] = value;
    }

    public void Remove<T>()
    {
      traits.Remove(typeof(T));
    }


    public bool IsEmpty()
    {
      return traits.Count == 0;
    }

    public void Add(Type t, object value)
    {
      traits[t] = value;
    }

    public void ForEachTrait<T>(Action<object, T> action, T context)
    {
      foreach (var trait in traits)
      {
        var v = trait.Value;
        action(v, context);
      }
    }

    public IEnumerator<KeyValuePair<Type, object>> GetEnumerator()
    {
      return traits.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}