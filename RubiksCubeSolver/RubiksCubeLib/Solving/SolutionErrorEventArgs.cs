using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.Solver
{
  public class SolutionErrorEventArgs : EventArgs
  {
    public string Step { get; private set; }
    public string Message { get; private set; }

    public SolutionErrorEventArgs(string step, string message)
    {
      this.Step = step;
      this.Message = message;
    }
  }
}
