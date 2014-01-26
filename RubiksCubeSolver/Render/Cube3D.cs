using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace VirtualRubik
{
  class Cube3D
  {

    public IEnumerable<Face3D> Faces;
    public List<Color> Colors = new List<Color>();

    [Flags]
    public enum RubikPosition
    {
      None = 0,
      TopLayer = 1,
      MiddleLayer = 2,
      BottomLayer = 4,
      FrontSlice = 8,
      MiddleSlice = 16,
      BackSlice = 32,
      LeftSlice = 64,
      MiddleSlice_Sides = 128,
      RightSlice = 256
    }
    public RubikPosition Position;

    public Cube3D(Point3D location, double scale, RubikPosition position)
    {
      //Stack<Color> colorStack = new Stack<Color>(colors);
      Faces = UniCube.genFaces(position);
      Faces.ToList().ForEach(f =>
      {
        //f.Color = colorStack.Pop(); //color
        f.Edges.ToList().ForEach(e =>
        {
          e.X *= scale; e.Y *= scale; e.Z *= scale; //scale
          e.X += location.X; e.Y += location.Y; e.Z += location.Z; //translate
        });
      });
      Position = position;
    }
    public Cube3D(IEnumerable<Face3D> faces, RubikPosition position)
    {
      Faces = faces;
      Position = position;
    }

    public Cube3D Clone()
    {
      Cube3D newCube = new Cube3D(new List<Face3D>(Faces.Select(f => f.Clone())), Position);
      newCube.Colors = new List<Color>(Colors);
      return newCube;
    }

    public Cube3D Rotate(Point3D.RotationType type, double angle, Point3D center)
    {
      //Deep Clone
      List<Face3D> faces = new List<Face3D>();
      foreach (Face3D f in Faces)
      {
        List<Point3D> edges = new List<Point3D>();
        foreach (Point3D p in f.Edges) { edges.Add(new Point3D(p.X, p.Y, p.Z)); }
        Face3D f2 = new Face3D(edges, f.Color, f.Position, f.MasterPosition, f.Selection);
        f2.Edges.ToList().ForEach(e => { e.X -= center.X; e.Y -= center.Y; e.Z -= center.Z; });
        f2.Rotate(type, angle);
        f2.Edges.ToList().ForEach(e => { e.X += center.X; e.Y += center.Y; e.Z += center.Z; });
        faces.Add(f2);
      }
      return new Cube3D(faces, Position);
    }

    public Cube3D Project(int viewWidth, int viewHeight, int fov, int viewDistance, double scale)
    {
      return new Cube3D(Faces.Select(f => f.Project(viewWidth, viewHeight, fov, viewDistance, scale)), Position);
    }

    public static Boolean isCorner(RubikPosition Position)
    {
      return ((Position == (RubikPosition.TopLayer | RubikPosition.FrontSlice | RubikPosition.LeftSlice))
          || (Position == (RubikPosition.TopLayer | RubikPosition.FrontSlice | RubikPosition.RightSlice))
          || (Position == (RubikPosition.TopLayer | RubikPosition.BackSlice | RubikPosition.LeftSlice))
          || (Position == (RubikPosition.TopLayer | RubikPosition.BackSlice | RubikPosition.RightSlice))
          || (Position == (RubikPosition.BottomLayer | RubikPosition.FrontSlice | RubikPosition.LeftSlice))
          || (Position == (RubikPosition.BottomLayer | RubikPosition.FrontSlice | RubikPosition.RightSlice))
          || (Position == (RubikPosition.BottomLayer | RubikPosition.BackSlice | RubikPosition.LeftSlice))
          || (Position == (RubikPosition.BottomLayer | RubikPosition.BackSlice | RubikPosition.RightSlice)));
    }

    public static Boolean isEdge(RubikPosition Position)
    {
      return ((Position == (RubikPosition.TopLayer | RubikPosition.FrontSlice | RubikPosition.MiddleSlice_Sides))
        || (Position == (RubikPosition.TopLayer | RubikPosition.BackSlice | RubikPosition.MiddleSlice_Sides))
        || (Position == (RubikPosition.TopLayer | RubikPosition.RightSlice | RubikPosition.MiddleSlice))
        || (Position == (RubikPosition.TopLayer | RubikPosition.LeftSlice | RubikPosition.MiddleSlice))
        || (Position == (RubikPosition.MiddleLayer | RubikPosition.FrontSlice | RubikPosition.RightSlice))
        || (Position == (RubikPosition.MiddleLayer | RubikPosition.FrontSlice | RubikPosition.LeftSlice))
        || (Position == (RubikPosition.MiddleLayer | RubikPosition.BackSlice | RubikPosition.RightSlice))
        || (Position == (RubikPosition.MiddleLayer | RubikPosition.BackSlice | RubikPosition.LeftSlice))
        || (Position == (RubikPosition.BottomLayer | RubikPosition.FrontSlice | RubikPosition.MiddleSlice_Sides))
        || (Position == (RubikPosition.BottomLayer | RubikPosition.BackSlice | RubikPosition.MiddleSlice_Sides))
        || (Position == (RubikPosition.BottomLayer | RubikPosition.RightSlice | RubikPosition.MiddleSlice))
        || (Position == (RubikPosition.BottomLayer | RubikPosition.LeftSlice | RubikPosition.MiddleSlice)));
    }

    public static Boolean isCenter(RubikPosition Position)
    {
      return ((Position == (Cube3D.RubikPosition.TopLayer | Cube3D.RubikPosition.MiddleSlice | Cube3D.RubikPosition.MiddleSlice_Sides))
          || (Position == (Cube3D.RubikPosition.BottomLayer | Cube3D.RubikPosition.MiddleSlice | Cube3D.RubikPosition.MiddleSlice_Sides))
          || (Position == (Cube3D.RubikPosition.LeftSlice | Cube3D.RubikPosition.MiddleSlice | Cube3D.RubikPosition.MiddleLayer))
          || (Position == (Cube3D.RubikPosition.RightSlice | Cube3D.RubikPosition.MiddleSlice | Cube3D.RubikPosition.MiddleLayer))
          || (Position == (Cube3D.RubikPosition.FrontSlice | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleLayer))
          || (Position == (Cube3D.RubikPosition.BackSlice | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleLayer)));
    }
    public static RubikPosition getCommonLayer(RubikPosition a, RubikPosition b, RubikPosition exclude)
    {
      for (int i = 0; i < 9; i++)
      {
        RubikPosition l = (Cube3D.RubikPosition)Math.Pow(2, i);
        if (a.HasFlag(l) && b.HasFlag(l) && l != exclude)
        {
          return l;
        }
      }
      return RubikPosition.None;
    }

  }

  static class UniCube
  {

    public static IEnumerable<Face3D> genFaces(Cube3D.RubikPosition masterPosition)
    {
      return new Face3D[] {
		        new Face3D(new Point3D[] { new Point3D(-1, 1, -1), new Point3D(1, 1, -1), new Point3D(1, -1, -1), new Point3D(-1, -1, -1) }, Color.Black, Face3D.FacePosition.Front, masterPosition, Face3D.SelectionMode.None),   
                new Face3D(new Point3D[] { new Point3D(-1, 1, 1), new Point3D(1, 1, 1), new Point3D(1, -1, 1), new Point3D(-1, -1, 1) }, Color.Black, Face3D.FacePosition.Back, masterPosition, Face3D.SelectionMode.None),     
                new Face3D(new Point3D[] { new Point3D(-1, -1, -1), new Point3D(1, -1, -1), new Point3D(1, -1, 1), new Point3D(-1, -1, 1) }, Color.Black, Face3D.FacePosition.Top, masterPosition, Face3D.SelectionMode.None),
                new Face3D(new Point3D[] { new Point3D(-1, 1, -1), new Point3D(1, 1, -1), new Point3D(1, 1, 1), new Point3D(-1, 1, 1) }, Color.Black, Face3D.FacePosition.Bottom, masterPosition, Face3D.SelectionMode.None),      
                new Face3D(new Point3D[] { new Point3D(1, 1, 1), new Point3D(1, 1, -1), new Point3D(1, -1, -1), new Point3D(1, -1, 1) }, Color.Black, Face3D.FacePosition.Right, masterPosition, Face3D.SelectionMode.None),     
                new Face3D(new Point3D[] { new Point3D(-1, 1, 1), new Point3D(-1, 1, -1), new Point3D(-1, -1, -1), new Point3D(-1, -1, 1) }, Color.Black, Face3D.FacePosition.Left, masterPosition, Face3D.SelectionMode.None)
		    };
    }

  }
}
