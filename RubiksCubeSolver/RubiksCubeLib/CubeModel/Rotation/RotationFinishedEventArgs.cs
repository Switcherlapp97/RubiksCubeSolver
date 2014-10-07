using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.CubeModel
{
  [Serializable]
  public class RotationFinishedEventArgs: EventArgs
  {
    public RotationInfo Info { get; private set; }
    public RotationFinishedEventArgs(RotationInfo info)
    {
      Info = info;
    }
  }
}
