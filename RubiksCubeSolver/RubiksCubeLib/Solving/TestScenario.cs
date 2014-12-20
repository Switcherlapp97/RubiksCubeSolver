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

    private Cube RefreshCube(Cube c)
    {
      return Rubik.Cubes.First(cu => CollectionMethods.ScrambledEquals(cu.Colors, c.Colors));
    }

  }
}
