using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace VirtualRubik
{
    class Rubik
    {

        public List<Cube3D> cubes;
        private List<Cube3D> cubesRender;
        public double[] Rotation = { 0, 0, 0 };
        public Dictionary<Cube3D.RubikPosition, double> LayerRotation = new Dictionary<Cube3D.RubikPosition, double>();

        public Rubik()
        {
            cubesRender = new List<Cube3D>();
            cubes = new List<Cube3D>();
            double ed = ((double)2 / (double)3);
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        cubes.Add(new Cube3D(new Point3D(ed * i, ed * j, ed * k), ed / 2, genSideFlags(i, j, k)));
                    }
                }
            }
            foreach (Cube3D.RubikPosition rp in (Cube3D.RubikPosition[])Enum.GetValues(typeof(Cube3D.RubikPosition)))
            {
                LayerRotation[rp] = 0;
            }

        }

        public Cube3D.RubikPosition genSideFlags(int i, int j, int k)
        {
            Cube3D.RubikPosition rp = Cube3D.RubikPosition.None;
            switch (i)
            {
                case -1: rp |= Cube3D.RubikPosition.LeftSlice; break;
                case 0: rp |= Cube3D.RubikPosition.MiddleSlice_Sides; break;
                case 1: rp |= Cube3D.RubikPosition.RightSlice; break;
            }
            switch (j)
            {
                case -1: rp |= Cube3D.RubikPosition.TopLayer; break;
                case 0: rp |= Cube3D.RubikPosition.MiddleLayer; break;
                case 1: rp |= Cube3D.RubikPosition.BottomLayer; break;
            }
            switch (k)
            {
                case -1: rp |= Cube3D.RubikPosition.FrontSlice; break;
                case 0: rp |= Cube3D.RubikPosition.MiddleSlice; break;
                case 1: rp |= Cube3D.RubikPosition.BackSlice; break;
            }
            return rp;
        }
        public List<Cube3D> genCubesRotated(bool subsOnly)
        {
            List<Cube3D> tempCubes = new List<Cube3D>();
            foreach (Cube3D c in cubes)
            {
                Cube3D cr = c.Rotate(0, 0, new Point3D(0, 0, 0));
                double ed = ((double)2 / (double)3);
                if (cr.Position.HasFlag(Cube3D.RubikPosition.TopLayer)) cr = cr.Rotate(Point3D.RotationType.Y, LayerRotation[Cube3D.RubikPosition.TopLayer], new Point3D(0, ed, 0));
                if (cr.Position.HasFlag(Cube3D.RubikPosition.MiddleLayer)) cr = cr.Rotate(Point3D.RotationType.Y, LayerRotation[Cube3D.RubikPosition.MiddleLayer], new Point3D(0, 0, 0));
                if (cr.Position.HasFlag(Cube3D.RubikPosition.BottomLayer)) cr = cr.Rotate(Point3D.RotationType.Y, LayerRotation[Cube3D.RubikPosition.BottomLayer], new Point3D(0, -ed, 0));
                if (cr.Position.HasFlag(Cube3D.RubikPosition.FrontSlice)) cr = cr.Rotate(Point3D.RotationType.Z, LayerRotation[Cube3D.RubikPosition.FrontSlice], new Point3D(0, 0, ed));
                if (cr.Position.HasFlag(Cube3D.RubikPosition.MiddleSlice)) cr = cr.Rotate(Point3D.RotationType.Z, LayerRotation[Cube3D.RubikPosition.MiddleSlice], new Point3D(0, 0, 0));
                if (cr.Position.HasFlag(Cube3D.RubikPosition.BackSlice)) cr = cr.Rotate(Point3D.RotationType.Z, LayerRotation[Cube3D.RubikPosition.BackSlice], new Point3D(0, 0, -ed));
                if (cr.Position.HasFlag(Cube3D.RubikPosition.LeftSlice)) cr = cr.Rotate(Point3D.RotationType.X, LayerRotation[Cube3D.RubikPosition.LeftSlice], new Point3D(-ed, 0, 0));
                if (cr.Position.HasFlag(Cube3D.RubikPosition.MiddleSlice_Sides)) cr = cr.Rotate(Point3D.RotationType.X, LayerRotation[Cube3D.RubikPosition.MiddleSlice_Sides], new Point3D(0, 0, 0));
                if (cr.Position.HasFlag(Cube3D.RubikPosition.RightSlice)) cr = cr.Rotate(Point3D.RotationType.X, LayerRotation[Cube3D.RubikPosition.RightSlice], new Point3D(ed, 0, 0));
                if (!subsOnly)
                {
                    cr = cr.Rotate(Point3D.RotationType.Y, Rotation[1], new Point3D(0, 0, 0));
                    cr = cr.Rotate(Point3D.RotationType.X, Rotation[0], new Point3D(0, 0, 0));
                    cr = cr.Rotate(Point3D.RotationType.Z, Rotation[2], new Point3D(0, 0, 0));
                }
                tempCubes.Add(cr);
            }
            return tempCubes;
        }

        public RubikManager.PositionSpec Render(Graphics g, Rectangle screen, double scale, Point mousePos)
        {
            RubikManager.PositionSpec result = new RubikManager.PositionSpec() { cubePos = Cube3D.RubikPosition.None, facePos = Face3D.FacePosition.None };
            cubesRender.Clear();
            cubesRender = genCubesRotated(false);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            IEnumerable<Face3D> facesProjected = cubesRender.Select(c => c.Project(screen.Width, screen.Height, 100, 4, scale).Faces).Aggregate((a, b) => a.Concat(b));
            facesProjected = facesProjected.OrderBy(p => p.Edges.ElementAt(0).Z).ToArray();
            foreach (Face3D face in facesProjected.Reverse())
            {
                PointF[] parr = face.Edges.Select(p => new PointF((float)p.X, (float)p.Y)).ToArray();
                GraphicsPath gp = new GraphicsPath();
                gp.AddPolygon(parr);
                double fak = ((Math.Sin((double)Environment.TickCount / (double)200) + 1 ) / 4) + 0.75;
                if (gp.IsVisible(mousePos)) result = new RubikManager.PositionSpec() { cubePos = face.MasterPosition, facePos = face.Position };
                if (face.Selection.HasFlag(Face3D.SelectionMode.Second)) g.FillPolygon(new HatchBrush(HatchStyle.Percent75, Color.Black, face.Color), parr);
                else if (face.Selection.HasFlag(Face3D.SelectionMode.NotPossible)) g.FillPolygon(new SolidBrush(Color.FromArgb(face.Color.A, (int)(face.Color.R * 0.15), (int)(face.Color.G * 0.15), (int)(face.Color.B * 0.15))), parr);
                else if (face.Selection.HasFlag(Face3D.SelectionMode.First)) g.FillPolygon(new HatchBrush(HatchStyle.Percent30, Color.Black, face.Color), parr);
                else if (face.Selection.HasFlag(Face3D.SelectionMode.Possible)) g.FillPolygon(new SolidBrush(Color.FromArgb(face.Color.A, (int)(Math.Min(face.Color.R * fak, 255)), (int)(Math.Min(face.Color.G * fak, 255)), (int)(Math.Min(face.Color.B * fak, 255)))), parr);
                else g.FillPolygon(new SolidBrush(face.Color), parr);
                g.DrawPolygon(Pens.Black, parr);
            }
            return result;
        }

    }
}

