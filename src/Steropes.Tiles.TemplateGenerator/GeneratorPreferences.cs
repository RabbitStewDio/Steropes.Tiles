using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Linq;
using Steropes.Tiles.Properties;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator
{
  public class GeneratorPreferences : INotifyPropertyChanged
  {
    TileType defaultTileType;
    int defaultWidth;
    int defaultHeight;
    int textSpacing;
    Color defaultTileHighlightColor;
    Color defaultTileColor;
    Color defaultTextColor;
    Font defaultFont;
    Color defaultBorderColor;
    Color? defaultBackgroundColor;
    public event PropertyChangedEventHandler PropertyChanged;
    int? defaultGridBorder;
    int? defaultGridMargin;
    int? defaultGridPadding;
    int? defaultCollectionBorder;
    int? defaultCollectionMargin;
    int? defaultCollectionPadding;
    int? defaultCellPadding;

    public GeneratorPreferences()
    {
      TileColors = new BulkChangeObservableCollection<Color>
      {
        Color.Aquamarine,
        Color.CornflowerBlue,
        Color.Coral,
        Color.Gold,
        Color.MediumOrchid,
        Color.Lavender
      };
      TileColors.CollectionChanged += (s, a) => OnPropertyChanged(nameof(TileColors));

      RecentFiles = new BulkChangeObservableCollection<string>();
      RecentFiles.CollectionChanged += (s, a) => OnPropertyChanged(nameof(RecentFiles));

      DefaultTileType = TileType.Isometric;
      DefaultWidth = 96;
      DefaultHeight = 48;

      DefaultBorderColor = Color.DarkGray;
      DefaultTileColor = Color.DarkGray;
      DefaultTileHighlightColor = Color.DimGray;
      DefaultTextColor = Color.Black;
      TextSpacing = 5;
      DefaultFont = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular);

      DefaultCellPadding = 1;
      DefaultCollectionBorder = 1;
      DefaultCollectionMargin = 1;
      DefaultCollectionPadding = 2;
      DefaultGridBorder = 1;
      DefaultGridMargin = 1;
      DefaultGridPadding = 2;
    }

    public int? DefaultCellPadding
    {
      get { return defaultCellPadding; }
      set
      {
        if (value == defaultCellPadding) return;
        defaultCellPadding = value;
        OnPropertyChanged();
      }
    }

    public int? DefaultGridBorder
    {
      get { return defaultGridBorder; }
      set
      {
        if (value == defaultGridBorder) return;
        defaultGridBorder = value;
        OnPropertyChanged();
      }
    }

    public int? DefaultGridMargin
    {
      get { return defaultGridMargin; }
      set
      {
        if (value == defaultGridMargin) return;
        defaultGridMargin = value;
        OnPropertyChanged();
      }
    }

    public int? DefaultGridPadding
    {
      get { return defaultGridPadding; }
      set
      {
        if (value == defaultGridPadding) return;
        defaultGridPadding = value;
        OnPropertyChanged();
      }
    }

    public int? DefaultCollectionBorder
    {
      get { return defaultCollectionBorder; }
      set
      {
        if (value == defaultCollectionBorder) return;
        defaultCollectionBorder = value;
        OnPropertyChanged();
      }
    }

    public int? DefaultCollectionMargin
    {
      get { return defaultCollectionMargin; }
      set
      {
        if (value == defaultCollectionMargin) return;
        defaultCollectionMargin = value;
        OnPropertyChanged();
      }
    }

    public int? DefaultCollectionPadding
    {
      get { return defaultCollectionPadding; }
      set
      {
        if (value == defaultCollectionPadding) return;
        defaultCollectionPadding = value;
        OnPropertyChanged();
      }
    }

    public Font DefaultFont
    {
      get { return defaultFont; }
      set
      {
        if (Equals(value, defaultFont)) return;
        defaultFont = value;
        OnPropertyChanged();
      }
    }

    public int TextSpacing
    {
      get { return textSpacing; }
      set
      {
        if (value == textSpacing) return;
        textSpacing = value;
        OnPropertyChanged();
      }
    }

    public Color DefaultTileHighlightColor
    {
      get { return defaultTileHighlightColor; }
      set
      {
        if (value.Equals(defaultTileHighlightColor)) return;
        defaultTileHighlightColor = value;
        OnPropertyChanged();
      }
    }

    public Color DefaultTileColor
    {
      get { return defaultTileColor; }
      set
      {
        if (value.Equals(defaultTileColor)) return;
        defaultTileColor = value;
        OnPropertyChanged();
      }
    }

    public Color DefaultTextColor
    {
      get { return defaultTextColor; }
      set
      {
        if (value.Equals(defaultTextColor)) return;
        defaultTextColor = value;
        OnPropertyChanged();
      }
    }

    public int DefaultWidth
    {
      get { return defaultWidth; }
      set
      {
        if (value == defaultWidth) return;
        defaultWidth = value;
        OnPropertyChanged();
      }
    }

    public int DefaultHeight
    {
      get { return defaultHeight; }
      set
      {
        if (value == defaultHeight) return;
        defaultHeight = value;
        OnPropertyChanged();
      }
    }

    public TileType DefaultTileType
    {
      get { return defaultTileType; }
      set
      {
        if (value == defaultTileType) return;
        defaultTileType = value;
        OnPropertyChanged();
      }
    }

    public BulkChangeObservableCollection<string> RecentFiles { get; }

    public BulkChangeObservableCollection<Color> TileColors { get; }

    public Color DefaultBorderColor
    {
      get { return defaultBorderColor; }
      set
      {
        if (value.Equals(defaultBorderColor)) return;
        defaultBorderColor = value;
        OnPropertyChanged();
      }
    }

    public Color? DefaultBackgroundColor
    {
      get { return defaultBackgroundColor; }
      set
      {
        if (value.Equals(defaultBackgroundColor)) return;
        defaultBackgroundColor = value;
        OnPropertyChanged();
      }
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void Save()
    {
    }

    public void AddRecentFile(string file)
    {
      if (RecentFiles.Contains(file))
      {
        RecentFiles.Remove(file);
      }

      RecentFiles.Add(file);
      if (RecentFiles.Count > 10)
      {
        RecentFiles.RemoveAt(0);
      }
    }
  }

  public class GeneratorPreferencesWriter
  {
    static readonly XNamespace Namespace = "http://www.steropes-ui.org/namespaces/tiles-preferences/1.0";
    readonly GeneratorPreferences preferences;

    public GeneratorPreferencesWriter(GeneratorPreferences preferences)
    {
      this.preferences = preferences ?? throw new ArgumentNullException(nameof(preferences));
    }

    public void Save()
    {
      var path = Path.Combine(Application.LocalUserAppDataPath, "preferences.xml");
      Save(path);
    }

    public void Save(string path)
    {
      Create().Save(path);
    }

    XDocument Create()
    {
      var root = new XElement(Namespace + "preferences");
      root.Add(CreateRecentFilesList());
      root.Add(CreateTileColorList());
      root.Add(CreateDefaultTileSettings());
      root.Add(CreateFormattingSettings());
      return new XDocument(root);
    }

    XElement CreateRecentFilesList()
    {
      var e = new XElement(Namespace + "recent-files");
      e.AddRange(preferences.RecentFiles
                   .Where(f => !string.IsNullOrWhiteSpace(f))
                   .Select(recentFile => new XElement(Namespace + "file", recentFile)));
      return e;
    }

    XElement CreateTileColorList()
    {
      var e = new XElement(Namespace + "tile-colors");
      e.AddRange(preferences.TileColors.Select(c => new XElement(Namespace + "color", c.AsText())));
      return e;
    }

    XElement CreateDefaultTileSettings()
    {
      var e = new XElement(Namespace + "tile-settings");

      e.Add(new XAttribute("width", preferences.DefaultWidth));
      e.Add(new XAttribute("height", preferences.DefaultHeight));
      e.Add(new XAttribute("tile-type", preferences.DefaultTileType));

      return e;
    }

    XElement CreateFormattingSettings()
    {
      var e = new XElement(Namespace + "formatting-settings");

      e.Add(new XAttribute("text-color", preferences.DefaultTextColor.AsText()));
      e.Add(new XAttribute("tile-color", preferences.DefaultTileColor.AsText()));
      e.Add(new XAttribute("highlight-color", preferences.DefaultTileHighlightColor.AsText()));
      e.Add(new XAttribute("border-color", preferences.DefaultBorderColor.AsText()));
      if (preferences.DefaultBackgroundColor != null)
      {
        e.Add(new XAttribute("background-color", preferences.DefaultBackgroundColor.AsText()));
      }

      e.Add(new XAttribute("text-spacing", preferences.TextSpacing));
      
      e.Add(new XAttribute("font-face", preferences.DefaultFont.Name));
      e.Add(new XAttribute("font-size", preferences.DefaultFont.Size));
      e.Add(new XAttribute("font-style", preferences.DefaultFont.Style));

      return e;
    }
  }

  public class GeneratorPreferencesLoader
  {
    static readonly XNamespace Namespace = "http://www.steropes-ui.org/namespaces/tiles-preferences/1.0";
    readonly GeneratorPreferences preferences;

    public GeneratorPreferencesLoader(GeneratorPreferences preferences)
    {
      this.preferences = preferences ?? throw new ArgumentNullException(nameof(preferences));
    }

    public void Load()
    {
      var path = Path.Combine(Application.LocalUserAppDataPath, "preferences.xml");
      Load(path);
    }

    public void Load(string path)
    {
      var doc = XDocument.Load(path);
      if (doc.Root == null)
      {
        // shuts up Resharper ..
        return;
      }

      var root = doc.Root;

      ParseRecentFiles(root);
      ParseTileColors(root);
      ParseDefaultTileSettings(root);
      ParseFormattingDefaults(root);
    }

    void ParseRecentFiles(XElement root)
    {
      var rf = root.Element(Namespace + "recent-files");
      if (rf != null)
      {
        preferences.RecentFiles.ReplaceAll(rf.Elements(Namespace + "file").Select(f => f.Value));
      }
    }

    void ParseTileColors(XElement root)
    {
      var colors = root.Element(Namespace + "tile-colors");
      var colorsParsed = colors?.Elements(Namespace + "color")
                           .Select(f => f.Value.AsColor())
                           .Where(n => n.HasValue)
                           .Select(n => n.Value)
                           .ToList() ??
                         preferences.TileColors.ToList();
      if (colorsParsed.Count > 0)
      {
        preferences.TileColors.ReplaceAll(colorsParsed);
      }
    }

    void ParseDefaultTileSettings(XElement root)
    {
      var tileDef = root.Element(Namespace + "tile-settings");
      if (tileDef != null)
      {
        preferences.DefaultTileType = TextureParserExtensions.ParseTextureType(tileDef, (string) tileDef.AttributeLocal("tile-type"), TileType.Grid);
        preferences.DefaultWidth = (int?) root.AttributeLocal("width") ?? preferences.DefaultWidth;
        preferences.DefaultHeight = (int?) root.AttributeLocal("height") ?? preferences.DefaultHeight;
      }
    }

    void ParseFormattingDefaults(XElement root)
    {
      var tileDef = root.Element(Namespace + "formatting-settings");
      if (tileDef != null)
      {
        preferences.DefaultTextColor = tileDef.AttributeLocal("text-color").AsColor() ?? preferences.DefaultTextColor;
        preferences.DefaultTileColor = tileDef.AttributeLocal("tile-color").AsColor() ?? preferences.DefaultTileColor;
        preferences.DefaultTileHighlightColor = tileDef.AttributeLocal("highlight-color").AsColor() ?? preferences.DefaultTileHighlightColor;
        preferences.DefaultBorderColor = tileDef.AttributeLocal("border-color").AsColor() ?? preferences.DefaultBorderColor;
        preferences.DefaultBackgroundColor = tileDef.AttributeLocal("background-color").AsColor() ?? preferences.DefaultBackgroundColor;
        preferences.TextSpacing = (int?) tileDef.Attribute("text-spacing") ?? preferences.TextSpacing;

        var fs = (float?) tileDef.AttributeLocal("font-size") ?? preferences.DefaultFont.Size;
        var name = (string) tileDef.AttributeLocal("font-face") ?? preferences.DefaultFont.Name;
        var style = tileDef.AttributeLocal("font-style")?.Value.ParseEnumLenient<FontStyle>() ?? preferences.DefaultFont.Style;
        preferences.DefaultFont = new Font(name, fs, style);
      }
    }
  }
}