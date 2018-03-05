using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.StructureTree
{
  class GridNode : TreeNode
  {
    readonly TextureGrid source;

    public GridNode(TextureGrid collection)
    {
      this.source = collection ?? throw new ArgumentNullException();
      this.source.Tiles.CollectionChanged += OnCollectionChanged;
      this.source.Groups.CollectionChanged += OnCollectionChanged;
      this.source.PropertyChanged += OnPropertyChange;
      this.Text = UpdateName();
      this.Tag = source;
      this.Expand();
      UpdateNodes();
    }

    void OnPropertyChange(object sender, PropertyChangedEventArgs e)
    {
      Text = UpdateName();
    }

    string UpdateName()
    {
      return source.Name;
    }

    void UpdateNodes()
    {
      var data = new List<GroupOrTile>();
      data.AddRange(source.Groups.Select(GroupOrTile.Wrap));
      data.AddRange(source.Tiles.Select(GroupOrTile.Wrap));
      this.Resync(data, c => c.Create());
    }

    void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      UpdateNodes();
    }

    struct GroupOrTile
    {
      public TextureGroup TextureGroup { get; }
      public TextureTile Tile { get; }

      public GroupOrTile(TextureGroup textureGroup) : this()
      {
        this.TextureGroup = textureGroup ?? throw new ArgumentNullException(nameof(textureGroup));
        this.Tile = null;
      }

      public GroupOrTile(TextureTile tile) : this()
      {
        this.Tile = tile ?? throw new ArgumentNullException(nameof(tile));
        this.TextureGroup = null;
      }

      public TreeNode Create()
      {
        if (Tile != null)
        {
          return new TileNode(Tile);
        }
        if (TextureGroup != null)
        {
          return new GroupNode(TextureGroup);
        }
        return null;
      }

      public static GroupOrTile Wrap(TextureGroup g)
      {
        return new GroupOrTile(g);
      }

      public static GroupOrTile Wrap(TextureTile g)
      {
        return new GroupOrTile(g);
      }
    }
  }
}