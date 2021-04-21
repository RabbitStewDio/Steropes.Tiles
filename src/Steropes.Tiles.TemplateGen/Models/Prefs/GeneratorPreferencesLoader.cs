using SkiaSharp;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Steropes.Tiles.TemplateGen.Models.Prefs
{
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
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Steropes.Tiles/preferences.xml");
            if (File.Exists(path))
            {
                Load(path);
            }
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
                                     .Select(n => n ?? default)
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
                preferences.DefaultTileType = TextureParserExtensions.ParseTextureType(tileDef, (string?) tileDef.AttributeLocal("tile-type"), TileType.Grid);
                preferences.DefaultWidth = (int?)root.AttributeLocal("width") ?? preferences.DefaultWidth;
                preferences.DefaultHeight = (int?)root.AttributeLocal("height") ?? preferences.DefaultHeight;
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
                preferences.DefaultTileAnchorColor = tileDef.AttributeLocal("anchor-color").AsColor() ?? preferences.DefaultTileAnchorColor;
                preferences.DefaultBorderColor = tileDef.AttributeLocal("border-color").AsColor() ?? preferences.DefaultBorderColor;
                preferences.DefaultBackgroundColor = tileDef.AttributeLocal("background-color").AsColor() ?? preferences.DefaultBackgroundColor;
                preferences.TextSpacing = (int?)tileDef.Attribute("text-spacing") ?? preferences.TextSpacing;

                var fs = (float?)tileDef.AttributeLocal("font-size") ?? preferences.DefaultFont.Size;
                var name = (string?)tileDef.AttributeLocal("font-face") ?? preferences.DefaultFont.Typeface ?? SKTypeface.Default.FamilyName;
                var style = (bool?)tileDef.AttributeLocal("font-weight") ?? preferences.DefaultFont.Bold;
                var weight = (bool?)tileDef.AttributeLocal("font-style") ?? preferences.DefaultFont.Italic;
                preferences.DefaultFont = new Font(name, fs, weight, style);
            }
        }
    }
}
