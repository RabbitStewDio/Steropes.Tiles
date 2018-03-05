using System;
using System.Collections.Specialized;
using System.Linq;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
  public class AddGroupsCommand : ModelCommand
  {
    int counter;

    public AddGroupsCommand(MainModel model) : base(model)
    {
      Model.Selection.CollectionChanged += OnSelectionChanged;
    }

    void OnSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      Enabled = Model.Content != null && Model.Selection.OfType<TextureGrid>().Any();
    }

    public override void OnActionPerformed(object source, EventArgs args)
    {
      if (Model.Content == null)
      {
        return;
      }

      var insertPoint = Model.Selection.OfType<TextureGrid>().FirstOrDefault();
      if (insertPoint == null)
      {
        return;
      }

      counter += 1;
      var textureGroup = new TextureGroup();
      textureGroup.Name = $"Group {counter}";
      insertPoint.Groups.Add(textureGroup);

      Model.Selection.Clear();
      Model.Selection.Add(textureGroup);

    }
  }
}