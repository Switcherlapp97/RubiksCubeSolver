using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RubiksCubeLib.RubiksCube;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace RubiksCubeLib.Solver
{
  /// <summary>
  /// Represents the CubeSolver, and forces all implementing classes to have several methods
  /// </summary>
  public abstract class CubeSolver : IPluginable
  {

    // *** PROPERTIES ***

    /// <summary>
    /// The Rubik which will be used to solve the transferred Rubik
    /// </summary>
    public Rubik Rubik { get; set; }

    /// <summary>
    /// A solved Rubik
    /// </summary>
    protected Rubik StandardCube { get; set; }

    /// <summary>
    /// Returns the solution for this solver used for the Rubik
    /// </summary>
    protected Solution Solution { get; set; }

    /// <summary>
    /// The name of this solver
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// The descrption of this solver
    /// </summary>
    public abstract string Description { get; }



    // *** METHODS ***

    /// <summary>
    /// Returns the solution for the transferred Rubik
    /// </summary>
    /// <param name="cube">Defines the Rubik to be solved</param>
    private Solution Solve(Rubik cube)
    {
      Rubik = cube.DeepClone();
      Solution = new Solution(this, cube);
      InitStandardCube();

      GetSolution();
      RemoveUnnecessaryMoves();
      return Solution;
    }

    public abstract void GetSolution();


    /// <summary>
    /// Returns true if the given Rubik is solvable
    /// </summary>
    /// <param name="rubik">Defines the Rubik to be solved</param>
    /// <param name="solution">Defines the solution to be set if the solving process was successful</param>
    /// <returns></returns>
    public bool TrySolve(Rubik rubik, out Solution solution)
    {
      solution = this.Solution;
      bool solvable = Solvability.FullTest(rubik);
      if (solvable)
        solution = Solve(rubik);
      return solvable;
    }

    public delegate void SolutionFoundEventHandler(object sender, SolutionFoundEventArgs e);
    public event SolutionFoundEventHandler OnSolutionFound;

    public void TrySolveAsync(Rubik rubik)
    {
      Thread t = new Thread(() => SolveAsync(rubik));
      t.Start();
    }

    private void SolveAsync(Rubik rubik)
    {
      Stopwatch sw = new Stopwatch();
      sw.Start();
      bool solvable = Solvability.FullTest(rubik);
      Solution solution = null;
      if (solvable)
        solution = Solve(rubik);
      sw.Stop();
      OnSolutionFound(this, new SolutionFoundEventArgs(solvable, solution, (int)sw.ElapsedMilliseconds));
    }


    /// <summary>
    /// Removes all unnecessary moves from the solution algorithm
    /// </summary>
    protected void RemoveUnnecessaryMoves()
    {
      bool finished = false;
      while (!finished)
      {
        finished = true;
        for (int i = 0; i < Solution.Algorithm.Moves.Count; i++)
        {
          IMove currentMove = Solution.Algorithm.Moves[i];
          if (i < Solution.Algorithm.Moves.Count - 1) if (currentMove.ReverseMove.Equals(Solution.Algorithm.Moves[i + 1]))
            {
              finished = false;
              Solution.Algorithm.Moves.RemoveAt(i + 1);
              Solution.Algorithm.Moves.RemoveAt(i);
              if (i != 0) i--;
            }

          if (i < Solution.Algorithm.Moves.Count - 2) if (currentMove.Equals(Solution.Algorithm.Moves[i + 1]) && currentMove.Equals(Solution.Algorithm.Moves[i + 2]))
            {
              finished = false;
              IMove reverse = Solution.Algorithm.Moves[i + 2].ReverseMove;
              Solution.Algorithm.Moves.RemoveAt(i + 1);
              Solution.Algorithm.Moves.RemoveAt(i);
              Solution.Algorithm.Moves[i] = reverse;
              if (i != 0)
                i--;
            }
        }
      }
    }

    /// <summary>
    /// Initializes the StandardCube
    /// </summary>
    protected void InitStandardCube()
    {
      StandardCube = Rubik.GenStandardCube();
    }

    /// <summary>
    /// Returns the position of given cube where it has to be when the Rubik is solved
    /// </summary>
    /// <param name="cube">Defines the cube to be analyzed</param>
    /// <returns></returns>
    protected CubeFlag GetTargetFlags(Cube cube)
    {
      return StandardCube.Cubes.First(cu => CollectionMethods.ScrambledEquals(cu.Colors, cube.Colors)).Position.Flags;
    }

    /// <summary>
    /// Adds n move to the solution and executes it on the Rubik
    /// </summary>
    /// <param name="layer">Defines the layer to be rotated</param>
    /// <param name="direction">Defines the direction of the rotation</param>
    protected void SolverMove(CubeFlag layer, bool direction)
    {
      Rubik.RotateLayer(layer, direction);
      Solution.Algorithm.Moves.Add(new LayerMove(layer, direction));
    }

    /// <summary>
    /// Adds a move to the solution and executes it on the Rubik
    /// </summary>
    /// <param name="move">Defines the move to be rotated</param>
    protected void SolverMove(IMove move)
    {
      Rubik.RotateLayer(move);
      Solution.Algorithm.Moves.Add(move);
    }

    /// <summary>
    /// Executes the given algorithm
    /// </summary>
    /// <param name="moves">Defines a notation string, which is filled with placeholders</param>
    /// <param name="placeholders">Defines the objects to be inserted for the placeholders</param>
    protected void SolverAlgorithm(string moves, params object[] placeholders)
    {
      Algorithm algorithm = new Algorithm(moves, placeholders);
      SolverAlgorithm(algorithm);
    }

    /// <summary>
    /// Executes the given alorithm on the Rubik
    /// </summary>
    /// <param name="algorithm">Defines the algorithm to be executed</param>
    protected void SolverAlgorithm(Algorithm algorithm)
    {
      foreach (IMove m in algorithm.Moves)
        SolverMove(m);
    }

  }
}
