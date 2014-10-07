using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
  public interface IPluginable
  {
    string Name { get; }
    string Description { get;}
  }
}
