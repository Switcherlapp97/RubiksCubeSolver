using RubiksCubeLib;
using RubiksCubeLib.RubiksCube;
using RubiksCubeLib.Solver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestApplication
{
  public partial class DialogSolutionFinder : Form
  {
    private CubeSolver solver;

    private List<PictureBox> stepImgs = new List<PictureBox>();
    private List<Label> stepLabels = new List<Label>();
    private int currentIndex = 0;

    public Algorithm Algorithm { get; private set; }


    public DialogSolutionFinder(CubeSolver solver, Rubik rubik, Form parent = null)
    {
      this.solver = solver;
      InitializeComponent();
      if (parent != null)
      {
        this.Owner = parent;
        this.StartPosition = FormStartPosition.CenterParent;
      }
      this.ShowInTaskbar = false;

      AddStepLabels(solver);

      solver.TrySolveAsync(rubik);
      solver.OnSolutionStepCompleted += solver_OnSolutionStepCompleted;
      solver.OnSolutionError += solver_OnSolutionError;

      stepLabels[currentIndex].Text = "In progress ...";
      stepImgs[currentIndex].Image = Properties.Resources.refresh;
    }

    void solver_OnSolutionError(object sender, SolutionErrorEventArgs e)
    {
      PictureBox currentStepImg = stepImgs[currentIndex];
      Label currentStep = stepLabels[currentIndex];
      if (currentStepImg.InvokeRequired) currentStepImg.Invoke((MethodInvoker)delegate() { currentStepImg.Image = Properties.Resources.cross_icon; });
      if (currentStep.InvokeRequired) currentStep.Invoke((MethodInvoker)delegate() { currentStep.Text = "Failed"; });
      if (lblHeader.InvokeRequired) lblHeader.Invoke((MethodInvoker)delegate() { lblHeader.Text = "Solving error."; });
      solver.OnSolutionStepCompleted -= solver_OnSolutionStepCompleted;
    }

    void solver_OnSolutionStepCompleted(object sender, SolutionStepCompletedEventArgs e)
    {
      if (!e.Finished)
      {
        PictureBox currentStepImg = stepImgs[currentIndex];
        Label currentStep = stepLabels[currentIndex];
        if (currentStepImg.InvokeRequired) currentStepImg.Invoke((MethodInvoker)delegate() { currentStepImg.Image = Properties.Resources.ok; });
        if (currentStep.InvokeRequired) currentStep.Invoke((MethodInvoker)delegate() { currentStep.Text = e.Type == SolutionStepType.Standard ? string.Format("{0} moves", e.Algorithm.Moves.Count) : string.Empty; });
        currentIndex++;

        if (currentIndex < stepImgs.Count)
        {
          currentStepImg = stepImgs[currentIndex];
          currentStep = stepLabels[currentIndex];
          if (currentStepImg.InvokeRequired) currentStepImg.Invoke((MethodInvoker)delegate() { currentStepImg.Image = Properties.Resources.refresh; });
          if (currentStep.InvokeRequired) currentStep.Invoke((MethodInvoker)delegate() { currentStep.Text = "In progress ..."; });
        }
      }
      else
      {
        if (lblTimeHeader.InvokeRequired) lblTimeHeader.Invoke((MethodInvoker)delegate() { lblTime.Text = string.Format("{0:f2}s", e.Milliseconds / 1000.0); });
        if (lblMovesHeader.InvokeRequired) lblMovesHeader.Invoke((MethodInvoker)delegate() { lblMoves.Text = string.Format("{0} moves", e.Algorithm.Moves.Count); });
        if (lblHeader.InvokeRequired) lblHeader.Invoke((MethodInvoker)delegate() { lblHeader.Text = "Solution found."; });
        if (btnAdd.InvokeRequired) btnAdd.Invoke((MethodInvoker)delegate() { btnAdd.Enabled = true; });
        solver.OnSolutionStepCompleted -= solver_OnSolutionStepCompleted;
        this.Algorithm = e.Algorithm;
      }
    }

    private void AddStepLabels(CubeSolver solver)
    {
      this.lblSolvingMethod.Text = solver.Name;
      // Start pos 15,62
      int y = 62, x = 15;

      foreach (KeyValuePair<string, Tuple<Action, SolutionStepType>> step in solver.SolutionSteps)
      {
        Label l = new Label();
        l.AutoSize = true;
        l.Font = new Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        l.Location = new Point(x, y);
        l.Name = string.Format("label{0}", step.Key);
        l.Text = step.Key;
        panel1.Controls.Add(l);

        PictureBox p = new PictureBox();
        p.Location = new Point(200, y);
        p.Name = string.Format("pb{0}", step.Key);
        p.Size = new Size(17, 15);
        p.SizeMode = PictureBoxSizeMode.StretchImage;
        p.TabStop = false;
        panel1.Controls.Add(p);
        stepImgs.Add(p);

        l = new Label();
        l.AutoSize = true;
        l.Font = new Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        l.Location = new Point(217, y);
        l.Name = string.Format("labelMoves{0}", step.Key);
        panel1.Controls.Add(l);
        stepLabels.Add(l);

        y += 18;
      }

      y += 10;
      this.lblTimeHeader.Location = new Point(10, y);
      y += 18;
      this.lblMovesHeader.Location = new Point(10, y);
      this.Size = new Size(338, y + 103);
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }
  }
}
