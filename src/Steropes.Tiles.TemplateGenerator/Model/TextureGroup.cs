using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Steropes.Tiles.TemplateGenerator.Annotations;

namespace Steropes.Tiles.TemplateGenerator.Model
{
  public class TextureGroup : INotifyPropertyChanged, ITextureTileParent
  {
    MatcherType matcherType;

    string name;
    string pattern;
    int x;
    int y;

    public TextureGroup()
    {
      Tiles = new ObservableCollection<TextureTile>();
      Tiles.CollectionChanged += OnTilesChanged;
      MatcherType = MatcherType.Basic;
    }

    public TextureGrid Parent { get; set; }

    public string Pattern
    {
      get { return pattern; }
      set
      {
        if (value == pattern)
        {
          return;
        }

        pattern = value;
        OnPropertyChanged();
      }
    }

    public int X
    {
      get { return x; }
      set
      {
        if (value == x)
        {
          return;
        }

        x = value;
        OnPropertyChanged();
      }
    }

    public int Y
    {
      get { return y; }
      set
      {
        if (value == y)
        {
          return;
        }

        y = value;
        OnPropertyChanged();
      }
    }

    public string Name
    {
      get { return name; }
      set
      {
        if (value == name)
        {
          return;
        }

        name = value;
        OnPropertyChanged();
      }
    }

    public MatcherType MatcherType
    {
      get { return matcherType; }
      set
      {
        if (value == matcherType)
        {
          return;
        }

        matcherType = value;
        OnPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public ObservableCollection<TextureTile> Tiles { get; }

    void OnTilesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.OldItems != null)
      {
        foreach (TextureTile item in e.OldItems)
        {
          item.Parent = null;
        }
      }

      if (e.NewItems != null)
      {
        foreach (TextureTile item in e.NewItems)
        {
          item.Parent?.Tiles.Remove(item);
          item.Parent = this;
        }
      }
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}