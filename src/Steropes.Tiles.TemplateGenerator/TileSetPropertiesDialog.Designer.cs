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
      this.buttonCarrierPanel = new System.Windows.Forms.Panel();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.editor = new Steropes.Tiles.TemplateGenerator.Editors.TextureFileEditor();
      this.buttonCarrierPanel.SuspendLayout();
      this.SuspendLayout();
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
      // editor
      // 
      this.editor.AutoSize = true;
      this.editor.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.editor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.editor.Location = new System.Drawing.Point(0, 0);
      this.editor.MinimumSize = new System.Drawing.Size(100, 50);
      this.editor.Name = "editor";
      this.editor.Size = new System.Drawing.Size(284, 235);
      this.editor.TabIndex = 9;
      // 
      // TileSetPropertiesDialog
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(284, 261);
      this.Controls.Add(this.editor);
      this.Controls.Add(this.buttonCarrierPanel);
      this.MinimumSize = new System.Drawing.Size(300, 300);
      this.Name = "TileSetPropertiesDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "TileSetPropertiesDialog";
      this.buttonCarrierPanel.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Panel buttonCarrierPanel;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
    private Editors.TextureFileEditor editor;
  }
}