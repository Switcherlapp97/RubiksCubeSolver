using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VirtualRubik
{
	public class CubeSolverBeginner : CubeSolver
	{
		public override RubikManager RubikManager { get; set; }
		public override RubikManager StandardCube { get; set; }

		public CubeSolverBeginner(RubikManager rubik)
		{
			RubikManager = rubik.Clone();
			GenerateStandardCube();
		}

		public override void GetSolution()
		{
			SolveFirstCross();
			CompleteFirstLayer();
			CompleteMiddleLayer();
			SolveCrossTopLayer();
			CompleteLastLayer();
		}

		//Solve the first cross on the bottom layer
		private void SolveFirstCross()
		{
			//Step 1: Get color of the bottom layer
			Color bottomColor = RubikManager.getFaceColor(Cube3D.RubikPosition.BottomLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Bottom);

			//Step 2: Get the edges with target position on the bottom layer
			IEnumerable<Cube3D> bottomEdges = RubikManager.RubikCube.cubes.Where(c => Cube3D.isEdge(c.Position) && GetTargetPosition(c).HasFlag(Cube3D.RubikPosition.BottomLayer));

			//Step 3: Rotate a correct orientated edge of the bottom layer to  target position
			IEnumerable<Cube3D> solvedBottomEdges = bottomEdges.Where(bE => bE.Position == GetTargetPosition(bE) && bE.Faces.First(f => f.Color == bottomColor).Position == Face3D.FacePosition.Bottom);
			if (bottomEdges.Count(bE => bE.Position.HasFlag(Cube3D.RubikPosition.BottomLayer) && bE.Faces.First(f => f.Color == bottomColor).Position == Face3D.FacePosition.Bottom) > 0)
			{
				while (solvedBottomEdges.Count() < 1)
				{
					RubikManager.SolutionMove(Cube3D.RubikPosition.BottomLayer, true);
					solvedBottomEdges = bottomEdges.Where(bE => RefreshCube(bE).Position == GetTargetPosition(RefreshCube(bE)) && RefreshCube(bE).Faces.First(f => f.Color == bottomColor).Position == Face3D.FacePosition.Bottom);
				}
			}

			//Step 4: Solve incorrect edges of the bottom layer
			while (solvedBottomEdges.Count() < 4)
			{
				IEnumerable<Cube3D> unsolvedBottomEdges = bottomEdges.Except(solvedBottomEdges);
				Cube3D e = (unsolvedBottomEdges.FirstOrDefault(c => c.Position.HasFlag(Cube3D.RubikPosition.TopLayer)) != null)
						? unsolvedBottomEdges.FirstOrDefault(c => c.Position.HasFlag(Cube3D.RubikPosition.TopLayer)) : unsolvedBottomEdges.First();
				Color secondColor = e.Colors.First(co => co != bottomColor && co != Color.Black);

				if (e.Position != GetTargetPosition(e))
				{
					//Rotate to top layer
					Cube3D.RubikPosition layer = FacePosToCubePos(e.Faces.First(f => (f.Color == bottomColor || f.Color == secondColor)
						&& f.Position != Face3D.FacePosition.Top && f.Position != Face3D.FacePosition.Bottom).Position);

					Cube3D.RubikPosition targetLayer = FacePosToCubePos(StandardCube.RubikCube.cubes.First(cu => ScrambledEquals(cu.Colors, e.Colors))
						.Faces.First(f => f.Color == secondColor).Position);

					if (e.Position.HasFlag(Cube3D.RubikPosition.MiddleLayer))
					{
						if (layer == targetLayer)
						{
							while (!RefreshCube(e).Position.HasFlag(Cube3D.RubikPosition.BottomLayer)) RubikManager.SolutionMove(layer, true);
						}
						else
						{
							RubikManager.SolutionMove(layer, true);
							if (RefreshCube(e).Position.HasFlag(Cube3D.RubikPosition.TopLayer))
							{
								RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
								RubikManager.SolutionMove(layer, false);
							}
							else
							{
								for (int i = 0; i < 2; i++) RubikManager.SolutionMove(layer, true);
								RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
								RubikManager.SolutionMove(layer, true);
							}
						}
					}
					if (e.Position.HasFlag(Cube3D.RubikPosition.BottomLayer)) for (int i = 0; i < 2; i++) RubikManager.SolutionMove(layer, true);

					//Rotate over target position
					while (!RefreshCube(e).Position.HasFlag(targetLayer)) RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);

					//Rotate to target position
					for (int i = 0; i < 2; i++) RubikManager.SolutionMove(targetLayer, true);
				}

				//Flip the incorrect orientated edges with the algorithm: Fi D Ri Di
				if (e.Faces.First(f => f.Position == Face3D.FacePosition.Bottom).Color != bottomColor)
				{
					Cube3D.RubikPosition frontLayer = FacePosToCubePos(RefreshCube(e).Faces.First(f => f.Color == bottomColor).Position);
					RubikManager.SolutionMove(frontLayer, false);
					RubikManager.SolutionMove(Cube3D.RubikPosition.BottomLayer, true);

					Cube3D.RubikPosition rightSlice = FacePosToCubePos(RefreshCube(e).Faces.First(f => f.Color == secondColor).Position);

					RubikManager.SolutionMove(rightSlice, false);
					RubikManager.SolutionMove(Cube3D.RubikPosition.BottomLayer, false);
				}
				solvedBottomEdges = bottomEdges.Where(bE => RefreshCube(bE).Position == GetTargetPosition(RefreshCube(bE)) && RefreshCube(bE).Faces.First(f => f.Color == bottomColor).Position == Face3D.FacePosition.Bottom);
			}
		}

		private void CompleteFirstLayer()
		{
			//Step 1: Get the color of the bottom layer
			Color bottomColor = RubikManager.getFaceColor(Cube3D.RubikPosition.BottomLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Bottom);

			//Step 2: Get the corners with target position on bottom layer
			IEnumerable<Cube3D> bottomCorners = RubikManager.RubikCube.cubes.Where(c => Cube3D.isCorner(c.Position) && GetTargetPosition(c).HasFlag(Cube3D.RubikPosition.BottomLayer));
			IEnumerable<Cube3D> solvedBottomCorners = bottomCorners.Where(bC => bC.Position == GetTargetPosition(bC) && bC.Faces.First(f => f.Color == bottomColor).Position == Face3D.FacePosition.Bottom);

			//Step 3: Solve incorrect edges
			while (solvedBottomCorners.Count() < 4)
			{
				IEnumerable<Cube3D> unsolvedBottomCorners = bottomCorners.Except(solvedBottomCorners);
				Cube3D c = (unsolvedBottomCorners.FirstOrDefault(bC => bC.Position.HasFlag(Cube3D.RubikPosition.TopLayer)) != null)
					? unsolvedBottomCorners.FirstOrDefault(bC => bC.Position.HasFlag(Cube3D.RubikPosition.TopLayer)) : unsolvedBottomCorners.First();

				if (c.Position != GetTargetPosition(c))
				{
					//Rotate to top layer
					if (c.Position.HasFlag(Cube3D.RubikPosition.BottomLayer))
					{
						Face3D leftFace = RefreshCube(c).Faces.First(f => f.Position != Face3D.FacePosition.Bottom && f.Color != Color.Black);
						Cube3D.RubikPosition leftSlice = FacePosToCubePos(leftFace.Position);
						RubikManager.SolutionMove(leftSlice, false);
						if (RefreshCube(c).Position.HasFlag(Cube3D.RubikPosition.BottomLayer))
						{
							RubikManager.SolutionMove(leftSlice, true);
							leftFace = RefreshCube(c).Faces.First(f => f.Position != Face3D.FacePosition.Bottom && f.Color != leftFace.Color && f.Color != Color.Black);
							leftSlice = FacePosToCubePos(leftFace.Position);
							RubikManager.SolutionMove(leftSlice, false);
						}
						RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
						RubikManager.SolutionMove(leftSlice, true);
						RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
					}

					//Rotate over target position
					Cube3D.RubikPosition targetPos = Cube3D.RubikPosition.None;
					foreach (Cube3D.RubikPosition p in GetFlags(GetTargetPosition(c)))
					{
						if (p != Cube3D.RubikPosition.BottomLayer)
							targetPos |= p;
					}

					while (!RefreshCube(c).Position.HasFlag(targetPos)) RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
				}

				//Rotate to target position with the algorithm: Li Ui L U
				Face3D leftFac = RefreshCube(c).Faces.First(f => f.Position != Face3D.FacePosition.Top && f.Position != Face3D.FacePosition.Bottom && f.Color != Color.Black);

				Cube3D.RubikPosition leftSlic = FacePosToCubePos(leftFac.Position);

				RubikManager.SolutionMove(leftSlic, false);
				if (!RefreshCube(c).Position.HasFlag(Cube3D.RubikPosition.TopLayer))
				{
					RubikManager.SolutionMove(leftSlic, true);
					leftFac = RefreshCube(c).Faces.First(f => f.Position != Face3D.FacePosition.Top && f.Position != Face3D.FacePosition.Bottom && f.Color != leftFac.Color && f.Color != Color.Black);
					leftSlic = FacePosToCubePos(leftFac.Position);
				}
				else RubikManager.SolutionMove(leftSlic, true);

				while (RefreshCube(c).Faces.First(f => f.Color == bottomColor).Position != Face3D.FacePosition.Bottom)
				{
					if (RefreshCube(c).Faces.First(f => f.Color == bottomColor).Position == Face3D.FacePosition.Top)
					{
						RubikManager.SolutionMove(leftSlic, false);
						RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
						RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
						RubikManager.SolutionMove(leftSlic, true);
						RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
					}
					else
					{
						Face3D frontFac = RefreshCube(c).Faces.First(f => f.Position != Face3D.FacePosition.Top && f.Position != Face3D.FacePosition.Bottom
							&& f.Color != Color.Black && f.Position != CubePosToFacePos(leftSlic));

						if (RefreshCube(c).Faces.First(f => f.Color == bottomColor).Position == frontFac.Position && !RefreshCube(c).Position.HasFlag(Cube3D.RubikPosition.BottomLayer))
						{
							RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
							RubikManager.SolutionMove(leftSlic, false);
							RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
							RubikManager.SolutionMove(leftSlic, true);
						}
						else
						{
							RubikManager.SolutionMove(leftSlic, false);
							RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
							RubikManager.SolutionMove(leftSlic, true);
							RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
						}
					}
				}
				solvedBottomCorners = bottomCorners.Where(bC => RefreshCube(bC).Position == GetTargetPosition(bC) && RefreshCube(bC).Faces.First(f => f.Color == bottomColor).Position == Face3D.FacePosition.Bottom);
			}
		}

		private void CompleteMiddleLayer()
		{
			//Step 1: Get the color of the bottom and top layer
			Color bottomColor = RubikManager.getFaceColor(Cube3D.RubikPosition.BottomLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Bottom);
			Color topColor = RubikManager.getFaceColor(Cube3D.RubikPosition.TopLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Top);

			//Step 2: Get the egdes of the middle layer
			IEnumerable<Cube3D> middleEdges = RubikManager.RubikCube.cubes.Where(c => Cube3D.isEdge(c.Position)).Where(c => c.Colors.Count(co => co == bottomColor || co == topColor) == 0);

			List<Face3D> coloredFaces = new List<Face3D>();
			RubikManager.RubikCube.cubes.Where(cu => Cube3D.isCenter(cu.Position)).ToList().ForEach(cu => coloredFaces.Add(cu.Faces.First(f => f.Color != Color.Black).Clone()));
			IEnumerable<Cube3D> solvedMiddleEdges = middleEdges.Where(mE => mE.Faces.Count(f => coloredFaces.Count(cf => cf.Color == f.Color && cf.Position == f.Position) == 1) == 2);

			while (solvedMiddleEdges.Count() < 4)
			{
				IEnumerable<Cube3D> unsolvedMiddleEdges = middleEdges.Except(solvedMiddleEdges);
				Cube3D c = (unsolvedMiddleEdges.FirstOrDefault(cu => !cu.Position.HasFlag(Cube3D.RubikPosition.MiddleLayer)) != null)
					? unsolvedMiddleEdges.FirstOrDefault(cu => !cu.Position.HasFlag(Cube3D.RubikPosition.MiddleLayer)) : unsolvedMiddleEdges.First();

				//Rotate to top layer
				if (!c.Position.HasFlag(Cube3D.RubikPosition.TopLayer))
				{
					Face3D frontFace = c.Faces.First(f => f.Color != Color.Black);
					Cube3D.RubikPosition frontSlice = FacePosToCubePos(frontFace.Position);
					Face3D face = c.Faces.First(f => f.Color != Color.Black && f.Color != frontFace.Color);
					Cube3D.RubikPosition slice = FacePosToCubePos(face.Position);

					RubikManager.SolutionMove(slice, true);
					if (RefreshCube(c).Position.HasFlag(Cube3D.RubikPosition.TopLayer))
					{
						RubikManager.SolutionMove(slice, false);
						//Algorithm to the right: U R Ui Ri Ui Fi U F
						RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
						RubikManager.SolutionMove(slice, true);
						RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
						RubikManager.SolutionMove(slice, false);
						RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
						RubikManager.SolutionMove(frontSlice, false);
						RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
						RubikManager.SolutionMove(frontSlice, true);
					}
					else
					{
						RubikManager.SolutionMove(slice, false);
						//Algorithm to the left: Ui Li U L U F Ui Fi
						RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
						RubikManager.SolutionMove(slice, false);
						RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
						RubikManager.SolutionMove(slice, true);
						RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
						RubikManager.SolutionMove(frontSlice, true);
						RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
						RubikManager.SolutionMove(frontSlice, false);
					}
				}

				//Rotate to start position for the algorithm
				IEnumerable<Cube3D> middles = RubikManager.RubikCube.cubes.Where(cu => Cube3D.isCenter(cu.Position)).Where(m => m.Colors.First(co => co != Color.Black)
						== RefreshCube(c).Faces.First(f => f.Color != Color.Black && f.Position != Face3D.FacePosition.Top).Color &&
						RemoveFlag(m.Position, Cube3D.RubikPosition.MiddleLayer) == RemoveFlag(RefreshCube(c).Position, Cube3D.RubikPosition.TopLayer));

				while (middles.Count() < 1)
				{
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
					middles = RubikManager.RubikCube.cubes.Where(cu => Cube3D.isCenter(cu.Position)).Where(m => m.Colors.First(co => co != Color.Black)
						== RefreshCube(c).Faces.First(f => f.Color != Color.Black && f.Position != Face3D.FacePosition.Top).Color &&
						RemoveFlag(m.Position, Cube3D.RubikPosition.MiddleLayer) == RemoveFlag(RefreshCube(c).Position, Cube3D.RubikPosition.TopLayer));
				}

				//Rotate to target position
				Face3D frontFac = RefreshCube(c).Faces.First(f => f.Color != Color.Black && f.Position != Face3D.FacePosition.Top);
				Cube3D.RubikPosition frontSlic = FacePosToCubePos(frontFac.Position);
				Cube3D.RubikPosition slic = Cube3D.RubikPosition.None;
				foreach (Cube3D.RubikPosition p in GetFlags(GetTargetPosition(c)))
				{
					if (p != Cube3D.RubikPosition.MiddleLayer && p != frontSlic)
						slic |= p;
				}

				RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
				if (!RefreshCube(c).Position.HasFlag(slic))
				{
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
					//Algorithm to the right: U R Ui Ri Ui Fi U F
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
					RubikManager.SolutionMove(slic, true);
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
					RubikManager.SolutionMove(slic, false);
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
					RubikManager.SolutionMove(frontSlic, false);
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
					RubikManager.SolutionMove(frontSlic, true);
				}
				else
				{
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
					//Algorithm to the left: Ui Li U L U F Ui Fi
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
					RubikManager.SolutionMove(slic, false);
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
					RubikManager.SolutionMove(slic, true);
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
					RubikManager.SolutionMove(frontSlic, true);
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
					RubikManager.SolutionMove(frontSlic, false);
				}
				solvedMiddleEdges = middleEdges.Where(mE => RefreshCube(mE).Faces.Count(f => coloredFaces.Count(cf => cf.Color == f.Color && cf.Position == f.Position) == 1) == 2);
			}
		}

		private void SolveCrossTopLayer()
		{
			//Step 1: Get the color of the top layer to start with cross on the last layer
			Color topColor = RubikManager.getFaceColor(Cube3D.RubikPosition.TopLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Top);

			//Step 2: Get edges with the color of the top face
			IEnumerable<Cube3D> topEdges = RubikManager.RubikCube.cubes.Where(c => Cube3D.isEdge(c.Position)).Where(c => c.Position.HasFlag(Cube3D.RubikPosition.TopLayer));

			//Check if the cube is insoluble
			if (topEdges.Where(tE => tE.Faces.First(f => f.Position == Face3D.FacePosition.Top).Color == topColor).Count() % 2 != 0) return;

			IEnumerable<Cube3D> correctEdges = topEdges.Where(c => c.Faces.First(f => f.Position == Face3D.FacePosition.Top).Color == topColor);
			Algorithm solveTopCrossAlgorithmI = new Algorithm("F R U Ri Ui Fi");
			Algorithm solveTopCrossAlgorithmII = new Algorithm("F Si R U Ri Ui Fi S");

			//Step 3: Solve the cross on the top layer
			if (CountEdgesWithCorrectOrientation(RubikManager) == 0)
			{
				solveTopCrossAlgorithmI.Moves.ForEach(m => RubikManager.SolutionMove(m.Layer, m.Direction));
				topEdges = topEdges.Select(c => RefreshCube(c));
				correctEdges = topEdges.Where(c => c.Faces.First(f => f.Position == Face3D.FacePosition.Top).Color == topColor);
			}

			if (CountEdgesWithCorrectOrientation(RubikManager) == 2)
			{
				Cube3D firstCorrect = correctEdges.First(); Cube3D secondCorrect = correctEdges.First(f => f != firstCorrect);
				bool opposite = false;
				foreach (Cube3D.RubikPosition flag in GetFlags(firstCorrect.Position))
				{
					Cube3D.RubikPosition pos = GetOppositeLayer(flag);
					if (secondCorrect.Position.HasFlag(pos) && pos != Cube3D.RubikPosition.None)
					{
						opposite = true;
						break;
					}
				}

				if (opposite)
				{
					while (correctEdges.Select(c => RefreshCube(c)).Count(c => c.Position.HasFlag(Cube3D.RubikPosition.RightSlice)) != 1) RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
					solveTopCrossAlgorithmI.Moves.ForEach(m => RubikManager.SolutionMove(m.Layer, m.Direction));
				}
				else
				{
					while (correctEdges.Select(c => RefreshCube(c)).Count(c => c.Position.HasFlag(Cube3D.RubikPosition.RightSlice) || c.Position.HasFlag(Cube3D.RubikPosition.FrontSlice)) != 2) RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
					solveTopCrossAlgorithmII.Moves.ForEach(m => RubikManager.SolutionMove(m.Layer, m.Direction));
				}
			}

			//Step 4: Move the edges of the cross to their target positions
			IEnumerable<Cube3D> CorrectEdges = topEdges.Where(c => c.Position == GetTargetPosition(c));
			while (CorrectEdges.Count() < 2)
			{
				RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
				CorrectEdges = CorrectEdges.Select(cE => RefreshCube(cE));
			}

			while (topEdges.Where(c => c.Position == GetTargetPosition(c)).Count() < 4)
			{
				CorrectEdges = topEdges.Where(c => c.Position == GetTargetPosition(c));
				while (CorrectEdges.Count() < 2)
				{
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
					CorrectEdges = CorrectEdges.Select(cE => RefreshCube(cE));
				}

				Cube3D.RubikPosition rightSlice = FacePosToCubePos(CorrectEdges.First().Faces
					.First(f => f.Color != topColor && f.Color != Color.Black).Position);
				RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
				CorrectEdges = CorrectEdges.Select(cE => RefreshCube(cE));

				if (CorrectEdges.Count(c => c.Position.HasFlag(rightSlice)) == 0)
				{
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
				}
				else
				{
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
					CorrectEdges = CorrectEdges.Select(cE => RefreshCube(cE));
					rightSlice = FacePosToCubePos(CorrectEdges.First(cE => !cE.Position.HasFlag(rightSlice)).Faces
						.First(f => f.Color != topColor && f.Color != Color.Black).Position);
				}
				//Algorithm: R U Ri U R U U Ri
				RubikManager.SolutionMove(rightSlice, true);
				RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
				RubikManager.SolutionMove(rightSlice, false);
				RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
				RubikManager.SolutionMove(rightSlice, true);
				RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
				RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
				RubikManager.SolutionMove(rightSlice, false);

				topEdges = topEdges.Select(tE => RefreshCube(tE));
				while (CorrectEdges.Count() < 2)
				{
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
					CorrectEdges = CorrectEdges.Select(cE => RefreshCube(cE));
				}
			}
		}

		private void CompleteLastLayer()
		{
			//Step 1: Get the color of the top layer to start with cross on the last layer
			Color topColor = RubikManager.getFaceColor(Cube3D.RubikPosition.TopLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Top);

			//Step 2: Get edges with the color of the top face
			IEnumerable<Cube3D> topCorners = RubikManager.RubikCube.cubes.Where(c => Cube3D.isCorner(c.Position)).Where(c => c.Position.HasFlag(Cube3D.RubikPosition.TopLayer));

			//Step 3: Bring corners to their target position
			while (topCorners.Where(c => c.Position == GetTargetPosition(c)).Count() < 4)
			{
				IEnumerable<Cube3D> correctCorners = topCorners.Where(c => c.Position == GetTargetPosition(c));
				Cube3D.RubikPosition rightSlice;
				if (correctCorners.Count() != 0)
				{
					Cube3D firstCube = correctCorners.First();
					Face3D rightFace = firstCube.Faces.First(f => f.Color != Color.Black && f.Position != Face3D.FacePosition.Top);
					rightSlice = FacePosToCubePos(rightFace.Position);
					RubikManager.SolutionMove(rightSlice, true);
					if (RefreshCube(firstCube).Position.HasFlag(Cube3D.RubikPosition.TopLayer))
					{
						RubikManager.SolutionMove(rightSlice, false);
					}
					else
					{
						RubikManager.SolutionMove(rightSlice, false);
						rightSlice = FacePosToCubePos(firstCube.Faces.First(f => f.Color != rightFace.Color && f.Color != Color.Black && f.Position != Face3D.FacePosition.Top).Position);
					}
				}
				else rightSlice = Cube3D.RubikPosition.RightSlice;

				Cube3D.RubikPosition leftSlice = GetOppositeFace(rightSlice);
				RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
				RubikManager.SolutionMove(rightSlice, true);
				RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
				RubikManager.SolutionMove(leftSlice, false);
				RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
				RubikManager.SolutionMove(rightSlice, false);
				RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
				RubikManager.SolutionMove(leftSlice, true);
				topCorners = topCorners.Select(tC => RefreshCube(tC));
				correctCorners = correctCorners.Select(cC => RefreshCube(cC));
			}

			//Step 4: Orientation of the corners on the top layer
			topCorners = topCorners.Select(tC => RefreshCube(tC));


			Face3D rightFac = RefreshCube(topCorners.First()).Faces.First(f => f.Color != Color.Black && f.Position != Face3D.FacePosition.Top);
			Cube3D.RubikPosition rightSlic = FacePosToCubePos(rightFac.Position);
			RubikManager.SolutionMove(rightSlic, true);
			if (RefreshCube(topCorners.First()).Position.HasFlag(Cube3D.RubikPosition.TopLayer))
			{
				RubikManager.SolutionMove(rightSlic, false);
			}
			else
			{
				RubikManager.SolutionMove(rightSlic, false);
				rightSlic = FacePosToCubePos(topCorners.First().Faces.First(f => f.Color != rightFac.Color && f.Color != Color.Black && f.Position != Face3D.FacePosition.Top).Position);
			}

			foreach (Cube3D c in topCorners)
			{
				while (!RefreshCube(c).Position.HasFlag(rightSlic))
				{
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
				}
				RubikManager.SolutionMove(rightSlic, true);
				if (RefreshCube(c).Position.HasFlag(Cube3D.RubikPosition.TopLayer))
				{
					RubikManager.SolutionMove(rightSlic, false);
				}
				else
				{
					RubikManager.SolutionMove(rightSlic, false);
					RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
				}

				//Algorithm: Ri Di R D
				while (RefreshCube(c).Faces.First(f => f.Position == Face3D.FacePosition.Top).Color != topColor)
				{
					RubikManager.SolutionMove(rightSlic, false);
					RubikManager.SolutionMove(Cube3D.RubikPosition.BottomLayer, false);
					RubikManager.SolutionMove(rightSlic, true);
					RubikManager.SolutionMove(Cube3D.RubikPosition.BottomLayer, true);
				}
			}

			topCorners = topCorners.Select(tC => RefreshCube(tC));
			while (topCorners.Count(tC => tC.Position == GetTargetPosition(tC)) != 4) RubikManager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
		}
	}
}
