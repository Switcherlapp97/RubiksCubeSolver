using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RubiksCubeLib.RubiksCube;

namespace RubiksCubeLib.CubeModel
{
  [Serializable]
  public class Cube3D
  {
    public IEnumerable<Face3D> Faces { get; set; }
    public CubeFlag Position { get; set; }

    public Cube3D(Point3D location, double scale, CubeFlag position, IEnumerable<Face> faces)
    {
      Faces = UniCube.GenFaces3D(position);
      this.Position = position;
      Faces.ToList().ForEach(f =>
      {
        f.Color = faces.First(face => face.Position == f.Position).Color;
        f.Vertices.ToList().ForEach(e =>
        {
          e.X *= scale; e.Y *= scale; e.Z *= scale; //scale
          e.X += location.X; e.Y += location.Y; e.Z += location.Z; //translate
        });
      });
    }

    public Cube3D(IEnumerable<Face3D> faces, CubeFlag position)
    {
      Faces = faces;
      Position = position;
    }

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
      return new Cube3D(faces, Position);
    }

    public Cube3D Project(int viewWidth, int viewHeight, int fov, int viewDistance, double scale)
    {
      return new Cube3D(Faces.Select(f => f.Project(viewWidth, viewHeight, fov, viewDistance, scale)), Position);
    }
  }
}
