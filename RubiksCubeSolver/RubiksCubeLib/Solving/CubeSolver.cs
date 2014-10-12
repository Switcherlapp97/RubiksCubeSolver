using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RubiksCubeLib.RubiksCube;
using System.Drawing;

namespace RubiksCubeLib.Solver
{
  public abstract class CubeSolver: IPluginable
  {
    public Rubik Rubik { get; set; }
    protected Rubik StandardCube { get; set; }
    protected Solution Solution { get; set; }

    public abstract string Name { get; }
    public abstract string Description { get; }

    public Solution Solve(Rubik cube)
    {
      Rubik = cube.DeepClone();
      Solution = new Solution(this, cube);
      InitStandardCube();

      GetSolution();
      RemoveUnnecessaryMoves();
      return Solution;
    }

    public abstract void GetSolution();

    public Rubik ReturnRubik(Rubik cube)
    {
      Solve(cube);
      return Rubik;
    }

    public bool TrySolve(Rubik rubik, out Solution solution)
    {
      solution = this.Solution;
      bool solvable = Solvability.FullTest(rubik);
      if (solvable) solution = Solve(rubik);
      return solvable;
    }

    protected void RemoveUnnecessaryMoves()
    {
      bool finished = false;
      while (!finished)
      {
        finished = true;
        for (int i = 0; i < Solution.Algorithm.Moves.Count; i++)
        {
          if (i != Solution.Algorithm.Moves.Count - 1) if (Solution.Algorithm.Moves[i].Layer == Solution.Algorithm.Moves[i + 1].Layer && Solution.Algorithm.Moves[i].Direction != Solution.Algorithm.Moves[i + 1].Direction)
            {
              finished = false;
              Solution.Algorithm.Moves.RemoveAt(i + 1);
              Solution.Algorithm.Moves.RemoveAt(i);
              if (i != 0) i--;
            }
          if (i < Solution.Algorithm.Moves.Count - 2) if (Solution.Algorithm.Moves[i].Layer == Solution.Algorithm.Moves[i + 1].Layer && Solution.Algorithm.Moves[i].Layer == Solution.Algorithm.Moves[i + 2].Layer
              && Solution.Algorithm.Moves[i].Direction == Solution.Algorithm.Moves[i + 1].Direction && Solution.Algorithm.Moves[i].Direction == Solution.Algorithm.Moves[i + 2].Direction)
            {
              finished = false;
              bool direction = !Solution.Algorithm.Moves[i + 2].Direction;
              Solution.Algorithm.Moves.RemoveAt(i + 1);
              Solution.Algorithm.Moves.RemoveAt(i);
              Solution.Algorithm.Moves[i].Direction = direction;
              if (i != 0) i--;
            }
        }
      }
    }

    protected void InitStandardCube()
    {
      StandardCube = Rubik.GenStandardCube();
    }

    protected CubeFlag GetTargetFlags(Cube cube)
    {
      return StandardCube.Cubes.First(cu => ScrambledEquals(cu.Colors, cube.Colors)).Position.Flags;
    }

    protected void SolverMove(CubeFlag layer, bool direction)
    {
      Rubik.RotateLayer(layer, direction);
      Solution.Algorithm.Moves.Add(new LayerMove(layer, direction));
    }

    protected void SolverAlgorithm(string moves, params object[] placeholders)
    {
      Algorithm algorithm = new Algorithm(moves, placeholders);
      SolverAlgorithm(algorithm);
    }

    protected void SolverAlgorithm(Algorithm algorithm)
    {
      foreach (LayerMove m in algorithm.Moves) SolverMove(m.Layer,m.Direction);
    }

    protected bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
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
