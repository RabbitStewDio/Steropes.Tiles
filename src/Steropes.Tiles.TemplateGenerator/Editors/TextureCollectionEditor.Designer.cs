namespace Steropes.Tiles.TemplateGenerator.Editors
{
  partial class TextureCollectionEditor
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.layoutContainer = new System.Windows.Forms.TableLayoutPanel();
      this.fileNameLabel = new System.Windows.Forms.Label();
      this.fileNameBox = new System.Windows.Forms.TextBox();
      this.selectFileButton = new System.Windows.Forms.Button();
      this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.layoutContainer.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // layoutContainer
      // 
      this.layoutContainer.AutoSize = true;
      this.layoutContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.layoutContainer.ColumnCount = 3;
      this.layoutContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
      this.layoutContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.layoutContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.layoutContainer.Controls.Add(this.fileNameLabel, 0, 0);
      this.layoutContainer.Controls.Add(this.fileNameBox, 1, 0);
      this.layoutContainer.Controls.Add(this.selectFileButton, 2, 0);
      this.layoutContainer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.layoutContainer.Location = new System.Drawing.Point(0, 0);
      this.layoutContainer.Name = "layoutContainer";
      this.layoutContainer.RowCount = 2;
      this.layoutContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutContainer.Size = new System.Drawing.Size(305, 29);
      this.layoutContainer.TabIndex = 0;
      // 
      // fileNameLabel
      // 
      this.fileNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.fileNameLabel.AutoSize = true;
      this.fileNameLabel.Location = new System.Drawing.Point(3, 8);
      this.fileNameLabel.Name = "fileNameLabel";
      this.fileNameLabel.Size = new System.Drawing.Size(96, 13);
      this.fileNameLabel.TabIndex = 0;
      this.fileNameLabel.Text = "Texture Reference";
      // 
      // fileNameBox
      // 
      this.fileNameBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.fileNameBox.Location = new System.Drawing.Point(123, 3);
      this.fileNameBox.Name = "fileNameBox";
      this.fileNameBox.Size = new System.Drawing.Size(150, 20);
      this.fileNameBox.TabIndex = 1;
      // 
      // selectFileButton
      // 
      this.selectFileButton.AutoSize = true;
      this.selectFileButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.selectFileButton.Location = new System.Drawing.Point(279, 3);
      this.selectFileButton.Name = "selectFileButton";
      this.selectFileButton.Size = new System.Drawing.Size(23, 23);
      this.selectFileButton.TabIndex = 2;
      this.selectFileButton.Text = "..";
      this.selectFileButton.UseVisualStyleBackColor = true;
      this.selectFileButton.Click += new System.EventHandler(this.OnSelectFile);
      // 
      // errorProvider
      // 
      this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.errorProvider.ContainerControl = this;
      // 
      // TextureCollectionEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.Controls.Add(this.layoutContainer);
      this.Name = "TextureCollectionEditor";
      this.Size = new System.Drawing.Size(305, 29);
      this.layoutContainer.ResumeLayout(false);
      this.layoutContainer.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel layoutContainer;
    private System.Windows.Forms.Label fileNameLabel;
    private System.Windows.Forms.TextBox fileNameBox;
    private System.Windows.Forms.Button selectFileButton;
    private System.Windows.Forms.ErrorProvider errorProvider;
  }
}
