using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Editors
{
  public class DetailEditorCoordinator
  {
    readonly MainModel model;
    readonly List<IConditionalPanelHolder> handlers;
    readonly BaseTextureElementEditor metaDataEditor;
    readonly TextureGridElementEditor textureGridElementEditor;

    public DetailEditorCoordinator(Panel targetPanel, MainModel model)
    {
      this.model = model ?? throw new ArgumentNullException();
      this.model.Selection.CollectionChanged += OnSelectionChanged;

      var splitter = new Panel
      {
        Dock = DockStyle.Fill,
        AutoSize = true,
      };
      
      textureGridElementEditor = new TextureGridElementEditor
      {
        Dock = DockStyle.Top,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink
      };
      textureGridElementEditor.InputReceived += OnTextureGridInputReceived;
      splitter.Controls.Add(textureGridElementEditor);

      metaDataEditor = new BaseTextureElementEditor
      {
        Dock = DockStyle.Top,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink
      };
      metaDataEditor.InputReceived += OnMetaDataInputReceived;
      splitter.Controls.Add(metaDataEditor);

      targetPanel.Controls.Add(splitter);

      this.handlers = new List<IConditionalPanelHolder>
      {
        new ConditionalPanelHolder<TextureFile>(new TextureFileEditor()), 
        new ConditionalPanelHolder<TextureCollection>(new TextureCollectionEditor()),
        new ConditionalPanelHolder<TextureGrid>(new TextureGridEditor()),
        new ConditionalPanelHolder<TextureTile>(new TextureTileEditor())
      };

      foreach (var handler in this.handlers)
      {
        var c = handler.EditorControl;
        if (c != null)
        {
          c.Dock = DockStyle.Top;
          splitter.Controls.Add(c);
        }
      }


      UpdateSelection();
    }

    void OnTextureGridInputReceived(object sender, EventArgs e)
    {
      var selection = model.Selection.OfType<TextureGrid>().FirstOrDefault();
      if (selection != null)
      {
        textureGridElementEditor.ApplyTo(selection.TextureTileFormattingMetaData);
      }
    }

    void OnMetaDataInputReceived(object sender, EventArgs e)
    {
      var selection = model.Selection.OfType<IFormattingInfoProvider>().FirstOrDefault();
      if (selection != null)
      {
        metaDataEditor.ApplyTo(selection.FormattingMetaData);
      }
    }

    void OnSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      UpdateSelection();
    }

    void UpdateSelection()
    {
      var selection = model.Selection.FirstOrDefault();

      foreach (var handler in handlers)
      {
        handler.Update(selection);
      }

      if (selection is IFormattingInfoProvider p)
      {
        metaDataEditor.ApplyFrom(p.FormattingMetaData);
        metaDataEditor.Visible = true;
      }
      else
      {
        metaDataEditor.Visible = false;
      }

      if (selection is TextureGrid g)
      {
        textureGridElementEditor.ApplyFrom(g.TextureTileFormattingMetaData);
        textureGridElementEditor.Visible = true;
      }
      else
      {
        textureGridElementEditor.Visible = false;
      }

    }

    interface IConditionalPanelHolder
    {
      bool Update(object o);
      Control EditorControl { get; }
    }

    class ConditionalPanelHolder<T> : IConditionalPanelHolder where T: class
    {
      readonly IDetailEditor<T> editor;
      T target;

      public ConditionalPanelHolder(IDetailEditor<T> editor)
      {
        this.editor = editor ?? throw new ArgumentNullException(nameof(editor));
        this.editor.InputReceived += OnValidated;
      }

      public Control EditorControl => editor as Control;

      void OnValidated(object sender, EventArgs e)
      {
        if (target != null)
        {
          Console.WriteLine("Input Received.");
          this.editor.ApplyTo(target);
        }
      }

      public bool Update(object o)
      {
        if (o is T data)
        {
          target = data;
          editor.Visible = true;
          editor.ApplyFrom(data);
          return true;
        }
        else
        {
          target = null;
          editor.Visible = false;
          editor.ApplyFrom(default(T));
          return false;
        }
      }
    }
  }
}