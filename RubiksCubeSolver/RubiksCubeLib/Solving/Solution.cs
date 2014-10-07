using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RubiksCubeLib.RubiksCube;

namespace RubiksCubeLib.Solver
{
  public class Solution
  {
    public Rubik StartingRubik { get; private set; }
    public Rubik CurrentRubik { get; private set; }
    public Algorithm Algorithm { get; private set; }
    public int MovesCount { get { return Algorithm.Moves.Count; } }
    public string SolvingMethod { get; private set; }

    public Solution(CubeSolver solver, Rubik rubik)
    {
      StartingRubik = rubik.DeepClone();
      CurrentRubik = rubik;
      SolvingMethod = solver.Name;
      Algorithm = new Algorithm();
    }
  }

  
}
