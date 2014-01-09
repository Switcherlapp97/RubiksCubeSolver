using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace VirtualRubik
{
    class Face3D
    {

        public IEnumerable<Point3D> Edges { get; set; }
        public Color Color;
        public enum FacePosition
        {
            None = 0,
            Top = 1,
            Bottom = 2,
            Left = 4,
            Right = 8,
            Back = 16,
            Front = 32
        }
        public FacePosition Position;
        public Cube3D.RubikPosition MasterPosition;
        public enum SelectionMode
        {
            None = 0,
            Second = 1,
            First = 2,
            Possible = 4,
            NotPossible = 8
        }
        public SelectionMode Selection;

        public Face3D(IEnumerable<Point3D> edges, Color color, FacePosition position, Cube3D.RubikPosition masterPosition, SelectionMode selection)
        {
            Edges = edges;
            Color = color;
            Position = position;
            MasterPosition = masterPosition;
            Selection = selection;
        }

        public void Rotate(Point3D.RotationType type, double angle)
        {
            this.Edges.ToList().ForEach(p => p.Rotate(type, angle));
        }

        public Face3D Project(int viewWidth, int viewHeight, int fov, int viewDistance, double scale)
        {
            IEnumerable<Point3D> parr = Edges.Select(p => p.Project(viewWidth, viewHeight, fov, viewDistance, scale));
            double mid = parr.Sum(p => p.Z) / parr.Count();
            parr = parr.Select(p => new Point3D(p.X, p.Y, mid));
            return new Face3D(parr, Color, Position, MasterPosition, Selection);
        }

        public static Cube3D.RubikPosition layerAssocToFace(FacePosition Position)
        {
            if (Position == FacePosition.Top) return Cube3D.RubikPosition.TopLayer;
            if (Position == FacePosition.Bottom) return Cube3D.RubikPosition.BottomLayer;
            if (Position == FacePosition.Left) return Cube3D.RubikPosition.LeftSlice;
            if (Position == FacePosition.Right) return Cube3D.RubikPosition.RightSlice;
            if (Position == FacePosition.Front) return Cube3D.RubikPosition.FrontSlice;
            if (Position == FacePosition.Back) return Cube3D.RubikPosition.BackSlice;
            return Cube3D.RubikPosition.None;
        }
        public static FacePosition faceAssocToLayer(Cube3D.RubikPosition Position)
        {
            if (Position == Cube3D.RubikPosition.TopLayer) return FacePosition.Top;
            if (Position == Cube3D.RubikPosition.BottomLayer) return FacePosition.Bottom;
            if (Position == Cube3D.RubikPosition.LeftSlice) return FacePosition.Left;
            if (Position == Cube3D.RubikPosition.RightSlice) return FacePosition.Right;
            if (Position == Cube3D.RubikPosition.FrontSlice) return FacePosition.Front;
            if (Position == Cube3D.RubikPosition.BackSlice) return FacePosition.Back;
            return FacePosition.None;
        }

    }
}

