namespace Steropes.Tiles.TemplateGenerator.Editors
{
  partial class TextureGridElementEditor
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
      this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
      this.outlineColorLabel = new System.Windows.Forms.Label();
      this.outlineColorBox = new Krypton.Toolkit.KryptonColorButton();
      this.highlightColorLabel = new System.Windows.Forms.Label();
      this.highlightColorBox = new Krypton.Toolkit.KryptonColorButton();
      this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.layoutPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // layoutPanel
      // 
      this.layoutPanel.AutoSize = true;
      this.layoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.layoutPanel.ColumnCount = 3;
      this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
      this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.layoutPanel.Controls.Add(this.outlineColorLabel, 0, 0);
      this.layoutPanel.Controls.Add(this.outlineColorBox, 1, 0);
      this.layoutPanel.Controls.Add(this.highlightColorLabel, 0, 1);
      this.layoutPanel.Controls.Add(this.highlightColorBox, 1, 1);
      this.layoutPanel.Location = new System.Drawing.Point(0, 0);
      this.layoutPanel.Name = "layoutPanel";
      this.layoutPanel.RowCount = 2;
      this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutPanel.Size = new System.Drawing.Size(236, 62);
      this.layoutPanel.TabIndex = 0;
      // 
      // outlineColorLabel
      // 
      this.outlineColorLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.outlineColorLabel.AutoSize = true;
      this.outlineColorLabel.Location = new System.Drawing.Point(3, 9);
      this.outlineColorLabel.Name = "outlineColorLabel";
      this.outlineColorLabel.Size = new System.Drawing.Size(67, 13);
      this.outlineColorLabel.TabIndex = 0;
      this.outlineColorLabel.Text = "Outline Color";
      // 
      // outlineColorBox
      // 
      this.outlineColorBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.outlineColorBox.Location = new System.Drawing.Point(123, 3);
      this.outlineColorBox.Name = "outlineColorBox";
      this.outlineColorBox.Size = new System.Drawing.Size(90, 25);
      this.outlineColorBox.TabIndex = 1;
      this.outlineColorBox.Values.Text = "Outline";
      // 
      // highlightColorLabel
      // 
      this.highlightColorLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.highlightColorLabel.AutoSize = true;
      this.highlightColorLabel.Location = new System.Drawing.Point(3, 40);
      this.highlightColorLabel.Name = "highlightColorLabel";
      this.highlightColorLabel.Size = new System.Drawing.Size(75, 13);
      this.highlightColorLabel.TabIndex = 2;
      this.highlightColorLabel.Text = "Highlight Color";
      // 
      // highlightColorBox
      // 
      this.highlightColorBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.highlightColorBox.Location = new System.Drawing.Point(123, 34);
      this.highlightColorBox.Name = "highlightColorBox";
      this.highlightColorBox.Size = new System.Drawing.Size(90, 25);
      this.highlightColorBox.TabIndex = 3;
      this.highlightColorBox.Values.Text = "Highlight";
      // 
      // errorProvider
      // 
      this.errorProvider.ContainerControl = this;
      // 
      // TextureGridElementEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.Controls.Add(this.layoutPanel);
      this.Name = "TextureGridElementEditor";
      this.Size = new System.Drawing.Size(239, 65);
      this.layoutPanel.ResumeLayout(false);
      this.layoutPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel layoutPanel;
    private System.Windows.Forms.Label outlineColorLabel;
    private Krypton.Toolkit.KryptonColorButton outlineColorBox;
    private System.Windows.Forms.Label highlightColorLabel;
    private Krypton.Toolkit.KryptonColorButton highlightColorBox;
    private System.Windows.Forms.ErrorProvider errorProvider;
  }
}
