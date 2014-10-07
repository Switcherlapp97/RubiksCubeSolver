using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
  public class LayerMove : IMove
  {
    public string Name { get { return string.Format("{0} {1}", Layer, Direction ? "Clockwise" : "Counter-Clockwise"); } }
    public CubeFlag Layer { get; set; }
    public bool Direction { get; set; }
    public bool Twice { get; set; }
    public bool MultipleLayers { get { return false; } }

    public LayerMove(CubeFlag layer, bool direction, bool twice = false)
    {
      if (CubeFlagService.CountFlags(layer) == 1)
      {
        this.Layer = layer;
        this.Direction = direction;
        this.Twice = twice;
      }
      else throw new Exception("Impossible movement");
    }

    public static LayerMoveCollection operator &(LayerMove first, LayerMove second)
    {
      LayerMoveCollection moves = new LayerMoveCollection();
      moves.Add(first);
      moves.Add(second);
      return moves;
    }

    public static LayerMove Parse(string notation)
    {
      string layer = notation[0].ToString();
      CubeFlag rotationLayer = CubeFlag.None;
      switch (layer)
      {
        case "R":
          rotationLayer = CubeFlag.RightSlice;
          break;
        case "L":
          rotationLayer = CubeFlag.LeftSlice;
          break;
        case "U":
          rotationLayer = CubeFlag.TopLayer;
          break;
        case "D":
          rotationLayer = CubeFlag.BottomLayer;
          break;
        case "F":
          rotationLayer = CubeFlag.FrontSlice;
          break;
        case "B":
          rotationLayer = CubeFlag.BackSlice;
          break;
        case "M":
          rotationLayer = CubeFlag.MiddleSliceSides;
          break;
        case "E":
          rotationLayer = CubeFlag.MiddleLayer;
          break;
        case "S":
          rotationLayer = CubeFlag.MiddleSlice;
          break;
      }
      bool direction = notation.Length == 1;
      return new LayerMove(rotationLayer, direction);
    }

    public override string ToString()
    {
      string c = "'";
      string move = string.Empty;

      switch (Layer)
      {
        case CubeFlag.TopLayer:
          move = "U";
          break;
        case CubeFlag.MiddleLayer:
          move = "E";
          break;
        case CubeFlag.BottomLayer:
          move = "D";
          break;
        case CubeFlag.FrontSlice:
          move = "F";
          break;
        case CubeFlag.MiddleSlice:
          move = "S";
          break;
        case CubeFlag.BackSlice:
          move = "B";
          break;
        case CubeFlag.LeftSlice:
          move = "L";
          break;
        case CubeFlag.MiddleSliceSides:
          move = "M";
          break;
        case CubeFlag.RightSlice:
          move = "R";
          break;
      }
      if (!Direction) move += c;
      return move;
    }
  }
}
