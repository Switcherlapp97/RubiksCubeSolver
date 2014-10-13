using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
  public interface IPluginable
  {
    /// <summary>
    /// Gets the name of the plugin
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Gets the plugin description
    /// </summary>
    string Description { get;}
  }
}
