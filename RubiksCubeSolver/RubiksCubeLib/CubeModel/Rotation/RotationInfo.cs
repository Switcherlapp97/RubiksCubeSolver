using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.CubeModel
{
  /// <summary>
  /// Represents all the requiered information to perform an animated layer rotation
  /// </summary>
  public class RotationInfo
  {
    /// <summary>
    /// Gets or sets the duration of animated rotation
    /// </summary>
    public int Milliseconds { get; set; }
    /// <summary>
    /// Gets or sets the collection of animated layer movements
    /// </summary>
    public List<AnimatedLayerMove> Moves { get; set; }

    public string Name { get; private set; }

    /// <summary>
    /// Initializes a new instance of the RotationInfo class
    /// </summary>
    /// <param name="move">Move or move collection that will be rotated animated</param>
    /// <param name="milliseconds">Duration of animated rotation</param>
    public RotationInfo(IMove move, int milliseconds)
    {
      Milliseconds = milliseconds;
      this.Name = move.Name;
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

  /// <summary>
  /// Represents an animated layer move
  /// </summary>
  public class AnimatedLayerMove
  {
    /// <summary>
    /// Gets the target angle of rotation in degrees
    /// </summary>
    public int Target { get; set; }

    /// <summary>
    /// Gets the layer move of the animated rotation
    /// </summary>
    public LayerMove Move { get; set; }

    /// <summary>
    /// Initializes a new instance of the AnimatedLayerMove class
    /// </summary>
    /// <param name="move">Movement that will be performed</param>
    public AnimatedLayerMove(LayerMove move)
    {
      bool d = move.Direction;
      if (move.Layer == CubeFlag.TopLayer || move.Layer == CubeFlag.MiddleLayer || move.Layer == CubeFlag.LeftSlice || move.Layer == CubeFlag.FrontSlice || move.Layer == CubeFlag.MiddleSlice) d = !d;
      int rotationTarget = move.Twice ? 180 : 90;
      if (d) rotationTarget *= -1;
      Target = rotationTarget;

      Move = new LayerMove(move.Layer, move.Direction, move.Twice);
    }
  }
}
