using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VirtualRubik
{
	public abstract class CubeSolver
	{
		public abstract RubikManager RubikManager { get; set; }
		public abstract RubikManager StandardCube { get; set; }

		public void Solve(bool ShowMoveCount)
		{
			GetSolution();
			RemoveUnnecessaryMoves();
			if (ShowMoveCount) System.Windows.Forms.MessageBox.Show(string.Format("Cube solved with {0} moves", RubikManager.Moves.Count));
		}

		public abstract void GetSolution();

		public RubikManager ReturnRubik()
		{
			Solve(true);
			return RubikManager;
		}


		public bool CanSolve()
		{
			RubikManager oldManager = RubikManager.Clone();
			//check colors
			bool correctColors = StandardCube.RubikCube.cubes.Count(sc => RubikManager.RubikCube.cubes
					.Where(c => ScrambledEquals(c.Colors, sc.Colors)).Count() == 1) == RubikManager.RubikCube.cubes.Count();

			//return false, if there are invalid cube colors
			if (!correctColors) return false;

			Solve(false);

			//check if all the cube faces are solved
			Cube3D.RubikPosition layers = Cube3D.RubikPosition.TopLayer | Cube3D.RubikPosition.BottomLayer | Cube3D.RubikPosition.RightSlice
					| Cube3D.RubikPosition.LeftSlice | Cube3D.RubikPosition.FrontSlice | Cube3D.RubikPosition.BackSlice;
			foreach (Cube3D.RubikPosition l in GetFlags(layers))
			{
				Face3D.FacePosition facePos = CubePosToFacePos(l);
				if (facePos != Face3D.FacePosition.None)
				{
					Cube3D.RubikPosition centerPos = RubikManager.RubikCube.cubes.First(c => Cube3D.isCenter(c.Position) && c.Position.HasFlag(l)).Position;
					Color faceColor = RubikManager.getFaceColor(centerPos, facePos);

					bool faceNotSolved = RubikManager.RubikCube.cubes.Count(c => c.Position.HasFlag(l) && c.Faces.First(f => f.Position == facePos).Color == faceColor) != 9;
					if (faceNotSolved) return false;
				}
			}

			RubikManager = oldManager;
			return true;
		}

		public void RemoveUnnecessaryMoves()
		{
			for (int j = 0; j < 3; j++)
			{
				for (int i = 0; i < RubikManager.Moves.Count; i++)
				{
					if (i != RubikManager.Moves.Count - 1) if (RubikManager.Moves[i].Layer == RubikManager.Moves[i + 1].Layer && RubikManager.Moves[i].Direction != RubikManager.Moves[i + 1].Direction)
						{
							RubikManager.Moves.RemoveAt(i + 1);
							RubikManager.Moves.RemoveAt(i);
							if (i != 0) i--;
						}
					if (i < RubikManager.Moves.Count - 2) if (RubikManager.Moves[i].Layer == RubikManager.Moves[i + 1].Layer && RubikManager.Moves[i].Layer == RubikManager.Moves[i + 2].Layer
							&& RubikManager.Moves[i].Direction == RubikManager.Moves[i + 1].Direction && RubikManager.Moves[i].Direction == RubikManager.Moves[i + 2].Direction)
						{
							bool direction = !RubikManager.Moves[i + 2].Direction;
							RubikManager.Moves.RemoveAt(i + 1);
							RubikManager.Moves.RemoveAt(i);
							RubikManager.Moves[i].Direction = direction;
							if (i != 0) i--;
						}
				}
			}
		}

		#region HelperMethods
		public Cube3D.RubikPosition FacePosToCubePos(Face3D.FacePosition position)
		{
			switch (position)
			{
				case Face3D.FacePosition.Top:
					return Cube3D.RubikPosition.TopLayer;
				case Face3D.FacePosition.Bottom:
					return Cube3D.RubikPosition.BottomLayer;
				case Face3D.FacePosition.Left:
					return Cube3D.RubikPosition.LeftSlice;
				case Face3D.FacePosition.Right:
					return Cube3D.RubikPosition.RightSlice;
				case Face3D.FacePosition.Back:
					return Cube3D.RubikPosition.BackSlice;
				case Face3D.FacePosition.Front:
					return Cube3D.RubikPosition.FrontSlice;
				default:
					return Cube3D.RubikPosition.None;
			}
		}

		public Face3D.FacePosition CubePosToFacePos(Cube3D.RubikPosition position)
		{
			switch (position)
			{
				case Cube3D.RubikPosition.TopLayer:
					return Face3D.FacePosition.Top;
				case Cube3D.RubikPosition.BottomLayer:
					return Face3D.FacePosition.Bottom;
				case Cube3D.RubikPosition.FrontSlice:
					return Face3D.FacePosition.Front;
				case Cube3D.RubikPosition.BackSlice:
					return Face3D.FacePosition.Back;
				case Cube3D.RubikPosition.LeftSlice:
					return Face3D.FacePosition.Left;
				case Cube3D.RubikPosition.RightSlice:
					return Face3D.FacePosition.Right;
				default:
					return Face3D.FacePosition.None;
			}
		}

		public Face3D.FacePosition GetOppositeFace(Face3D.FacePosition position)
		{
			switch (position)
			{
				case Face3D.FacePosition.Top:
					return Face3D.FacePosition.Bottom;
				case Face3D.FacePosition.Bottom:
					return Face3D.FacePosition.Top;
				case Face3D.FacePosition.Left:
					return Face3D.FacePosition.Right;
				case Face3D.FacePosition.Right:
					return Face3D.FacePosition.Left;
				case Face3D.FacePosition.Back:
					return Face3D.FacePosition.Front;
				case Face3D.FacePosition.Front:
					return Face3D.FacePosition.Back;
				default:
					return Face3D.FacePosition.None;
			}
		}

		public int CountCornersWithCorrectOrientation(RubikManager manager)
		{
			Color topColor = manager.getFaceColor(Cube3D.RubikPosition.TopLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Top);
			return manager.RubikCube.cubes.Count(c => Cube3D.isCorner(c.Position) && c.Faces.First(f => f.Position == Face3D.FacePosition.Top).Color == topColor);
		}

		public int CountEdgesWithCorrectOrientation(RubikManager manager)
		{
			Color topColor = manager.getFaceColor(Cube3D.RubikPosition.TopLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Top);
			return manager.RubikCube.cubes.Count(c => Cube3D.isEdge(c.Position) && c.Faces.First(f => f.Position == Face3D.FacePosition.Top).Color == topColor);
		}

		public int CountTopCornersAtTargetPosition(RubikManager manager)
		{
			return manager.RubikCube.cubes.Count(c => Cube3D.isCorner(c.Position) && c.Position.HasFlag(Cube3D.RubikPosition.TopLayer) && c.Position == GetTargetPosition(c));
		}

		public int CountTopEdgesAtTargetPosition(RubikManager manager)
		{
			return manager.RubikCube.cubes.Count(c => Cube3D.isEdge(c.Position) && c.Position.HasFlag(Cube3D.RubikPosition.TopLayer) && c.Position == GetTargetPosition(c));
		}

		public Cube3D.RubikPosition GetOppositeLayer(Cube3D.RubikPosition position)
		{
			switch (position)
			{
				case Cube3D.RubikPosition.FrontSlice:
					return Cube3D.RubikPosition.BackSlice;
				case Cube3D.RubikPosition.BackSlice:
					return Cube3D.RubikPosition.FrontSlice;
				case Cube3D.RubikPosition.LeftSlice:
					return Cube3D.RubikPosition.RightSlice;
				case Cube3D.RubikPosition.RightSlice:
					return Cube3D.RubikPosition.LeftSlice;
				default:
					return Cube3D.RubikPosition.None;
			}
		}

		public Cube3D.RubikPosition RemoveFlag(Cube3D.RubikPosition oldPosition, Cube3D.RubikPosition item)
		{
			return oldPosition &= ~item;
		}

		public Cube3D.RubikPosition GetTargetPosition(Cube3D cube)
		{
			return StandardCube.RubikCube.cubes.First(cu => ScrambledEquals(cu.Colors, cube.Colors)).Position;
		}

		public Cube3D RefreshCube(Cube3D cube)
		{
			return RubikManager.RubikCube.cubes.First(cu => ScrambledEquals(cu.Colors, cube.Colors));
		}

		public Cube3D RefreshCube(Cube3D cube, RubikManager manager)
		{
			return manager.RubikCube.cubes.First(cu => ScrambledEquals(cu.Colors, cube.Colors));
		}

		public Cube3D.RubikPosition GetOppositeFace(Cube3D.RubikPosition layer)
		{
			switch (layer)
			{
				case Cube3D.RubikPosition.TopLayer:
					return Cube3D.RubikPosition.BottomLayer;
				case Cube3D.RubikPosition.BottomLayer:
					return Cube3D.RubikPosition.TopLayer;
				case Cube3D.RubikPosition.FrontSlice:
					return Cube3D.RubikPosition.BackSlice;
				case Cube3D.RubikPosition.BackSlice:
					return Cube3D.RubikPosition.FrontSlice;
				case Cube3D.RubikPosition.LeftSlice:
					return Cube3D.RubikPosition.RightSlice;
				case Cube3D.RubikPosition.RightSlice:
					return Cube3D.RubikPosition.LeftSlice;
				default:
					return Cube3D.RubikPosition.None;
			}
		}

		public void GenerateStandardCube()
		{
			StandardCube = new RubikManager();
			StandardCube.setFaceColor(Cube3D.RubikPosition.TopLayer, Face3D.FacePosition.Top,
				RubikManager.getFaceColor(Cube3D.RubikPosition.TopLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Top));

			StandardCube.setFaceColor(Cube3D.RubikPosition.BottomLayer, Face3D.FacePosition.Bottom,
				RubikManager.getFaceColor(Cube3D.RubikPosition.BottomLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Bottom));

			StandardCube.setFaceColor(Cube3D.RubikPosition.RightSlice, Face3D.FacePosition.Right,
				RubikManager.getFaceColor(Cube3D.RubikPosition.RightSlice | Cube3D.RubikPosition.MiddleSlice | Cube3D.RubikPosition.MiddleLayer, Face3D.FacePosition.Right));

			StandardCube.setFaceColor(Cube3D.RubikPosition.LeftSlice, Face3D.FacePosition.Left,
				RubikManager.getFaceColor(Cube3D.RubikPosition.LeftSlice | Cube3D.RubikPosition.MiddleSlice | Cube3D.RubikPosition.MiddleLayer, Face3D.FacePosition.Left));

			StandardCube.setFaceColor(Cube3D.RubikPosition.FrontSlice, Face3D.FacePosition.Front,
				RubikManager.getFaceColor(Cube3D.RubikPosition.MiddleLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.FrontSlice, Face3D.FacePosition.Front));

			StandardCube.setFaceColor(Cube3D.RubikPosition.BackSlice, Face3D.FacePosition.Back,
				RubikManager.getFaceColor(Cube3D.RubikPosition.MiddleLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.BackSlice, Face3D.FacePosition.Back));
		}

		public static IEnumerable<Enum> GetFlags(Enum input)
		{
			foreach (Enum value in Enum.GetValues(input.GetType()))
				if (input.HasFlag(value))
					yield return value;
		}

		public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
		{
			var cnt = new Dictionary<T, int>();
			foreach (T s in list1)
			{
				if (cnt.ContainsKey(s))
				{
					cnt[s]++;
				}
				else
				{
					cnt.Add(s, 1);
				}
			}
			foreach (T s in list2)
			{
				if (cnt.ContainsKey(s))
				{
					cnt[s]--;
				}
				else
				{
					return false;
				}
			}
			return cnt.Values.All(c => c == 0);
		}
		#endregion
	}
}
