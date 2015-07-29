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
    protected Rubik Rubik { get; set; }

    /// <summary>
    /// A solved Rubik
    /// </summary>
    protected Rubik StandardCube { get; set; }

    /// <summary>
    /// Returns the solution for this solver used for the Rubik
    /// </summary>
    protected Algorithm Algorithm { get; set; }

    /// <summary>
    /// The name of this solver
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// The descrption of this solver
    /// </summary>
    public abstract string Description { get; }

    public Dictionary<string, Tuple<Action, SolutionStepType>> SolutionSteps { get; protected set; }

    private List<IMove> _movesOfStep = new List<IMove>();
    private Thread solvingThread;

    public delegate void SolutionStepCompletedEventHandler(object sender, SolutionStepCompletedEventArgs e);
    public event SolutionStepCompletedEventHandler OnSolutionStepCompleted;

    public delegate void SolutionErrorEventHandler(object sender, SolutionErrorEventArgs e);
    public event SolutionErrorEventHandler OnSolutionError;

    protected void BroadcastOnSolutionError(string step, string message)
    {
      if (OnSolutionError != null) this.OnSolutionError(this, new SolutionErrorEventArgs(step, message));
      solvingThread.Abort();
    }

    protected void AddSolutionStep(string key, Action action, SolutionStepType type = SolutionStepType.Standard)
    {
      this.SolutionSteps.Add(key, new Tuple<Action, SolutionStepType>(action, type));
    }

    // *** METHODS ***

    /// <summary>
    /// Returns the solution for the transferred Rubik
    /// </summary>
    /// <param name="cube">Defines the Rubik to be solved</param>
    protected virtual void Solve(Rubik cube)
    {
      Rubik = cube.DeepClone();
      Algorithm = new Algorithm();
      InitStandardCube();

      GetSolution();
      RemoveUnnecessaryMoves();
    }

    protected void GetSolution()
    {
      Stopwatch sw = new Stopwatch();
      foreach (KeyValuePair<string,Tuple<Action,SolutionStepType>> step in this.SolutionSteps)
      {
        sw.Restart();
        step.Value.Item1();
        sw.Stop();
        if (OnSolutionStepCompleted != null) OnSolutionStepCompleted(this, new SolutionStepCompletedEventArgs(step.Key, false, new Algorithm() { Moves = _movesOfStep }, (int)sw.ElapsedMilliseconds,step.Value.Item2));
        _movesOfStep.Clear();
      }
    }

    protected abstract void AddSolutionSteps();

    public void TrySolveAsync(Rubik rubik)
    {
      solvingThread = new Thread(() => SolveAsync(rubik));
      solvingThread.Start();
    }

    private void SolveAsync(Rubik rubik)
    {
      bool solvable = Solvability.FullTest(rubik);
      if (solvable)
      {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Solve(rubik);
        sw.Stop();
        if (OnSolutionStepCompleted != null) OnSolutionStepCompleted(this, new SolutionStepCompletedEventArgs(this.Name, true, this.Algorithm, (int)sw.ElapsedMilliseconds));
        solvingThread.Abort();
      }
      else
      {
        this.BroadcastOnSolutionError(this.Name, "Unsolvable cube");
      }
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
        for (int i = 0; i < Algorithm.Moves.Count; i++)
        {
          IMove currentMove = Algorithm.Moves[i];
          if (i < Algorithm.Moves.Count - 1)
            if (currentMove.ReverseMove.Equals(Algorithm.Moves[i + 1]))
            {
              finished = false;
              Algorithm.Moves.RemoveAt(i + 1);
              Algorithm.Moves.RemoveAt(i);
              if (i != 0) i--;
            }

          if (i < Algorithm.Moves.Count - 2)
            if (currentMove.Equals(Algorithm.Moves[i + 1]) && currentMove.Equals(Algorithm.Moves[i + 2]))
            {
              finished = false;
              IMove reverse = Algorithm.Moves[i + 2].ReverseMove;
              Algorithm.Moves.RemoveAt(i + 1);
              Algorithm.Moves.RemoveAt(i);
              Algorithm.Moves[i] = reverse;
              if (i != 0) i--;
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
      Algorithm.Moves.Add(new LayerMove(layer, direction));
      _movesOfStep.Add(new LayerMove(layer, direction));
    }

    /// <summary>
    /// Adds a move to the solution and executes it on the Rubik
    /// </summary>
    /// <param name="move">Defines the move to be rotated</param>
    protected void SolverMove(IMove move)
    {
      Rubik.RotateLayer(move);
      Algorithm.Moves.Add(move);
      _movesOfStep.Add(move);
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
