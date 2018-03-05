using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Steropes.Tiles.TemplateGenerator.Annotations;

namespace Steropes.Tiles.TemplateGenerator.Model
{
  public class TextureGrid : INotifyPropertyChanged, ITextureTileParent
  {
    int anchorX;
    int anchorY;
    int borderX;
    int borderY;
    int height;

    string name;
    int width;
    int x;
    int y;

    public TextureGrid()
    {
      Groups = new ObservableCollection<TextureGroup>();
      Groups.CollectionChanged += OnGroupsChanged;
      Tiles = new ObservableCollection<TextureTile>();
      Tiles.CollectionChanged += OnTilesChanged;
    }

    public ObservableCollection<TextureGroup> Groups { get; }

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

    public int Width
    {
      get { return width; }
      set
      {
        if (value == width)
        {
          return;
        }

        width = value;
        OnPropertyChanged();
      }
    }

    public int Height
    {
      get { return height; }
      set
      {
        if (value == height)
        {
          return;
        }

        height = value;
        OnPropertyChanged();
      }
    }

    public int BorderX
    {
      get { return borderX; }
      set
      {
        if (value == borderX)
        {
          return;
        }

        borderX = value;
        OnPropertyChanged();
      }
    }

    public int BorderY
    {
      get { return borderY; }
      set
      {
        if (value == borderY)
        {
          return;
        }

        borderY = value;
        OnPropertyChanged();
      }
    }

    public int AnchorX
    {
      get { return anchorX; }
      set
      {
        if (value == anchorX)
        {
          return;
        }

        anchorX = value;
        OnPropertyChanged();
      }
    }

    public int AnchorY
    {
      get { return anchorY; }
      set
      {
        if (value == anchorY)
        {
          return;
        }

        anchorY = value;
        OnPropertyChanged();
      }
    }

    public TextureCollection Parent { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;
    public ObservableCollection<TextureTile> Tiles { get; }

    void OnGroupsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.OldItems != null)
      {
        foreach (TextureGroup item in e.OldItems)
        {
          item.Parent = null;
        }
      }

      if (e.NewItems != null)
      {
        foreach (TextureGroup item in e.NewItems)
        {
          item.Parent?.Groups.Remove(item);
          item.Parent = this;
        }
      }
    }

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