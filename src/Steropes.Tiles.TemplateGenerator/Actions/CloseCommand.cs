using System;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
    public class CloseCommand : ModelCommand
    {
        public CloseCommand(MainModel model) : base(model)
        { }

        public override void OnActionPerformed(object source, EventArgs args)
        {
            if (Model.QueryShouldClose())
            {
                Model.Content = null;
            }
        }
    }
}
