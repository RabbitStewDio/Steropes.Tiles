using System.Windows.Forms;

namespace Steropes.Tiles.TemplateGenerator
{
  partial class MainWindow
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.ToolStripSeparator fileMenuSeparator;
      System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
      this.rootContainer = new System.Windows.Forms.SplitContainer();
      this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
      this.structureTree = new System.Windows.Forms.TreeView();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.newFileToolButton = new System.Windows.Forms.ToolStripButton();
      this.openFileToolButton = new System.Windows.Forms.ToolStripButton();
      this.saveFileToolButton = new System.Windows.Forms.ToolStripButton();
      this.exportToolButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.addCollectionToolButton = new System.Windows.Forms.ToolStripButton();
      this.addGridToolButton = new System.Windows.Forms.ToolStripButton();
      this.addTileToolButton = new System.Windows.Forms.ToolStripButton();
      this.arrangeToolButton = new System.Windows.Forms.ToolStripButton();
      this.previewToolButton = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.removeItemToolButton = new System.Windows.Forms.ToolStripButton();
      this.panel1 = new System.Windows.Forms.Panel();
      this.contentSplitter = new System.Windows.Forms.SplitContainer();
      this.selectedContentPane = new System.Windows.Forms.Panel();
      this.previewPicture = new System.Windows.Forms.PictureBox();
      this.menuBar = new System.Windows.Forms.MenuStrip();
      this.fileMenu = new System.Windows.Forms.ToolStripMenuItem ();
      this.newMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.recentSubMenu = new System.Windows.Forms.ToolStripMenuItem ();
      this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.exportMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.editMenu = new System.Windows.Forms.ToolStripMenuItem ();
      this.deleteMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.editSeparator = new System.Windows.Forms.ToolStripSeparator ();
      this.addCollectionMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.addGridMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.addTileMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.separator2 = new System.Windows.Forms.ToolStripSeparator();
      this.arrangeMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.previewMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.separator3 = new System.Windows.Forms.ToolStripSeparator ();
      this.preferencesMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.menuItem1 = new System.Windows.Forms.ToolStripMenuItem ();
      this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      fileMenuSeparator = new System.Windows.Forms.ToolStripSeparator();
      toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      ((System.ComponentModel.ISupportInitialize)(this.rootContainer)).BeginInit();
      this.rootContainer.Panel1.SuspendLayout();
      this.rootContainer.Panel2.SuspendLayout();
      this.rootContainer.SuspendLayout();
      this.toolStripContainer1.ContentPanel.SuspendLayout();
      this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
      this.toolStripContainer1.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.contentSplitter)).BeginInit();
      this.contentSplitter.Panel1.SuspendLayout();
      this.contentSplitter.Panel2.SuspendLayout();
      this.contentSplitter.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).BeginInit();
      this.SuspendLayout();
      // 
      // fileMenuSeparator
      // 
      fileMenuSeparator.Text = "-";
      // 
      // toolStripSeparator1
      // 
      toolStripSeparator1.Name = "toolStripSeparator1";
      toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // rootContainer
      // 
      this.rootContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.rootContainer.Location = new System.Drawing.Point(0, 0);
      this.rootContainer.Name = "rootContainer";
      // 
      // rootContainer.Panel1
      // 
      this.rootContainer.Panel1.Controls.Add(this.toolStripContainer1);
      // 
      // rootContainer.Panel2
      // 
      this.rootContainer.Panel2.Controls.Add(this.panel1);
      this.rootContainer.Size = new System.Drawing.Size(946, 537);
      this.rootContainer.SplitterDistance = 282;
      this.rootContainer.TabIndex = 0;
      // 
      // toolStripContainer1
      // 
      this.toolStripContainer1.BottomToolStripPanelVisible = false;
      // 
      // toolStripContainer1.ContentPanel
      // 
      this.toolStripContainer1.ContentPanel.Controls.Add(this.structureTree);
      this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(282, 512);
      this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolStripContainer1.LeftToolStripPanelVisible = false;
      this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
      this.toolStripContainer1.Name = "toolStripContainer1";
      this.toolStripContainer1.RightToolStripPanelVisible = false;
      this.toolStripContainer1.Size = new System.Drawing.Size(282, 537);
      this.toolStripContainer1.TabIndex = 1;
      this.toolStripContainer1.Text = "toolStripContainer1";
      // 
      // toolStripContainer1.TopToolStripPanel
      // 
      this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
      // 
      // structureTree
      // 
      this.structureTree.Dock = System.Windows.Forms.DockStyle.Fill;
      this.structureTree.Location = new System.Drawing.Point(0, 0);
      this.structureTree.Name = "structureTree";
      this.structureTree.Size = new System.Drawing.Size(282, 512);
      this.structureTree.TabIndex = 0;
      // 
      // toolStrip1
      // 
      this.toolStrip1.AllowMerge = false;
      this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
      this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFileToolButton,
            this.openFileToolButton,
            this.saveFileToolButton,
            this.exportToolButton,
            this.toolStripSeparator2,
            this.addCollectionToolButton,
            this.addGridToolButton,
            this.addTileToolButton,
            toolStripSeparator1,
            this.arrangeToolButton,
            this.previewToolButton,
            this.toolStripSeparator3,
            this.removeItemToolButton});
      this.toolStrip1.Location = new System.Drawing.Point(3, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(251, 25);
      this.toolStrip1.TabIndex = 0;
      this.toolStrip1.Text = "Text";
      // 
      // newFileToolButton
      // 
      this.newFileToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.newFileToolButton.Image = global::Steropes.Tiles.TemplateGenerator.Properties.Resources.NewDocument;
      this.newFileToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.newFileToolButton.Name = "newFileToolButton";
      this.newFileToolButton.Size = new System.Drawing.Size(23, 22);
      this.newFileToolButton.Text = "New File";
      // 
      // openFileToolButton
      // 
      this.openFileToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.openFileToolButton.Image = global::Steropes.Tiles.TemplateGenerator.Properties.Resources.Open_48px;
      this.openFileToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.openFileToolButton.Name = "openFileToolButton";
      this.openFileToolButton.Size = new System.Drawing.Size(23, 22);
      this.openFileToolButton.Text = "Open File";
      // 
      // saveFileToolButton
      // 
      this.saveFileToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.saveFileToolButton.Image = global::Steropes.Tiles.TemplateGenerator.Properties.Resources.Save_48px;
      this.saveFileToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.saveFileToolButton.Name = "saveFileToolButton";
      this.saveFileToolButton.Size = new System.Drawing.Size(23, 22);
      this.saveFileToolButton.Text = "Save File";
      // 
      // exportToolButton
      // 
      this.exportToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.exportToolButton.Image = global::Steropes.Tiles.TemplateGenerator.Properties.Resources.Export_48px;
      this.exportToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.exportToolButton.Name = "exportToolButton";
      this.exportToolButton.Size = new System.Drawing.Size(23, 22);
      this.exportToolButton.Text = "Export";
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // addCollectionToolButton
      // 
      this.addCollectionToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.addCollectionToolButton.Image = global::Steropes.Tiles.TemplateGenerator.Properties.Resources.AddCollection;
      this.addCollectionToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.addCollectionToolButton.Name = "addCollectionToolButton";
      this.addCollectionToolButton.Size = new System.Drawing.Size(23, 22);
      this.addCollectionToolButton.Text = "Add Collection ..";
      // 
      // addGridToolButton
      // 
      this.addGridToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.addGridToolButton.Image = global::Steropes.Tiles.TemplateGenerator.Properties.Resources.AddGrid;
      this.addGridToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.addGridToolButton.Name = "addGridToolButton";
      this.addGridToolButton.Size = new System.Drawing.Size(23, 22);
      this.addGridToolButton.Text = "Add Grid ..";
      // 
      // addTileToolButton
      // 
      this.addTileToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.addTileToolButton.Image = global::Steropes.Tiles.TemplateGenerator.Properties.Resources.AddTile;
      this.addTileToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.addTileToolButton.Name = "addTileToolButton";
      this.addTileToolButton.Size = new System.Drawing.Size(23, 22);
      this.addTileToolButton.Text = "Add Tile ..";
      // 
      // arrangeToolButton
      // 
      this.arrangeToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.arrangeToolButton.Image = global::Steropes.Tiles.TemplateGenerator.Properties.Resources.Categorize_48px;
      this.arrangeToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.arrangeToolButton.Name = "arrangeToolButton";
      this.arrangeToolButton.Size = new System.Drawing.Size(23, 22);
      this.arrangeToolButton.Text = "Arrange Grids";
      // 
      // previewToolButton
      // 
      this.previewToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.previewToolButton.Image = global::Steropes.Tiles.TemplateGenerator.Properties.Resources.PreviewPane_48px;
      this.previewToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.previewToolButton.Name = "previewToolButton";
      this.previewToolButton.Size = new System.Drawing.Size(23, 22);
      this.previewToolButton.Text = "Preview ";
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
      // 
      // removeItemToolButton
      // 
      this.removeItemToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.removeItemToolButton.Image = global::Steropes.Tiles.TemplateGenerator.Properties.Resources.RemoveImage_48px;
      this.removeItemToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.removeItemToolButton.Name = "removeItemToolButton";
      this.removeItemToolButton.Size = new System.Drawing.Size(23, 22);
      this.removeItemToolButton.Text = "Remove Item";
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.contentSplitter);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(660, 537);
      this.panel1.TabIndex = 1;
      // 
      // contentSplitter
      // 
      this.contentSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
      this.contentSplitter.Location = new System.Drawing.Point(0, 0);
      this.contentSplitter.Name = "contentSplitter";
      // 
      // contentSplitter.Panel1
      // 
      this.contentSplitter.Panel1.AutoScroll = true;
      this.contentSplitter.Panel1.BackColor = System.Drawing.SystemColors.Control;
      this.contentSplitter.Panel1.Controls.Add(this.selectedContentPane);
      // 
      // contentSplitter.Panel2
      // 
      this.contentSplitter.Panel2.AutoScroll = true;
      this.contentSplitter.Panel2.BackColor = System.Drawing.Color.White;
      this.contentSplitter.Panel2.Controls.Add(this.previewPicture);
      this.contentSplitter.Size = new System.Drawing.Size(660, 537);
      this.contentSplitter.SplitterDistance = 277;
      this.contentSplitter.TabIndex = 0;
      // 
      // selectedContentPane
      // 
      this.selectedContentPane.AutoScroll = true;
      this.selectedContentPane.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.selectedContentPane.Dock = System.Windows.Forms.DockStyle.Fill;
      this.selectedContentPane.Location = new System.Drawing.Point(0, 0);
      this.selectedContentPane.Name = "selectedContentPane";
      this.selectedContentPane.Size = new System.Drawing.Size(277, 537);
      this.selectedContentPane.TabIndex = 0;
      // 
      // previewPicture
      // 
      this.previewPicture.BackColor = System.Drawing.Color.White;
      this.previewPicture.Location = new System.Drawing.Point(0, 0);
      this.previewPicture.Name = "previewPicture";
      this.previewPicture.Size = new System.Drawing.Size(387, 537);
      this.previewPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.previewPicture.TabIndex = 0;
      this.previewPicture.TabStop = false;
      // 
      // menuBar
      // 
      this.menuBar.Items.AddRange(new ToolStripItem[] {
            this.fileMenu,
            this.editMenu,
            this.menuItem1});
      // 
      // fileMenu
      // 
      this.fileMenu.DropDownItems.AddRange(new ToolStripItem[] {
            this.newMenuItem,
            this.openMenuItem,
            this.recentSubMenu,
            this.saveMenuItem,
            this.saveAsMenuItem,
            this.exportMenuItem,
            fileMenuSeparator,
            this.exitMenuItem});
      this.fileMenu.Text = "&File";
      // 
      // newMenuItem
      // 
      this.newMenuItem.ShortcutKeys = Keys.Control | Keys.N;
      this.newMenuItem.Text = "&New ...";
      // 
      // openMenuItem
      // 
      this.openMenuItem.ShortcutKeys = Keys.Control | Keys.O;
      this.openMenuItem.Text = "&Open ...";
      // 
      // recentSubMenu
      // 
      this.recentSubMenu.Text = "&Recent";
      // 
      // saveMenuItem
      // 
      this.saveMenuItem.ShortcutKeys = Keys.Control | Keys.S;
      this.saveMenuItem.Text = "&Save";
      // 
      // saveAsMenuItem
      // 
      this.saveAsMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.O;
      this.saveAsMenuItem.Text = "Save &As ...";
      // 
      // exportMenuItem
      // 
      this.exportMenuItem.Text = "Export";
      // 
      // exitMenuItem
      // 
      this.exitMenuItem.Text = "E&xit";
      this.exitMenuItem.Click += new System.EventHandler(this.OnExit);
      // 
      // editMenu
      // 
      this.editMenu.DropDownItems.AddRange(new ToolStripItem[] {
            this.deleteMenuItem,
            this.editSeparator,
            this.addCollectionMenuItem,
            this.addGridMenuItem,
            this.addTileMenuItem,
            this.separator2,
            this.arrangeMenuItem,
            this.previewMenuItem,
            this.separator3,
            this.preferencesMenuItem});
      this.editMenu.Text = "&Edit";
      // 
      // deleteMenuItem
      // 
      this.deleteMenuItem.ShortcutKeys = Keys.Delete;
      this.deleteMenuItem.Text = "Delete Item";
      // 
      // editSeparator
      // 
      this.editSeparator.Text = "-";
      // 
      // addCollectionMenuItem
      // 
      this.addCollectionMenuItem.ShortcutKeys = Keys.Control | Keys.D1;
      this.addCollectionMenuItem.Text = "Add Collection";
      // 
      // addGridMenuItem
      // 
      this.addGridMenuItem.ShortcutKeys = Keys.Control | Keys.D2;
      this.addGridMenuItem.Text = "Add Grid";
      // 
      // addTileMenuItem
      // 
      this.addTileMenuItem.ShortcutKeys = Keys.Control | Keys.D3;
      this.addTileMenuItem.Text = "Add Tile";
      // 
      // separator2
      // 
      this.separator2.Text = "-";
      // 
      // arrangeMenuItem
      // 
      this.arrangeMenuItem.ShortcutKeys = Keys.Control | Keys.L;
      this.arrangeMenuItem.Text = "Arrange";
      // 
      // previewMenuItem
      // 
      this.previewMenuItem.ShortcutKeys = Keys.Control | Keys.P;
      this.previewMenuItem.Text = "Preview";
      // 
      // separator3
      // 
      this.separator3.Text = "-";
      // 
      // preferencesMenuItem
      // 
      this.preferencesMenuItem.Text = "Preferences";
      // 
      // menuItem1
      // 
      this.menuItem1.DropDownItems.Add(this.aboutMenuItem);
      this.menuItem1.Text = "?";
      // 
      // menuItem2
      // 
      this.aboutMenuItem.Text = "About";
      this.aboutMenuItem.Click += new System.EventHandler(this.OnAboutClick);
      // 
      // MainWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(946, 537);
      this.Controls.Add(this.rootContainer);
      this.MainMenuStrip = this.menuBar;
      this.MinimumSize = new System.Drawing.Size(600, 400);
      this.Name = "MainWindow";
      this.Text = "Tile Generator";
      this.rootContainer.Panel1.ResumeLayout(false);
      this.rootContainer.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.rootContainer)).EndInit();
      this.rootContainer.ResumeLayout(false);
      this.toolStripContainer1.ContentPanel.ResumeLayout(false);
      this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
      this.toolStripContainer1.TopToolStripPanel.PerformLayout();
      this.toolStripContainer1.ResumeLayout(false);
      this.toolStripContainer1.PerformLayout();
      this.toolStrip1.ResumeLayout(false);
      this.toolStrip1.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.contentSplitter.Panel1.ResumeLayout(false);
      this.contentSplitter.Panel2.ResumeLayout(false);
      this.contentSplitter.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.contentSplitter)).EndInit();
      this.contentSplitter.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer rootContainer;
    private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    private System.Windows.Forms.TreeView structureTree;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton addCollectionToolButton;
    private System.Windows.Forms.ToolStripButton addGridToolButton;
    private System.Windows.Forms.MenuStrip menuBar;
    private System.Windows.Forms.ToolStripMenuItem fileMenu;
    private System.Windows.Forms.ToolStripMenuItem openMenuItem;
    private System.Windows.Forms.ToolStripMenuItem recentSubMenu;
    private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editMenu;
    private System.Windows.Forms.ToolStripMenuItem deleteMenuItem;
    private System.Windows.Forms.ToolStripSeparator editSeparator;
    private System.Windows.Forms.ToolStripMenuItem addCollectionMenuItem;
    private System.Windows.Forms.ToolStripMenuItem addGridMenuItem;
    private System.Windows.Forms.ToolStripMenuItem addTileMenuItem;
    private System.Windows.Forms.SplitContainer contentSplitter;
    private System.Windows.Forms.ToolStripMenuItem newMenuItem;
    private System.Windows.Forms.ToolStripButton addTileToolButton;
    private System.Windows.Forms.ToolStripButton removeItemToolButton;
    private System.Windows.Forms.PictureBox previewPicture;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Panel selectedContentPane;
    private System.Windows.Forms.ToolStripButton newFileToolButton;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripButton arrangeToolButton;
    private System.Windows.Forms.ToolStripButton previewToolButton;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripSeparator separator2;
    private System.Windows.Forms.ToolStripMenuItem arrangeMenuItem;
    private System.Windows.Forms.ToolStripMenuItem previewMenuItem;
    private System.Windows.Forms.ToolStripSeparator separator3;
    private System.Windows.Forms.ToolStripMenuItem preferencesMenuItem;
    private System.Windows.Forms.ToolStripButton openFileToolButton;
    private System.Windows.Forms.ToolStripButton saveFileToolButton;
    private System.Windows.Forms.ToolStripMenuItem menuItem1;
    private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
    private System.Windows.Forms.ToolStripButton exportToolButton;
    private System.Windows.Forms.ToolStripMenuItem exportMenuItem;
  }
}

