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
            this.source.PropertyChanged += OnPropertyChange;
            this.Text = UpdateName();
            this.Tag = source;
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
            return source.Name;
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
