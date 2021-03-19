using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Steropes.Tiles.TemplateGenerator.Model
{
  public class TextureFile : INotifyPropertyChanged, IFormattingInfoProvider
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
      FormattingMetaData = new FormattingMetaData();
      properties = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
      Collections = new ObservableCollection<TextureCollection>();
      Collections.CollectionChanged += OnCollectionChanged;
      IncludeFiles = new ObservableCollection<string>();
    }

    public FormattingMetaData FormattingMetaData { get; }

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

    public string BasePath
    {
      get
      {
        try
        {
          var path = SourcePath;
          if (path != null)
          {
            var p = Path.GetFullPath(path);
            var retval = Directory.GetParent(p)?.FullName;
            if (!string.IsNullOrEmpty(retval))
            {
              if (retval.EndsWith("" + Path.DirectorySeparatorChar) || 
                  retval.EndsWith("" + Path.AltDirectorySeparatorChar))
              {
                return retval;
              }

              return retval + Path.DirectorySeparatorChar;
            }
          }

          return "";
        }
        catch (IOException)
        {
          // yeah, ugly, but this stuff above can fail for a myriad of reasons,
          // not all of them sane.
          return "";
        }
      }
    }

    public string MakeRelative(string path)
    {
      var fp = Path.GetFullPath(path);
      var basePath = BasePath;
      if (fp.StartsWith(basePath))
      {
        fp = fp.Substring(basePath.Length);
      }

      return fp;
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}