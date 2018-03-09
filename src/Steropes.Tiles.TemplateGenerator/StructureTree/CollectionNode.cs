using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.StructureTree
{
  class CollectionNode : TreeNode
  {
    readonly TextureCollection collection;

    public CollectionNode(TextureCollection collection)
    {
      this.collection = collection ?? throw new ArgumentNullException();
      this.collection.Grids.CollectionChanged += OnCollectionChanged;
      this.collection.PropertyChanged += OnPropertyChange;
      this.Text = UpdateName();
      this.Tag = collection;
      this.Expand();
      UpdateNodes();
    }

    void OnPropertyChange(object sender, PropertyChangedEventArgs e)
    {
      TreeView?.BeginUpdate();
      Text = UpdateName();
      TreeView?.EndUpdate();
    }

    string UpdateName()
    {
      return collection.Id;
    }

    void UpdateNodes()
    {
      this.Resync(collection.Grids, c => new GridNode(c));
    }

    void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      UpdateNodes();
    }
  }
}