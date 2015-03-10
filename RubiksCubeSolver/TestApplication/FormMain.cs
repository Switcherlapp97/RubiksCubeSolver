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

namespace TestApplication
{
  public partial class FormMain : Form
  {
    PluginCollection<CubeSolver> solverPlugins = new PluginCollection<CubeSolver>();
    BindingList<IMove> rotations = new BindingList<IMove>();

    public FormMain()
    {
      InitializeComponent();
      cubeModel.StartRender();
      solverPlugins.AddFolder(@"C:\Users\Anwender\Desktop\RubiksCubeSolver\trunk\RubiksCubeSolver\FridrichSolver\bin\Debug");
      //solverPlugins.AddFolder(@"C:\Users\Anwender\Desktop\RubiksCubeSolver\trunk\RubiksCubeSolver\BeginnerSolver\bin\Debug");
      AddSolvingHandlers();

      foreach (CubeFlag flag in Enum.GetValues(typeof(CubeFlag)))
      {
        if (flag != CubeFlag.None && flag != CubeFlag.XFlags && flag != CubeFlag.YFlags && flag != CubeFlag.ZFlags)
          comboBoxLayers.Items.Add(flag.ToString());
      }

      listBoxQueue.DataSource = rotations;
      listBoxQueue.DisplayMember = "Name";
    }

    private void AddSolvingHandlers()
    {
      foreach (CubeSolver solver in solverPlugins.GetAll())
      {
        solver.OnSolutionStepCompleted -= ExecuteSolution;
        solver.OnSolutionStepCompleted += ExecuteSolution;
      }
    }

    private void loadToolStripMenuItem_Click(object sender, EventArgs e)
    {
      FolderBrowserDialog fbd = new FolderBrowserDialog();
      if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        solverPlugins.AddFolder(fbd.SelectedPath);
        AddSolvingHandlers();
      }
    }

    private void solveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      PluginSelectorDialog<CubeSolver> dlg = new PluginSelectorDialog<CubeSolver>(solverPlugins);
      if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        cubeModel.MouseHandling = false;
        solverPlugins.StandardPlugin.TrySolveAsync(cubeModel.Rubik);
      }
    }

    private void ExecuteSolution(object sender, SolutionStepCompletedEventArgs e)
    {
      //MessageBox.Show(string.Format("{0} completed with {1} moves in {2} milliseconds", e.Step, e.Algorithm.Moves.Count, e.Milliseconds));
      //if (e.Finished)e.Algorithm.Moves.ForEach(m => cubeModel.RotateLayerAnimated(m));
        //MessageBox.Show(string.Format("Solution found with {0}: Moves count: {1}; Elapsed Milliseconds {2}", e.Step, e.Algorithm.Moves.Count, e.Milliseconds));
        // e.Algorithm.Moves.ForEach(m => cubeModel.RotateLayerAnimated(m));
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

    private void cornerTestToolStripMenuItem_Click(object sender, EventArgs e)
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

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      
    }

    private void manageSolversToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DialogSolutionFinder dlg = new DialogSolutionFinder(solverPlugins.StandardPlugin, this.cubeModel.Rubik);
      dlg.ShowDialog();
    }

  }
}
