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
		// **** FIELDS ****

		//The collection
		public List<LayerMove> Moves { get; set; }



		// **** CONSTRUCTORS ****

		/// <summary>
		/// Constructor
		/// </summary>
		public Algorithm()
		{
			Moves = new List<LayerMove>();
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
			Moves = new List<LayerMove>();
			foreach (string s in algorithm.Split((char.Parse(" "))))
			{
				Moves.Add(LayerMove.Parse(s));
			}
		}



		// **** METHODS ****

		/// <summary>
		/// Converts the collection into a notation
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Join(" ", this.Moves);
		}
	}
}
