namespace Steropes.Tiles.TemplateGenerator.Editors
{
  partial class TextureTileEditor
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
      this.layoutPane = new System.Windows.Forms.TableLayoutPanel();
      this.gridBoxLabel = new System.Windows.Forms.Label();
      this.gridXBox = new System.Windows.Forms.NumericUpDown();
      this.gridYLabel = new System.Windows.Forms.Label();
      this.gridYBox = new System.Windows.Forms.NumericUpDown();
      this.tagsLabel = new System.Windows.Forms.Label();
      this.tagsBox = new System.Windows.Forms.TextBox();
      this.anchorXLabel = new System.Windows.Forms.Label();
      this.anchorXBox = new System.Windows.Forms.NumericUpDown();
      this.anchorYLabel = new System.Windows.Forms.Label();
      this.anchorYBox = new System.Windows.Forms.NumericUpDown();
      this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.layoutPane.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gridXBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.gridYBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.anchorXBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.anchorYBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // layoutPane
      // 
      this.layoutPane.AutoSize = true;
      this.layoutPane.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.layoutPane.ColumnCount = 2;
      this.layoutPane.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
      this.layoutPane.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.layoutPane.Controls.Add(this.gridBoxLabel, 0, 0);
      this.layoutPane.Controls.Add(this.gridXBox, 1, 0);
      this.layoutPane.Controls.Add(this.gridYLabel, 0, 1);
      this.layoutPane.Controls.Add(this.gridYBox, 1, 1);
      this.layoutPane.Controls.Add(this.tagsLabel, 0, 2);
      this.layoutPane.Controls.Add(this.tagsBox, 1, 2);
      this.layoutPane.Controls.Add(this.anchorXLabel, 0, 3);
      this.layoutPane.Controls.Add(this.anchorXBox, 1, 3);
      this.layoutPane.Controls.Add(this.anchorYLabel, 0, 4);
      this.layoutPane.Controls.Add(this.anchorYBox, 1, 4);
      this.layoutPane.Location = new System.Drawing.Point(0, 0);
      this.layoutPane.Name = "layoutPane";
      this.layoutPane.RowCount = 5;
      this.layoutPane.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutPane.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutPane.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutPane.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutPane.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutPane.Size = new System.Drawing.Size(246, 254);
      this.layoutPane.TabIndex = 0;
      // 
      // gridBoxLabel
      // 
      this.gridBoxLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.gridBoxLabel.AutoSize = true;
      this.gridBoxLabel.Location = new System.Drawing.Point(3, 6);
      this.gridBoxLabel.Name = "gridBoxLabel";
      this.gridBoxLabel.Size = new System.Drawing.Size(36, 13);
      this.gridBoxLabel.TabIndex = 0;
      this.gridBoxLabel.Text = "Grid X";
      // 
      // gridXBox
      // 
      this.gridXBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gridXBox.Location = new System.Drawing.Point(123, 3);
      this.gridXBox.Name = "gridXBox";
      this.gridXBox.Size = new System.Drawing.Size(120, 20);
      this.gridXBox.TabIndex = 1;
      // 
      // gridYLabel
      // 
      this.gridYLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.gridYLabel.AutoSize = true;
      this.gridYLabel.Location = new System.Drawing.Point(3, 32);
      this.gridYLabel.Name = "gridYLabel";
      this.gridYLabel.Size = new System.Drawing.Size(36, 13);
      this.gridYLabel.TabIndex = 2;
      this.gridYLabel.Text = "Grid Y";
      // 
      // gridYBox
      // 
      this.gridYBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gridYBox.Location = new System.Drawing.Point(123, 29);
      this.gridYBox.Name = "gridYBox";
      this.gridYBox.Size = new System.Drawing.Size(120, 20);
      this.gridYBox.TabIndex = 3;
      // 
      // tagsLabel
      // 
      this.tagsLabel.AutoSize = true;
      this.tagsLabel.Location = new System.Drawing.Point(3, 52);
      this.tagsLabel.Name = "tagsLabel";
      this.tagsLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
      this.tagsLabel.Size = new System.Drawing.Size(113, 18);
      this.tagsLabel.TabIndex = 4;
      this.tagsLabel.Text = "Tags (one tag per line)";
      // 
      // tagsBox
      // 
      this.tagsBox.AcceptsReturn = true;
      this.tagsBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tagsBox.Location = new System.Drawing.Point(123, 55);
      this.tagsBox.Multiline = true;
      this.tagsBox.Name = "tagsBox";
      this.tagsBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.tagsBox.Size = new System.Drawing.Size(120, 144);
      this.tagsBox.TabIndex = 5;
      // 
      // anchorXLabel
      // 
      this.anchorXLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.anchorXLabel.AutoSize = true;
      this.anchorXLabel.Location = new System.Drawing.Point(3, 208);
      this.anchorXLabel.Name = "anchorXLabel";
      this.anchorXLabel.Size = new System.Drawing.Size(51, 13);
      this.anchorXLabel.TabIndex = 6;
      this.anchorXLabel.Text = "Anchor X";
      // 
      // anchorXBox
      // 
      this.anchorXBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.anchorXBox.Location = new System.Drawing.Point(123, 205);
      this.anchorXBox.Name = "anchorXBox";
      this.anchorXBox.Size = new System.Drawing.Size(120, 20);
      this.anchorXBox.TabIndex = 7;
      // 
      // anchorYLabel
      // 
      this.anchorYLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.anchorYLabel.AutoSize = true;
      this.anchorYLabel.Location = new System.Drawing.Point(3, 234);
      this.anchorYLabel.Name = "anchorYLabel";
      this.anchorYLabel.Size = new System.Drawing.Size(51, 13);
      this.anchorYLabel.TabIndex = 8;
      this.anchorYLabel.Text = "Anchor Y";
      // 
      // anchorYBox
      // 
      this.anchorYBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.anchorYBox.Location = new System.Drawing.Point(123, 231);
      this.anchorYBox.Name = "anchorYBox";
      this.anchorYBox.Size = new System.Drawing.Size(120, 20);
      this.anchorYBox.TabIndex = 9;
      // 
      // errorProvider
      // 
      this.errorProvider.ContainerControl = this;
      // 
      // TextureTileEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.Controls.Add(this.layoutPane);
      this.Name = "TextureTileEditor";
      this.Size = new System.Drawing.Size(249, 257);
      this.layoutPane.ResumeLayout(false);
      this.layoutPane.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.gridXBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.gridYBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.anchorXBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.anchorYBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel layoutPane;
    private System.Windows.Forms.Label gridBoxLabel;
    private System.Windows.Forms.NumericUpDown gridXBox;
    private System.Windows.Forms.Label gridYLabel;
    private System.Windows.Forms.NumericUpDown gridYBox;
    private System.Windows.Forms.Label tagsLabel;
    private System.Windows.Forms.TextBox tagsBox;
    private System.Windows.Forms.Label anchorXLabel;
    private System.Windows.Forms.NumericUpDown anchorXBox;
    private System.Windows.Forms.Label anchorYLabel;
    private System.Windows.Forms.NumericUpDown anchorYBox;
    private System.Windows.Forms.ErrorProvider errorProvider;
  }
}
