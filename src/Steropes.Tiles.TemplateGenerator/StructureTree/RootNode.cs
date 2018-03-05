using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.StructureTree
{
  class RootNode : TreeNode
  {
    readonly TextureFile source;

    public RootNode(TextureFile source)
    {
      this.source = source ?? throw new ArgumentNullException();
      this.source.PropertyChanged += OnPropertyChange;
      this.source.Collections.CollectionChanged += OnCollectionChanged;
      this.Text = UpdateName();
      this.Tag = source;
      this.Expand();
      UpdateNodes();
    }

    void UpdateNodes()
    {
      foreach (var textureCollection in source.Collections)
      {
        Nodes.Add(new CollectionNode(textureCollection));
      }
    }

    void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      this.Resync(source.Collections, c => new CollectionNode(c));
    }

    void OnPropertyChange(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(TextureFile.Name))
      {
        Text = UpdateName();
      }
    }

    string UpdateName()
    {
      if (string.IsNullOrEmpty(source.SourcePath))
      {
        return source.Name;
      }

      return $"{source.Name} ({source.SourcePath})";
    }
  }
}