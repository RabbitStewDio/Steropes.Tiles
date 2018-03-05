namespace Steropes.Tiles.TemplateGenerator
{
  partial class TileSetPropertiesDialog
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
      this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
      this.nameLabel = new System.Windows.Forms.Label();
      this.tileTypeLabel = new System.Windows.Forms.Label();
      this.widthLabel = new System.Windows.Forms.Label();
      this.heightLabel = new System.Windows.Forms.Label();
      this.nameTextBox = new System.Windows.Forms.TextBox();
      this.typeTypeBox = new System.Windows.Forms.ComboBox();
      this.widthBox = new System.Windows.Forms.NumericUpDown();
      this.heightBox = new System.Windows.Forms.NumericUpDown();
      this.buttonCarrierPanel = new System.Windows.Forms.Panel();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.mainLayout.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.widthBox)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.heightBox)).BeginInit();
      this.buttonCarrierPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // mainLayout
      // 
      this.mainLayout.ColumnCount = 2;
      this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
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
      this.mainLayout.Size = new System.Drawing.Size(284, 261);
      this.mainLayout.TabIndex = 2;
      // 
      // nameLabel
      // 
      this.nameLabel.AutoSize = true;
      this.nameLabel.Dock = System.Windows.Forms.DockStyle.Left;
      this.nameLabel.Location = new System.Drawing.Point(3, 0);
      this.nameLabel.Name = "nameLabel";
      this.nameLabel.Size = new System.Drawing.Size(35, 26);
      this.nameLabel.TabIndex = 0;
      this.nameLabel.Text = "Name";
      this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tileTypeLabel
      // 
      this.tileTypeLabel.AutoSize = true;
      this.tileTypeLabel.Dock = System.Windows.Forms.DockStyle.Left;
      this.tileTypeLabel.Location = new System.Drawing.Point(3, 26);
      this.tileTypeLabel.Name = "tileTypeLabel";
      this.tileTypeLabel.Size = new System.Drawing.Size(51, 27);
      this.tileTypeLabel.TabIndex = 1;
      this.tileTypeLabel.Text = "Tile Type";
      this.tileTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // widthLabel
      // 
      this.widthLabel.AutoSize = true;
      this.widthLabel.Dock = System.Windows.Forms.DockStyle.Left;
      this.widthLabel.Location = new System.Drawing.Point(3, 53);
      this.widthLabel.Name = "widthLabel";
      this.widthLabel.Size = new System.Drawing.Size(35, 26);
      this.widthLabel.TabIndex = 2;
      this.widthLabel.Text = "Width";
      this.widthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // heightLabel
      // 
      this.heightLabel.AutoSize = true;
      this.heightLabel.Dock = System.Windows.Forms.DockStyle.Left;
      this.heightLabel.Location = new System.Drawing.Point(3, 79);
      this.heightLabel.Name = "heightLabel";
      this.heightLabel.Size = new System.Drawing.Size(38, 26);
      this.heightLabel.TabIndex = 3;
      this.heightLabel.Text = "Height";
      this.heightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // nameTextBox
      // 
      this.nameTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.nameTextBox.Location = new System.Drawing.Point(60, 3);
      this.nameTextBox.Name = "nameTextBox";
      this.nameTextBox.Size = new System.Drawing.Size(221, 20);
      this.nameTextBox.TabIndex = 4;
      // 
      // typeTypeBox
      // 
      this.typeTypeBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.typeTypeBox.FormattingEnabled = true;
      this.typeTypeBox.Location = new System.Drawing.Point(60, 29);
      this.typeTypeBox.Name = "typeTypeBox";
      this.typeTypeBox.Size = new System.Drawing.Size(221, 21);
      this.typeTypeBox.TabIndex = 5;
      // 
      // widthBox
      // 
      this.widthBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.widthBox.Location = new System.Drawing.Point(60, 56);
      this.widthBox.Maximum = new decimal(new int[] {
            1024,
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
      this.heightBox.Location = new System.Drawing.Point(60, 82);
      this.heightBox.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
      this.heightBox.Name = "heightBox";
      this.heightBox.Size = new System.Drawing.Size(221, 20);
      this.heightBox.TabIndex = 7;
      // 
      // buttonCarrierPanel
      // 
      this.buttonCarrierPanel.AutoSize = true;
      this.buttonCarrierPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.buttonCarrierPanel.Controls.Add(this.cancelButton);
      this.buttonCarrierPanel.Controls.Add(this.okButton);
      this.buttonCarrierPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.buttonCarrierPanel.Location = new System.Drawing.Point(0, 235);
      this.buttonCarrierPanel.Name = "buttonCarrierPanel";
      this.buttonCarrierPanel.Size = new System.Drawing.Size(284, 26);
      this.buttonCarrierPanel.TabIndex = 8;
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(125, 0);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 1;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(206, 0);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 0;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // TileSetPropertiesDialog
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(284, 261);
      this.Controls.Add(this.buttonCarrierPanel);
      this.Controls.Add(this.mainLayout);
      this.MinimumSize = new System.Drawing.Size(300, 300);
      this.Name = "TileSetPropertiesDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "TileSetPropertiesDialog";
      this.mainLayout.ResumeLayout(false);
      this.mainLayout.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.widthBox)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.heightBox)).EndInit();
      this.buttonCarrierPanel.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel mainLayout;
    private System.Windows.Forms.Label nameLabel;
    private System.Windows.Forms.Label tileTypeLabel;
    private System.Windows.Forms.Label widthLabel;
    private System.Windows.Forms.TextBox nameTextBox;
    private System.Windows.Forms.ComboBox typeTypeBox;
    private System.Windows.Forms.NumericUpDown widthBox;
    private System.Windows.Forms.NumericUpDown heightBox;
    private System.Windows.Forms.Label heightLabel;
    private System.Windows.Forms.Panel buttonCarrierPanel;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
  }
}