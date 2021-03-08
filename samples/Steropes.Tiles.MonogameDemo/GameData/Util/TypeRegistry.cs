using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Steropes.Tiles.MonogameDemo.GameData.Util
{
  public class TypeRegistry<T>: ITypeRegistry<T> 
  {
    readonly List<T> types;

    public TypeRegistry(T defaultValue)
    {
      this.DefaultValue = defaultValue;

      types = new List<T>();
      // Crude way to eliminate null values when possible values may be structs.
      // Costs one boxing allocation during setup.
      if (defaultValue != null)
      {
        types.Add(defaultValue);
      }
    }

    public void Add(T val)
    {
      if (val == null)
      {
        throw new ArgumentNullException(nameof(val));
      }
      if (types.Contains(val))
      {
        throw new ArgumentException("Duplicate value");
      }
      types.Add(val);
    }

    public T DefaultValue { get; }

    public T this[int idx]
    {
      get { return types[idx]; }
    }

    public IEnumerator<T> GetEnumerator()
    {
      return types.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int Count => types.Count;
  }

  public static class TypeRegistry
  {
    public static TypeRegistry<T> CreateFrom<T>(object o) where T: class
    {
      return CreateFrom(o, v => default(T));
    }

    public static TypeRegistry<T> AppendFrom<T, TSource>(this TypeRegistry<T> reg, TSource o)
    {
      var defaultValue = reg.DefaultValue;
      foreach (var pi in o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
      {
        if (!pi.CanRead)
        {
          continue;
        }
        if (!typeof(T).IsAssignableFrom(pi.PropertyType))
        {
          continue;
        }

        var mth = pi.GetMethod;
        if (mth.IsPublic && !mth.IsAbstract && mth.GetParameters().Length == 0)
        {
          var value = (T) pi.GetValue(o);
          if (value == null || ReferenceEquals(value, defaultValue))
          {
            continue;
          }
          reg.Add(value);
        }
      }
      return reg;
    }

    public static TypeRegistry<T> CreateFrom<T, TSource>(TSource o, Func<TSource, T> defaultFn) where T:class
    {
      var defaultValue = defaultFn(o);
      var reg = new TypeRegistry<T>(defaultValue);
      AppendFrom(reg, o);
      return reg;
    }

    public static TypeRegistry<T> CreateFrom<T>(T nothing, params T[] data) where T : class
    {
      var reg = new TypeRegistry<T>(nothing);
      foreach (var d in data)
      {
        reg.Add(d);
      }
      return reg;
    }

  }
}