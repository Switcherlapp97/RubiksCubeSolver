using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.CubeModel
{

	/// <summary>
	/// Represents the class that contains event data for the SelectionChanged event
	/// </summary>
	public class SelectionChangedEventArgs : EventArgs
	{

		// *** CONSTRUCTOR ***

		/// <summary>
		/// Initializes a new instance of the SelectionChangedEventArgs class
		/// </summary>
		/// <param name="cubePos">Position of the parent cube</param>
		/// <param name="facePos">Position of the face</param>
		public SelectionChangedEventArgs(CubeFlag cubePos, FacePosition facePos)
		{
			this.Position = new PositionSpec() { CubePosition = cubePos, FacePosition = facePos };
		}



		// *** PROPERTIES ***

		/// <summary>
		/// Gets the face position and the position of the parent cube of a face
		/// </summary>
		public PositionSpec Position { get; private set; }

	}
}
