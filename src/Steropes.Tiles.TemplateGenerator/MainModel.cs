using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Steropes.Tiles.TemplateGenerator.Actions;
using Steropes.Tiles.TemplateGenerator.Annotations;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator
{
  public class MainModel: INotifyPropertyChanged
  {
    TextureFile content;
    Bitmap previewBitmap;

    public event PropertyChangedEventHandler PropertyChanged;

    public MainModel()
    {
      Selection = new ObservableCollection<object>();
    }

    public ObservableCollection<object> Selection { get; }

    public TextureFile Content
    {
      get { return content; }
      set
      {
        if (Equals(value, content)) return;
        content = value;
        OnPropertyChanged();
      }
    }

    public Bitmap PreviewBitmap
    {
      get
      {
        return previewBitmap;
      }
      set
      {
        if (Equals(value, previewBitmap))
        {
          return;
        }

        previewBitmap?.Dispose();
        previewBitmap = value;
        OnPropertyChanged();
      }
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public static void HandleError(string message, string title)
    {
      MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public bool QueryShouldClose()
    {
      if (Content != null && Content.Modified)
      {
        var r = MessageBox.Show("You have unsaved changes. Save before closing?",
                                "Warning: Unsaved changes", 
                                MessageBoxButtons.YesNoCancel);
        if (r == DialogResult.Yes)
        {
          try
          {
            var saveCommand = new SaveCommand(this, false);
            saveCommand.PerformSave();
          }
          catch (SaveException e)
          {
            HandleError($"Unable to save file in {e.File}.\n\nError Message: {e}", "Error Saving File");
            return false;
          }
          catch (Exception e)
          {
            HandleError($"Unable to save file.\n\nError Message: {e}", "Error Saving File");
            return false;
          }
        }

        if (r == DialogResult.Cancel)
        {
          return false;
        }
      }

      return true;
    }
  }
}