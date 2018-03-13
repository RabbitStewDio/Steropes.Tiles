using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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

    int? cellWidth;
    int? cellHeight;
    int cellSpacing;

    MatcherType matcherType;
    string pattern;
    string name;
    string cellMapElements;

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

    public string CellMapElements
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
        if (string.IsNullOrWhiteSpace(value))
        {
          value = null;
        }

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
        if (string.IsNullOrWhiteSpace(value))
        {
          value = null;
        }

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

    public int? CellWidth
    {
      get { return cellWidth; }
      set
      {
        if (value == cellWidth) return;
        cellWidth = value;
        OnPropertyChanged();
      }
    }

    public int? CellHeight
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

    public List<string> EffectiveCellMapElements
    {
      get
      {
        if (string.IsNullOrWhiteSpace(CellMapElements))
        {
          return new List<string>();
        }

        return CellMapElements.Split(null).Distinct().ToList();
      }
    }
    

    /// <summary>
    ///  Returns the effective tile size, that is the standardized 
    /// space a tile consumes on screen. Tiles can be larger than
    /// that to overlap with neighbouring cells if needed, which is
    /// measured as EffectiveCellSize.
    /// </summary>
    public Size EffectiveTileSize
    {
      get
      {
        var retval = new Size();
        var parent = Parent?.Parent;
        if (parent != null)
        {
          retval.Width = parent.Width;
          retval.Height = parent.Height;
        }
        if (matcherType == MatcherType.Corner)
        {
          retval.Width /= 2;
          retval.Height /= 2;
        }

        return retval;
      }
    }

    public Size EffectiveCellSize
    {
      get
      {
        var retval = EffectiveTileSize;
        retval.Width = cellWidth ?? retval.Width;
        retval.Height = cellHeight ?? retval.Height;
        return retval;
      }
    }

    /// <summary>
    ///  The effective anchor point, if not defined otherwise
    ///  will be aligned to the bottom edge of the graphic and
    ///  centred horizontally.
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    public Point ComputeEffectiveAnchorPoint(TextureTile tile)
    {
      var tileSize = EffectiveTileSize;
      var size = EffectiveCellSize;
      var anchorPointX = tile.AnchorX ?? AnchorX ?? size.Width / 2;
      var anchorPointY = tile.AnchorY ?? AnchorY ?? size.Height - tileSize.Height / 2;
      return new Point(anchorPointX, anchorPointY);
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