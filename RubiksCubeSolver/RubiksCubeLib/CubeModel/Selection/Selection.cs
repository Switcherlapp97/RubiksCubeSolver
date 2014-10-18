using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.CubeModel
{
	/// <summary>
	/// All sorts of possible face selections (multiple values permitted)
	/// </summary>
	[Flags]
	public enum Selection
	{
		None = 0,
		Second = 1,
		First = 2,
		Possible = 4,
		NotPossible = 8
	}
}
