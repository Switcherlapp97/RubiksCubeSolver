namespace TestApplication
{
  partial class FormMain
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
      this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.rubikToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.scrambleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.solveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.parityTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.manageSolversToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.groupBox3D = new System.Windows.Forms.GroupBox();
      this.cubeModel = new RubiksCubeLib.CubeModel.CubeModel();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.listBoxQueue = new System.Windows.Forms.ListBox();
      this.btnClear = new System.Windows.Forms.Button();
      this.btnExecute = new System.Windows.Forms.Button();
      this.groupBoxMoves = new System.Windows.Forms.GroupBox();
      this.btnAddToQueue = new System.Windows.Forms.Button();
      this.btnRotate = new System.Windows.Forms.Button();
      this.comboBoxLayers = new System.Windows.Forms.ComboBox();
      this.checkBoxDirection = new System.Windows.Forms.CheckBox();
      this.menuStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.groupBox3D.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBoxMoves.SuspendLayout();
      this.SuspendLayout();
      // 
      // menuStrip1
      // 
      this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.rubikToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(694, 24);
      this.menuStrip1.TabIndex = 2;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem1,
            this.toolStripSeparator,
            this.saveToolStripMenuItem1,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "&File";
      // 
      // openToolStripMenuItem1
      // 
      this.openToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem1.Image")));
      this.openToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
      this.openToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
      this.openToolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
      this.openToolStripMenuItem1.Text = "&Open";
      this.openToolStripMenuItem1.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // toolStripSeparator
      // 
      this.toolStripSeparator.Name = "toolStripSeparator";
      this.toolStripSeparator.Size = new System.Drawing.Size(143, 6);
      // 
      // saveToolStripMenuItem1
      // 
      this.saveToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem1.Image")));
      this.saveToolStripMenuItem1.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
      this.saveToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
      this.saveToolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
      this.saveToolStripMenuItem1.Text = "&Save";
      this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(143, 6);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
      this.exitToolStripMenuItem.Text = "E&xit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // rubikToolStripMenuItem
      // 
      this.rubikToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scrambleToolStripMenuItem,
            this.toolStripSeparator2,
            this.solveToolStripMenuItem1,
            this.parityTestToolStripMenuItem,
            this.toolStripSeparator3,
            this.resetToolStripMenuItem});
      this.rubikToolStripMenuItem.Name = "rubikToolStripMenuItem";
      this.rubikToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
      this.rubikToolStripMenuItem.Text = "&Rubik";
      // 
      // scrambleToolStripMenuItem
      // 
      this.scrambleToolStripMenuItem.Name = "scrambleToolStripMenuItem";
      this.scrambleToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.scrambleToolStripMenuItem.Text = "&Scramble";
      this.scrambleToolStripMenuItem.Click += new System.EventHandler(this.scrambleToolStripMenuItem_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(126, 6);
      // 
      // solveToolStripMenuItem1
      // 
      this.solveToolStripMenuItem1.Name = "solveToolStripMenuItem1";
      this.solveToolStripMenuItem1.Size = new System.Drawing.Size(129, 22);
      this.solveToolStripMenuItem1.Text = "S&olve";
      this.solveToolStripMenuItem1.Click += new System.EventHandler(this.solveToolStripMenuItem1_Click);
      // 
      // parityTestToolStripMenuItem
      // 
      this.parityTestToolStripMenuItem.Name = "parityTestToolStripMenuItem";
      this.parityTestToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.parityTestToolStripMenuItem.Text = "&Parity Test";
      this.parityTestToolStripMenuItem.Click += new System.EventHandler(this.parityTestToolStripMenuItem_Click);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(126, 6);
      // 
      // resetToolStripMenuItem
      // 
      this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
      this.resetToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
      this.resetToolStripMenuItem.Text = "&Reset";
      this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
      // 
      // toolsToolStripMenuItem
      // 
      this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem,
            this.manageSolversToolStripMenuItem});
      this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
      this.toolsToolStripMenuItem.Text = "&Tools";
      // 
      // optionsToolStripMenuItem
      // 
      this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
      this.optionsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
      this.optionsToolStripMenuItem.Text = "&Options";
      this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
      // 
      // manageSolversToolStripMenuItem
      // 
      this.manageSolversToolStripMenuItem.Name = "manageSolversToolStripMenuItem";
      this.manageSolversToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
      this.manageSolversToolStripMenuItem.Text = "&Manage Solvers...";
      this.manageSolversToolStripMenuItem.Click += new System.EventHandler(this.manageSolversToolStripMenuItem_Click);
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
      this.helpToolStripMenuItem.Text = "&Help";
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
      this.aboutToolStripMenuItem.Text = "&About...";
      this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
      this.splitContainer1.Location = new System.Drawing.Point(0, 24);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.groupBox3D);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
      this.splitContainer1.Panel2.Controls.Add(this.groupBoxMoves);
      this.splitContainer1.Size = new System.Drawing.Size(694, 376);
      this.splitContainer1.SplitterDistance = 450;
      this.splitContainer1.TabIndex = 4;
      // 
      // groupBox3D
      // 
      this.groupBox3D.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox3D.Controls.Add(this.cubeModel);
      this.groupBox3D.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBox3D.Location = new System.Drawing.Point(12, 3);
      this.groupBox3D.Name = "groupBox3D";
      this.groupBox3D.Size = new System.Drawing.Size(435, 357);
      this.groupBox3D.TabIndex = 4;
      this.groupBox3D.TabStop = false;
      this.groupBox3D.Text = "3D View";
      // 
      // cubeModel
      // 
      this.cubeModel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cubeModel.DrawingMode = RubiksCubeLib.CubeModel.DrawingMode.ThreeDimensional;
      this.cubeModel.Location = new System.Drawing.Point(6, 19);
      this.cubeModel.MaxFps = 50D;
      this.cubeModel.MouseHandling = true;
      this.cubeModel.Name = "cubeModel";
      this.cubeModel.RotationSpeed = 250;
      this.cubeModel.Size = new System.Drawing.Size(423, 336);
      this.cubeModel.TabIndex = 3;
      this.cubeModel.Zoom = 1D;
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.listBoxQueue);
      this.groupBox1.Controls.Add(this.btnClear);
      this.groupBox1.Controls.Add(this.btnExecute);
      this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBox1.Location = new System.Drawing.Point(3, 84);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(225, 280);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Queue";
      // 
      // listBoxQueue
      // 
      this.listBoxQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listBoxQueue.FormattingEnabled = true;
      this.listBoxQueue.Location = new System.Drawing.Point(6, 19);
      this.listBoxQueue.Name = "listBoxQueue";
      this.listBoxQueue.Size = new System.Drawing.Size(214, 225);
      this.listBoxQueue.TabIndex = 0;
      // 
      // btnClear
      // 
      this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClear.Location = new System.Drawing.Point(117, 251);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new System.Drawing.Size(102, 23);
      this.btnClear.TabIndex = 2;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
      // 
      // btnExecute
      // 
      this.btnExecute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnExecute.Location = new System.Drawing.Point(6, 251);
      this.btnExecute.Name = "btnExecute";
      this.btnExecute.Size = new System.Drawing.Size(105, 23);
      this.btnExecute.TabIndex = 2;
      this.btnExecute.Text = "Execute";
      this.btnExecute.UseVisualStyleBackColor = true;
      this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
      // 
      // groupBoxMoves
      // 
      this.groupBoxMoves.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxMoves.Controls.Add(this.btnAddToQueue);
      this.groupBoxMoves.Controls.Add(this.btnRotate);
      this.groupBoxMoves.Controls.Add(this.comboBoxLayers);
      this.groupBoxMoves.Controls.Add(this.checkBoxDirection);
      this.groupBoxMoves.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBoxMoves.Location = new System.Drawing.Point(3, 3);
      this.groupBoxMoves.Name = "groupBoxMoves";
      this.groupBoxMoves.Size = new System.Drawing.Size(225, 75);
      this.groupBoxMoves.TabIndex = 0;
      this.groupBoxMoves.TabStop = false;
      this.groupBoxMoves.Text = "Control";
      // 
      // btnAddToQueue
      // 
      this.btnAddToQueue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnAddToQueue.Location = new System.Drawing.Point(117, 46);
      this.btnAddToQueue.Name = "btnAddToQueue";
      this.btnAddToQueue.Size = new System.Drawing.Size(102, 23);
      this.btnAddToQueue.TabIndex = 2;
      this.btnAddToQueue.Text = "Add To Queue";
      this.btnAddToQueue.UseVisualStyleBackColor = true;
      this.btnAddToQueue.Click += new System.EventHandler(this.btnAddToQueue_Click);
      // 
      // btnRotate
      // 
      this.btnRotate.Location = new System.Drawing.Point(6, 46);
      this.btnRotate.Name = "btnRotate";
      this.btnRotate.Size = new System.Drawing.Size(105, 23);
      this.btnRotate.TabIndex = 2;
      this.btnRotate.Text = "Rotate";
      this.btnRotate.UseVisualStyleBackColor = true;
      this.btnRotate.Click += new System.EventHandler(this.btnRotate_Click);
      // 
      // comboBoxLayers
      // 
      this.comboBoxLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxLayers.FormattingEnabled = true;
      this.comboBoxLayers.Location = new System.Drawing.Point(6, 19);
      this.comboBoxLayers.Name = "comboBoxLayers";
      this.comboBoxLayers.Size = new System.Drawing.Size(133, 21);
      this.comboBoxLayers.TabIndex = 1;
      // 
      // checkBoxDirection
      // 
      this.checkBoxDirection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.checkBoxDirection.AutoSize = true;
      this.checkBoxDirection.Checked = true;
      this.checkBoxDirection.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxDirection.Location = new System.Drawing.Point(142, 21);
      this.checkBoxDirection.Name = "checkBoxDirection";
      this.checkBoxDirection.Size = new System.Drawing.Size(77, 17);
      this.checkBoxDirection.TabIndex = 0;
      this.checkBoxDirection.Text = "Clockwise";
      this.checkBoxDirection.UseVisualStyleBackColor = true;
      // 
      // FormMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(694, 400);
      this.Controls.Add(this.splitContainer1);
      this.Controls.Add(this.menuStrip1);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "FormMain";
      this.Text = "Rubik\'s Cube Solver";
      this.Activated += new System.EventHandler(this.FormMain_Activated);
      this.Deactivate += new System.EventHandler(this.FormMain_Deactivate);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.groupBox3D.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBoxMoves.ResumeLayout(false);
      this.groupBoxMoves.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem rubikToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem scrambleToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem solveToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
    private RubiksCubeLib.CubeModel.CubeModel cubeModel;
    private System.Windows.Forms.ToolStripMenuItem parityTestToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem manageSolversToolStripMenuItem;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.GroupBox groupBox3D;
    private System.Windows.Forms.GroupBox groupBoxMoves;
    private System.Windows.Forms.CheckBox checkBoxDirection;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.ListBox listBoxQueue;
    private System.Windows.Forms.Button btnAddToQueue;
    private System.Windows.Forms.Button btnRotate;
    private System.Windows.Forms.ComboBox comboBoxLayers;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.Button btnClear;
    private System.Windows.Forms.Button btnExecute;


  }
}

