using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.StructureTree
{
  class GroupNode : TreeNode
  {
    readonly TextureGroup source;

    public GroupNode(TextureGroup source)
    {
      this.source = source ?? throw new ArgumentNullException(nameof(source));
      this.source.Tiles.CollectionChanged += OnCollectionChanged;
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
      return $"({source.X}, {source.Y}): {source.MatcherType} - {source.Name}";
    }

    void UpdateNodes()
    {
      this.Resync(source.Tiles, c => new TileNode(c));
    }

    void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      UpdateNodes();
    }

  }
}