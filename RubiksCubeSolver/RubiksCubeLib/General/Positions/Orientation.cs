using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
  /// <summary>
  /// Defines the orientation of a cube
  /// </summary>
  public enum Orientation
  {
    Correct = 0,
    Clockwise = 1,
    CounterClockwise = 2,
    None = 4
  }
}
