using System;
using System.Drawing;

namespace RubiksCubeLib.RubiksCube
{

	/// <summary>
	/// Represents a face of a Rubik
	/// </summary>
	[Serializable]
	public class Face
	{


		// **** CONSTRUCTORS ****

		/// <summary>
		/// Empty constructor (black color, no position)
		/// </summary>
		public Face() : this(Color.Black, FacePosition.None) { }

		/// <summary>
		/// Constructor with color and position
		/// </summary>
		/// <param name="color">Defines the color of this face</param>
		/// <param name="position">Defines the position of this face</param>
		public Face(Color color, FacePosition position)
		{
			this.Color = color;
			this.Position = position;
		}




		// **** PROPERTIES ****

		/// <summary>
		/// The color of this face
		/// </summary>
		public Color Color { get; set; }

		/// <summary>
		/// The position of this face
		/// </summary>
		public FacePosition Position { get; set; }


	}
}
