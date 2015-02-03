using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RubiksCubeLib.RubiksCube;

namespace RubiksCubeLib.CubeModel
{

	/// <summary>
	/// Represents a 3D cube
	/// </summary>
	[Serializable]
	public class Cube3D
	{

		// *** CONSTRUCTOR ***

		/// <summary>
		/// Initializes a new instance of the Cube3D class
		/// </summary>
		/// <param name="location">Center point</param>
		/// <param name="scale">Scale</param>
		/// <param name="position">Position</param>
		/// <param name="faces">Faces</param>
		public Cube3D(Point3D location, double scale, CubeFlag position, IEnumerable<Face> faces)
		{
			this.Faces = UniCube.GenFaces3D(position);
			this.Position = position;
      this.Location = location;
      this.Scale = scale;
			this.Faces.ToList().ForEach(f =>
			{
				f.Color = faces.First(face => face.Position == f.Position).Color;
				f.Vertices.ToList().ForEach(e =>
				{
					e.X *= scale;
					e.Y *= scale;
					e.Z *= scale; //scale
					e.X += location.X;
					e.Y += location.Y;
					e.Z += location.Z; //translate
				});
			});
		}

		/// <summary>
		/// Initializes a new instance of the Cube3D class
		/// </summary>
		/// <param name="faces">Faces</param>
		/// <param name="position">Position</param>
		public Cube3D(IEnumerable<Face3D> faces, CubeFlag position, Point3D location, double scale)
		{
			this.Faces = faces;
			this.Position = position;
      this.Location = location;
      this.Scale = scale;
		}




		// *** PROPERTIES ***
		
		/// <summary>
		/// Gets the faces of the 3D cube
		/// </summary>
		public IEnumerable<Face3D> Faces { get; set; }

		/// <summary>
		/// Gets the position of the 3D cube
		/// </summary>
		public CubeFlag Position { get; set; }
    public Point3D Location { get; private set; }
    public double Scale { get; private set; }




		// *** METHODS ***

		/// <summary>
		/// Rotates the point around a particular axis
		/// </summary>
		/// <param name="type">Rotation axis</param>
		/// <param name="angle">Angle to be rotated</param>
		/// <param name="center">Center point of rotation</param>
    public Cube3D Rotate(RotationType type, double angle, Point3D center)
    {
      //Deep Clone
      List<Face3D> faces = new List<Face3D>();
      foreach (Face3D f in Faces)
      {
        List<Point3D> edges = new List<Point3D>();
        foreach (Point3D p in f.Vertices) { edges.Add(new Point3D(p.X, p.Y, p.Z)); }
        Face3D f2 = new Face3D(edges, f.Color, f.Position, f.MasterPosition);
        f2.Vertices.ToList().ForEach(e => { e.X -= center.X; e.Y -= center.Y; e.Z -= center.Z; });
        f2.Rotate(type, angle);
        f2.Vertices.ToList().ForEach(e => { e.X += center.X; e.Y += center.Y; e.Z += center.Z; });
        faces.Add(f2);
      }
      return new Cube3D(faces, this.Position, this.Location,this.Scale);
    }

		/// <summary>
		/// Projects the 3D cube to 2D view
		/// </summary>
		/// <param name="viewWidth">Width of projection screen</param>
		/// <param name="viewHeight">Height of projection screen</param>
		/// <param name="fov">Factor</param>
		/// <param name="viewDistance">View distance to cube</param>
		/// <param name="scale">Scale</param>
		/// <returns>Projected cube</returns>
		public Cube3D Project(int viewWidth, int viewHeight, int fov, int viewDistance, double scale)
		{
			return new Cube3D(this.Faces.Select(f => f.Project(viewWidth, viewHeight, fov, viewDistance, scale)), this.Position, this.Location,this.Scale);
		}

	}
}
