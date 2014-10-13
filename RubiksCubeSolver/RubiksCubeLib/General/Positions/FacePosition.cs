using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{

	/// <summary>
	/// Defines the position of a face
	/// </summary>
	public enum FacePosition
	{
		None = 0,
		Top = 1,
		Bottom = 2,
		Left = 4,
		Right = 8,
		Back = 16,
		Front = 32,

		XPos = Right | Left,
		YPos = Top | Bottom,
		ZPos = Front | Back
	}
}
