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
  public partial class DialogParityCheckResult : Form
  {
    public DialogParityCheckResult(Rubik rubik, Form parent = null)
    {
      InitializeComponent();
      this.ShowInTaskbar = false;
      if (parent != null)
      {
        this.Owner = parent;
        this.StartPosition = FormStartPosition.CenterParent;
      }

      // Color test
      bool colors = Solvability.CorrectColors(rubik);
      lblColorTest.Text = colors ? "Passed" : "Failed";
      pbColorTest.Image = colors ? Properties.Resources.ok : Properties.Resources.cross_icon;

      if (!colors)
      {
        lblPermutationTest.Text = "Not tested";
        lblCornerTest.Text = "Not tested";
        lblEdgeTest.Text = "Not tested";

        pbCornerTest.Image = Properties.Resources.questionmark;
        pbEdgeTest.Image = Properties.Resources.questionmark;
        pbPermutationTest.Image = Properties.Resources.questionmark;
        lblHeader.Text = "This cube is unsolvable.";
      }
      else
      {
        // Permutation parity test
        bool permutation = Solvability.PermutationParityTest(rubik);
        lblPermutationTest.Text = permutation ? "Passed" : "Failed";
        pbPermutationTest.Image = permutation ? Properties.Resources.ok : Properties.Resources.cross_icon;

        // Corner parity test
        bool corner = Solvability.CornerParityTest(rubik);
        lblCornerTest.Text = corner ? "Passed" : "Failed";
        pbCornerTest.Image = corner ? Properties.Resources.ok : Properties.Resources.cross_icon;

        // Edge parity test
        bool edge = Solvability.EdgeParityTest(rubik);
        lblEdgeTest.Text = edge ? "Passed" : "Failed";
        pbEdgeTest.Image = edge ? Properties.Resources.ok : Properties.Resources.cross_icon;

        lblHeader.Text = permutation && corner && edge && colors ? "This cube is solvable." : "This cube is unsolvable.";
      }

    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
