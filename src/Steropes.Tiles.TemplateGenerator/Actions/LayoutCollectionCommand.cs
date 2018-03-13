using System;
using System.Collections.Specialized;
using Steropes.Tiles.TemplateGenerator.Layout;

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
      Enabled = SelectedCollection != null;
    }

    public override void OnActionPerformed(object source, EventArgs args)
    {
      if (Model.Content == null)
      {
        return;
      }

      var insertPoint = SelectedCollection;
      if (insertPoint == null)
      {
        return;
      }

      var gen = new GridGenerator();
      gen.Regenerate(insertPoint);

      var producer = new GridLayouter();
      producer.ArrangeGrids(insertPoint);
    }
  }
}