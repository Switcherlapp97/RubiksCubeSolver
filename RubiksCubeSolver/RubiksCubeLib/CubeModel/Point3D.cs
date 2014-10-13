using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RubiksCubeLib.CubeModel
{
  /// <summary>
  /// Represents a 3D point
  /// </summary>
  [Serializable]
  public class Point3D
  {
    /// <summary>
    /// Gets or sets the X coordinate of the 3D point
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the 3D point
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Gets or sets the Z coordinate of the 3D point
    /// </summary>
    public double Z { get; set; }

    /// <summary>
    /// Initializes a new instance of the Point3D class
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <param name="z">Z coordinate</param>
    public Point3D(double x, double y, double z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    /// <summary>
    /// Rotates the point around a particular axis
    /// </summary>
    /// <param name="type">Rotation axis</param>
    /// <param name="angleInDeg">Angle to be rotated</param>
    public void Rotate(RotationType type, double angleInDeg)
    {
      // Rotation matrix: http://de.wikipedia.org/wiki/Drehmatrix
      double rad = angleInDeg * Math.PI / 180;
      double cosa = Math.Cos(rad);
      double sina = Math.Sin(rad);

      Point3D old = new Point3D(X, Y, Z);

      switch (type)
      {
        case RotationType.X:
          Y = old.Y * cosa - old.Z * sina;
          Z = old.Y * sina + old.Z * cosa;
          break;
        case RotationType.Y:
          X = old.Z * sina + old.X * cosa;
          Z = old.Z * cosa - old.X * sina;
          break;
        case RotationType.Z:
          X = old.X * cosa - old.Y * sina;
          Y = old.X * sina + old.Y * cosa;
          break;
      }
    }

    /// <summary>
    /// Projects the 3D point to 2D view
    /// </summary>
    /// <param name="viewWidth">Width of projection screen</param>
    /// <param name="viewHeight">Height of projection screen</param>
    /// <param name="fov">Factor</param>
    /// <param name="viewDistance">View distance to point</param>
    /// <param name="scale">Scale</param>
    /// <returns>Projected point</returns>
    public Point3D Project(int viewWidth, int viewHeight, int fov, int viewDistance, double scale)
    {
      double factor = fov / (viewDistance + this.Z) * scale;
      double Xn = this.X * factor + viewWidth / 2;
      double Yn = this.Y * factor + viewHeight / 2;
      return new Point3D(Xn, Yn, this.Z);
    }
  }
}
