using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.Solver
{
  public class SolutionFoundEventArgs : EventArgs
  {
    public bool Solvable { get; private set; }
    public Solution Solution { get; private set; }
    public int Milliseconds { get; private set; }

    public SolutionFoundEventArgs(bool solvable, Solution solution, int milliseconds)
    {
      this.Solution = solution;
      this.Solvable = solvable;
      this.Milliseconds = milliseconds;
    }
  }
}
