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
    readonly Panel targetPanel;
    readonly MainModel model;
    readonly List<IConditionalPanelHolder> handlers;
    readonly BaseTextureElementEditor metaDataEditor;

    public DetailEditorCoordinator(Panel targetPanel, MainModel model)
    {
      this.targetPanel = targetPanel ?? throw new ArgumentNullException(nameof(targetPanel));

      this.model = model ?? throw new ArgumentNullException();
      this.model.Selection.CollectionChanged += OnSelectionChanged;

      var splitter = new SplitContainer
      {
        Orientation = Orientation.Horizontal,
        Dock = DockStyle.Fill,
        AutoSize = true,
      };
      
      metaDataEditor = new BaseTextureElementEditor
      {
        Dock = DockStyle.Fill,
        AutoSize = true,
        AutoSizeMode = AutoSizeMode.GrowAndShrink
      };
      metaDataEditor.InputReceived += OnMetaDataInputReceived;

      splitter.Panel2.Controls.Add(metaDataEditor);

      this.targetPanel.Controls.Add(splitter);

      this.handlers = new List<IConditionalPanelHolder>
      {
        new ConditionalPanelHolder<TextureFile>(new TextureFileEditor()), 
        new ConditionalPanelHolder<TextureCollection>(new TextureCollectionEditor()),
        new ConditionalPanelHolder<TextureGrid>(new TextureGridEditor())
      };

      foreach (var handler in this.handlers)
      {
        var c = handler.EditorControl;
        if (c != null)
        {
          c.Dock = DockStyle.Fill;
          splitter.Panel1.Controls.Add(c);
        }
      }

      UpdateSelection();
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

  public interface IDetailEditor<T>
  {
    bool Valid { get; }
    bool Visible { get; set; }
    void ApplyFrom(T data);
    T ApplyTo(T data);

    event EventHandler InputReceived;

  }
}