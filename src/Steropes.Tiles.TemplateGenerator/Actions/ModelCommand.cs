using System;
using System.Collections.Specialized;
using System.ComponentModel;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
    public class ModelCommand : Command
    {
        TextureFile content;

        public ModelCommand(MainModel model)
        {
            this.Model = model ?? throw new ArgumentNullException(nameof(model));

            this.Model.PropertyChanged += OnContentChanged;
            this.Model.Selection.CollectionChanged += OnSelectionChanged;
        }

        void OnSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Enabled = RefreshEnabled();
        }

        public MainModel Model { get; }

        protected TextureFile Content
        {
            get { return content; }
            set
            {
                if (Equals(value, content))
                {
                    return;
                }

                var old = content;
                content = value;
                OnPropertyChanged();
                NotifyContentChanged(old, content);
            }
        }

        protected TextureFile SelectedFile
        {
            get
            {
                foreach (var s in Model.Selection)
                {
                    if (s is TextureFile c)
                    {
                        return c;
                    }
                }

                return SelectedCollection?.Parent;
            }
        }

        protected TextureCollection SelectedCollection
        {
            get
            {
                foreach (var s in Model.Selection)
                {
                    if (s is TextureCollection c)
                    {
                        return c;
                    }
                }

                return SelectedGrid?.Parent;
            }
        }

        protected TextureGrid SelectedGrid
        {
            get
            {
                foreach (var s in Model.Selection)
                {
                    if (s is TextureGrid c)
                    {
                        return c;
                    }
                }

                return SelectedTile?.Parent;
            }
        }

        protected TextureTile SelectedTile
        {
            get
            {
                foreach (var s in Model.Selection)
                {
                    if (s is TextureTile c)
                    {
                        return c;
                    }
                }

                return null;
            }
        }

        protected virtual void NotifyContentChanged(TextureFile old, TextureFile textureFile)
        {
            Enabled = RefreshEnabled();
        }

        protected virtual bool RefreshEnabled()
        {
            return Model.Content != null;
        }

        void OnContentChanged(object sender, PropertyChangedEventArgs e)
        {
            Content = Model.Content;
        }
    }
}
