using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using RubiksCubeLib.RubiksCube;
using RubiksCubeLib.CubeModel;

namespace RubiksCubeLib
{

	/// <summary>
	/// Represents the standard cube in solved form
	/// </summary>
	public static class UniCube
	{

		/// <summary>
		/// Returns a collection with six entries containing the six faces with a black color
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<Face> GenFaces()
		{
			return new Face[] {
        new Face(Color.Black, FacePosition.Front),
        new Face(Color.Black, FacePosition.Back),
        new Face(Color.Black, FacePosition.Top),
        new Face(Color.Black, FacePosition.Bottom),
        new Face(Color.Black, FacePosition.Right),
        new Face(Color.Black, FacePosition.Left)
      };
		}

		/// <summary>
		/// Returns a collection with six entries containing the six Faces3D with a black color
		/// </summary>
		/// <param name="masterPosition">Defines the master position of the Faces3D</param>
		/// <returns></returns>
		public static IEnumerable<Face3D> GenFaces3D(CubeFlag masterPosition)
		{
			return new Face3D[] {
        new Face3D(new Point3D[] { new Point3D(-1, 1, -1), new Point3D(1, 1, -1), new Point3D(1, -1, -1), new Point3D(-1, -1, -1) }, Color.Black, FacePosition.Front, masterPosition),   
                new Face3D(new Point3D[] { new Point3D(-1, 1, 1), new Point3D(1, 1, 1), new Point3D(1, -1, 1), new Point3D(-1, -1, 1) }, Color.Black, FacePosition.Back, masterPosition),     
                new Face3D(new Point3D[] { new Point3D(-1, -1, -1), new Point3D(1, -1, -1), new Point3D(1, -1, 1), new Point3D(-1, -1, 1) }, Color.Black, FacePosition.Top, masterPosition),
                new Face3D(new Point3D[] { new Point3D(-1, 1, -1), new Point3D(1, 1, -1), new Point3D(1, 1, 1), new Point3D(-1, 1, 1) }, Color.Black, FacePosition.Bottom, masterPosition),      
                new Face3D(new Point3D[] { new Point3D(1, 1, 1), new Point3D(1, 1, -1), new Point3D(1, -1, -1), new Point3D(1, -1, 1) }, Color.Black, FacePosition.Right, masterPosition),     
                new Face3D(new Point3D[] { new Point3D(-1, 1, 1), new Point3D(-1, 1, -1), new Point3D(-1, -1, -1), new Point3D(-1, -1, 1) }, Color.Black, FacePosition.Left, masterPosition)
      };
		}

	}
}
