namespace TestApplication
{
  partial class FormAbout
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
      this.pbLogo = new System.Windows.Forms.PictureBox();
      this.panel1 = new System.Windows.Forms.Panel();
      this.linkMail = new System.Windows.Forms.LinkLabel();
      this.linkGitHub = new System.Windows.Forms.LinkLabel();
      this.linkYoutube = new System.Windows.Forms.LinkLabel();
      this.label5 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.lblDev1 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.lblCornerTest = new System.Windows.Forms.Label();
      this.lblDev2 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.lblHeader = new System.Windows.Forms.Label();
      this.btnClose = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // pbLogo
      // 
      this.pbLogo.Dock = System.Windows.Forms.DockStyle.Top;
      this.pbLogo.Image = global::TestApplication.Properties.Resources.logo;
      this.pbLogo.Location = new System.Drawing.Point(0, 0);
      this.pbLogo.Name = "pbLogo";
      this.pbLogo.Size = new System.Drawing.Size(291, 68);
      this.pbLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pbLogo.TabIndex = 0;
      this.pbLogo.TabStop = false;
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.Color.White;
      this.panel1.Controls.Add(this.linkMail);
      this.panel1.Controls.Add(this.linkGitHub);
      this.panel1.Controls.Add(this.linkYoutube);
      this.panel1.Controls.Add(this.label5);
      this.panel1.Controls.Add(this.label11);
      this.panel1.Controls.Add(this.lblDev1);
      this.panel1.Controls.Add(this.label4);
      this.panel1.Controls.Add(this.label3);
      this.panel1.Controls.Add(this.lblCornerTest);
      this.panel1.Controls.Add(this.lblDev2);
      this.panel1.Controls.Add(this.label2);
      this.panel1.Controls.Add(this.label1);
      this.panel1.Controls.Add(this.lblHeader);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
      this.panel1.Location = new System.Drawing.Point(0, 68);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(291, 161);
      this.panel1.TabIndex = 1;
      // 
      // linkMail
      // 
      this.linkMail.AutoSize = true;
      this.linkMail.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.linkMail.Location = new System.Drawing.Point(155, 132);
      this.linkMail.Name = "linkMail";
      this.linkMail.Size = new System.Drawing.Size(29, 13);
      this.linkMail.TabIndex = 7;
      this.linkMail.TabStop = true;
      this.linkMail.Text = "Mail";
      this.linkMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkMail_LinkClicked);
      // 
      // linkGitHub
      // 
      this.linkGitHub.AutoSize = true;
      this.linkGitHub.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.linkGitHub.Location = new System.Drawing.Point(155, 114);
      this.linkGitHub.Name = "linkGitHub";
      this.linkGitHub.Size = new System.Drawing.Size(44, 13);
      this.linkGitHub.TabIndex = 7;
      this.linkGitHub.TabStop = true;
      this.linkGitHub.Text = "GitHub";
      this.linkGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGitHub_LinkClicked);
      // 
      // linkYoutube
      // 
      this.linkYoutube.AutoSize = true;
      this.linkYoutube.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.linkYoutube.Location = new System.Drawing.Point(155, 95);
      this.linkYoutube.Name = "linkYoutube";
      this.linkYoutube.Size = new System.Drawing.Size(50, 13);
      this.linkYoutube.TabIndex = 7;
      this.linkYoutube.TabStop = true;
      this.linkYoutube.Text = "Youtube";
      this.linkYoutube.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkYoutube_LinkClicked);
      // 
      // label5
      // 
      this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.label5.ForeColor = System.Drawing.SystemColors.Desktop;
      this.label5.Location = new System.Drawing.Point(0, 159);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(320, 2);
      this.label5.TabIndex = 1;
      this.label5.Text = "\r\n";
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label11.Location = new System.Drawing.Point(12, 45);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(65, 15);
      this.label11.TabIndex = 5;
      this.label11.Text = "Developers";
      // 
      // lblDev1
      // 
      this.lblDev1.AutoSize = true;
      this.lblDev1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblDev1.Location = new System.Drawing.Point(155, 45);
      this.lblDev1.Name = "lblDev1";
      this.lblDev1.Size = new System.Drawing.Size(94, 15);
      this.lblDev1.TabIndex = 6;
      this.lblDev1.Text = "Switcherlapp97";
      this.lblDev1.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(12, 132);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(69, 15);
      this.label4.TabIndex = 2;
      this.label4.Text = "Contact me";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(12, 94);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(71, 15);
      this.label3.TabIndex = 2;
      this.label3.Text = "Video demo";
      // 
      // lblCornerTest
      // 
      this.lblCornerTest.AutoSize = true;
      this.lblCornerTest.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblCornerTest.Location = new System.Drawing.Point(155, 75);
      this.lblCornerTest.Name = "lblCornerTest";
      this.lblCornerTest.Size = new System.Drawing.Size(52, 15);
      this.lblCornerTest.TabIndex = 2;
      this.lblCornerTest.Text = "Artentus";
      this.lblCornerTest.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblDev2
      // 
      this.lblDev2.AutoSize = true;
      this.lblDev2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblDev2.Location = new System.Drawing.Point(155, 60);
      this.lblDev2.Name = "lblDev2";
      this.lblDev2.Size = new System.Drawing.Size(63, 15);
      this.lblDev2.TabIndex = 2;
      this.lblDev2.Text = "StarGate01";
      this.lblDev2.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(12, 113);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(43, 15);
      this.label2.TabIndex = 2;
      this.label2.Text = "Source";
      // 
      // label1
      // 
      this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
      this.label1.Location = new System.Drawing.Point(12, 39);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(251, 2);
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
      this.lblHeader.Size = new System.Drawing.Size(241, 21);
      this.lblHeader.TabIndex = 0;
      this.lblHeader.Text = "Rubik\'s Cube Solver - Release 1.0";
      // 
      // btnClose
      // 
      this.btnClose.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnClose.Location = new System.Drawing.Point(204, 235);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 23);
      this.btnClose.TabIndex = 2;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      // 
      // FormAbout
      // 
      this.AcceptButton = this.btnClose;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(291, 266);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.pbLogo);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MaximumSize = new System.Drawing.Size(342, 321);
      this.MinimizeBox = false;
      this.Name = "FormAbout";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.Text = "FormAbout";
      ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox pbLogo;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.Label lblDev1;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label lblCornerTest;
    private System.Windows.Forms.Label lblDev2;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label lblHeader;
    private System.Windows.Forms.LinkLabel linkYoutube;
    private System.Windows.Forms.LinkLabel linkGitHub;
    private System.Windows.Forms.LinkLabel linkMail;
    private System.Windows.Forms.Button btnClose;
  }
}