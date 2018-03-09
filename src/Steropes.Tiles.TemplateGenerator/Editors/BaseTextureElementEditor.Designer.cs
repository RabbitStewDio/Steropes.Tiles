namespace Steropes.Tiles.TemplateGenerator.Editors
{
  partial class BaseTextureElementEditor
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
      this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
      this.marginLabel = new System.Windows.Forms.Label();
      this.paddingLabel = new System.Windows.Forms.Label();
      this.borderLabel = new System.Windows.Forms.Label();
      this.borderColorLabel = new System.Windows.Forms.Label();
      this.textColorLabel = new System.Windows.Forms.Label();
      this.backgroundColorLabel = new System.Windows.Forms.Label();
      this.titleLabel = new System.Windows.Forms.Label();
      this.marginInput = new System.Windows.Forms.NumericUpDown();
      this.paddingInput = new System.Windows.Forms.NumericUpDown();
      this.borderInput = new System.Windows.Forms.NumericUpDown();
      this.borderColorInput = new ComponentFactory.Krypton.Toolkit.KryptonColorButton();
      this.textColorInput = new ComponentFactory.Krypton.Toolkit.KryptonColorButton();
      this.backgroundInput = new ComponentFactory.Krypton.Toolkit.KryptonColorButton();
      this.titleInput = new System.Windows.Forms.TextBox();
      this.layoutPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.marginInput)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.paddingInput)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.borderInput)).BeginInit();
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
      this.layoutPanel.Controls.Add(this.marginLabel, 0, 0);
      this.layoutPanel.Controls.Add(this.paddingLabel, 0, 1);
      this.layoutPanel.Controls.Add(this.borderLabel, 0, 2);
      this.layoutPanel.Controls.Add(this.borderColorLabel, 0, 3);
      this.layoutPanel.Controls.Add(this.textColorLabel, 0, 4);
      this.layoutPanel.Controls.Add(this.backgroundColorLabel, 0, 5);
      this.layoutPanel.Controls.Add(this.titleLabel, 0, 6);
      this.layoutPanel.Controls.Add(this.marginInput, 1, 0);
      this.layoutPanel.Controls.Add(this.paddingInput, 1, 1);
      this.layoutPanel.Controls.Add(this.borderInput, 1, 2);
      this.layoutPanel.Controls.Add(this.borderColorInput, 1, 3);
      this.layoutPanel.Controls.Add(this.textColorInput, 1, 4);
      this.layoutPanel.Controls.Add(this.backgroundInput, 1, 5);
      this.layoutPanel.Controls.Add(this.titleInput, 1, 6);
      this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.layoutPanel.Location = new System.Drawing.Point(0, 0);
      this.layoutPanel.Name = "layoutPanel";
      this.layoutPanel.RowCount = 8;
      this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.layoutPanel.Size = new System.Drawing.Size(315, 199);
      this.layoutPanel.TabIndex = 0;
      // 
      // marginLabel
      // 
      this.marginLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.marginLabel.AutoSize = true;
      this.marginLabel.Location = new System.Drawing.Point(3, 6);
      this.marginLabel.Name = "marginLabel";
      this.marginLabel.Size = new System.Drawing.Size(39, 13);
      this.marginLabel.TabIndex = 0;
      this.marginLabel.Text = "Margin";
      // 
      // paddingLabel
      // 
      this.paddingLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.paddingLabel.AutoSize = true;
      this.paddingLabel.Location = new System.Drawing.Point(3, 32);
      this.paddingLabel.Name = "paddingLabel";
      this.paddingLabel.Size = new System.Drawing.Size(46, 13);
      this.paddingLabel.TabIndex = 2;
      this.paddingLabel.Text = "Padding";
      // 
      // borderLabel
      // 
      this.borderLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.borderLabel.AutoSize = true;
      this.borderLabel.Location = new System.Drawing.Point(3, 58);
      this.borderLabel.Name = "borderLabel";
      this.borderLabel.Size = new System.Drawing.Size(38, 13);
      this.borderLabel.TabIndex = 1;
      this.borderLabel.Text = "Border";
      // 
      // borderColorLabel
      // 
      this.borderColorLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.borderColorLabel.AutoSize = true;
      this.borderColorLabel.Location = new System.Drawing.Point(3, 87);
      this.borderColorLabel.Name = "borderColorLabel";
      this.borderColorLabel.Size = new System.Drawing.Size(65, 13);
      this.borderColorLabel.TabIndex = 3;
      this.borderColorLabel.Text = "Border Color";
      // 
      // textColorLabel
      // 
      this.textColorLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.textColorLabel.AutoSize = true;
      this.textColorLabel.Location = new System.Drawing.Point(3, 118);
      this.textColorLabel.Name = "textColorLabel";
      this.textColorLabel.Size = new System.Drawing.Size(55, 13);
      this.textColorLabel.TabIndex = 4;
      this.textColorLabel.Text = "Text Color";
      // 
      // backgroundColorLabel
      // 
      this.backgroundColorLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.backgroundColorLabel.AutoSize = true;
      this.backgroundColorLabel.Location = new System.Drawing.Point(3, 149);
      this.backgroundColorLabel.Name = "backgroundColorLabel";
      this.backgroundColorLabel.Size = new System.Drawing.Size(92, 13);
      this.backgroundColorLabel.TabIndex = 5;
      this.backgroundColorLabel.Text = "Background Color";
      // 
      // titleLabel
      // 
      this.titleLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
      this.titleLabel.AutoSize = true;
      this.titleLabel.Location = new System.Drawing.Point(3, 177);
      this.titleLabel.Name = "titleLabel";
      this.titleLabel.Size = new System.Drawing.Size(27, 13);
      this.titleLabel.TabIndex = 6;
      this.titleLabel.Text = "Title";
      // 
      // marginInput
      // 
      this.marginInput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.marginInput.Location = new System.Drawing.Point(123, 3);
      this.marginInput.Name = "marginInput";
      this.marginInput.Size = new System.Drawing.Size(169, 20);
      this.marginInput.TabIndex = 7;
      // 
      // paddingInput
      // 
      this.paddingInput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.paddingInput.Location = new System.Drawing.Point(123, 29);
      this.paddingInput.Name = "paddingInput";
      this.paddingInput.Size = new System.Drawing.Size(169, 20);
      this.paddingInput.TabIndex = 8;
      // 
      // borderInput
      // 
      this.borderInput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.borderInput.Location = new System.Drawing.Point(123, 55);
      this.borderInput.Name = "borderInput";
      this.borderInput.Size = new System.Drawing.Size(169, 20);
      this.borderInput.TabIndex = 9;
      // 
      // borderColorInput
      // 
      this.borderColorInput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.borderColorInput.Location = new System.Drawing.Point(123, 81);
      this.borderColorInput.Name = "borderColorInput";
      this.borderColorInput.Size = new System.Drawing.Size(169, 25);
      this.borderColorInput.Splitter = false;
      this.borderColorInput.TabIndex = 10;
      this.borderColorInput.Values.Text = "Border";
      // 
      // textColorInput
      // 
      this.textColorInput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textColorInput.Location = new System.Drawing.Point(123, 112);
      this.textColorInput.Name = "textColorInput";
      this.textColorInput.Size = new System.Drawing.Size(169, 25);
      this.textColorInput.Splitter = false;
      this.textColorInput.TabIndex = 11;
      this.textColorInput.Values.Text = "Text";
      // 
      // backgroundInput
      // 
      this.backgroundInput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.backgroundInput.Location = new System.Drawing.Point(123, 143);
      this.backgroundInput.Name = "backgroundInput";
      this.backgroundInput.Size = new System.Drawing.Size(169, 25);
      this.backgroundInput.Splitter = false;
      this.backgroundInput.TabIndex = 12;
      this.backgroundInput.Values.Text = "Background";
      // 
      // titleInput
      // 
      this.titleInput.Dock = System.Windows.Forms.DockStyle.Fill;
      this.titleInput.Location = new System.Drawing.Point(123, 174);
      this.titleInput.Name = "titleInput";
      this.titleInput.Size = new System.Drawing.Size(169, 20);
      this.titleInput.TabIndex = 13;
      // 
      // BaseTextureElementEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.Controls.Add(this.layoutPanel);
      this.Name = "BaseTextureElementEditor";
      this.Size = new System.Drawing.Size(315, 199);
      this.layoutPanel.ResumeLayout(false);
      this.layoutPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.marginInput)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.paddingInput)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.borderInput)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel layoutPanel;
    private System.Windows.Forms.Label marginLabel;
    private System.Windows.Forms.Label borderLabel;
    private System.Windows.Forms.Label paddingLabel;
    private System.Windows.Forms.Label borderColorLabel;
    private System.Windows.Forms.Label textColorLabel;
    private System.Windows.Forms.Label backgroundColorLabel;
    private System.Windows.Forms.Label titleLabel;
    private System.Windows.Forms.NumericUpDown marginInput;
    private System.Windows.Forms.NumericUpDown paddingInput;
    private System.Windows.Forms.NumericUpDown borderInput;
    private ComponentFactory.Krypton.Toolkit.KryptonColorButton borderColorInput;
    private ComponentFactory.Krypton.Toolkit.KryptonColorButton textColorInput;
    private ComponentFactory.Krypton.Toolkit.KryptonColorButton backgroundInput;
    private System.Windows.Forms.TextBox titleInput;
  }
}
