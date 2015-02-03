using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using RubiksCubeLib.RubiksCube;

namespace RubiksCubeLib.CubeModel
{

	/// <summary>
	/// Represents a 3D face
	/// </summary>
	[Serializable]
	public class Face3D
	{

		// *** CONSTRUCTOR ***

		/// <summary>
		/// Initializes a new instance of the Face3D class
		/// </summary>
		/// <param name="vertices">Vertices of the 3D face</param>
		/// <param name="color">Color</param>
		/// <param name="position">Position</param>
		/// <param name="masterPosition">Position of the parent 3D cube</param>
		public Face3D(IEnumerable<Point3D> vertices, Color color, FacePosition position, CubeFlag masterPosition)
		{
			this.Vertices = vertices;
			this.Color = color;
			this.Position = position;
			this.MasterPosition = masterPosition;
		}




		// *** PROPERTIES ***

		/// <summary>
		/// Gets the the 3D vertices of the 3D face
		/// </summary>
		public IEnumerable<Point3D> Vertices { get; private set; }

		/// <summary>
		/// Gets or sets the color of the face
		/// </summary>
		public Color Color { get; set; }

		/// <summary>
		/// Gets or sets the position of the face
		/// </summary>
		public FacePosition Position { get; set; }

		/// <summary>
		/// Gets the position of the parent 3D cube
		/// </summary>
		public CubeFlag MasterPosition { get; private set; }




		// *** METHODS ***

		/// <summary>
		/// Rotates the point around a particular axis
		/// </summary>
		/// <param name="type">Rotation axis</param>
		/// <param name="angleInDeg">Angle to be rotated</param>
    public void Rotate(RotationType type, double angleInDeg)
    {
      this.Vertices.ToList().ForEach(v => v.Rotate(type, angleInDeg));
    }


		/// <summary>
		/// Projects the 3D face to 2D view
		/// </summary>
		/// <param name="viewWidth">Width of projection screen</param>
		/// <param name="viewHeight">Height of projection screen</param>
		/// <param name="fov">Factor</param>
		/// <param name="viewDistance">View distance to face</param>
		/// <param name="scale">Scale</param>
		/// <returns>Projected face</returns>
		public Face3D Project(int viewWidth, int viewHeight, int fov, int viewDistance, double scale)
		{
			IEnumerable<Point3D> parr = this.Vertices.Select(v => v.Project(viewWidth, viewHeight, fov, viewDistance, scale));
			double mid = parr.Average(v => v.Z);
			parr = parr.Select(p => new Point3D(p.X, p.Y, mid));
			return new Face3D(parr, this.Color, this.Position, this.MasterPosition);
		}

    public Point3D GetCenter()
    {
      return new Point3D(this.Vertices.Average(v => v.X), this.Vertices.Average(v => v.Y), this.Vertices.Average(v => v.Z));
    }

	}
}
