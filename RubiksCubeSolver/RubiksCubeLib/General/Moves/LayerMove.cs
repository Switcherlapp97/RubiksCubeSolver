using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{

	public class LayerMove : IMove
	{

		// **** PROPERTIES ****

		/// <summary>
		/// Returns the name (the notation) of this LayerMove
		/// </summary>
		public string Name { get { return string.Format("{0} {1}", Layer, Direction ? "Clockwise" : "Counter-Clockwise"); } }
		
		/// <summary>
		/// Describes the layer of this LayerMove
		/// </summary>
		public CubeFlag Layer { get; set; }

		/// <summary>
		/// Describes the direction of this LayerMove
		/// </summary>
		public bool Direction { get; set; }

		/// <summary>
		/// Describes whether this LayerMove will be executed twice
		/// </summary>
		public bool Twice { get; set; }

		/// <summary>
		/// Returns true if MultipleLayers are allowed
		/// </summary>
		public bool MultipleLayers { get { return false; } }



		// **** CONSTRUCTORS ****

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="layer">Defines the layer to be moved</param>
		/// <param name="direction">Defines the direction (true == clockwise and false == counter-clockwise)</param>
		/// <param name="twice">Defines whether this layer will be turned twice or not</param>
		/// <exception cref="System.Exception">Thrown when layer contains more than one flag</exception>
		public LayerMove(CubeFlag layer, bool direction = true, bool twice = false)
		{
			if (CubeFlagService.CountFlags(layer) == 1)
			{
				this.Layer = layer;
				this.Direction = direction;
				this.Twice = twice;
			}
			else
				throw new Exception("Impossible movement");
		}



		// **** OPERATORS ****

		/// <summary>
		/// Combines two LayerMoves into a LayerMoveCollection
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static LayerMoveCollection operator &(LayerMove first, LayerMove second)
		{
			LayerMoveCollection moves = new LayerMoveCollection();
			moves.Add(first);
			moves.Add(second);
			return moves;
		}



		// **** METHODS ****

		/// <summary>
		/// Parses a notation string into a LayerMove
		/// </summary>
		/// <param name="notation">Defines to string to be parsed</param>
		/// <returns></returns>
		public static LayerMove Parse(string notation)
		{
			string layer = notation[0].ToString();
			CubeFlag rotationLayer = CubeFlag.None;
			switch (layer)
			{
				case "R":
					rotationLayer = CubeFlag.RightSlice;
					break;
				case "L":
					rotationLayer = CubeFlag.LeftSlice;
					break;
				case "U":
					rotationLayer = CubeFlag.TopLayer;
					break;
				case "D":
					rotationLayer = CubeFlag.BottomLayer;
					break;
				case "F":
					rotationLayer = CubeFlag.FrontSlice;
					break;
				case "B":
					rotationLayer = CubeFlag.BackSlice;
					break;
				case "M":
					rotationLayer = CubeFlag.MiddleSliceSides;
					break;
				case "E":
					rotationLayer = CubeFlag.MiddleLayer;
					break;
				case "S":
					rotationLayer = CubeFlag.MiddleSlice;
					break;
			}
			bool direction = notation.Length == 1;
			return new LayerMove(rotationLayer, direction);
		}


		/// <summary>
		/// Converts this LayerMove into a notation string
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string c = "'";
			string move = string.Empty;

			switch (Layer)
			{
				case CubeFlag.TopLayer:
					move = "U";
					break;
				case CubeFlag.MiddleLayer:
					move = "E";
					break;
				case CubeFlag.BottomLayer:
					move = "D";
					break;
				case CubeFlag.FrontSlice:
					move = "F";
					break;
				case CubeFlag.MiddleSlice:
					move = "S";
					break;
				case CubeFlag.BackSlice:
					move = "B";
					break;
				case CubeFlag.LeftSlice:
					move = "L";
					break;
				case CubeFlag.MiddleSliceSides:
					move = "M";
					break;
				case CubeFlag.RightSlice:
					move = "R";
					break;
			}
			if (!Direction)
				move += c;
			return move;
		}
	}
}
