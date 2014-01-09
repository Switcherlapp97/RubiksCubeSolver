namespace VirtualRubik
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.panel2 = new System.Windows.Forms.Panel();
      this.listBox1 = new System.Windows.Forms.ListBox();
      this.splitContainer2 = new System.Windows.Forms.SplitContainer();
      this.button2 = new System.Windows.Forms.Button();
      this.button3 = new System.Windows.Forms.Button();
      this.splitContainer3 = new System.Windows.Forms.SplitContainer();
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.button5 = new System.Windows.Forms.Button();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.button4 = new System.Windows.Forms.Button();
      this.button1 = new System.Windows.Forms.Button();
      this.statusStrip2 = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.rubikToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.scrambleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.button6 = new System.Windows.Forms.Button();
      this.statusStrip1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.panel2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
      this.splitContainer3.Panel1.SuspendLayout();
      this.splitContainer3.Panel2.SuspendLayout();
      this.splitContainer3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.statusStrip2.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
      this.statusStrip1.Location = new System.Drawing.Point(0, 413);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(668, 22);
      this.statusStrip1.SizingGrip = false;
      this.statusStrip1.TabIndex = 5;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // toolStripStatusLabel1
      // 
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
      this.toolStripStatusLabel1.Text = "Ready";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.panel2);
      this.groupBox1.Controls.Add(this.splitContainer2);
      this.groupBox1.Controls.Add(this.splitContainer3);
      this.groupBox1.Controls.Add(this.splitContainer1);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
      this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBox1.ForeColor = System.Drawing.Color.Black;
      this.groupBox1.Location = new System.Drawing.Point(448, 24);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(220, 367);
      this.groupBox1.TabIndex = 8;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Controls";
      // 
      // panel2
      // 
      this.panel2.Controls.Add(this.listBox1);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel2.Location = new System.Drawing.Point(3, 76);
      this.panel2.Name = "panel2";
      this.panel2.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
      this.panel2.Size = new System.Drawing.Size(214, 261);
      this.panel2.TabIndex = 7;
      // 
      // listBox1
      // 
      this.listBox1.BackColor = System.Drawing.SystemColors.Control;
      this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.listBox1.FormattingEnabled = true;
      this.listBox1.IntegralHeight = false;
      this.listBox1.Location = new System.Drawing.Point(1, 0);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new System.Drawing.Size(212, 261);
      this.listBox1.TabIndex = 4;
      this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
      // 
      // splitContainer2
      // 
      this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Top;
      this.splitContainer2.IsSplitterFixed = true;
      this.splitContainer2.Location = new System.Drawing.Point(3, 48);
      this.splitContainer2.Name = "splitContainer2";
      // 
      // splitContainer2.Panel1
      // 
      this.splitContainer2.Panel1.Controls.Add(this.button2);
      // 
      // splitContainer2.Panel2
      // 
      this.splitContainer2.Panel2.Controls.Add(this.button3);
      this.splitContainer2.Size = new System.Drawing.Size(214, 28);
      this.splitContainer2.SplitterDistance = 105;
      this.splitContainer2.SplitterWidth = 1;
      this.splitContainer2.TabIndex = 8;
      // 
      // button2
      // 
      this.button2.Dock = System.Windows.Forms.DockStyle.Top;
      this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.button2.ForeColor = System.Drawing.Color.Black;
      this.button2.Location = new System.Drawing.Point(0, 0);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(105, 25);
      this.button2.TabIndex = 2;
      this.button2.Text = "Rotate";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // button3
      // 
      this.button3.Dock = System.Windows.Forms.DockStyle.Top;
      this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.button3.ForeColor = System.Drawing.Color.Black;
      this.button3.Location = new System.Drawing.Point(0, 0);
      this.button3.Name = "button3";
      this.button3.Size = new System.Drawing.Size(108, 25);
      this.button3.TabIndex = 3;
      this.button3.Text = "Add to queue";
      this.button3.UseVisualStyleBackColor = true;
      this.button3.Click += new System.EventHandler(this.button3_Click);
      // 
      // splitContainer3
      // 
      this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Top;
      this.splitContainer3.IsSplitterFixed = true;
      this.splitContainer3.Location = new System.Drawing.Point(3, 22);
      this.splitContainer3.Name = "splitContainer3";
      // 
      // splitContainer3.Panel1
      // 
      this.splitContainer3.Panel1.Controls.Add(this.comboBox1);
      this.splitContainer3.Panel1.Padding = new System.Windows.Forms.Padding(1, 1, 0, 0);
      // 
      // splitContainer3.Panel2
      // 
      this.splitContainer3.Panel2.Controls.Add(this.button5);
      this.splitContainer3.Size = new System.Drawing.Size(214, 26);
      this.splitContainer3.SplitterDistance = 170;
      this.splitContainer3.SplitterWidth = 1;
      this.splitContainer3.TabIndex = 9;
      // 
      // comboBox1
      // 
      this.comboBox1.Dock = System.Windows.Forms.DockStyle.Top;
      this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.comboBox1.ForeColor = System.Drawing.Color.Black;
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new System.Drawing.Point(1, 1);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(169, 21);
      this.comboBox1.TabIndex = 0;
      // 
      // button5
      // 
      this.button5.Dock = System.Windows.Forms.DockStyle.Top;
      this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.button5.Location = new System.Drawing.Point(0, 0);
      this.button5.Name = "button5";
      this.button5.Size = new System.Drawing.Size(43, 23);
      this.button5.TabIndex = 9;
      this.button5.Text = "CW";
      this.button5.UseVisualStyleBackColor = true;
      this.button5.Click += new System.EventHandler(this.button5_Click);
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.splitContainer1.IsSplitterFixed = true;
      this.splitContainer1.Location = new System.Drawing.Point(3, 337);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.button4);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.button1);
      this.splitContainer1.Size = new System.Drawing.Size(214, 27);
      this.splitContainer1.SplitterDistance = 105;
      this.splitContainer1.SplitterWidth = 1;
      this.splitContainer1.TabIndex = 7;
      // 
      // button4
      // 
      this.button4.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.button4.ForeColor = System.Drawing.Color.Black;
      this.button4.Location = new System.Drawing.Point(0, 2);
      this.button4.Name = "button4";
      this.button4.Size = new System.Drawing.Size(105, 25);
      this.button4.TabIndex = 5;
      this.button4.Text = "Execute queue";
      this.button4.UseVisualStyleBackColor = true;
      this.button4.Click += new System.EventHandler(this.button4_Click);
      // 
      // button1
      // 
      this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.button1.ForeColor = System.Drawing.Color.Black;
      this.button1.Location = new System.Drawing.Point(0, 2);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(108, 25);
      this.button1.TabIndex = 1;
      this.button1.Text = "Clear queue";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // statusStrip2
      // 
      this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2});
      this.statusStrip2.Location = new System.Drawing.Point(0, 391);
      this.statusStrip2.Name = "statusStrip2";
      this.statusStrip2.Size = new System.Drawing.Size(668, 22);
      this.statusStrip2.SizingGrip = false;
      this.statusStrip2.TabIndex = 6;
      this.statusStrip2.Text = "statusStrip2";
      // 
      // toolStripStatusLabel2
      // 
      this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
      this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rubikToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.toolStripMenuItem1});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
      this.menuStrip1.Size = new System.Drawing.Size(668, 24);
      this.menuStrip1.TabIndex = 7;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // rubikToolStripMenuItem
      // 
      this.rubikToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scrambleToolStripMenuItem,
            this.resetToolStripMenuItem});
      this.rubikToolStripMenuItem.Name = "rubikToolStripMenuItem";
      this.rubikToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
      this.rubikToolStripMenuItem.Text = "Rubik";
      // 
      // scrambleToolStripMenuItem
      // 
      this.scrambleToolStripMenuItem.Name = "scrambleToolStripMenuItem";
      this.scrambleToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
      this.scrambleToolStripMenuItem.Text = "Scramble";
      this.scrambleToolStripMenuItem.Click += new System.EventHandler(this.scrambleToolStripMenuItem_Click);
      // 
      // resetToolStripMenuItem
      // 
      this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
      this.resetToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
      this.resetToolStripMenuItem.Text = "Reset";
      this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
      this.aboutToolStripMenuItem.Text = "About";
      this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(83, 20);
      this.toolStripMenuItem1.Text = "Controls >>";
      this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
      // 
      // button6
      // 
      this.button6.Location = new System.Drawing.Point(367, 361);
      this.button6.Name = "button6";
      this.button6.Size = new System.Drawing.Size(75, 23);
      this.button6.TabIndex = 9;
      this.button6.Text = "Solve Cross";
      this.button6.UseVisualStyleBackColor = true;
      this.button6.Click += new System.EventHandler(this.button6_Click);
      // 
      // Form1
      // 
      this.AcceptButton = this.button2;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Control;
      this.ClientSize = new System.Drawing.Size(668, 435);
      this.Controls.Add(this.button6);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.statusStrip2);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.menuStrip1);
      this.Cursor = System.Windows.Forms.Cursors.Default;
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.KeyPreview = true;
      this.MainMenuStrip = this.menuStrip1;
      this.MinimumSize = new System.Drawing.Size(684, 473);
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "VirtualRubik";
      this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
      this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
      this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
      this.Resize += new System.EventHandler(this.Form1_Resize);
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
      this.splitContainer2.ResumeLayout(false);
      this.splitContainer3.Panel1.ResumeLayout(false);
      this.splitContainer3.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
      this.splitContainer3.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.statusStrip2.ResumeLayout(false);
      this.statusStrip2.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem rubikToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scrambleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;



    }
}

