﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Steropes.Tiles.TemplateGenerator.Annotations;

namespace Steropes.Tiles.TemplateGenerator.Model
{
  public class TextureCollection : INotifyPropertyChanged
  {
    string id;

    public TextureCollection(TextureFile parent = null)
    {
      Parent = parent;
      Grids = new ObservableCollection<TextureGrid>();
      Grids.CollectionChanged += OnCollectionChanged;
    }

    public ObservableCollection<TextureGrid> Grids { get; }

    public TextureFile Parent { get; set; }

    public string Id
    {
      get { return id; }
      set
      {
        if (value == id)
        {
          return;
        }

        id = value;
        OnPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.OldItems != null)
      {
        foreach (TextureGrid item in e.OldItems)
        {
          item.Parent = null;
        }
      }

      if (e.NewItems != null)
      {
        foreach (TextureGrid item in e.NewItems)
        {
          item.Parent?.Grids.Remove(item);
          item.Parent = this;
        }
      }
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}