using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Steropes.Tiles.TemplateGenerator.Annotations;

namespace Steropes.Tiles.TemplateGenerator.Model
{
  public class TextureFile : INotifyPropertyChanged
  {
    int height;
    bool modified;
    string name;
    IReadOnlyDictionary<string, string> properties;

    string sourcePath;
    TileType tileType;
    int width;

    public TextureFile()
    {
      properties = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
      Collections = new ObservableCollection<TextureCollection>();
      Collections.CollectionChanged += OnCollectionChanged;
      IncludeFiles = new ObservableCollection<string>();
    }

    public ObservableCollection<string> IncludeFiles { get; }

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

    public TileType TileType
    {
      get { return tileType; }
      set
      {
        if (value == tileType)
        {
          return;
        }

        tileType = value;
        OnPropertyChanged();
      }
    }

    public ObservableCollection<TextureCollection> Collections { get; }

    public string SourcePath
    {
      get { return sourcePath; }
      set
      {
        if (value == sourcePath)
        {
          return;
        }

        sourcePath = value;
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

    public bool Modified
    {
      get { return modified; }
      set
      {
        if (value == modified)
        {
          return;
        }

        modified = value;
        OnPropertyChanged();
      }
    }

    public IReadOnlyDictionary<string, string> Properties
    {
      get { return properties; }
      set
      {
        if (Equals(value, properties))
        {
          return;
        }

        properties = value;
        OnPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.OldItems != null)
      {
        foreach (TextureCollection item in e.OldItems)
        {
          item.Parent = null;
        }
      }

      if (e.NewItems != null)
      {
        foreach (TextureCollection item in e.NewItems)
        {
          item.Parent?.Collections.Remove(item);
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