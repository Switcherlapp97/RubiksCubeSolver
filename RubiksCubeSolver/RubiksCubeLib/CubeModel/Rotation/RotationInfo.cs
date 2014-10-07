using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.CubeModel
{
   public class RotationInfo
  {
    public int Milliseconds { get; set; }
    public List<AnimatedLayerMove> Moves { get; set; }
    public string Description { get; private set; }

    public RotationInfo(IMove move, int milliseconds)
    {
      Description = move.Name;
      Milliseconds = milliseconds;
      Moves = new List<AnimatedLayerMove>();
      if (move.MultipleLayers)
      {
        foreach (LayerMove m in (LayerMoveCollection)move)
        {
          Moves.Add(new AnimatedLayerMove(m));
        }
      }
      else
      {
        Moves.Add(new AnimatedLayerMove((LayerMove)move));
      }
    }
  }

  public class AnimatedLayerMove
  {
    public int Target { get; set; }
    public LayerMove Move { get; set; }

    public AnimatedLayerMove(LayerMove move)
    {
      bool d = move.Direction;
      if (move.Layer == CubeFlag.TopLayer || move.Layer == CubeFlag.MiddleLayer || move.Layer == CubeFlag.LeftSlice || move.Layer == CubeFlag.FrontSlice || move.Layer == CubeFlag.MiddleSlice) d = !d;
      int rotationTarget = 90;
      if (d) rotationTarget = -90;
      Target = rotationTarget;

      Move = new LayerMove(move.Layer, move.Direction);
    }
  }
}
