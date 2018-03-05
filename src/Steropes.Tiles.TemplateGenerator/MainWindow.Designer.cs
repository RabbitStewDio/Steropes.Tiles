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
      System.Windows.Forms.MenuItem fileMenuSeparator;
      System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
      this.rootContainer = new System.Windows.Forms.SplitContainer();
      this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
      this.structureTree = new System.Windows.Forms.TreeView();
      this.toolStrip1 = new System.Windows.Forms.ToolStrip();
      this.addCollectionToolButton = new System.Windows.Forms.ToolStripButton();
      this.addGridToolButton = new System.Windows.Forms.ToolStripButton();
      this.addGroupToolButton = new System.Windows.Forms.ToolStripButton();
      this.addTileToolButton = new System.Windows.Forms.ToolStripButton();
      this.editItemToolButton = new System.Windows.Forms.ToolStripButton();
      this.removeItemToolButton = new System.Windows.Forms.ToolStripButton();
      this.contentSplitter = new System.Windows.Forms.SplitContainer();
      this.menuBar = new System.Windows.Forms.MainMenu(this.components);
      this.fileMenu = new System.Windows.Forms.MenuItem();
      this.newMenuItem = new System.Windows.Forms.MenuItem();
      this.openMenuItem = new System.Windows.Forms.MenuItem();
      this.recentSubMenu = new System.Windows.Forms.MenuItem();
      this.saveMenuItem = new System.Windows.Forms.MenuItem();
      this.saveAsMenuItem = new System.Windows.Forms.MenuItem();
      this.closeMenuItem = new System.Windows.Forms.MenuItem();
      this.exitMenuItem = new System.Windows.Forms.MenuItem();
      this.editMenu = new System.Windows.Forms.MenuItem();
      this.editMenuItem = new System.Windows.Forms.MenuItem();
      this.deleteMenuItem = new System.Windows.Forms.MenuItem();
      this.editSeparator = new System.Windows.Forms.MenuItem();
      this.addCollectionMenuItem = new System.Windows.Forms.MenuItem();
      this.addGridMenuItem = new System.Windows.Forms.MenuItem();
      this.addGroupMenuItem = new System.Windows.Forms.MenuItem();
      this.addTileMenuItem = new System.Windows.Forms.MenuItem();
      this.previewPicture = new System.Windows.Forms.PictureBox();
      this.panel1 = new System.Windows.Forms.Panel();
      this.selectedContentPane = new System.Windows.Forms.Panel();
      fileMenuSeparator = new System.Windows.Forms.MenuItem();
      toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      ((System.ComponentModel.ISupportInitialize)(this.rootContainer)).BeginInit();
      this.rootContainer.Panel1.SuspendLayout();
      this.rootContainer.Panel2.SuspendLayout();
      this.rootContainer.SuspendLayout();
      this.toolStripContainer1.ContentPanel.SuspendLayout();
      this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
      this.toolStripContainer1.SuspendLayout();
      this.toolStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.contentSplitter)).BeginInit();
      this.contentSplitter.Panel1.SuspendLayout();
      this.contentSplitter.Panel2.SuspendLayout();
      this.contentSplitter.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).BeginInit();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // fileMenuSeparator
      // 
      fileMenuSeparator.Index = 6;
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
      this.rootContainer.Size = new System.Drawing.Size(946, 621);
      this.rootContainer.SplitterDistance = 195;
      this.rootContainer.TabIndex = 0;
      // 
      // toolStripContainer1
      // 
      this.toolStripContainer1.BottomToolStripPanelVisible = false;
      // 
      // toolStripContainer1.ContentPanel
      // 
      this.toolStripContainer1.ContentPanel.Controls.Add(this.structureTree);
      this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(195, 596);
      this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolStripContainer1.LeftToolStripPanelVisible = false;
      this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
      this.toolStripContainer1.Name = "toolStripContainer1";
      this.toolStripContainer1.RightToolStripPanelVisible = false;
      this.toolStripContainer1.Size = new System.Drawing.Size(195, 621);
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
      this.structureTree.Size = new System.Drawing.Size(195, 596);
      this.structureTree.TabIndex = 0;
      // 
      // toolStrip1
      // 
      this.toolStrip1.AllowMerge = false;
      this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
      this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
      this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCollectionToolButton,
            this.addGridToolButton,
            this.addGroupToolButton,
            this.addTileToolButton,
            toolStripSeparator1,
            this.editItemToolButton,
            this.removeItemToolButton});
      this.toolStrip1.Location = new System.Drawing.Point(3, 0);
      this.toolStrip1.Name = "toolStrip1";
      this.toolStrip1.Size = new System.Drawing.Size(147, 25);
      this.toolStrip1.TabIndex = 0;
      this.toolStrip1.Text = "Text";
      // 
      // addCollectionToolButton
      // 
      this.addCollectionToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.addCollectionToolButton.Image = ((System.Drawing.Image)(resources.GetObject("addCollectionToolButton.Image")));
      this.addCollectionToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.addCollectionToolButton.Name = "addCollectionToolButton";
      this.addCollectionToolButton.Size = new System.Drawing.Size(23, 22);
      this.addCollectionToolButton.Text = "Add Collection ..";
      // 
      // addGridToolButton
      // 
      this.addGridToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.addGridToolButton.Image = ((System.Drawing.Image)(resources.GetObject("addGridToolButton.Image")));
      this.addGridToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.addGridToolButton.Name = "addGridToolButton";
      this.addGridToolButton.Size = new System.Drawing.Size(23, 22);
      this.addGridToolButton.Text = "Add Grid ..";
      // 
      // addGroupToolButton
      // 
      this.addGroupToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.addGroupToolButton.Image = ((System.Drawing.Image)(resources.GetObject("addGroupToolButton.Image")));
      this.addGroupToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.addGroupToolButton.Name = "addGroupToolButton";
      this.addGroupToolButton.Size = new System.Drawing.Size(23, 22);
      this.addGroupToolButton.Text = "Add Group ..";
      // 
      // addTileToolButton
      // 
      this.addTileToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.addTileToolButton.Image = ((System.Drawing.Image)(resources.GetObject("addTileToolButton.Image")));
      this.addTileToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.addTileToolButton.Name = "addTileToolButton";
      this.addTileToolButton.Size = new System.Drawing.Size(23, 22);
      this.addTileToolButton.Text = "Add Tile ..";
      // 
      // editItemToolButton
      // 
      this.editItemToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.editItemToolButton.Image = ((System.Drawing.Image)(resources.GetObject("editItemToolButton.Image")));
      this.editItemToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.editItemToolButton.Name = "editItemToolButton";
      this.editItemToolButton.Size = new System.Drawing.Size(23, 22);
      this.editItemToolButton.Text = "Edit Item";
      // 
      // removeItemToolButton
      // 
      this.removeItemToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.removeItemToolButton.Image = ((System.Drawing.Image)(resources.GetObject("removeItemToolButton.Image")));
      this.removeItemToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.removeItemToolButton.Name = "removeItemToolButton";
      this.removeItemToolButton.Size = new System.Drawing.Size(23, 22);
      this.removeItemToolButton.Text = "Remove Item";
      // 
      // contentSplitter
      // 
      this.contentSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
      this.contentSplitter.Location = new System.Drawing.Point(0, 0);
      this.contentSplitter.Name = "contentSplitter";
      // 
      // contentSplitter.Panel1
      // 
      this.contentSplitter.Panel1.BackColor = System.Drawing.SystemColors.Control;
      this.contentSplitter.Panel1.Controls.Add(this.selectedContentPane);
      // 
      // contentSplitter.Panel2
      // 
      this.contentSplitter.Panel2.Controls.Add(this.previewPicture);
      this.contentSplitter.Size = new System.Drawing.Size(747, 621);
      this.contentSplitter.SplitterDistance = 248;
      this.contentSplitter.TabIndex = 0;
      // 
      // menuBar
      // 
      this.menuBar.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileMenu,
            this.editMenu});
      // 
      // fileMenu
      // 
      this.fileMenu.Index = 0;
      this.fileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.newMenuItem,
            this.openMenuItem,
            this.recentSubMenu,
            this.saveMenuItem,
            this.saveAsMenuItem,
            this.closeMenuItem,
            fileMenuSeparator,
            this.exitMenuItem});
      this.fileMenu.Text = "&File";
      // 
      // newMenuItem
      // 
      this.newMenuItem.Index = 0;
      this.newMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
      this.newMenuItem.Text = "&New ...";
      // 
      // openMenuItem
      // 
      this.openMenuItem.Index = 1;
      this.openMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
      this.openMenuItem.Text = "&Open ...";
      // 
      // recentSubMenu
      // 
      this.recentSubMenu.Index = 2;
      this.recentSubMenu.Text = "&Recent";
      // 
      // saveMenuItem
      // 
      this.saveMenuItem.Index = 3;
      this.saveMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
      this.saveMenuItem.Text = "&Save";
      // 
      // saveAsMenuItem
      // 
      this.saveAsMenuItem.Index = 4;
      this.saveAsMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftS;
      this.saveAsMenuItem.Text = "Save &As ...";
      // 
      // closeMenuItem
      // 
      this.closeMenuItem.Index = 5;
      this.closeMenuItem.Text = "Close";
      // 
      // exitMenuItem
      // 
      this.exitMenuItem.Index = 7;
      this.exitMenuItem.Text = "E&xit";
      this.exitMenuItem.Click += new System.EventHandler(this.OnExit);
      // 
      // editMenu
      // 
      this.editMenu.Index = 1;
      this.editMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.editMenuItem,
            this.deleteMenuItem,
            this.editSeparator,
            this.addCollectionMenuItem,
            this.addGridMenuItem,
            this.addGroupMenuItem,
            this.addTileMenuItem});
      this.editMenu.Text = "&Edit";
      // 
      // editMenuItem
      // 
      this.editMenuItem.Index = 0;
      this.editMenuItem.Text = "Edit Item";
      // 
      // deleteMenuItem
      // 
      this.deleteMenuItem.Index = 1;
      this.deleteMenuItem.Text = "Delete Item";
      // 
      // editSeparator
      // 
      this.editSeparator.Index = 2;
      this.editSeparator.Text = "-";
      // 
      // addCollectionMenuItem
      // 
      this.addCollectionMenuItem.Index = 3;
      this.addCollectionMenuItem.Text = "Add Collection";
      // 
      // addGridMenuItem
      // 
      this.addGridMenuItem.Index = 4;
      this.addGridMenuItem.Text = "Add Grid";
      // 
      // addGroupMenuItem
      // 
      this.addGroupMenuItem.Index = 5;
      this.addGroupMenuItem.Text = "Add Group";
      // 
      // addTileMenuItem
      // 
      this.addTileMenuItem.Index = 6;
      this.addTileMenuItem.Text = "Add Tile";
      // 
      // previewPicture
      // 
      this.previewPicture.Dock = System.Windows.Forms.DockStyle.Fill;
      this.previewPicture.Location = new System.Drawing.Point(0, 0);
      this.previewPicture.Name = "previewPicture";
      this.previewPicture.Size = new System.Drawing.Size(495, 621);
      this.previewPicture.TabIndex = 0;
      this.previewPicture.TabStop = false;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.contentSplitter);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(747, 621);
      this.panel1.TabIndex = 1;
      // 
      // selectedContentPane
      // 
      this.selectedContentPane.AutoScroll = true;
      this.selectedContentPane.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.selectedContentPane.Dock = System.Windows.Forms.DockStyle.Fill;
      this.selectedContentPane.Location = new System.Drawing.Point(0, 0);
      this.selectedContentPane.Name = "selectedContentPane";
      this.selectedContentPane.Size = new System.Drawing.Size(248, 621);
      this.selectedContentPane.TabIndex = 0;
      // 
      // MainWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(946, 621);
      this.Controls.Add(this.rootContainer);
      this.Menu = this.menuBar;
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
      this.contentSplitter.Panel1.ResumeLayout(false);
      this.contentSplitter.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.contentSplitter)).EndInit();
      this.contentSplitter.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).EndInit();
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer rootContainer;
    private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    private System.Windows.Forms.TreeView structureTree;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton addCollectionToolButton;
    private System.Windows.Forms.ToolStripButton addGridToolButton;
    private System.Windows.Forms.ToolStripButton addGroupToolButton;
    private System.Windows.Forms.MainMenu menuBar;
    private System.Windows.Forms.MenuItem fileMenu;
    private System.Windows.Forms.MenuItem openMenuItem;
    private System.Windows.Forms.MenuItem recentSubMenu;
    private System.Windows.Forms.MenuItem saveMenuItem;
    private System.Windows.Forms.MenuItem saveAsMenuItem;
    private System.Windows.Forms.MenuItem exitMenuItem;
    private System.Windows.Forms.MenuItem editMenu;
    private System.Windows.Forms.MenuItem editMenuItem;
    private System.Windows.Forms.MenuItem deleteMenuItem;
    private System.Windows.Forms.MenuItem editSeparator;
    private System.Windows.Forms.MenuItem addCollectionMenuItem;
    private System.Windows.Forms.MenuItem addGridMenuItem;
    private System.Windows.Forms.MenuItem addGroupMenuItem;
    private System.Windows.Forms.MenuItem addTileMenuItem;
    private System.Windows.Forms.SplitContainer contentSplitter;
    private System.Windows.Forms.MenuItem newMenuItem;
    private System.Windows.Forms.ToolStripButton addTileToolButton;
    private System.Windows.Forms.ToolStripButton editItemToolButton;
    private System.Windows.Forms.ToolStripButton removeItemToolButton;
    private System.Windows.Forms.MenuItem closeMenuItem;
    private System.Windows.Forms.PictureBox previewPicture;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Panel selectedContentPane;
  }
}

