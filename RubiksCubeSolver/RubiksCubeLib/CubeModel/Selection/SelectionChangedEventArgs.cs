using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.CubeModel
{
  public class SelectionChangedEventArgs: EventArgs 
  {
    public PositionSpec Position { get; set; }

    public SelectionChangedEventArgs(CubeFlag cubePos, FacePosition facePos)
    {
      Position = new PositionSpec() { CubePosition = cubePos, FacePosition = facePos };
    }
  }
}
