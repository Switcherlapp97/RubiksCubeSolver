using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RubiksCubeLib.RubiksCube;
using RubiksCubeLib;
using RubiksCubeLib.Solver;

namespace TwoPhaseAlgorithmSolver
{
  public partial class TwoPhaseAlgorithm
  {
    private CoordCube ToCoordCube(Rubik rubik)
    {
      // get corner perm and orientation
      string[] corners = new string[N_CORNER] { "UFR", "UFL", "UBL", "URB", "DFR", "DFL", "DBL", "DRB" };
      byte[] cornerPermutation = new byte[N_CORNER];
      byte[] cornerOrientation = new byte[N_CORNER];
      for (int i = 0; i < N_CORNER; i++)
      {
        CubeFlag pos = CubeFlagService.Parse(corners[i]);
        Cube matchingCube = rubik.Cubes.First(c => c.Position.Flags == pos);
        CubeFlag targetPos = rubik.GetTargetFlags(matchingCube);
        cornerOrientation[i] = (byte)Solvability.GetOrientation(rubik, matchingCube);

        for (int j = 0; j < N_CORNER; j++)
          if (corners[j] == CubeFlagService.ToNotationString(targetPos))
            cornerPermutation[i] = (byte)(j + 1);
      }

      // get edge perm and orientation
      string[] edges = new string[N_EDGE] { "UR", "UF", "UL", "UB", "DR", "DF", "DL", "DB", "FR", "FL", "BL", "RB" };
      byte[] edgePermutation = new byte[N_EDGE];
      byte[] edgeOrientation = new byte[N_EDGE];
      for (int i = 0; i < N_EDGE; i++)
      {
        CubeFlag pos = CubeFlagService.Parse(edges[i]);
        Cube matchingCube = rubik.Cubes.Where(c => c.IsEdge).First(c => c.Position.Flags.HasFlag(pos));
        CubeFlag targetPos = rubik.GetTargetFlags(matchingCube);
        edgeOrientation[i] = (byte)Solvability.GetOrientation(rubik, matchingCube);

        for (int j = 0; j < N_EDGE; j++)
          if (CubeFlagService.ToNotationString(targetPos).Contains(edges[j]))
            edgePermutation[i] = (byte)(j + 1);
      }

      byte[] cornerInv = CoordCube.ToInversions(cornerPermutation);
      byte[] edgeInv = CoordCube.ToInversions(edgePermutation);

      return new CoordCube(cornerPermutation, edgePermutation, cornerOrientation, edgeOrientation);
    }

    private LayerMove IntsToLayerMove(int axis, int power)
    {
      string[] axes = new string[] { "U", "R", "F", "D", "L", "B" };
      LayerMove newMove = LayerMove.Parse(string.Format("{0}{1}", axes[axis], power == 3 ? "'" : power == 2 ? "2" : ""));
      return newMove;
    }
  }
}
