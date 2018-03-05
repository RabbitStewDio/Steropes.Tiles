using System.Collections.Specialized;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
  public class EditCommand: ModelCommand
  {
    public EditCommand(MainModel model) : base(model)
    {
      Model.Selection.CollectionChanged += OnSelectionChanged;
    }

    void OnSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      Enabled = Model.Content != null && Model.Selection.Count > 0;
    }
  }
}