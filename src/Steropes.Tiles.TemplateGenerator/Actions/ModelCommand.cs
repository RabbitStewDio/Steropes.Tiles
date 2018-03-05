using System;
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
      Enabled = model.Content != null;
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

    protected virtual void NotifyContentChanged(TextureFile old, TextureFile textureFile)
    {
      Enabled = Model.Content != null;
    }

    void OnContentChanged(object sender, PropertyChangedEventArgs e)
    {
      Content = Model.Content;
    }
  }
}