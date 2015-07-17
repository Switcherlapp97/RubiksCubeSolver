using RubiksCubeLib;
using RubiksCubeLib.CubeModel;
using RubiksCubeLib.RubiksCube;
using RubiksCubeLib.Solver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwoPhaseAlgorithmSolver;

namespace TestApplication
{
  public partial class FormMain : Form
  {
    PluginCollection<CubeSolver> solverPlugins = new PluginCollection<CubeSolver>();
    BindingList<IMove> rotations = new BindingList<IMove>();

    public FormMain()
    {
      InitializeComponent();

      //foreach (string path in Properties.Settings.Default.PluginPaths)
      //{
      //  solverPlugins.AddDll(path);
      //}

      cubeModel.StartRender();

      foreach (CubeFlag flag in Enum.GetValues(typeof(CubeFlag)))
      {
        if (flag != CubeFlag.None && flag != CubeFlag.XFlags && flag != CubeFlag.YFlags && flag != CubeFlag.ZFlags)
          comboBoxLayers.Items.Add(flag.ToString());
      }

      listBoxQueue.DataSource = rotations;
      listBoxQueue.DisplayMember = "Name";
    }


    private void loadToolStripMenuItem_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog fbd = new FolderBrowserDialog();
      if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        solverPlugins.AddFolder(fbd.SelectedPath);
      }
    }

    private void resetToolStripMenuItem_Click(object sender, EventArgs e)
    {
      cubeModel.ResetCube();
    }

    private void scrambleToolStripMenuItem_Click(object sender, EventArgs e)
    {
      cubeModel.Rubik.Scramble(50);
    }

    private void solveToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      DialogSolutionFinder dlg = new DialogSolutionFinder(solverPlugins.StandardPlugin, this.cubeModel.Rubik);
      if (dlg.ShowDialog() == DialogResult.OK)
      {
        dlg.Algorithm.Moves.ForEach(m => rotations.Add(m));
      }
    }

    private void parityTestToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DialogParityCheckResult parityCheck = new DialogParityCheckResult(cubeModel.Rubik);
      parityCheck.ShowDialog();
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (SaveFileDialog sfd = new SaveFileDialog())
      {
        sfd.Filter = "XML-Files|*.xml";
        if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
          cubeModel.SavePattern(sfd.FileName);
        }
      }
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e)
    {
      using (OpenFileDialog ofd = new OpenFileDialog())
      {
        ofd.Filter = "XML-Files|*.xml";
        if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
          cubeModel.LoadPattern(ofd.FileName);
        }
      }
    }

    private void btnRotate_Click(object sender, EventArgs e)
    {
      CubeFlag layer = (CubeFlag)Enum.Parse(typeof(CubeFlag), comboBoxLayers.SelectedItem.ToString());
      cubeModel.RotateLayerAnimated(layer, checkBoxDirection.Checked);
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.rotations.Clear();
    }

    private void btnAddToQueue_Click(object sender, EventArgs e)
    {
      CubeFlag layer = (CubeFlag)Enum.Parse(typeof(CubeFlag), comboBoxLayers.SelectedItem.ToString());
      this.rotations.Add(new LayerMove(layer, checkBoxDirection.Checked));
    }

    private void btnExecute_Click(object sender, EventArgs e)
    {
      foreach (IMove move in rotations)
        cubeModel.RotateLayerAnimated(move);
    }

    private void manageSolversToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      FormAbout frmAbout = new FormAbout();
      frmAbout.ShowDialog();
    }




   private void button1_Click(object sender, EventArgs e)
    {
      TwoPhaseAlgorithm t = new TwoPhaseAlgorithm();
      CoordCube c = new CoordCube();
      c.Flip = 312;
      c.Twist = 1423;
      c.URFtoDRB = 24213;
      c.URtoBR = 931252;
      t.Solution(c, 30, 1000);
      //byte b = coord.GetPruning(coord.sliceTwistPrun, 32543);
      //MessageBox.Show(CoordinateMove.IntToOrientation(1494, 3, 8).ToString());

      //"URF;1,DFR;2,DLF;1,UFL;2,UBR;2,DRB;1,DBL;2,ULB;1";

      //test.GenBasicMoves();
      //test.InitMoveTables();
      //test.InitPruningTable();
      //test.solve(new int[] { 4, 4 });

      //MessageBox.Show(sw.ElapsedMilliseconds.ToString() + "ms");
      //MessageBox.Show(string.Join(",",test.InverseCoordinatePermutation(new byte[8] { 0, 0, 1, 1, 4, 1, 0, 0 })));
    }

  }
}
