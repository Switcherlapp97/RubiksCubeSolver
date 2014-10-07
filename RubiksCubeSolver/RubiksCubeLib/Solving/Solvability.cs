using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RubiksCubeLib.RubiksCube;
using System.Drawing;

namespace RubiksCubeLib.Solving
{
  public static class Solvability
  {
    #region position definitions
    private static List<CubePosition> cornerPos = new List<CubePosition>
    { 
      new CubePosition(CubeFlag.LeftSlice,CubeFlag.BackSlice,CubeFlag.TopLayer),
      new CubePosition(CubeFlag.RightSlice,CubeFlag.BackSlice,CubeFlag.TopLayer),
      new CubePosition(CubeFlag.RightSlice,CubeFlag.FrontSlice,CubeFlag.TopLayer),
      new CubePosition(CubeFlag.LeftSlice,CubeFlag.FrontSlice,CubeFlag.TopLayer),
      new CubePosition(CubeFlag.LeftSlice,CubeFlag.BackSlice,CubeFlag.BottomLayer),
      new CubePosition(CubeFlag.RightSlice,CubeFlag.BackSlice,CubeFlag.BottomLayer),
      new CubePosition(CubeFlag.RightSlice,CubeFlag.FrontSlice,CubeFlag.BottomLayer),
      new CubePosition(CubeFlag.LeftSlice,CubeFlag.FrontSlice,CubeFlag.BottomLayer),
    };

    private static List<CubePosition> edgePos = new List<CubePosition>
    {
      new CubePosition(CubeFlag.MiddleSliceSides, CubeFlag.BackSlice,CubeFlag.TopLayer),
      new CubePosition(CubeFlag.RightSlice, CubeFlag.MiddleSlice, CubeFlag.TopLayer),
      new CubePosition(CubeFlag.MiddleSliceSides, CubeFlag.FrontSlice, CubeFlag.TopLayer),
      new CubePosition(CubeFlag.LeftSlice, CubeFlag.MiddleSlice,CubeFlag.TopLayer),
       
      new CubePosition(CubeFlag.LeftSlice, CubeFlag.BackSlice,CubeFlag.MiddleLayer),
      new CubePosition(CubeFlag.RightSlice, CubeFlag.BackSlice, CubeFlag.MiddleLayer),
      new CubePosition(CubeFlag.RightSlice, CubeFlag.FrontSlice, CubeFlag.MiddleLayer),
      new CubePosition(CubeFlag.LeftSlice, CubeFlag.FrontSlice,CubeFlag.MiddleLayer),

      new CubePosition(CubeFlag.MiddleSliceSides, CubeFlag.BackSlice,CubeFlag.BottomLayer),
      new CubePosition(CubeFlag.RightSlice, CubeFlag.MiddleSlice, CubeFlag.BottomLayer),
      new CubePosition(CubeFlag.MiddleSliceSides, CubeFlag.FrontSlice, CubeFlag.BottomLayer),
      new CubePosition(CubeFlag.LeftSlice, CubeFlag.MiddleSlice,CubeFlag.BottomLayer),
    };
    #endregion

    public static bool PermutationParityTest(Rubik rubik)
    {
      int inversions = 0;
      Rubik standard = rubik.GenStandardCube();
      inversions = CountInversions(cornerPos.Select(p => p.Flags).ToList(), Order(rubik, cornerPos))
        + CountInversions(edgePos.Select(p => p.Flags).ToList(), Order(rubik, edgePos));
      return inversions % 2 == 0;
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
      int sum = 0;
      return sum % 3 == 0;
    }
  }
}
