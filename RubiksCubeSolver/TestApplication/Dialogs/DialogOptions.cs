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
  public partial class DialogOptions : Form
  {
    public DialogOptions(Rubik rubik)
    {
      InitializeComponent();

    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
