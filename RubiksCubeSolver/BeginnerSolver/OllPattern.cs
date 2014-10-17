using RubiksCubeLib;
using RubiksCubeLib.Solver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeginnerSolver
{
  public class OllPattern : PatternTable
  {
    private Dictionary<Pattern, Algorithm> patterns = new Dictionary<Pattern, Algorithm>()
    {
      // Corners correct, edges flipped
      { new Pattern(new string[,] {{"LSU", "1"}, {"MBU", "1"} }), new Algorithm("M' U M U U M' U M") }, 
      { new Pattern(new string[,] {{"MFU", "1"}, {"MBU", "1"} }), new Algorithm("R U R' U' M' U R U' R' M'") }
    };

    public override Dictionary<Pattern, Algorithm> Patterns { get { return patterns; } }
  }
}
