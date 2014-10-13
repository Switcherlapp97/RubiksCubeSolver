using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RubiksCubeLib.RubiksCube;

namespace RubiksCubeLib.Solver
{

	/// <summary>
	/// Represents a solution for a Rubik
	/// </summary>
	public class Solution
	{

		// **** PROPERTIES ****

		/// <summary>
		/// The scrambled Rubik which will be solved
		/// </summary>
		public Rubik StartingRubik { get; private set; }

		/// <summary>
		/// The Rubik after executing the algorithm
		/// </summary>
		public Rubik CurrentRubik { get; private set; }

		/// <summary>
		/// The algorithm for the solution
		/// </summary>
		public Algorithm Algorithm { get; private set; }

		/// <summary>
		/// Returns the amount of moves of the algorithm
		/// </summary>
		public int MovesCount { get { return Algorithm.Moves.Count; } }

		/// <summary>
		/// The name of the solving method
		/// </summary>
		public string SolvingMethod { get; private set; }



		// **** CONSTRUCTORS ****

		/// <summary>
		/// Constructor with the Solver used for this and the Rubik
		/// </summary>
		/// <param name="solver">Defines the solver to be used</param>
		/// <param name="rubik">Defines the Rubik to be solved</param>
		public Solution(CubeSolver solver, Rubik rubik)
		{
			StartingRubik = rubik.DeepClone();
			CurrentRubik = rubik;
			SolvingMethod = solver.Name;
			Algorithm = new Algorithm();
		}
	}


}
