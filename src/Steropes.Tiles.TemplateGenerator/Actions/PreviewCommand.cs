using System;
using System.Collections.Specialized;
using System.Linq;
using Steropes.Tiles.TemplateGenerator.Layout;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
    public class LayoutCollectionCommand: ModelCommand
  {
    public LayoutCollectionCommand(MainModel model): base(model)
    {
      Model.Selection.CollectionChanged += OnSelectionChanged;
    }

    void OnSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      Enabled = Model.Content != null && Model.Selection.OfType<TextureCollection>().Any();
    }

    public override void OnActionPerformed(object source, EventArgs args)
    {
      if (Model.Content == null)
      {
        return;
      }

      var insertPoint = Model.Selection.OfType<TextureCollection>().FirstOrDefault();
      if (insertPoint == null)
      {
        return;
      }

      var producer = new GridLayouter();
      producer.ArrangeGrids(insertPoint);
    }
  }

  public class PreviewCommand: ModelCommand
  {
    public PreviewCommand(MainModel model): base(model)
    {
      Model.Selection.CollectionChanged += OnSelectionChanged;
    }

    void OnSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      Enabled = Model.Content != null && Model.Selection.OfType<TextureCollection>().Any();
    }

    public override void OnActionPerformed(object source, EventArgs args)
    {
      if (Model.Content == null)
      {
        return;
      }

      var insertPoint = Model.Selection.OfType<TextureCollection>().FirstOrDefault();
      if (insertPoint == null)
      {
        return;
      }

      var producer = new GridProducer();
      var bitmap = producer.Produce(insertPoint);
      Model.PreviewBitmap = bitmap;
    }
  }
}