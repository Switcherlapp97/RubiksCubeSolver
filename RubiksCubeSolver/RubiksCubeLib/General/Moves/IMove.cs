using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
  public interface IMove
  {
    string Name { get; }
    bool MultipleLayers { get; }
  }
}
