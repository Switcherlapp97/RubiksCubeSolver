using RubiksCubeLib.RubiksCube;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.Solver
{
  public class TestScenario
  {
    public Rubik Rubik { get; private set; }
    public Algorithm Algorithm { get; set; }

    public TestScenario(Rubik rubik, Algorithm moves)
    {
      Rubik = rubik.DeepClone();
      Algorithm = moves;
    }

    public TestScenario(Rubik rubik, LayerMove move) : this(rubik, new Algorithm(move.ToString())) { }

    public TestScenario(Rubik rubik) : this(rubik, new Algorithm()) { }

    public bool Test(Func<Rubik, bool> func)
    {
      foreach(LayerMove move in Algorithm.Moves)
      {
        Rubik.RotateLayer(move);
      }
      return func(Rubik);
    }

    public bool TestCubePosition(Cube c, CubeFlag endPos)
    {
      foreach(LayerMove move in Algorithm.Moves)
      {
        Rubik.RotateLayer(move);
      }
      bool result = RefreshCube(c).Position.HasFlag(endPos);
      return result;
    }

    public bool IsSolved(Solution sol)
    {
      // check colors
      bool correctColors = Rubik.GenStandardCube().Cubes.Count(sc => Rubik.Cubes
        .Where(c => ScrambledEquals(c.Colors, sc.Colors)).Count() == 1) == Rubik.Cubes.Count();

      if (!correctColors) return false;

      // solve with solution algorithm
      foreach (LayerMove m in sol.Algorithm.Moves) Rubik.RotateLayer(m);

      //check if all the cube faces are solved
      CubeFlag layers = CubeFlag.TopLayer | CubeFlag.BottomLayer | CubeFlag.RightSlice | CubeFlag.LeftSlice | CubeFlag.FrontSlice | CubeFlag.BackSlice;
      foreach (CubeFlag l in CubeFlagService.GetFlags(layers))
      {
        FacePosition facePos = CubeFlagService.ToFacePosition(l);
        if (facePos != FacePosition.None)
        {
          CubeFlag centerPos = Rubik.Cubes.First(c => c.IsCenter && c.Position.HasFlag(l)).Position.Flags;
          Color faceColor = Rubik.GetFaceColor(centerPos, facePos);

          bool faceSolved = Rubik.Cubes.Count(c => c.Position.HasFlag(l) && c.Faces.First(f => f.Position == facePos).Color == faceColor) == 9;
          if (!faceSolved) return false;
        }
      }
      return true;
    }

    private Cube RefreshCube(Cube c)
    {
      return Rubik.Cubes.First(cu => ScrambledEquals(cu.Colors, c.Colors));
    }

    private bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
    {
      var cnt = new Dictionary<T, int>();
      foreach (T s in list1)
      {
        if (cnt.ContainsKey(s))
        {
          cnt[s]++;
        }
        else
        {
          cnt.Add(s, 1);
        }
      }
      foreach (T s in list2)
      {
        if (cnt.ContainsKey(s))
        {
          cnt[s]--;
        }
        else
        {
          return false;
        }
      }
      return cnt.Values.All(c => c == 0);
    }

  }
}
