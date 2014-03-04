using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace VirtualRubik
{
    class CubeSolver
    {
        RubikManager Manager;
        private RubikManager standardCube = new RubikManager();

        public CubeSolver(RubikManager rubik)
        {
            Manager = rubik.Clone();

            //Change colors of the faces
            standardCube.setFaceColor(Cube3D.RubikPosition.TopLayer, Face3D.FacePosition.Top,
              Manager.getFaceColor(Cube3D.RubikPosition.TopLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Top));

            standardCube.setFaceColor(Cube3D.RubikPosition.BottomLayer, Face3D.FacePosition.Bottom,
              Manager.getFaceColor(Cube3D.RubikPosition.BottomLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Bottom));

            standardCube.setFaceColor(Cube3D.RubikPosition.RightSlice, Face3D.FacePosition.Right,
              Manager.getFaceColor(Cube3D.RubikPosition.RightSlice | Cube3D.RubikPosition.MiddleSlice | Cube3D.RubikPosition.MiddleLayer, Face3D.FacePosition.Right));

            standardCube.setFaceColor(Cube3D.RubikPosition.LeftSlice, Face3D.FacePosition.Left,
              Manager.getFaceColor(Cube3D.RubikPosition.LeftSlice | Cube3D.RubikPosition.MiddleSlice | Cube3D.RubikPosition.MiddleLayer, Face3D.FacePosition.Left));

            standardCube.setFaceColor(Cube3D.RubikPosition.FrontSlice, Face3D.FacePosition.Front,
              Manager.getFaceColor(Cube3D.RubikPosition.MiddleLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.FrontSlice, Face3D.FacePosition.Front));

            standardCube.setFaceColor(Cube3D.RubikPosition.BackSlice, Face3D.FacePosition.Back,
              Manager.getFaceColor(Cube3D.RubikPosition.MiddleLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.BackSlice, Face3D.FacePosition.Back));
        }

        private void Solve()
        {
            SolveFirstCross();
            CompleteFirstLayer();
            CompleteMiddleLayer();
            SolveCrossTopLayer();
            CompleteLastLayer();
            RemoveUnnecessaryMoves();
        }

        public RubikManager ReturnRubik()
        {
            Solve();
            return Manager;
        }

        public bool CanSolve()
        {
            RubikManager oldManager = Manager.Clone();
            //check colors
            bool correctColors = standardCube.RubikCube.cubes.Count(sc => Manager.RubikCube.cubes
                .Where(c => ScrambledEquals(c.Colors, sc.Colors)).Count() == 1) == Manager.RubikCube.cubes.Count();

            //return false, if there are invalid cube colors
            if (!correctColors) return false;

            Solve();

            //check if all the cube faces are solved
            Cube3D.RubikPosition layers = Cube3D.RubikPosition.TopLayer | Cube3D.RubikPosition.BottomLayer | Cube3D.RubikPosition.RightSlice
                | Cube3D.RubikPosition.LeftSlice | Cube3D.RubikPosition.FrontSlice | Cube3D.RubikPosition.BackSlice;
            foreach (Cube3D.RubikPosition l in GetFlags(layers))
            {
                Face3D.FacePosition facePos = CubePosToFacePos(l);
                if (facePos != Face3D.FacePosition.None)
                {
                    Cube3D.RubikPosition centerPos = Manager.RubikCube.cubes.First(c => Cube3D.isCenter(c.Position) && c.Position.HasFlag(l)).Position;
                    Color faceColor = Manager.getFaceColor(centerPos, facePos);

                    bool faceNotSolved = Manager.RubikCube.cubes.Count(c => c.Position.HasFlag(l) && c.Faces.First(f => f.Position == facePos).Color == faceColor) != 9;
                    if (faceNotSolved) return false;
                }
            }

            Manager = oldManager;
            return true;
        }

        //Solve the first cross on the bottom layer
        private void SolveFirstCross()
        {
            //Step 1: Get the color of the bottom layer to start with the first cross
            Color bottomColor = Manager.getFaceColor(Cube3D.RubikPosition.BottomLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Bottom);

            //Step 2: Get edges with the color of the bottom face
            IEnumerable<Cube3D> bottomEdges = Manager.RubikCube.cubes.Where(c => Cube3D.isEdge(c.Position)).Where(c => c.Colors.Count(co => co == bottomColor) == 1);

            //Step 3: Solve the first cross
            foreach (Cube3D c in bottomEdges)
            {
                //Step 3.1: Get the target position and the second color of the edge
                Cube3D.RubikPosition targetPosition = standardCube.RubikCube.cubes.First(cu => ScrambledEquals(cu.Colors, c.Colors)).Position;
                Color secondColor = c.Colors.First(co => co != bottomColor && co != Color.Black);

                //Step 3.2: Rotate to target position
                if (c.Position != targetPosition)
                {
                    //Rotate to top layer
                    Cube3D.RubikPosition layer = FacePosToCubePos(c.Faces.First(f => (f.Color == bottomColor || f.Color == secondColor)
                      && f.Position != Face3D.FacePosition.Top && f.Position != Face3D.FacePosition.Bottom).Position);

                    if (c.Position.HasFlag(Cube3D.RubikPosition.MiddleLayer))
                    {
                        Manager.SolutionMove(layer, true);
                        if (RefreshCube(c).Position.HasFlag(Cube3D.RubikPosition.TopLayer))
                        {
                            Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                            Manager.SolutionMove(layer, false);
                        }
                        else
                        {
                            for (int i = 0; i < 2; i++) Manager.SolutionMove(layer, true);
                            Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                            Manager.SolutionMove(layer, true);
                        }
                    }
                    if (c.Position.HasFlag(Cube3D.RubikPosition.BottomLayer)) for (int i = 0; i < 2; i++) Manager.SolutionMove(layer, true);

                    //Rotate over target position
                    Cube3D.RubikPosition targetLayer = FacePosToCubePos(standardCube.RubikCube.cubes.First(cu => ScrambledEquals(cu.Colors, c.Colors))
                      .Faces.First(f => f.Color == secondColor).Position);

                    while (!RefreshCube(c).Position.HasFlag(targetLayer))
                    {
                        Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                    }

                    //Rotate to target position
                    for (int i = 0; i < 2; i++) Manager.SolutionMove(targetLayer, true);
                }

                //Step 3.3: Flip the incorrect orientated edges with the algorithm: Fi D Ri Di
                if (c.Faces.First(f => f.Position == Face3D.FacePosition.Bottom).Color != bottomColor)
                {
                    Cube3D.RubikPosition frontLayer = FacePosToCubePos(RefreshCube(c).Faces.First(f => f.Color == bottomColor).Position);
                    Manager.SolutionMove(frontLayer, false);
                    Manager.SolutionMove(Cube3D.RubikPosition.BottomLayer, true);

                    Cube3D.RubikPosition rightSlice = FacePosToCubePos(RefreshCube(c).Faces.First(f => f.Color == secondColor).Position);

                    Manager.SolutionMove(rightSlice, false);
                    Manager.SolutionMove(Cube3D.RubikPosition.BottomLayer, false);
                }
            }
        }

        private void CompleteFirstLayer()
        {
            //Step 1: Get the color of the bottom layer
            Color bottomColor = Manager.getFaceColor(Cube3D.RubikPosition.BottomLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Bottom);

            //Step 2: Get corners with the color of the bottom face
            IEnumerable<Cube3D> bottomCorners = Manager.RubikCube.cubes.Where(c => Cube3D.isCorner(c.Position)).Where(c => c.Colors.Count(co => co == bottomColor) == 1);

            //Step 3: Complete the first layer
            foreach (Cube3D c in bottomCorners)
            {
                //3.1 Get the target position
                Cube3D.RubikPosition targetPosition = standardCube.RubikCube.cubes.First(cu => ScrambledEquals(cu.Colors, c.Colors)).Position;

                //3.2 Rotate to target position
                if (c.Position != targetPosition)
                {
                    //Rotate to top layer
                    if (c.Position.HasFlag(Cube3D.RubikPosition.BottomLayer))
                    {
                        Face3D leftFace = RefreshCube(c).Faces.First(f => f.Position != Face3D.FacePosition.Bottom && f.Color != Color.Black);
                        Cube3D.RubikPosition leftSlice = FacePosToCubePos(leftFace.Position);
                        Manager.SolutionMove(leftSlice, false);
                        if (RefreshCube(c).Position.HasFlag(Cube3D.RubikPosition.BottomLayer))
                        {
                            Manager.SolutionMove(leftSlice, true);
                            leftFace = RefreshCube(c).Faces.First(f => f.Position != Face3D.FacePosition.Bottom && f.Color != leftFace.Color && f.Color != Color.Black);
                            leftSlice = FacePosToCubePos(leftFace.Position);
                            Manager.SolutionMove(leftSlice, false);
                        }
                        Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                        Manager.SolutionMove(leftSlice, true);
                        Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                    }

                    //Rotate over target position
                    Cube3D.RubikPosition targetPos = Cube3D.RubikPosition.None;
                    foreach (Cube3D.RubikPosition p in GetFlags(targetPosition))
                    {
                        if (p != Cube3D.RubikPosition.BottomLayer)
                            targetPos |= p;
                    }

                    while (!RefreshCube(c).Position.HasFlag(targetPos)) Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                }

                //Rotate to target position with the algorithm: Li Ui L U
                Face3D leftFac = RefreshCube(c).Faces.First(f => f.Position != Face3D.FacePosition.Top && f.Position != Face3D.FacePosition.Bottom && f.Color != Color.Black);
                Cube3D.RubikPosition leftSlic = FacePosToCubePos(leftFac.Position);
                Manager.SolutionMove(leftSlic, false);
                if (!RefreshCube(c).Position.HasFlag(Cube3D.RubikPosition.TopLayer))
                {
                    Manager.SolutionMove(leftSlic, true);
                    leftFac = RefreshCube(c).Faces.First(f => f.Position != Face3D.FacePosition.Top && f.Position != Face3D.FacePosition.Bottom && f.Color != leftFac.Color && f.Color != Color.Black);
                    leftSlic = FacePosToCubePos(leftFac.Position);
                }
                else Manager.SolutionMove(leftSlic, true);

                while (RefreshCube(c).Faces.First(f => f.Color == bottomColor).Position != Face3D.FacePosition.Bottom)
                {
                    Manager.SolutionMove(leftSlic, false);
                    Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                    Manager.SolutionMove(leftSlic, true);
                    Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                }
            }
        }

        private void CompleteMiddleLayer()
        {
            //Step 1: Get the color of the bottom and top layer
            Color bottomColor = Manager.getFaceColor(Cube3D.RubikPosition.BottomLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Bottom);
            Color topColor = Manager.getFaceColor(Cube3D.RubikPosition.TopLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Top);

            //Step 2: Get the egdes of the middle layer
            IEnumerable<Cube3D> middleEdges = Manager.RubikCube.cubes.Where(c => Cube3D.isEdge(c.Position)).Where(c => c.Colors.Count(co => co == bottomColor || co == topColor) == 0);

            //Step 3: Complete the middle layer
            foreach (Cube3D c in middleEdges)
            {
                //3.1 Get the target position
                Cube3D.RubikPosition targetPosition = standardCube.RubikCube.cubes.First(cu => ScrambledEquals(cu.Colors, c.Colors)).Position;

                //Check correct orientation
                List<Face3D> coloredFaces = new List<Face3D>();
                Manager.RubikCube.cubes.Where(cu => Cube3D.isCenter(cu.Position)).ToList().ForEach(cu => coloredFaces.Add(cu.Faces.First(f => f.Color != Color.Black)));
                bool correctOrientation = c.Faces.Count(f => coloredFaces.Count(cf => cf.Color == f.Color && cf.Position == f.Position) == 1) == 2;

                //3.2 Rotate to target position
                if (c.Position != targetPosition || (c.Position == targetPosition && !correctOrientation))
                {
                    //Rotate to top layer
                    if (!c.Position.HasFlag(Cube3D.RubikPosition.TopLayer))
                    {
                        Face3D frontFace = c.Faces.First(f => f.Color != Color.Black);
                        Cube3D.RubikPosition frontSlice = FacePosToCubePos(frontFace.Position);
                        Face3D face = c.Faces.First(f => f.Color != Color.Black && f.Color != frontFace.Color);
                        Cube3D.RubikPosition slice = FacePosToCubePos(face.Position);

                        Manager.SolutionMove(slice, true);
                        if (RefreshCube(c).Position.HasFlag(Cube3D.RubikPosition.TopLayer))
                        {
                            Manager.SolutionMove(slice, false);
                            //Algorithm to the right: U R Ui Ri Ui Fi U F
                            Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                            Manager.SolutionMove(slice, true);
                            Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                            Manager.SolutionMove(slice, false);
                            Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                            Manager.SolutionMove(frontSlice, false);
                            Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                            Manager.SolutionMove(frontSlice, true);
                        }
                        else
                        {
                            Manager.SolutionMove(slice, false);
                            //Algorithm to the left: Ui Li U L U F Ui Fi
                            Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                            Manager.SolutionMove(slice, false);
                            Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                            Manager.SolutionMove(slice, true);
                            Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                            Manager.SolutionMove(frontSlice, true);
                            Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                            Manager.SolutionMove(frontSlice, false);
                        }
                    }

                    //Rotate to start position for the algorithm
                    IEnumerable<Cube3D> middles = Manager.RubikCube.cubes.Where(cu => Cube3D.isCenter(cu.Position)).Where(m => m.Colors.First(co => co != Color.Black)
                        == RefreshCube(c).Faces.First(f => f.Color != Color.Black && f.Position != Face3D.FacePosition.Top).Color &&
                        RemoveFlag(m.Position, Cube3D.RubikPosition.MiddleLayer) == RemoveFlag(RefreshCube(c).Position, Cube3D.RubikPosition.TopLayer));

                    while (middles.Count() < 1)
                    {
                        Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                        middles = Manager.RubikCube.cubes.Where(cu => Cube3D.isCenter(cu.Position)).Where(m => m.Colors.First(co => co != Color.Black)
                          == RefreshCube(c).Faces.First(f => f.Color != Color.Black && f.Position != Face3D.FacePosition.Top).Color &&
                          RemoveFlag(m.Position, Cube3D.RubikPosition.MiddleLayer) == RemoveFlag(RefreshCube(c).Position, Cube3D.RubikPosition.TopLayer));
                    }

                    //Rotate to target position
                    Face3D frontFac = RefreshCube(c).Faces.First(f => f.Color != Color.Black && f.Position != Face3D.FacePosition.Top);
                    Cube3D.RubikPosition frontSlic = FacePosToCubePos(frontFac.Position);
                    Cube3D.RubikPosition slic = Cube3D.RubikPosition.None;
                    foreach (Cube3D.RubikPosition p in GetFlags(targetPosition))
                    {
                        if (p != Cube3D.RubikPosition.MiddleLayer && p != frontSlic)
                            slic |= p;
                    }

                    Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                    if (!RefreshCube(c).Position.HasFlag(slic))
                    {
                        Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                        //Algorithm to the right: U R Ui Ri Ui Fi U F
                        Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                        Manager.SolutionMove(slic, true);
                        Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                        Manager.SolutionMove(slic, false);
                        Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                        Manager.SolutionMove(frontSlic, false);
                        Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                        Manager.SolutionMove(frontSlic, true);
                    }
                    else
                    {
                        Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                        //Algorithm to the left: Ui Li U L U F Ui Fi
                        Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                        Manager.SolutionMove(slic, false);
                        Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                        Manager.SolutionMove(slic, true);
                        Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                        Manager.SolutionMove(frontSlic, true);
                        Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                        Manager.SolutionMove(frontSlic, false);
                    }
                }
            }
        }

        private void SolveCrossTopLayer()
        {
            //Step 1: Get the color of the top layer to start with cross on the last layer
            Color topColor = Manager.getFaceColor(Cube3D.RubikPosition.TopLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Top);

            //Step 2: Get edges with the color of the top face
            IEnumerable<Cube3D> topEdges = Manager.RubikCube.cubes.Where(c => Cube3D.isEdge(c.Position)).Where(c => c.Colors.Count(co => co == topColor) == 1);

            //Check if the cube is insoluble
            if (topEdges.Where(tE => tE.Faces.First(f => f.Position == Face3D.FacePosition.Top).Color == topColor).Count() % 2 != 0)
            {
                return;
            }

            //Step 3: Solve the cross on the top layer
            while (topEdges.Where(c => c.Faces.First(f => f.Position == Face3D.FacePosition.Top).Color == topColor).Count() < 4)
            {
                if (topEdges.Where(c => c.Faces.First(f => f.Position == Face3D.FacePosition.Top).Color == topColor).Count() != 0)
                {
                    while ((Manager.getFaceColor(Cube3D.RubikPosition.TopLayer | Cube3D.RubikPosition.LeftSlice | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Top) != topColor))
                    {
                        Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                    }
                }
                if (Manager.getFaceColor(Cube3D.RubikPosition.TopLayer | Cube3D.RubikPosition.FrontSlice | Cube3D.RubikPosition.MiddleSlice_Sides, Face3D.FacePosition.Top) == topColor)
                    Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);

                Manager.SolutionMove(Cube3D.RubikPosition.FrontSlice, true);
                Manager.SolutionMove(Cube3D.RubikPosition.RightSlice, true);
                Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                Manager.SolutionMove(Cube3D.RubikPosition.RightSlice, false);
                Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                Manager.SolutionMove(Cube3D.RubikPosition.FrontSlice, false);

                topEdges = Manager.RubikCube.cubes.Where(c => Cube3D.isEdge(c.Position)).Where(c => c.Colors.Count(co => co == topColor) == 1);
            }

            //Step 4: Move the edges of the cross to their target positions
            IEnumerable<Cube3D> correctEdges = topEdges.Where(c => c.Position == GetTargetPosition(c));
            while (correctEdges.Count() < 2)
            {
                Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                correctEdges = correctEdges.Select(cE => RefreshCube(cE));
            }

            while (topEdges.Where(c => c.Position == GetTargetPosition(c)).Count() < 4)
            {
                correctEdges = topEdges.Where(c => c.Position == GetTargetPosition(c));
                while (correctEdges.Count() < 2)
                {
                    Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                    correctEdges = correctEdges.Select(cE => RefreshCube(cE));
                }

                Cube3D.RubikPosition rightSlice = FacePosToCubePos(correctEdges.First().Faces
                  .First(f => f.Color != topColor && f.Color != Color.Black).Position);
                Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                correctEdges = correctEdges.Select(cE => RefreshCube(cE));

                if (correctEdges.Count(c => c.Position.HasFlag(rightSlice)) == 0)
                {
                    Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                }
                else
                {
                    Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                    correctEdges = correctEdges.Select(cE => RefreshCube(cE));
                    rightSlice = FacePosToCubePos(correctEdges.First(cE => !cE.Position.HasFlag(rightSlice)).Faces
                      .First(f => f.Color != topColor && f.Color != Color.Black).Position);
                }
                //Algorithm: R U Ri U R U U Ri
                Manager.SolutionMove(rightSlice, true);
                Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                Manager.SolutionMove(rightSlice, false);
                Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                Manager.SolutionMove(rightSlice, true);
                Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                Manager.SolutionMove(rightSlice, false);

                topEdges = topEdges.Select(tE => RefreshCube(tE));
                while (correctEdges.Count() < 2)
                {
                    Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                    correctEdges = correctEdges.Select(cE => RefreshCube(cE));
                }
            }
        }

        private void CompleteLastLayer()
        {
            //Step 1: Get the color of the top layer to start with cross on the last layer
            Color topColor = Manager.getFaceColor(Cube3D.RubikPosition.TopLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Top);

            //Step 2: Get edges with the color of the top face
            IEnumerable<Cube3D> topCorners = Manager.RubikCube.cubes.Where(c => Cube3D.isCorner(c.Position)).Where(c => c.Position.HasFlag(Cube3D.RubikPosition.TopLayer));

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
                    Manager.SolutionMove(rightSlice, true);
                    if (RefreshCube(firstCube).Position.HasFlag(Cube3D.RubikPosition.TopLayer))
                    {
                        Manager.SolutionMove(rightSlice, false);
                    }
                    else
                    {
                        Manager.SolutionMove(rightSlice, false);
                        rightSlice = FacePosToCubePos(firstCube.Faces.First(f => f.Color != rightFace.Color && f.Color != Color.Black && f.Position != Face3D.FacePosition.Top).Position);
                    }
                }
                else rightSlice = Cube3D.RubikPosition.RightSlice;

                Cube3D.RubikPosition leftSlice = GetOppositeFace(rightSlice);
                Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                Manager.SolutionMove(rightSlice, true);
                Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                Manager.SolutionMove(leftSlice, false);
                Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                Manager.SolutionMove(rightSlice, false);
                Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, false);
                Manager.SolutionMove(leftSlice, true);
                topCorners = topCorners.Select(tC => RefreshCube(tC));
                correctCorners = correctCorners.Select(cC => RefreshCube(cC));
            }

            //Step 4: Orientation of the corners on the top layer
            topCorners = topCorners.Select(tC => RefreshCube(tC));


            Face3D rightFac = RefreshCube(topCorners.First()).Faces.First(f => f.Color != Color.Black && f.Position != Face3D.FacePosition.Top);
            Cube3D.RubikPosition rightSlic = FacePosToCubePos(rightFac.Position);
            Manager.SolutionMove(rightSlic, true);
            if (RefreshCube(topCorners.First()).Position.HasFlag(Cube3D.RubikPosition.TopLayer))
            {
                Manager.SolutionMove(rightSlic, false);
            }
            else
            {
                Manager.SolutionMove(rightSlic, false);
                rightSlic = FacePosToCubePos(topCorners.First().Faces.First(f => f.Color != rightFac.Color && f.Color != Color.Black && f.Position != Face3D.FacePosition.Top).Position);
            }

            foreach (Cube3D c in topCorners)
            {
                while (!RefreshCube(c).Position.HasFlag(rightSlic))
                {
                    Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                }
                Manager.SolutionMove(rightSlic, true);
                if (RefreshCube(c).Position.HasFlag(Cube3D.RubikPosition.TopLayer))
                {
                    Manager.SolutionMove(rightSlic, false);
                }
                else
                {
                    Manager.SolutionMove(rightSlic, false);
                    Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
                }

                //Algorithm: Ri Di R D
                while (RefreshCube(c).Faces.First(f => f.Position == Face3D.FacePosition.Top).Color != topColor)
                {
                    Manager.SolutionMove(rightSlic, false);
                    Manager.SolutionMove(Cube3D.RubikPosition.BottomLayer, false);
                    Manager.SolutionMove(rightSlic, true);
                    Manager.SolutionMove(Cube3D.RubikPosition.BottomLayer, true);
                }
            }

            topCorners = topCorners.Select(tC => RefreshCube(tC));
            while (topCorners.Count(tC => tC.Position == GetTargetPosition(tC)) != 4) Manager.SolutionMove(Cube3D.RubikPosition.TopLayer, true);
        }

        private void RemoveUnnecessaryMoves()
        {
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < Manager.Moves.Count; i++)
                {
                    if (i != Manager.Moves.Count - 1) if (Manager.Moves[i].Layer == Manager.Moves[i + 1].Layer && Manager.Moves[i].Direction != Manager.Moves[i + 1].Direction)
                        {
                            Manager.Moves.RemoveAt(i + 1);
                            Manager.Moves.RemoveAt(i);
                            if (i != 0) i--;
                        }
                    if (i < Manager.Moves.Count - 2) if (Manager.Moves[i].Layer == Manager.Moves[i + 1].Layer && Manager.Moves[i].Layer == Manager.Moves[i + 2].Layer
                        && Manager.Moves[i].Direction == Manager.Moves[i + 1].Direction && Manager.Moves[i].Direction == Manager.Moves[i + 2].Direction)
                        {
                            bool direction = !Manager.Moves[i + 2].Direction;
                            Manager.Moves.RemoveAt(i + 1);
                            Manager.Moves.RemoveAt(i);
                            Manager.Moves[i].Direction = direction;
                            if (i != 0) i--;
                        }
                }
            }
        }

        private Cube3D.RubikPosition FacePosToCubePos(Face3D.FacePosition position)
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

        private Face3D.FacePosition CubePosToFacePos(Cube3D.RubikPosition position)
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

        private Cube3D.RubikPosition RemoveFlag(Cube3D.RubikPosition oldPosition, Cube3D.RubikPosition item)
        {
            return oldPosition &= ~item;
        }

        private Cube3D.RubikPosition GetTargetPosition(Cube3D cube)
        {
            return standardCube.RubikCube.cubes.First(cu => ScrambledEquals(cu.Colors, cube.Colors)).Position;
        }

        private Cube3D RefreshCube(Cube3D cube)
        {
            return Manager.RubikCube.cubes.First(cu => ScrambledEquals(cu.Colors, cube.Colors));
        }

        private Cube3D.RubikPosition GetOppositeFace(Cube3D.RubikPosition layer)
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

        static IEnumerable<Enum> GetFlags(Enum input)
        {
            foreach (Enum value in Enum.GetValues(input.GetType()))
                if (input.HasFlag(value))
                    yield return value;
        }

        private static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
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
    }
}
