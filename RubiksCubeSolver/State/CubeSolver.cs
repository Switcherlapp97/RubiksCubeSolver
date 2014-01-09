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
      Manager = rubik;

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

    //Solve the first cross on the bottom layer
    public Stack<LayerMove> SolveFirstCross()
    {
      List<LayerMove> Moves = new List<LayerMove>();

      //Step 1: Get the color of the bottom layer to start with the first cross
      Color bottomColor = Manager.getFaceColor(Cube3D.RubikPosition.BottomLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Bottom);

      //Step 3: Get edges with the color of the bottom face
      IEnumerable<Cube3D> bottomEdges = Manager.RubikCube.cubes.Where(c => Cube3D.isEdge(c.Position)).Where(c => c.Colors.Count(co => co == bottomColor) == 1);

      //Step 4: Solve the first cross
      foreach (Cube3D c in bottomEdges)
      {
        //Step 4.1: Get the target position and the second color of the edge
        Cube3D.RubikPosition targetPosition = standardCube.RubikCube.cubes.First(cu => ScrambledEquals(cu.Colors, c.Colors)).Position;
        Color secondColor = c.Colors.First(co => co != bottomColor && co != Color.Black);

        //Step 4.2: Rotate to target position
        if (c.Position != targetPosition)
        {
          //Rotate to top layer
          Cube3D.RubikPosition layer = FacePosToCubePos(c.Faces.First(f => (f.Color == bottomColor || f.Color == secondColor)
            && f.Position != Face3D.FacePosition.Top && f.Position != Face3D.FacePosition.Bottom).Position);

          if (c.Position.HasFlag(Cube3D.RubikPosition.MiddleLayer))
          {
            Manager.Rotate90Sync(layer, true);
            if (RefreshCube(c).Position.HasFlag(Cube3D.RubikPosition.TopLayer))
            {
              Moves.Add(new LayerMove(layer, true));
              Manager.Rotate90Sync(Cube3D.RubikPosition.TopLayer, true);
              Moves.Add(new LayerMove(Cube3D.RubikPosition.TopLayer, true));
              Manager.Rotate90Sync(layer, false);
              Moves.Add(new LayerMove(layer, false));
            }
            else
            {
              for (int i = 0; i < 2; i++) Manager.Rotate90Sync(layer, true);
              Moves.Add(new LayerMove(layer, false));
              Manager.Rotate90Sync(Cube3D.RubikPosition.TopLayer, true);
              Moves.Add(new LayerMove(Cube3D.RubikPosition.TopLayer, true));
              Manager.Rotate90Sync(layer, true);
              Moves.Add(new LayerMove(layer, true));
            }
          }
          if (c.Position.HasFlag(Cube3D.RubikPosition.BottomLayer)) for (int i = 0; i < 2; i++) Manager.Rotate90Sync(layer, true);

          //Rotate over target position
          Cube3D.RubikPosition targetLayer = FacePosToCubePos(standardCube.RubikCube.cubes.First(cu => ScrambledEquals(cu.Colors, c.Colors))
            .Faces.First(f => f.Color == secondColor).Position);
          while (!RefreshCube(c).Position.HasFlag(targetLayer)) { Manager.Rotate90Sync(Cube3D.RubikPosition.TopLayer, true); Moves.Add(new LayerMove(Cube3D.RubikPosition.TopLayer, true)); }

          //Rotate to target position
          for (int i = 0; i < 2; i++) { Manager.Rotate90Sync(targetLayer, true); Moves.Add(new LayerMove(targetLayer, true)); }
        }

        //Step 4.3: Flip the incorrect orientated edges with the algorithm: Fi D Ri Di
        if (c.Faces.First(f => f.Position == Face3D.FacePosition.Bottom).Color != bottomColor)
        {
          Cube3D.RubikPosition frontLayer = FacePosToCubePos(RefreshCube(c).Faces.First(f => f.Color == bottomColor).Position);
          Manager.Rotate90Sync(frontLayer, false);
          Moves.Add(new LayerMove(frontLayer, false));
          Manager.Rotate90Sync(Cube3D.RubikPosition.BottomLayer, true);
          Moves.Add(new LayerMove(Cube3D.RubikPosition.BottomLayer, true));

          Cube3D.RubikPosition rightSlice = FacePosToCubePos(RefreshCube(c).Faces.First(f => f.Color == secondColor).Position);

          Manager.Rotate90Sync(rightSlice, false);
          Moves.Add(new LayerMove(rightSlice, false));
          Manager.Rotate90Sync(Cube3D.RubikPosition.BottomLayer, false);
          Moves.Add(new LayerMove(Cube3D.RubikPosition.BottomLayer, false));
        }
      }
      Moves.Reverse();

      return new Stack<LayerMove>(Moves);
    }

    public Stack<LayerMove> CompleteFirstLayer()
    {
      List<LayerMove> Moves = new List<LayerMove>();

      //Step 1: Get the color of the bottom layer
      Color bottomColor = Manager.getFaceColor(Cube3D.RubikPosition.BottomLayer | Cube3D.RubikPosition.MiddleSlice_Sides | Cube3D.RubikPosition.MiddleSlice, Face3D.FacePosition.Bottom);

      //Step 2: Get corners with the color of the bottom face
      IEnumerable<Cube3D> bottomCorners = Manager.RubikCube.cubes.Where(c => Cube3D.isCorner(c.Position)).Where(c => c.Colors.Count(co => co == bottomColor) == 1);

      //Step 3: Complete the first layer
      foreach (Cube3D c in bottomCorners)
      {

      }
      return new Stack<LayerMove>(Moves);
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

    private Cube3D RefreshCube(Cube3D cube)
    {
      return Manager.RubikCube.cubes.First(cu => ScrambledEquals(cu.Colors, cube.Colors));
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
  }
}
