using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Steropes.Tiles.TemplateGenerator.Annotations;

namespace Steropes.Tiles.TemplateGenerator.Model
{
  public class TextureGrid : INotifyPropertyChanged, IFormattingInfoProvider
  {
    int? anchorX;
    int? anchorY;

    int x;
    int y;
    int? width;
    int? height;

    int cellWidth;
    int cellHeight;
    int cellSpacing;

    MatcherType matcherType;
    string pattern;
    string name;
    int cellMapElements;

    public TextureGrid()
    {
      Tiles = new ObservableCollection<TextureTile>();
      Tiles.CollectionChanged += OnTilesChanged;
      FormattingMetaData = new FormattingMetaData();
    }

    public FormattingMetaData FormattingMetaData { get; }

    public int CellSpacing
    {
      get { return cellSpacing; }
      set
      {
        if (value == cellSpacing) return;
        cellSpacing = value;
        OnPropertyChanged();
      }
    }

    public int CellMapElements
    {
      get { return cellMapElements; }
      set
      {
        if (value == cellMapElements) return;
        cellMapElements = value;
        OnPropertyChanged();
      }
    }

    public MatcherType MatcherType
    {
      get { return matcherType; }
      set
      {
        if (value == matcherType) return;
        matcherType = value;
        OnPropertyChanged();
      }
    }

    public string Pattern
    {
      get { return pattern; }
      set
      {
        if (value == pattern) return;
        pattern = value;
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

    public int CellWidth
    {
      get { return cellWidth; }
      set
      {
        if (value == cellWidth) return;
        cellWidth = value;
        OnPropertyChanged();
      }
    }

    public int CellHeight
    {
      get { return cellHeight; }
      set
      {
        if (value == cellHeight) return;
        cellHeight = value;
        OnPropertyChanged();
      }
    }

    public int? Width
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

    public int? Height
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

    public int? AnchorX
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

    public int? AnchorY
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