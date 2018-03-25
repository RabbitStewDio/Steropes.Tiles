namespace Steropes.Tiles.TemplateGenerator.Editors
{
  partial class TextureFileEditor
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
      this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
      this.nameLabel = new System.Windows.Forms.Label();
      this.tileTypeLabel = new System.Windows.Forms.Label();
      this.widthLabel = new System.Windows.Forms.Label();
      this.heightLabel = new System.Windows.Forms.Label();
      this.nameTextBox = new System.Windows.Forms.TextBox();
      this.typeTypeBox = new System.Windows.Forms.ComboBox();
      this.widthBox = new System.Windows.Forms.NumericUpDown();
      this.heightBox = new System.Windows.Forms.NumericUpDown();
      this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.mainLayout.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.widthBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.heightBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // mainLayout
      // 
      this.mainLayout.AutoSize = true;
      this.mainLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.mainLayout.ColumnCount = 3;
      this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
      this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.mainLayout.Controls.Add(this.nameLabel, 0, 0);
      this.mainLayout.Controls.Add(this.tileTypeLabel, 0, 1);
      this.mainLayout.Controls.Add(this.widthLabel, 0, 2);
      this.mainLayout.Controls.Add(this.heightLabel, 0, 3);
      this.mainLayout.Controls.Add(this.nameTextBox, 1, 0);
      this.mainLayout.Controls.Add(this.typeTypeBox, 1, 1);
      this.mainLayout.Controls.Add(this.widthBox, 1, 2);
      this.mainLayout.Controls.Add(this.heightBox, 1, 3);
      this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
      this.mainLayout.Location = new System.Drawing.Point(0, 0);
      this.mainLayout.Name = "mainLayout";
      this.mainLayout.RowCount = 5;
      this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.mainLayout.Size = new System.Drawing.Size(367, 105);
      this.mainLayout.TabIndex = 3;
      // 
      // nameLabel
      // 
      this.nameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.nameLabel.AutoSize = true;
      this.nameLabel.Location = new System.Drawing.Point(3, 6);
      this.nameLabel.Name = "nameLabel";
      this.nameLabel.Size = new System.Drawing.Size(35, 13);
      this.nameLabel.TabIndex = 0;
      this.nameLabel.Text = "Name";
      this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tileTypeLabel
      // 
      this.tileTypeLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.tileTypeLabel.AutoSize = true;
      this.tileTypeLabel.Location = new System.Drawing.Point(3, 33);
      this.tileTypeLabel.Name = "tileTypeLabel";
      this.tileTypeLabel.Size = new System.Drawing.Size(51, 13);
      this.tileTypeLabel.TabIndex = 1;
      this.tileTypeLabel.Text = "Tile Type";
      this.tileTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // widthLabel
      // 
      this.widthLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.widthLabel.AutoSize = true;
      this.widthLabel.Location = new System.Drawing.Point(3, 59);
      this.widthLabel.Name = "widthLabel";
      this.widthLabel.Size = new System.Drawing.Size(35, 13);
      this.widthLabel.TabIndex = 2;
      this.widthLabel.Text = "Width";
      this.widthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // heightLabel
      // 
      this.heightLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.heightLabel.AutoSize = true;
      this.heightLabel.Location = new System.Drawing.Point(3, 85);
      this.heightLabel.Name = "heightLabel";
      this.heightLabel.Size = new System.Drawing.Size(38, 13);
      this.heightLabel.TabIndex = 3;
      this.heightLabel.Text = "Height";
      this.heightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // nameTextBox
      // 
      this.nameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.nameTextBox.Location = new System.Drawing.Point(123, 3);
      this.nameTextBox.Name = "nameTextBox";
      this.nameTextBox.Size = new System.Drawing.Size(221, 20);
      this.nameTextBox.TabIndex = 4;
      // 
      // typeTypeBox
      // 
      this.typeTypeBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.typeTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.typeTypeBox.FormattingEnabled = true;
      this.typeTypeBox.Location = new System.Drawing.Point(123, 29);
      this.typeTypeBox.Name = "typeTypeBox";
      this.typeTypeBox.Size = new System.Drawing.Size(221, 21);
      this.typeTypeBox.TabIndex = 5;
      // 
      // widthBox
      // 
      this.widthBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.widthBox.Location = new System.Drawing.Point(123, 56);
      this.widthBox.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
      this.widthBox.Name = "widthBox";
      this.widthBox.Size = new System.Drawing.Size(221, 20);
      this.widthBox.TabIndex = 6;
      // 
      // heightBox
      // 
      this.heightBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.heightBox.Location = new System.Drawing.Point(123, 82);
      this.heightBox.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
      this.heightBox.Name = "heightBox";
      this.heightBox.Size = new System.Drawing.Size(221, 20);
      this.heightBox.TabIndex = 7;
      // 
      // errorProvider
      // 
      this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.errorProvider.ContainerControl = this;
      // 
      // TextureFileEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.Controls.Add(this.mainLayout);
      this.MinimumSize = new System.Drawing.Size(100, 50);
      this.Name = "TextureFileEditor";
      this.Size = new System.Drawing.Size(367, 105);
      this.mainLayout.ResumeLayout(false);
      this.mainLayout.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.widthBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.heightBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel mainLayout;
    private System.Windows.Forms.Label nameLabel;
    private System.Windows.Forms.Label tileTypeLabel;
    private System.Windows.Forms.Label widthLabel;
    private System.Windows.Forms.Label heightLabel;
    private System.Windows.Forms.TextBox nameTextBox;
    private System.Windows.Forms.ComboBox typeTypeBox;
    private System.Windows.Forms.NumericUpDown widthBox;
    private System.Windows.Forms.NumericUpDown heightBox;
    private System.Windows.Forms.ErrorProvider errorProvider;
  }
}
