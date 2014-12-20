using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RubiksCubeLib.RubiksCube;
using System.Drawing;

namespace RubiksCubeLib.Solver
{
  /// <summary>
  /// A collection of solvability tests
  /// </summary>
  public static class Solvability
  {
    /// <summary>
    /// Permutation parity test
    /// </summary>
    /// <param name="rubik">Rubik to be tested</param>
    /// <returns>True, if the given Rubik passes the permutation parity test</returns>
    public static bool PermutationParityTest(Rubik rubik)
    {
      Pattern p = Pattern.FromRubik(rubik);
      return p.Inversions % 2 == 0;
    }

    /// <summary>
    /// Corner parity test
    /// </summary>
    /// <param name="rubik">Rubik to be tested</param>
    /// <returns>True, if the given Rubik passes the corner parity test</returns>
    public static bool CornerParityTest(Rubik rubik)
    {
      return rubik.Cubes.Where(c => c.IsCorner).Sum(c => (int)GetOrientation(rubik,c)) % 3 == 0;
    }

    /// <summary>
    /// Edge parity test
    /// </summary>
    /// <param name="rubik">Rubik to be tested</param>
    /// <returns>True, if the given Rubik passes the edge parity test</returns>
    public static bool EdgeParityTest(Rubik rubik)
    {
      return rubik.Cubes.Where(c => c.IsEdge).Sum(c => (int)GetOrientation(rubik, c)) % 2 == 0;
    }

    /// <summary>
    /// Refreshes the position of a cube
    /// </summary>
    /// <param name="r">Parent rubik of the cube</param>
    private static Cube RefreshCube(Rubik r, Cube c)
    {
      return r.Cubes.First(cu => CollectionMethods.ScrambledEquals(cu.Colors, c.Colors));
    }

    /// <summary>
    /// Returns the
    /// </summary>
    /// <param name="rubik"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static Orientation GetOrientation(Rubik rubik, Cube c)
    {
      Orientation orientation = Orientation.Correct;
      if (c.IsEdge)
      {
        CubeFlag targetFlags = rubik.GetTargetFlags(c);
        Rubik clone = rubik.DeepClone();

        if (!targetFlags.HasFlag(CubeFlag.MiddleLayer))
        {
          while (RefreshCube(clone, c).Position.HasFlag(CubeFlag.MiddleLayer)) clone.RotateLayer(c.Position.X, true);

          Cube clonedCube = RefreshCube(clone, c);
          Face yFace = clonedCube.Faces.First(f => f.Color == rubik.TopColor || f.Color == rubik.BottomColor);
          if (!FacePosition.YPos.HasFlag(yFace.Position)) orientation = Orientation.Clockwise;
        }
        else
        {
          Face zFace = c.Faces.First(f => f.Color == rubik.FrontColor || f.Color == rubik.BackColor);
          if (c.Position.HasFlag(CubeFlag.MiddleLayer))
          {
            if (!FacePosition.ZPos.HasFlag(zFace.Position)) orientation = Orientation.Clockwise;
          }
          else
          {
            if (!FacePosition.YPos.HasFlag(zFace.Position)) orientation = Orientation.Clockwise;
          }
        }
      }
      else if(c.IsCorner)
      {
        Face face = c.Faces.First(f => f.Color == rubik.TopColor || f.Color == rubik.BottomColor);
        if (!FacePosition.YPos.HasFlag(face.Position))
        {
          if (FacePosition.XPos.HasFlag(face.Position) ^ !(c.Position.HasFlag(CubeFlag.BottomLayer) ^ (c.Position.HasFlag(CubeFlag.FrontSlice) ^ c.Position.HasFlag(CubeFlag.RightSlice))))
          {
            orientation = Orientation.CounterClockwise;
          }
          else orientation = Orientation.Clockwise;
        }
      }

      return orientation;
    }

    public static bool CorrectColors(Rubik r)
    {
      return r.GenStandardCube().Cubes.Count(sc => r.Cubes
        .Where(c => CollectionMethods.ScrambledEquals(c.Colors, sc.Colors)).Count() == 1) == r.Cubes.Count();
    }

    public static bool FullParityTest(Rubik r)
    {
      return PermutationParityTest(r) && CornerParityTest(r) && EdgeParityTest(r);
    }

    public static bool FullTest(Rubik r)
    {
      return CorrectColors(r) && FullParityTest(r);
    }
  }
}
