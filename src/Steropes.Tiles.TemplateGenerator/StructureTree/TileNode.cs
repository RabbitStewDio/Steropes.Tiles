using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.StructureTree
{
  class TileNode : TreeNode
  {
    readonly TextureTile source;

    public TileNode(TextureTile source)
    {
      this.source = source ?? throw new ArgumentNullException(nameof(source));
      this.source.PropertyChanged += OnPropertyChanged;
      this.source.Tags.CollectionChanged += OnTagsChanged;
      this.Text = UpdateName();
      this.Tag = source;
    }

    void OnTagsChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.Text = UpdateName();
    }

    void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      this.Text = UpdateName();
    }

    string UpdateName()
    {
      if (source.Tags.Count == 0)
      {
        return $"({source.X},{source.Y}): <empty>";
      }
      return $"({source.X},{source.Y}): {string.Join(", ", source.Tags)}";
    }
  }
}