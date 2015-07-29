using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.Solver
{
  public class SolutionStepCompletedEventArgs : EventArgs
  {
    public bool Finished { get; private set; }
    public Algorithm Algorithm { get; private set; }
    public int Milliseconds { get; private set; }
    public string Step { get; private set; }
    public SolutionStepType Type { get; private set; }

    public SolutionStepCompletedEventArgs(string step, bool finished, Algorithm moves, int milliseconds, SolutionStepType type = SolutionStepType.Standard)
    {
      this.Step = step;
      this.Finished = finished;
      this.Algorithm = moves;
      this.Milliseconds = milliseconds;
      this.Type = type;
    }
  }
}
