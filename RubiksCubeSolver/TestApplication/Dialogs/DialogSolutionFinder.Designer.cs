namespace TestApplication
{
  partial class DialogSolutionFinder

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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogSolutionFinder));
      this.panel1 = new System.Windows.Forms.Panel();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.lblHeader = new System.Windows.Forms.Label();
      this.lblMoves = new System.Windows.Forms.Label();
      this.lblTime = new System.Windows.Forms.Label();
      this.lblSolvingMethod = new System.Windows.Forms.Label();
      this.lblTimeHeader = new System.Windows.Forms.Label();
      this.lblMovesHeader = new System.Windows.Forms.Label();
      this.btnClose = new System.Windows.Forms.Button();
      this.btnAdd = new System.Windows.Forms.Button();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
      this.panel1.BackColor = System.Drawing.Color.White;
      this.panel1.Controls.Add(this.label2);
      this.panel1.Controls.Add(this.label1);
      this.panel1.Controls.Add(this.lblHeader);
      this.panel1.Controls.Add(this.lblMoves);
      this.panel1.Controls.Add(this.lblTime);
      this.panel1.Controls.Add(this.lblSolvingMethod);
      this.panel1.Controls.Add(this.lblTimeHeader);
      this.panel1.Controls.Add(this.lblMovesHeader);
      this.panel1.Location = new System.Drawing.Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(323, 179);
      this.panel1.TabIndex = 0;
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.label2.ForeColor = System.Drawing.SystemColors.Desktop;
      this.label2.Location = new System.Drawing.Point(12, 131);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(296, 2);
      this.label2.TabIndex = 7;
      this.label2.Text = "\r\n";
      // 
      // label1
      // 
      this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
      this.label1.Location = new System.Drawing.Point(12, 39);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(296, 2);
      this.label1.TabIndex = 1;
      this.label1.Text = "\r\n";
      // 
      // lblHeader
      // 
      this.lblHeader.AutoSize = true;
      this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblHeader.ForeColor = System.Drawing.SystemColors.HotTrack;
      this.lblHeader.Location = new System.Drawing.Point(8, 9);
      this.lblHeader.Name = "lblHeader";
      this.lblHeader.Size = new System.Drawing.Size(152, 21);
      this.lblHeader.TabIndex = 0;
      this.lblHeader.Text = "Calculating Solution.";
      // 
      // lblMoves
      // 
      this.lblMoves.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblMoves.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblMoves.Location = new System.Drawing.Point(104, 155);
      this.lblMoves.Name = "lblMoves";
      this.lblMoves.Size = new System.Drawing.Size(204, 15);
      this.lblMoves.TabIndex = 6;
      this.lblMoves.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblTime
      // 
      this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTime.Location = new System.Drawing.Point(110, 137);
      this.lblTime.Name = "lblTime";
      this.lblTime.Size = new System.Drawing.Size(198, 15);
      this.lblTime.TabIndex = 6;
      this.lblTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblSolvingMethod
      // 
      this.lblSolvingMethod.AutoSize = true;
      this.lblSolvingMethod.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblSolvingMethod.Location = new System.Drawing.Point(10, 43);
      this.lblSolvingMethod.Name = "lblSolvingMethod";
      this.lblSolvingMethod.Size = new System.Drawing.Size(88, 15);
      this.lblSolvingMethod.TabIndex = 6;
      this.lblSolvingMethod.Text = "Beginner solver";
      this.lblSolvingMethod.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblTimeHeader
      // 
      this.lblTimeHeader.AutoSize = true;
      this.lblTimeHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblTimeHeader.Location = new System.Drawing.Point(10, 137);
      this.lblTimeHeader.Name = "lblTimeHeader";
      this.lblTimeHeader.Size = new System.Drawing.Size(94, 15);
      this.lblTimeHeader.TabIndex = 2;
      this.lblTimeHeader.Text = "Calculation time";
      // 
      // lblMovesHeader
      // 
      this.lblMovesHeader.AutoSize = true;
      this.lblMovesHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblMovesHeader.Location = new System.Drawing.Point(10, 155);
      this.lblMovesHeader.Name = "lblMovesHeader";
      this.lblMovesHeader.Size = new System.Drawing.Size(89, 15);
      this.lblMovesHeader.TabIndex = 2;
      this.lblMovesHeader.Text = "Moves required";
      // 
      // btnClose
      // 
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnClose.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnClose.Location = new System.Drawing.Point(220, 185);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(89, 23);
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnAdd
      // 
      this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnAdd.Enabled = false;
      this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnAdd.Location = new System.Drawing.Point(12, 185);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new System.Drawing.Size(89, 23);
      this.btnAdd.TabIndex = 1;
      this.btnAdd.Text = "Add To Queue";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
      // 
      // DialogSolutionFinder
      // 
      this.AcceptButton = this.btnAdd;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnClose;
      this.ClientSize = new System.Drawing.Size(322, 220);
      this.Controls.Add(this.btnAdd);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.panel1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DialogSolutionFinder";
      this.Text = "Rubik\'s Cube Solution Finder";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label lblMovesHeader;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label lblHeader;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Label lblSolvingMethod;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Label lblMoves;
    private System.Windows.Forms.Label lblTime;
    private System.Windows.Forms.Label lblTimeHeader;
    private System.Windows.Forms.Label label2;
  }
}