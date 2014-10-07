using RubiksCubeLib.RubiksCube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.ScanInput
{
  public class ScanEventArgs: EventArgs
  {
    public Rubik Rubik { get; set; }
  }
}
