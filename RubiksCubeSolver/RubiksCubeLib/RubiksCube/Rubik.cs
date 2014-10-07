using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RubiksCubeLib.ScanInput;
using System.Xml.Serialization;

namespace RubiksCubeLib.RubiksCube
{
  [Serializable]
  public class Rubik
  {
    public List<Cube> Cubes { get; set; }

    public Color FrontColor { get { return GetFaceColor(CubeFlag.FrontSlice | CubeFlag.MiddleSliceSides | CubeFlag.MiddleLayer, FacePosition.Front); } }
    public Color BackColor { get { return GetFaceColor(CubeFlag.BackSlice | CubeFlag.MiddleSliceSides | CubeFlag.MiddleLayer, FacePosition.Back); } }
    public Color TopColor { get { return GetFaceColor(CubeFlag.TopLayer | CubeFlag.MiddleSliceSides | CubeFlag.MiddleSlice, FacePosition.Top); } }
    public Color BottomColor { get { return GetFaceColor(CubeFlag.BottomLayer | CubeFlag.MiddleSliceSides | CubeFlag.MiddleSlice, FacePosition.Bottom); } }
    public Color RightColor { get { return GetFaceColor(CubeFlag.RightSlice | CubeFlag.MiddleLayer | CubeFlag.MiddleSlice, FacePosition.Right); } }
    public Color LeftColor { get { return GetFaceColor(CubeFlag.LeftSlice | CubeFlag.MiddleSlice | CubeFlag.MiddleLayer, FacePosition.Left); } }

    public Color[] Colors { get; set; }

    public Rubik() : this(Color.Orange, Color.Red, Color.Yellow, Color.White, Color.Blue, Color.Green) { }

    public Rubik(Color cfront, Color cback, Color ctop, Color cbottom, Color cright, Color cleft)
    {
      Colors = new Color[] { cfront, cback, ctop, cbottom, cright, cleft };
      Cubes = new List<Cube>();
      for (int i = -1; i <= 1; i++)
      {
        for (int j = -1; j <= 1; j++)
        {
          for (int k = -1; k <= 1; k++)
          {
            Cubes.Add(new Cube(GenSideFlags(i, j, k)));
          }
        }
      }
      SetFaceColor(CubeFlag.FrontSlice, FacePosition.Front, cfront);
      SetFaceColor(CubeFlag.BackSlice, FacePosition.Back, cback);
      SetFaceColor(CubeFlag.TopLayer, FacePosition.Top, ctop);
      SetFaceColor(CubeFlag.BottomLayer, FacePosition.Bottom, cbottom);
      SetFaceColor(CubeFlag.RightSlice, FacePosition.Right, cright);
      SetFaceColor(CubeFlag.LeftSlice, FacePosition.Left, cleft);
    }

    public Rubik DeepClone()
    {
      using (var ms = new MemoryStream())
      {
        var formatter = new BinaryFormatter();
        formatter.Serialize(ms, this);
        ms.Position = 0;

        return (Rubik)formatter.Deserialize(ms);
      }
    }

    public CubeFlag GenSideFlags(int i, int j, int k)
    {
      CubeFlag p = new CubeFlag();
      switch (i)
      {
        case -1: p |= CubeFlag.LeftSlice; break;
        case 0: p |= CubeFlag.MiddleSliceSides; break;
        case 1: p |= CubeFlag.RightSlice; break;
      }
      switch (j)
      {
        case -1: p |= CubeFlag.TopLayer; break;
        case 0: p |= CubeFlag.MiddleLayer; break;
        case 1: p |= CubeFlag.BottomLayer; break;
      }
      switch (k)
      {
        case -1: p |= CubeFlag.FrontSlice; break;
        case 0: p |= CubeFlag.MiddleSlice; break;
        case 1: p |= CubeFlag.BackSlice; break;
      }
      return p;
    }

    public void SetFaceColor(CubeFlag affected, FacePosition face, Color color)
    {
      Cubes.Where(c => c.Position.HasFlag(affected)).ToList().ForEach(c => c.SetFaceColor(face, color));
      Cubes.ToList().ForEach(c => { c.Colors.Clear(); c.Faces.ToList().ForEach(f => c.Colors.Add(f.Color)); });
    }

    public Color GetFaceColor(CubeFlag position, FacePosition face)
    {
      return Cubes.First(c => c.Position.Flags == position).GetFaceColor(face);
    }

    public void RotateLayer(IMove move)
    {
      if (move.MultipleLayers)
      {
        LayerMoveCollection moves = (LayerMoveCollection)move;
        foreach (LayerMove m in moves)
        {
          RotateLayer(m);
        }
      }
      else RotateLayer((LayerMove)move);
    }

    private void RotateLayer(LayerMove move)
    {
      int repititions = move.Twice ? 2 : 1;
      for (int i = 0; i < 1; i++)
      {
        IEnumerable<Cube> affected = Cubes.Where(c => c.Position.HasFlag(move.Layer));
        affected.ToList().ForEach(c => c.NextPos(move.Layer, move.Direction));
      }
    }

    public void RotateCube(RotationType type, bool direction)
    {
      switch (type)
      {
        case RotationType.X:
          RotateLayer(new LayerMove(CubeFlag.RightSlice, direction) & new LayerMove(CubeFlag.MiddleSliceSides, direction) & new LayerMove(CubeFlag.LeftSlice, !direction));
          break;
        case RotationType.Y:
          RotateLayer(new LayerMove(CubeFlag.TopLayer, direction) & new LayerMove(CubeFlag.MiddleLayer, direction) & new LayerMove(CubeFlag.BottomLayer, !direction));
          break;
        case RotationType.Z:
          RotateLayer(new LayerMove(CubeFlag.FrontSlice, direction) & new LayerMove(CubeFlag.MiddleSlice, direction) & new LayerMove(CubeFlag.BackSlice, !direction));
          break;
      }
    }

    public void RotateLayer(CubeFlag layer, bool direction)
    {
      IEnumerable<Cube> affected = Cubes.Where(c => c.Position.HasFlag(layer));
      affected.ToList().ForEach(c => c.NextPos(layer, direction));
    }

    public void Scramble(int moves)
    {
      Random rnd = new Random();
      for (int i = 0; i < 50; i++) RotateLayer(new LayerMove((CubeFlag)Math.Pow(2, rnd.Next(0, 9)), Convert.ToBoolean(rnd.Next(0, 2))));
    }

    public int CountCornersWithCorrectOrientation()
    {
      Color topColor = GetFaceColor(CubeFlag.TopLayer | CubeFlag.MiddleSliceSides | CubeFlag.MiddleSlice, FacePosition.Top);
      return Cubes.Count(c => c.IsCorner && c.Faces.First(f => f.Position == FacePosition.Top).Color == topColor);
    }

    public int CountEdgesWithCorrectOrientation()
    {
      Color topColor = GetFaceColor(CubeFlag.TopLayer | CubeFlag.MiddleSliceSides | CubeFlag.MiddleSlice, FacePosition.Top);
      return Cubes.Count(c => c.IsEdge && c.Faces.First(f => f.Position == FacePosition.Top).Color == topColor);
    }

    public Rubik GenStandardCube()
    {
      Rubik standardCube = new Rubik();
      standardCube.SetFaceColor(CubeFlag.TopLayer, FacePosition.Top, TopColor);
      standardCube.SetFaceColor(CubeFlag.BottomLayer, FacePosition.Bottom, BottomColor);
      standardCube.SetFaceColor(CubeFlag.RightSlice, FacePosition.Right, RightColor);
      standardCube.SetFaceColor(CubeFlag.LeftSlice, FacePosition.Left, LeftColor);
      standardCube.SetFaceColor(CubeFlag.FrontSlice, FacePosition.Front, FrontColor);
      standardCube.SetFaceColor(CubeFlag.BackSlice, FacePosition.Back, BackColor);

      return standardCube;
    }

    public CubeFlag GetTargetFlags(Cube cube)
    {
      return GenStandardCube().Cubes.First(cu => ScrambledEquals(cu.Colors, cube.Colors)).Position.Flags;
    }

    protected bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
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

    public static Rubik Scan(CubeScanner scanner)
    {
      return scanner.Scan();
    }
  }
}
