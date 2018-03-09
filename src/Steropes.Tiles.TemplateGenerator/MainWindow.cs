﻿using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Steropes.Tiles.TemplateGenerator.Actions;
using Steropes.Tiles.TemplateGenerator.Annotations;
using Steropes.Tiles.TemplateGenerator.Editors;
using Steropes.Tiles.TemplateGenerator.StructureTree;

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
namespace Steropes.Tiles.TemplateGenerator
{
  public partial class MainWindow : Form
  {
    readonly NewCommand newCommand;
    readonly OpenCommand openCommand;
    readonly SaveCommand saveCommand;
    readonly SaveCommand saveAsCommand;
    readonly CloseCommand closeCommand;
    readonly DeleteCommnad deleteCommnad;

    readonly AddCollectionCommand addCollectionCommand;
    readonly AddGridCommand addGridCommand;
    readonly AddTilesCommand addTilesCommand;

    readonly LayoutCollectionCommand layoutCollectionCommand;
    readonly PreviewCommand previewCommand;

    readonly MainModel model;
    readonly TileSetPropertiesDialog tileSetProperties;
    [UsedImplicitly] readonly DetailEditorCoordinator editorCoordinator;

    readonly KryptonManager manager;

    public MainWindow()
    {
      manager = new KryptonManager();
      manager.GlobalPaletteMode = PaletteModeManager.ProfessionalSystem;

      model = new MainModel();
      model.PropertyChanged += OnModelChange;

      tileSetProperties = new TileSetPropertiesDialog();
      tileSetProperties.Owner = this;

      InitializeComponent();

      editorCoordinator = new DetailEditorCoordinator(selectedContentPane, model);

      newCommand = new NewCommand(model, tileSetProperties);
      newCommand.Install(newMenuItem);
      newCommand.Install(newFileToolButton);

      openCommand = new OpenCommand(model);
      openCommand.Install(openMenuItem);

      saveCommand = new SaveCommand(model, false);
      saveCommand.Install(saveMenuItem);

      saveAsCommand = new SaveCommand(model, true);
      saveAsCommand.Install(saveAsMenuItem);

      closeCommand = new CloseCommand(model);
      closeCommand.Install(closeMenuItem);

      deleteCommnad = new DeleteCommnad(model);
      deleteCommnad.Install(deleteMenuItem);
      deleteCommnad.Install(removeItemToolButton);

      addGridCommand = new AddGridCommand(model);
      addGridCommand.Install(addGridMenuItem);
      addGridCommand.Install(addGridToolButton);

      addCollectionCommand = new AddCollectionCommand(model);
      addCollectionCommand.Install(addCollectionMenuItem);
      addCollectionCommand.Install(addCollectionToolButton);

      addTilesCommand = new AddTilesCommand(model);
      addTilesCommand.Install(addTileMenuItem);
      addTilesCommand.Install(addTileToolButton);

      previewCommand = new PreviewCommand(model);
      previewCommand.Install(previewMenuItem);
      previewCommand.Install(previewToolButton);

      layoutCollectionCommand = new LayoutCollectionCommand(model);
      layoutCollectionCommand.Install(arrangeMenuItem);
      layoutCollectionCommand.Install(arrangeToolButton);

      recentSubMenu.Enabled = false;

      structureTree.AfterSelect += OnTreeSelectionChanged;
      model.Selection.CollectionChanged += OnSelectionChanged;
      this.Closing += OnClosing;
    }

    bool processingSelectionSync;

    void OnSelectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      TreeNode FindNodeByTag(TreeNodeCollection c, object tag)
      {
        if (tag == null)
        {
          return null;
        }

        foreach (var o in c.AsList())
        {
          if (o.Tag == tag)
          {
            return o;
          }

          var rec = FindNodeByTag(o.Nodes, tag);
          if (rec != null)
          {
            return rec;
          }
        }

        return null;
      }

      if (processingSelectionSync)
      {
        return;
      }

      try
      {
        processingSelectionSync = true;
        var selection = model.Selection.LastOrDefault();
        structureTree.SelectedNode = FindNodeByTag(structureTree.Nodes, selection);
        structureTree.SelectedNode?.Expand();
      }
      finally
      {
        processingSelectionSync = false;
      }
    }

    void OnTreeSelectionChanged(object sender, TreeViewEventArgs e)
    {
      if (processingSelectionSync)
      {
        return;
      }

      try
      {
        processingSelectionSync = true;
        model.Selection.Clear();
        if (structureTree.SelectedNode != null)
        {
          model.Selection.Add(structureTree.SelectedNode.Tag);
        }
      }
      finally
      {
        processingSelectionSync = false;
      }
    }

    void OnModelChange(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(MainModel.Content))
      {
        structureTree.SuspendLayout();
        structureTree.Nodes.Clear();
        if (model.Content != null)
        {
          structureTree.Nodes.Add(new RootNode(model.Content));
        }

        structureTree.ResumeLayout();
      }

      if (e.PropertyName == nameof(MainModel.PreviewBitmap))
      {
        Console.WriteLine("Updated preview image");
        previewPicture.Image = model.PreviewBitmap;
      }
    }

    void OnClosing(object sender, CancelEventArgs e)
    {
      if (!AttemptExit())
      {
        e.Cancel = true;
      }
    }

    void OnExit(object sender, EventArgs e)
    {
      this.Close();
    }

    bool AttemptExit()
    {
      return true;
    }
  }
}