using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RubiksCubeSolver.RubiksCube;
using System.Drawing;

namespace RubiksCubeSolver.Solver
{
  public class FridrichSolver: ISolver
  {
    public FridrichSolver(Rubik rubik)
    {
      Rubik = rubik.DeepClone();
      Solution = new Solution(rubik);
      InitStandardCube();
    }

    public override void GetSolution()
    {
      SolveFirstCross();
      CompleteFirstTwoLayers();
    }

    private void SolveFirstCross()
    {
      //Step 1: Get color of the bottom layer
      Color bottomColor = Rubik.GetFaceColor(CubeFlag.BottomLayer | CubeFlag.MiddleSliceSides | CubeFlag.MiddleSlice, FacePosition.Bottom);

      //Step 2: Get the edges with target position on the bottom layer
      IEnumerable<Cube> bottomEdges = Rubik.Cubes.Where(c => c.IsEdge && GetTargetFlags(c).HasFlag(CubeFlag.BottomLayer));

      //Step 3: Rotate a correct orientated edge of the bottom layer to target position
      IEnumerable<Cube> solvedBottomEdges = bottomEdges.Where(bE => bE.Position.Flags == GetTargetFlags(bE) && bE.Faces.First(f => f.Color == bottomColor).Position == FacePosition.Bottom);
      if (bottomEdges.Count(bE => bE.Position.HasFlag(CubeFlag.BottomLayer) && bE.Faces.First(f => f.Color == bottomColor).Position == FacePosition.Bottom) > 0)
      {
        while (solvedBottomEdges.Count() < 1)
        {
          SolverMove(CubeFlag.BottomLayer, true);
          solvedBottomEdges = bottomEdges.Where(bE => bE.Position.Flags == GetTargetFlags(bE) && bE.Faces.First(f => f.Color == bottomColor).Position == FacePosition.Bottom);
        }
      }

      //Step 4: Solve incorrect edges of the bottom layer
      while (solvedBottomEdges.Count() < 4)
      {
        IEnumerable<Cube> unsolvedBottomEdges = bottomEdges.Except(solvedBottomEdges);
        Cube e = (unsolvedBottomEdges.FirstOrDefault(c => c.Position.HasFlag(CubeFlag.TopLayer)) != null)
            ? unsolvedBottomEdges.FirstOrDefault(c => c.Position.HasFlag(CubeFlag.TopLayer)) : unsolvedBottomEdges.First();
        Color secondColor = e.Colors.First(co => co != bottomColor && co != Color.Black);

        if (e.Position.Flags != GetTargetFlags(e))
        {
          //Rotate to top layer
          CubeFlag layer = CubePosition.FacePosToLayerPos(e.Faces.First(f => (f.Color == bottomColor || f.Color == secondColor)
            && f.Position != FacePosition.Top && f.Position != FacePosition.Bottom).Position);

          CubeFlag targetLayer = CubePosition.FacePosToLayerPos(StandardCube.Cubes.First(cu => ScrambledEquals(cu.Colors, e.Colors))
            .Faces.First(f => f.Color == secondColor).Position);

          if (e.Position.HasFlag(CubeFlag.MiddleLayer))
          {
            if (layer == targetLayer)
            {
              while (!e.Position.HasFlag(CubeFlag.BottomLayer)) SolverMove(layer, true);
            }
            else
            {
              SolverMove(layer, true);
              if (e.Position.HasFlag(CubeFlag.TopLayer))
              {
                SolverMove(CubeFlag.TopLayer, true);
                SolverMove(layer, false);
              }
              else
              {
                for (int i = 0; i < 2; i++) SolverMove(layer, true);
                SolverMove(CubeFlag.TopLayer, true);
                SolverMove(layer, true);
              }
            }
          }
          if (e.Position.HasFlag(CubeFlag.BottomLayer)) for (int i = 0; i < 2; i++) SolverMove(layer, true);

          //Rotate over target position
          while (!e.Position.HasFlag(targetLayer)) SolverMove(CubeFlag.TopLayer, true);

          //Rotate to target position
          for (int i = 0; i < 2; i++) SolverMove(targetLayer, true);
          CubeFlag targetPos = GetTargetFlags(e);
        }

        //Flip the incorrect orientated edges with the algorithm: Fi D Ri Di
        if (e.Faces.First(f => f.Position == FacePosition.Bottom).Color != bottomColor)
        {
          CubeFlag frontSlice = CubePosition.FacePosToLayerPos(e.Faces.First(f => f.Color == bottomColor).Position);

          SolverMove(frontSlice, false);
          SolverMove(CubeFlag.BottomLayer, true);

          CubeFlag rightSlice = CubePosition.FacePosToLayerPos(e.Faces.First(f => f.Color == secondColor).Position);

          SolverMove(rightSlice, false);

          SolverMove(CubeFlag.BottomLayer, false);
        }
        List<Face> faces = e.Faces.ToList();
        solvedBottomEdges = bottomEdges.Where(bE => bE.Position.Flags == GetTargetFlags(bE) && bE.Faces.First(f => f.Color == bottomColor).Position == FacePosition.Bottom);
      }
    }

    private void CompleteFirstTwoLayers()
    {
      Color bottomColor = Rubik.GetFaceColor(CubeFlag.BottomLayer | CubeFlag.MiddleSliceSides | CubeFlag.MiddleSlice, FacePosition.Bottom);

      IEnumerable<Cube> middleEdges = Rubik.Cubes.Where(c => c.IsEdge && GetTargetFlags(c).HasFlag(CubeFlag.MiddleLayer));
      IEnumerable<Cube> bottomCorners = Rubik.Cubes.Where(c => c.IsCorner && GetTargetFlags(c).HasFlag(CubeFlag.BottomLayer));

      IEnumerable<Cube> solvedMiddleEdges = middleEdges.Where(c => c.Position == GetTargetCubePosition(c));
      IEnumerable<Cube> solvedBottomCorners = bottomCorners.Where(c => c.Position == GetTargetCubePosition(c));

      while (solvedBottomCorners.Count() < 4 || solvedMiddleEdges.Count() < 4)
      {
        //Get pairs
        Cube corner = bottomCorners.Except(solvedBottomCorners).FirstOrDefault(c => c.Position.HasFlag(CubeFlag.TopLayer));
        if (corner == null) corner = bottomCorners.Except(solvedBottomCorners).First();
        IEnumerable<Color> cornerColors = corner.Colors.Where(co => co != bottomColor && co != Color.Black);
        Cube edge = middleEdges.First(c => ScrambledEquals(cornerColors, c.Colors.Where(co => co != Color.Black)));

        #region Move corner and edge to the top layer

        if (corner.Position.HasFlag(CubeFlag.BottomLayer))
        {
          //Rotate corner to top layer
          CubeFlag l = corner.Position.FirstFlag(CubeFlag.BottomLayer | CubeFlag.None);

          bool direction = new TestScenario(Rubik, new LayerMove(l, true)).TestCubePosition(corner, CubeFlag.TopLayer);
          SolverMove(l, direction);
          SolverMove(CubeFlag.TopLayer, true);
          SolverMove(l, !direction);
        }

        if (edge.Position.HasFlag(CubeFlag.MiddleLayer))
        {
          CubeFlag l = edge.Position.FirstFlag(CubeFlag.MiddleLayer | CubeFlag.None);
          bool direction = new TestScenario(Rubik, new LayerMove(l, true)).TestCubePosition(edge, CubeFlag.TopLayer);
          SolverMove(l, direction);
          while ((corner.Position.HasFlag(l) && corner.Position.HasFlag(CubeFlag.TopLayer)) || edge.Position.HasFlag(l)) SolverMove(CubeFlag.TopLayer, true);
          SolverMove(l, !direction);
        }
        #endregion

        #region Seperate corner and edge if necessary
        bool nextTo = false;

        CubeFlag commonLayers = corner.Position.CommonFlags(edge.Position.Flags);
        nextTo = commonLayers != CubeFlag.None;

        bool colorsCorrect = corner.Faces.First(f => f.Position == FacePosition.Top).Color == edge.Faces.First(f => f.Position == FacePosition.Top).Color &&
          corner.Faces.First(f => f.Position != FacePosition.Top && f.Color != bottomColor).Position == edge.Faces.First(f => f.Position != FacePosition.Top && f.Color != bottomColor).Position;

        if (nextTo && !colorsCorrect)
        {
          CubeFlag l = new CubePosition(corner.Position.ExceptFlag(commonLayers)).FirstFlag(CubeFlag.TopLayer | CubeFlag.None);
          SolverMove(l, true);
          do
          {
            SolverMove(CubeFlag.TopLayer, true);
          } while (((corner.Position.HasFlag(l) && corner.Position.HasFlag(CubeFlag.TopLayer)) || edge.Position.HasFlag(l)));
          SolverMove(l, false);
        }
        #endregion

        #region Bottom face on top layer
        if (corner.Faces.First(f => f.Position == FacePosition.Top).Color == bottomColor)
        {
          Color edgeFaceColor = edge.Faces.First(f => f.Position != FacePosition.Top && f.Color != Color.Black).Color;
          CubeFlag edgePos = edge.Position.Flags;
          Cube centerCube = Rubik.Cubes.First(c => c.IsCenter && c.Faces.First(f => f.Color != Color.Black).Color == edgeFaceColor);
          CubeFlag edgeStartLayer = CubeFlag.None;

          //Rotate edge to start position of algorithm
          while ((edge.Position.Flags & ~CubeFlag.TopLayer) != (centerCube.Position.Flags & ~CubeFlag.MiddleLayer)) SolverMove(CubeFlag.TopLayer, true);
          if (edgeFaceColor == centerCube.Faces.First(f => f.Color != Color.Black).Color)
            edgeStartLayer = CubePosition.FacePosToLayerPos(centerCube.Faces.First(f => f.Color != Color.Black).Position);

          //Get rotation direction of edge start layer with virtual rubik object
          //Func<Rubik, bool> getRotationDirection = new Func<RubiksCube.Rubik, bool>(delegate(Rubik r)
          //  {
          //    r.RotateLayer(edgeStartLayer, true);
          //    while((RefreshCube(corner,r).Position.Flags & ~CubeFlag.TopLayer) != (RefreshCube(edge,r).Position.Flags & ~CubeFlag.MiddleLayer)) r.RotateLayer(CubeFlag.TopLayer, true);
          //    r.RotateLayer(edgeStartLayer, false);
          //    return RefreshCube(edge, r).Faces.First(f => f.Position == FacePosition.Top) == RefreshCube(corner, r).Faces.First(f => f.Position == FacePosition.Top);
          //  });

          //bool direction = new TestScenario(Rubik).Test(getRotationDirection);

          Rubik testRubik = Rubik.DeepClone();
          testRubik.RotateLayer(edgeStartLayer, true);
          while ((RefreshCube(corner, testRubik).Position.Flags & ~CubeFlag.TopLayer) != (RefreshCube(edge, testRubik).Position.Flags & ~CubeFlag.MiddleLayer)) testRubik.RotateLayer(CubeFlag.TopLayer, true);
          testRubik.RotateLayer(edgeStartLayer, false);
          bool direction = RefreshCube(edge, testRubik).Faces.First(f => f.Position == FacePosition.Top) == RefreshCube(corner, testRubik).Faces.First(f => f.Position == FacePosition.Top);

          //Fit edge and corner together
          SolverMove(edgeStartLayer, direction);
          while ((corner.Position.Flags & ~CubeFlag.TopLayer) != (edge.Position.Flags & ~CubeFlag.MiddleLayer)) SolverMove(CubeFlag.TopLayer, true);
          SolverMove(edgeStartLayer, !direction);

          ////Rotate edge and corner to target slot
          //bool directionTop = new TestScenario(Rubik, new LayerMove(CubeFlag.TopLayer, true)).TestCubePosition(corner, edgeStartLayer);
          //bool directionSlice = !new TestScenario(Rubik, new LayerMove(edgeStartLayer, true)).TestCubePosition(corner, CubeFlag.BottomLayer);
          //SolverAlgorithm("U{0} {1} U{2} {3}", directionTop ? "" : "'", directionSlice ? CubePosition.CubeFlagToString(edgeStartLayer) : CubePosition.CubeFlagToString(edgeStartLayer) + "'",
          //  directionTop ? "'" : "", directionSlice ? CubePosition.CubeFlagToString(edgeStartLayer) + "'" : CubePosition.CubeFlagToString(edgeStartLayer));

          break;
        }
        #endregion
        else break;

        //To Do:
        //CompleteFirstTwoLayers implementation
      }
    }
  }
}
