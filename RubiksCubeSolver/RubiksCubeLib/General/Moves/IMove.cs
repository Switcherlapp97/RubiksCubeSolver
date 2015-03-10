using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{

	/// <summary>
	/// Every class implementing this interface are defined by a name and the information whether it allows multiple layers
	/// </summary>
	public interface IMove
	{
		// **** PROPERTIES ***

		/// <summary>
		/// Describes the name of the move (i.e the notation)
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Describes whether the implementing class allowes multiple layers as move
		/// </summary>
		bool MultipleLayers { get; }

    /// <summary>
    /// Gets the reverse move
    /// </summary>
    IMove ReverseMove { get; }
    /// <summary>
    /// Transforms the move
    /// </summary>
    /// <param name="type">Rotation layer</param>
    IMove Transform(CubeFlag type);
	}
}
