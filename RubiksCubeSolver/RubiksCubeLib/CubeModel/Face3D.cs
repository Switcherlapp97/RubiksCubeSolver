using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace RubiksCubeLib.CubeModel
{
  [Serializable]
  public class Face3D
  {
    public IEnumerable<Point3D> Vertices { get; set; }
    public Color Color { get; set; }
    public FacePosition Position { get; set; }
    public CubeFlag MasterPosition { get; set; }

    public Face3D(IEnumerable<Point3D> vertices, Color color, FacePosition position, CubeFlag masterPosition)
    {
      Vertices = vertices;
      Color = color;
      Position = position;
      MasterPosition = masterPosition;
    }

    public void Rotate(RotationType type, double angleInDeg)
    {
      this.Vertices.ToList().ForEach(v => v.Rotate(type, angleInDeg));
    }

    public Face3D Project(int viewWidth, int viewHeight, int fov, int viewDistance, double scale)
    {
      IEnumerable<Point3D> parr = Vertices.Select(v => v.Project(viewWidth, viewHeight, fov, viewDistance, scale));
      double mid = parr.Sum(v => v.Z) / parr.Count();
      parr = parr.Select(p => new Point3D(p.X, p.Y, mid));
      return new Face3D(parr, Color, Position, MasterPosition);
    }
  }
}
