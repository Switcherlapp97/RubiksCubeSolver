using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RubiksCubeLib.RubiksCube;
using System.Drawing;

namespace RubiksCubeLib.Solver
{
  public static class Solvability
  {
    public static bool PermutationParityTest(Rubik rubik)
    {
      Pattern p = Pattern.FromRubik(rubik);
      return p.Inversions % 2 == 0;
    }

    private static List<CubeFlag> Order(Rubik r, List<CubePosition> pos)
    {
      Random rnd = new Random();
      pos.OrderBy(p => rnd.Next());

      List<CubeFlag> result = new List<CubeFlag>();
      foreach (CubePosition p in pos)
      {
        result.Add(r.Cubes.First(c => r.GetTargetFlags(c) == p.Flags).Position.Flags);
      }
      return result;
    }

    private static int CountInversions(List<CubeFlag> standard, List<CubeFlag> input)
    {
      int inversions = 0;
      for (int i = 0; i < input.Count; i++)
      {
        int index = standard.IndexOf(input[i]);
        for (int j = 0; j < input.Count; j++)
        {
          int index2 = standard.IndexOf(input[j]);
          if (index2 > index && j < i)
          {
            inversions++;
          }
        }
      }
      return inversions;
    }

    public static bool CornerParityTest(Rubik rubik)
    {
      return rubik.Cubes.Where(c => c.IsCorner).Sum(c => PositionOrientation.GetOrientation(rubik,c)) % 3 == 0;
    }

    public static bool EdgeParityTest(Rubik rubik)
    {
      return rubik.Cubes.Where(c => c.IsEdge).Sum(c => PositionOrientation.GetOrientation(rubik, c)) % 2 == 0;
    }

    private static Cube RefreshCube(Rubik r, Cube c)
    {
      return r.Cubes.First(cu => CollectionMethods.ScrambledEquals(cu.Colors, c.Colors));
    }

    public static int GetOrientation(Rubik rubik, Cube c)
    {
      int orientation = 0;
      if (c.IsEdge)
      {
        CubeFlag targetFlags = rubik.GetTargetFlags(c);
        Rubik clone = rubik.DeepClone();

        if (!targetFlags.HasFlag(CubeFlag.MiddleLayer))
        {
          while (RefreshCube(clone, c).Position.HasFlag(CubeFlag.MiddleLayer)) clone.RotateLayer(c.Position.X, true);

          Cube clonedCube = RefreshCube(clone, c);
          Face yFace = clonedCube.Faces.First(f => f.Color == rubik.TopColor || f.Color == rubik.BottomColor);
          if (!FacePosition.YPos.HasFlag(yFace.Position)) orientation = 1;
        }
        else
        {
          Face zFace = c.Faces.First(f => f.Color == rubik.FrontColor || f.Color == rubik.BackColor);
          if (c.Position.HasFlag(CubeFlag.MiddleLayer))
          {
            if (!FacePosition.ZPos.HasFlag(zFace.Position)) orientation = 1;
          }
          else
          {
            if (!FacePosition.YPos.HasFlag(zFace.Position)) orientation = 1;
          }
        }
      }
      else if(c.IsCorner)
      {
        Face face = c.Faces.First(f => f.Color == rubik.TopColor || f.Color == rubik.BottomColor);
        if (!FacePosition.YPos.HasFlag(face.Position))
        {
          if (FacePosition.XPos.HasFlag(face.Position) ^ !((c.Position.HasFlag(CubeFlag.BottomLayer) ^ (c.Position.HasFlag(CubeFlag.FrontSlice) ^ c.Position.HasFlag(CubeFlag.RightSlice)))))
          {
            orientation = 1;
          }
          else orientation = 2;
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
