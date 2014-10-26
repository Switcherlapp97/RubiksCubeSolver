using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
	/// <summary>
	/// Represents a collection of layermoves
	/// </summary>
	public class Algorithm
	{

		// *** CONSTRUCTORS ***

		/// <summary>
		/// Constructor
		/// </summary>
		public Algorithm()
		{
			this.Moves = new List<IMove>();
		}

		/// <summary>
		/// Constructor with a notation string, but splitted into two parameters for string.Format()
		/// </summary>
		/// <param name="format">Defines the format pattern of string.Format()</param>
		/// <param name="args">Defines the arguments of string.Format()</param>
		public Algorithm(string format, params object[] args) : this(string.Format(format, args)) { }

		/// <summary>
		/// Converts the notation of an algorithm in a collection of layer moves
		/// </summary>
		/// <param name="algorithm">Notation: separator = " " (space); counter-clockwise = any character (', i)</param>
		/// 
		public Algorithm(string algorithm)
		{
			this.Moves = new List<IMove>();
			foreach (string s in algorithm.Split(' '))
			{
        LayerMove move;
        LayerMoveCollection collection;
        if (LayerMove.TryParse(s, out move))
        {
          this.Moves.Add(move);
        }
        else if (LayerMoveCollection.TryParse(s, out collection))
        {
          this.Moves.Add(collection);
        }
        else throw new Exception("Invalid notation");
			}
		}




		// *** PROPERTIES ***

		/// <summary>
		/// Gets or sets the the collection of layer moves
		/// </summary>
		public List<IMove> Moves { get; set; }




		// *** METHODS ***

		/// <summary>
		/// Converts the collection into a notation
		/// </summary>
		public override string ToString()
		{
			return string.Join(" ", this.Moves);
		}

    /// <summary>
    /// Transforms the algorithm
    /// </summary>
    /// <param name="rotationLayer">Transformation layer</param>
    /// <returns>Transformed algorithm</returns>
    public Algorithm Transform(CubeFlag rotationLayer)
    {
      Algorithm newAlgorithm = new Algorithm();
      for (int i = 0; i < Moves.Count; i++)
      {
        newAlgorithm.Moves.Add(Moves[i].Transform(rotationLayer));
      }
      return newAlgorithm;
    }

	}
}
