namespace TestApplication
{
  partial class Form1
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
      this.components = new System.ComponentModel.Container();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.rubikToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.scrambleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.solveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.cornerTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.solverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.solveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.cubeModel = new RubiksCubeLib.CubeModel.CubeModel();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rubikToolStripMenuItem,
            this.solverToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(380, 24);
      this.menuStrip1.TabIndex = 2;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // rubikToolStripMenuItem
      // 
      this.rubikToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scrambleToolStripMenuItem,
            this.solveToolStripMenuItem1,
            this.resetToolStripMenuItem,
            this.cornerTestToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.openToolStripMenuItem});
      this.rubikToolStripMenuItem.Name = "rubikToolStripMenuItem";
      this.rubikToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
      this.rubikToolStripMenuItem.Text = "Rubik";
      // 
      // scrambleToolStripMenuItem
      // 
      this.scrambleToolStripMenuItem.Name = "scrambleToolStripMenuItem";
      this.scrambleToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.scrambleToolStripMenuItem.Text = "Scramble";
      this.scrambleToolStripMenuItem.Click += new System.EventHandler(this.scrambleToolStripMenuItem_Click);
      // 
      // solveToolStripMenuItem1
      // 
      this.solveToolStripMenuItem1.Name = "solveToolStripMenuItem1";
      this.solveToolStripMenuItem1.Size = new System.Drawing.Size(129, 22);
      this.solveToolStripMenuItem1.Text = "Solve";
      this.solveToolStripMenuItem1.Click += new System.EventHandler(this.solveToolStripMenuItem1_Click);
      // 
      // resetToolStripMenuItem
      // 
      this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
      this.resetToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.resetToolStripMenuItem.Text = "Reset";
      this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
      // 
      // cornerTestToolStripMenuItem
      // 
      this.cornerTestToolStripMenuItem.Name = "cornerTestToolStripMenuItem";
      this.cornerTestToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.cornerTestToolStripMenuItem.Text = "Parity Test";
      this.cornerTestToolStripMenuItem.Click += new System.EventHandler(this.cornerTestToolStripMenuItem_Click);
      // 
      // saveToolStripMenuItem
      // 
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      this.saveToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.saveToolStripMenuItem.Text = "Save...";
      this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.openToolStripMenuItem.Text = "Open...";
      this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // solverToolStripMenuItem
      // 
      this.solverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.solveToolStripMenuItem});
      this.solverToolStripMenuItem.Name = "solverToolStripMenuItem";
      this.solverToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
      this.solverToolStripMenuItem.Text = "Solver";
      // 
      // loadToolStripMenuItem
      // 
      this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
      this.loadToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
      this.loadToolStripMenuItem.Text = "Load Plugins...";
      this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
      // 
      // solveToolStripMenuItem
      // 
      this.solveToolStripMenuItem.Name = "solveToolStripMenuItem";
      this.solveToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
      this.solveToolStripMenuItem.Text = "Select Plugin";
      this.solveToolStripMenuItem.Click += new System.EventHandler(this.solveToolStripMenuItem_Click);
      // 
      // cubeModel
      // 
      this.cubeModel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.cubeModel.Location = new System.Drawing.Point(0, 24);
      this.cubeModel.MaxFps = 50D;
      this.cubeModel.MouseHandling = true;
      this.cubeModel.Name = "cubeModel";
      this.cubeModel.Size = new System.Drawing.Size(380, 309);
      this.cubeModel.TabIndex = 3;
      this.cubeModel.Zoom = 2.3175D;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(380, 333);
      this.Controls.Add(this.cubeModel);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "Form1";
      this.Text = "Form1";
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem solverToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem solveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem rubikToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem scrambleToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem solveToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
    private RubiksCubeLib.CubeModel.CubeModel cubeModel;
    private System.Windows.Forms.ToolStripMenuItem cornerTestToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;


  }
}

