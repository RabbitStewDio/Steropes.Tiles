﻿using System;
using System.Collections.Specialized;
using Steropes.Tiles.TemplateGenerator.Layout;

namespace Steropes.Tiles.TemplateGenerator.Actions
{
    public sealed class PreviewCommand : ModelCommand
    {
        public PreviewCommand(MainModel model) : base(model)
        {
            Enabled = RefreshEnabled();
        }

        protected override bool RefreshEnabled()
        {
            return base.RefreshEnabled() && SelectedCollection != null;
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

            var producer = new GridCollectionPainter(Model.Preferences);
            var bitmap = producer.Produce(insertPoint);
            Model.PreviewBitmap = bitmap;
        }
    }
}
