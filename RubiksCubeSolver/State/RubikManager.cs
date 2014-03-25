using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Windows.Forms;

namespace VirtualRubik
{
	class RubikManager
	{
		public Rubik RubikCube;
		private Boolean Rotating;
		private double rotationStep;
		private int rotationTime; //in ms
		private Boolean rotationDirection;
		public Cube3D.RubikPosition rotationLayer;
		private int rotationTarget;
		public delegate void RotatingFinishedHandler(object sender);
		public event RotatingFinishedHandler OnRotatingFinished;


		private void BroadcastRotatingFinished()
		{
			if (OnRotatingFinished == null) return;
			OnRotatingFinished(this);
		}

		public RubikManager(Color cfront, Color cback, Color ctop, Color cbottom, Color cright, Color cleft)
		{
			RubikCube = new Rubik();
			setFaceColor(Cube3D.RubikPosition.FrontSlice, Face3D.FacePosition.Front, cfront);
			setFaceColor(Cube3D.RubikPosition.BackSlice, Face3D.FacePosition.Back, cback);
			setFaceColor(Cube3D.RubikPosition.TopLayer, Face3D.FacePosition.Top, ctop);
			setFaceColor(Cube3D.RubikPosition.BottomLayer, Face3D.FacePosition.Bottom, cbottom);
			setFaceColor(Cube3D.RubikPosition.RightSlice, Face3D.FacePosition.Right, cright);
			setFaceColor(Cube3D.RubikPosition.LeftSlice, Face3D.FacePosition.Left, cleft);
			Rotating = false;
		}

		public RubikManager() : this(Color.ForestGreen, Color.RoyalBlue, Color.White, Color.Yellow, Color.Red, Color.Orange) { }

		public RubikManager Clone()
		{
			RubikManager newRubikManager = new RubikManager();
			newRubikManager.Rotating = Rotating;
			newRubikManager.rotationLayer = rotationLayer;
			newRubikManager.rotationStep = rotationStep;
			newRubikManager.rotationTarget = rotationTarget;
			newRubikManager.RubikCube = RubikCube.Clone();
			newRubikManager.Moves = new List<LayerMove>(Moves.Select(m => m.Clone()));
			return newRubikManager;
		}

		public void Rotate90(Cube3D.RubikPosition layer, bool direction, int rotationTime)
		{
			if (!Rotating)
			{
				Rotating = true;
				rotationLayer = layer;
				this.rotationTime = rotationTime;
				if (layer == Cube3D.RubikPosition.TopLayer || layer == Cube3D.RubikPosition.LeftSlice || layer == Cube3D.RubikPosition.FrontSlice) direction = !direction;
				this.rotationDirection = direction;
				if (direction) rotationStep *= (-1);
				rotationTarget = 90;
				if (direction) rotationTarget = -90;
			}
		}

		public void Rotate90Sync(Cube3D.RubikPosition layer, bool direction)
		{
			if (!Rotating)
			{
				Rotating = true;
				if (layer == Cube3D.RubikPosition.TopLayer || layer == Cube3D.RubikPosition.LeftSlice || layer == Cube3D.RubikPosition.FrontSlice) direction = !direction;
				RubikCube.LayerRotation[layer] += (direction) ? -90 : 90;
				resetFlagsSync(layer, (direction) ? -90 : 90);
			}
		}

		public List<LayerMove> Moves = new List<LayerMove>();

		public void SolutionMove(Cube3D.RubikPosition layer, bool direction)
		{
			Rotate90Sync(layer, direction);
			if (layer == Cube3D.RubikPosition.TopLayer || layer == Cube3D.RubikPosition.LeftSlice || layer == Cube3D.RubikPosition.FrontSlice) direction = !direction;
			Moves.Add(new LayerMove(layer, direction));
		}

		public RotationInfo GetRotationInfo()
		{
			return new RotationInfo() { Direction = rotationDirection, Layer = rotationLayer, Milliseconds = rotationTime, Target = rotationTarget, Rotating = Rotating };
		}

		public void setFaceColor(Cube3D.RubikPosition affected, Face3D.FacePosition face, Color color)
		{
			RubikCube.cubes.Where(c => c.Position.HasFlag(affected)).ToList().ForEach(c => c.Faces.Where(f => f.Position == face).ToList().ForEach(f => f.Color = color));
			RubikCube.cubes.ToList().ForEach(c => { c.Colors.Clear(); c.Faces.ToList().ForEach(f => c.Colors.Add(f.Color)); });
		}

		public Color getFaceColor(Cube3D.RubikPosition position, Face3D.FacePosition face)
		{
			return RubikCube.cubes.First(c => c.Position.HasFlag(position)).Faces.First(f => f.Position == face).Color;
		}

		public void setFaceSelection(Cube3D.RubikPosition affected, Face3D.FacePosition face, Face3D.SelectionMode selection)
		{
			RubikCube.cubes.Where(c => c.Position.HasFlag(affected)).ToList().ForEach(c => c.Faces.Where(f => f.Position == face).ToList().ForEach(f =>
			{
				if (f.Selection.HasFlag(Face3D.SelectionMode.Possible))
				{
					f.Selection = selection | Face3D.SelectionMode.Possible;
				}
				else if (f.Selection.HasFlag(Face3D.SelectionMode.NotPossible))
				{
					f.Selection = selection | Face3D.SelectionMode.NotPossible;
				}
				else
				{
					f.Selection = selection;
				}
			}));
		}
		public void setFaceSelection(Face3D.SelectionMode selection)
		{
			RubikCube.cubes.ToList().ForEach(c => c.Faces.ToList().ForEach(f =>
			{
				if (f.Selection.HasFlag(Face3D.SelectionMode.Possible))
				{
					f.Selection = selection | Face3D.SelectionMode.Possible;
				}
				else if (f.Selection.HasFlag(Face3D.SelectionMode.NotPossible))
				{
					f.Selection = selection | Face3D.SelectionMode.NotPossible;
				}
				else
				{
					f.Selection = selection;
				}
			}));
		}

		public void resetFlagsSync(Cube3D.RubikPosition layer, int target)
		{
			RubikCube.LayerRotation[layer] = target;
			List<Cube3D> affected = RubikCube.cubes.Where(c => c.Position.HasFlag(layer)).ToList();
			if (layer == Cube3D.RubikPosition.LeftSlice || layer == Cube3D.RubikPosition.MiddleSlice_Sides || layer == Cube3D.RubikPosition.RightSlice)
			{
				affected.ForEach(c => c.Faces.ToList().ForEach(f => f.Rotate(Point3D.RotationType.X, target)));
			}
			if (layer == Cube3D.RubikPosition.TopLayer || layer == Cube3D.RubikPosition.MiddleLayer || layer == Cube3D.RubikPosition.BottomLayer)
			{
				affected.ForEach(c => c.Faces.ToList().ForEach(f => f.Rotate(Point3D.RotationType.Y, target)));
			}
			if (layer == Cube3D.RubikPosition.BackSlice || layer == Cube3D.RubikPosition.MiddleSlice || layer == Cube3D.RubikPosition.FrontSlice)
			{
				affected.ForEach(c => c.Faces.ToList().ForEach(f => f.Rotate(Point3D.RotationType.Z, target)));
			}
			double ed = ((double)2 / (double)3);
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					for (int k = -1; k <= 1; k++)
					{
						//Reset Flags but keep Colors
						Cube3D.RubikPosition flags = RubikCube.genSideFlags(i, j, k); ;
						Cube3D cube = RubikCube.cubes.First(c => (Math.Round(c.Faces.Sum(f => f.Edges.Sum(e => e.X)) / 24, 4) == Math.Round(i * ed, 4))
								&& (Math.Round(c.Faces.Sum(f => f.Edges.Sum(e => e.Y)) / 24, 4) == Math.Round(j * ed, 4))
								&& (Math.Round(c.Faces.Sum(f => f.Edges.Sum(e => e.Z)) / 24, 4) == Math.Round(k * ed, 4)));
						cube.Position = flags;
						cube.Faces.ToList().ForEach(f => f.MasterPosition = flags);
						cube.Faces.First(f => (Math.Round(f.Edges.Sum(e => e.X) / 4, 4) == Math.Round(i * ed, 4))
								&& (Math.Round(f.Edges.Sum(e => e.Y) / 4, 4) == Math.Round((j * ed) - (ed / 2), 4))
								&& (Math.Round(f.Edges.Sum(e => e.Z) / 4, 4) == Math.Round(k * ed, 4))).Position = Face3D.FacePosition.Top;
						cube.Faces.First(f => (Math.Round(f.Edges.Sum(e => e.X) / 4, 4) == Math.Round(i * ed, 4))
							 && (Math.Round(f.Edges.Sum(e => e.Y) / 4, 4) == Math.Round((j * ed) + (ed / 2), 4))
							 && (Math.Round(f.Edges.Sum(e => e.Z) / 4, 4) == Math.Round(k * ed, 4))).Position = Face3D.FacePosition.Bottom;
						cube.Faces.First(f => (Math.Round(f.Edges.Sum(e => e.X) / 4, 4) == Math.Round((i * ed) - (ed / 2), 4))
								&& (Math.Round(f.Edges.Sum(e => e.Y) / 4, 4) == Math.Round(j * ed, 4))
								&& (Math.Round(f.Edges.Sum(e => e.Z) / 4, 4) == Math.Round(k * ed, 4))).Position = Face3D.FacePosition.Left;
						cube.Faces.First(f => (Math.Round(f.Edges.Sum(e => e.X) / 4, 4) == Math.Round((i * ed) + (ed / 2), 4))
							 && (Math.Round(f.Edges.Sum(e => e.Y) / 4, 4) == Math.Round(j * ed, 4))
							 && (Math.Round(f.Edges.Sum(e => e.Z) / 4, 4) == Math.Round(k * ed, 4))).Position = Face3D.FacePosition.Right;
						cube.Faces.First(f => (Math.Round(f.Edges.Sum(e => e.X) / 4, 4) == Math.Round(i * ed, 4))
							 && (Math.Round(f.Edges.Sum(e => e.Y) / 4, 4) == Math.Round(j * ed, 4))
							 && (Math.Round(f.Edges.Sum(e => e.Z) / 4, 4) == Math.Round((k * ed) - (ed / 2), 4))).Position = Face3D.FacePosition.Front;
						cube.Faces.First(f => (Math.Round(f.Edges.Sum(e => e.X) / 4, 4) == Math.Round(i * ed, 4))
							 && (Math.Round(f.Edges.Sum(e => e.Y) / 4, 4) == Math.Round(j * ed, 4))
							 && (Math.Round(f.Edges.Sum(e => e.Z) / 4, 4) == Math.Round((k * ed) + (ed / 2), 4))).Position = Face3D.FacePosition.Back;
					}
				}
			}
			foreach (Cube3D.RubikPosition rp in (Cube3D.RubikPosition[])Enum.GetValues(typeof(Cube3D.RubikPosition))) RubikCube.LayerRotation[rp] = 0;
			Rotating = false;
		}

		public void resetFlags(bool fireFinished)
		{
			RubikCube.LayerRotation[rotationLayer] = rotationTarget;
			List<Cube3D> affected = RubikCube.cubes.Where(c => c.Position.HasFlag(rotationLayer)).ToList();
			if (rotationLayer == Cube3D.RubikPosition.LeftSlice || rotationLayer == Cube3D.RubikPosition.MiddleSlice_Sides || rotationLayer == Cube3D.RubikPosition.RightSlice)
			{
				affected.ForEach(c => c.Faces.ToList().ForEach(f => f.Rotate(Point3D.RotationType.X, rotationTarget)));
			}
			if (rotationLayer == Cube3D.RubikPosition.TopLayer || rotationLayer == Cube3D.RubikPosition.MiddleLayer || rotationLayer == Cube3D.RubikPosition.BottomLayer)
			{
				affected.ForEach(c => c.Faces.ToList().ForEach(f => f.Rotate(Point3D.RotationType.Y, rotationTarget)));
			}
			if (rotationLayer == Cube3D.RubikPosition.BackSlice || rotationLayer == Cube3D.RubikPosition.MiddleSlice || rotationLayer == Cube3D.RubikPosition.FrontSlice)
			{
				affected.ForEach(c => c.Faces.ToList().ForEach(f => f.Rotate(Point3D.RotationType.Z, rotationTarget)));
			}
			double ed = ((double)2 / (double)3);
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					for (int k = -1; k <= 1; k++)
					{
						//Reset Flags but keep Colors
						Cube3D.RubikPosition flags = RubikCube.genSideFlags(i, j, k); ;
						Cube3D cube = RubikCube.cubes.First(c => (Math.Round(c.Faces.Sum(f => f.Edges.Sum(e => e.X)) / 24, 4) == Math.Round(i * ed, 4))
								&& (Math.Round(c.Faces.Sum(f => f.Edges.Sum(e => e.Y)) / 24, 4) == Math.Round(j * ed, 4))
								&& (Math.Round(c.Faces.Sum(f => f.Edges.Sum(e => e.Z)) / 24, 4) == Math.Round(k * ed, 4)));
						cube.Position = flags;
						cube.Faces.ToList().ForEach(f => f.MasterPosition = flags);
						cube.Faces.First(f => (Math.Round(f.Edges.Sum(e => e.X) / 4, 4) == Math.Round(i * ed, 4))
								&& (Math.Round(f.Edges.Sum(e => e.Y) / 4, 4) == Math.Round((j * ed) - (ed / 2), 4))
								&& (Math.Round(f.Edges.Sum(e => e.Z) / 4, 4) == Math.Round(k * ed, 4))).Position = Face3D.FacePosition.Top;
						cube.Faces.First(f => (Math.Round(f.Edges.Sum(e => e.X) / 4, 4) == Math.Round(i * ed, 4))
							 && (Math.Round(f.Edges.Sum(e => e.Y) / 4, 4) == Math.Round((j * ed) + (ed / 2), 4))
							 && (Math.Round(f.Edges.Sum(e => e.Z) / 4, 4) == Math.Round(k * ed, 4))).Position = Face3D.FacePosition.Bottom;
						cube.Faces.First(f => (Math.Round(f.Edges.Sum(e => e.X) / 4, 4) == Math.Round((i * ed) - (ed / 2), 4))
								&& (Math.Round(f.Edges.Sum(e => e.Y) / 4, 4) == Math.Round(j * ed, 4))
								&& (Math.Round(f.Edges.Sum(e => e.Z) / 4, 4) == Math.Round(k * ed, 4))).Position = Face3D.FacePosition.Left;
						cube.Faces.First(f => (Math.Round(f.Edges.Sum(e => e.X) / 4, 4) == Math.Round((i * ed) + (ed / 2), 4))
							 && (Math.Round(f.Edges.Sum(e => e.Y) / 4, 4) == Math.Round(j * ed, 4))
							 && (Math.Round(f.Edges.Sum(e => e.Z) / 4, 4) == Math.Round(k * ed, 4))).Position = Face3D.FacePosition.Right;
						cube.Faces.First(f => (Math.Round(f.Edges.Sum(e => e.X) / 4, 4) == Math.Round(i * ed, 4))
							 && (Math.Round(f.Edges.Sum(e => e.Y) / 4, 4) == Math.Round(j * ed, 4))
							 && (Math.Round(f.Edges.Sum(e => e.Z) / 4, 4) == Math.Round((k * ed) - (ed / 2), 4))).Position = Face3D.FacePosition.Front;
						cube.Faces.First(f => (Math.Round(f.Edges.Sum(e => e.X) / 4, 4) == Math.Round(i * ed, 4))
							 && (Math.Round(f.Edges.Sum(e => e.Y) / 4, 4) == Math.Round(j * ed, 4))
							 && (Math.Round(f.Edges.Sum(e => e.Z) / 4, 4) == Math.Round((k * ed) + (ed / 2), 4))).Position = Face3D.FacePosition.Back;
					}
				}
			}
			foreach (Cube3D.RubikPosition rp in (Cube3D.RubikPosition[])Enum.GetValues(typeof(Cube3D.RubikPosition))) RubikCube.LayerRotation[rp] = 0;
			Rotating = false;
			if (fireFinished) BroadcastRotatingFinished();
		}

	}
}
