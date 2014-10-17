using RubiksCubeLib;
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

namespace TestApplication
{
  public partial class Form1 : Form
  {
    PluginCollection<CubeSolver> solverPlugins = new PluginCollection<CubeSolver>();

    public Form1()
    {
      InitializeComponent();
      solverPlugins.AddFolder(@"C:\Users\Anwender\Desktop\plugins");
    }

    private void cubeModel_OnSelectionChanged(object sender, RubiksCubeLib.CubeModel.SelectionChangedEventArgs e)
    {
      statusLblSelection.Text = string.Format("[{0}] | {1}", e.Position.CubePosition, e.Position.FacePosition);
    }

    private void loadToolStripMenuItem_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog fbd = new FolderBrowserDialog();
      if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        solverPlugins.AddFolder(fbd.SelectedPath);
      }
    }

    private void solveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      PluginSelectorDialog<CubeSolver> dlg = new PluginSelectorDialog<CubeSolver>(solverPlugins);
      if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        Solution s;
        if (solverPlugins.StandardPlugin.TrySolve(cubeModel.Rubik, out s))
        {
          s.Algorithm.Moves.ForEach(m => cubeModel.RotateLayerAnimated(m.Layer, m.Direction));
        }
        else
        {
          MessageBox.Show("Unsolvable");
        }
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
      Solution s;
      if (solverPlugins.StandardPlugin.TrySolve(cubeModel.Rubik,out s))
      {
        s.Algorithm.Moves.ForEach(m => cubeModel.RotateLayerAnimated(m.Layer, m.Direction));
      }
      else
      {
        MessageBox.Show("Unsolvable");
      }
    }

    private void cornerTestToolStripMenuItem_Click(object sender, EventArgs e)
    {
      MessageBox.Show(Solvability.FullTest(cubeModel.Rubik) ? "Solvable" : "Unsolvable");
    }
  }
}
