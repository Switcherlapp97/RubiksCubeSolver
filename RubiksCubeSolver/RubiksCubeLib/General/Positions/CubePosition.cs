using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{

	/// <summary>
	/// Represents the position of a cube and its orientation
	/// </summary> 
	[Serializable]
	public class CubePosition
	{

		// *** CONSTRUCTORS ***

		/// <summary>
		/// Constructor
		/// </summary>
		public CubePosition() { }

		/// <summary>
		/// Constructor with X-, Y- and ZFlag
		/// </summary>
		/// <param name="x">Defines the XFlag</param>
		/// <param name="y">Defines the YFlag</param>
		/// <param name="z">Defines the ZFlag</param>
		public CubePosition(CubeFlag x, CubeFlag y, CubeFlag z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		/// <summary>
		/// Constructor with one CubeFlag
		/// </summary>
		/// <param name="flags">Defines the CubeFlag where the X-, Y- and ZFlag are filtered out</param>
		public CubePosition(CubeFlag flags) : this(CubeFlagService.FirstXFlag(flags), CubeFlagService.FirstYFlag(flags), CubeFlagService.FirstZFlag(flags)) { }





		// *** PROPERTIES ***

		/// <summary>
		/// The XFlag of the cube
		/// </summary>
		public CubeFlag X { get; set; }

		/// <summary>
		/// The YFlag of the cube
		/// </summary>
		public CubeFlag Y { get; set; }

		/// <summary>
		/// The ZFlag of the cube
		/// </summary>
		public CubeFlag Z { get; set; }

		/// <summary>
		/// Returns all CubeFlags in one
		/// </summary>
		public CubeFlag Flags { get { return this.X | this.Y | this.Z; } }






		// **** METHODS ****

		/// <summary>
		/// Returns true if the given CubeFlag describes a corner cube 
		/// </summary>
		/// <param name="position">Defines the CubeFlag to be analyzed</param>
		/// <returns></returns>
		public static bool IsCorner(CubeFlag position)
		{
			return ((position == (CubeFlag.TopLayer | CubeFlag.FrontSlice | CubeFlag.LeftSlice))
				|| (position == (CubeFlag.TopLayer | CubeFlag.FrontSlice | CubeFlag.RightSlice))
				|| (position == (CubeFlag.TopLayer | CubeFlag.BackSlice | CubeFlag.LeftSlice))
				|| (position == (CubeFlag.TopLayer | CubeFlag.BackSlice | CubeFlag.RightSlice))
				|| (position == (CubeFlag.BottomLayer | CubeFlag.FrontSlice | CubeFlag.LeftSlice))
				|| (position == (CubeFlag.BottomLayer | CubeFlag.FrontSlice | CubeFlag.RightSlice))
				|| (position == (CubeFlag.BottomLayer | CubeFlag.BackSlice | CubeFlag.LeftSlice))
				|| (position == (CubeFlag.BottomLayer | CubeFlag.BackSlice | CubeFlag.RightSlice)));
		}

		/// <summary>
		/// Returns true if the given CubeFlag describes a edge cube 
		/// </summary>
		/// <param name="position">Defines the CubeFlag to be analyzed</param>
		/// <returns></returns>
		public static bool IsEdge(CubeFlag position)
		{
			return ((position == (CubeFlag.TopLayer | CubeFlag.FrontSlice | CubeFlag.MiddleSliceSides))
			  || (position == (CubeFlag.TopLayer | CubeFlag.BackSlice | CubeFlag.MiddleSliceSides))
			  || (position == (CubeFlag.TopLayer | CubeFlag.RightSlice | CubeFlag.MiddleSlice))
			  || (position == (CubeFlag.TopLayer | CubeFlag.LeftSlice | CubeFlag.MiddleSlice))
			  || (position == (CubeFlag.MiddleLayer | CubeFlag.FrontSlice | CubeFlag.RightSlice))
			  || (position == (CubeFlag.MiddleLayer | CubeFlag.FrontSlice | CubeFlag.LeftSlice))
			  || (position == (CubeFlag.MiddleLayer | CubeFlag.BackSlice | CubeFlag.RightSlice))
			  || (position == (CubeFlag.MiddleLayer | CubeFlag.BackSlice | CubeFlag.LeftSlice))
			  || (position == (CubeFlag.BottomLayer | CubeFlag.FrontSlice | CubeFlag.MiddleSliceSides))
			  || (position == (CubeFlag.BottomLayer | CubeFlag.BackSlice | CubeFlag.MiddleSliceSides))
			  || (position == (CubeFlag.BottomLayer | CubeFlag.RightSlice | CubeFlag.MiddleSlice))
			  || (position == (CubeFlag.BottomLayer | CubeFlag.LeftSlice | CubeFlag.MiddleSlice)));
		}

		/// <summary>
		/// Returns true if the given CubeFlag describes a center cube 
		/// </summary>
		/// <param name="position">Defines the CubeFlag to be analyzed</param>
		/// <returns></returns>
		public static bool IsCenter(CubeFlag position)
		{
			return ((position == (CubeFlag.TopLayer | CubeFlag.MiddleSlice | CubeFlag.MiddleSliceSides))
				|| (position == (CubeFlag.BottomLayer | CubeFlag.MiddleSlice | CubeFlag.MiddleSliceSides))
				|| (position == (CubeFlag.LeftSlice | CubeFlag.MiddleSlice | CubeFlag.MiddleLayer))
				|| (position == (CubeFlag.RightSlice | CubeFlag.MiddleSlice | CubeFlag.MiddleLayer))
				|| (position == (CubeFlag.FrontSlice | CubeFlag.MiddleSliceSides | CubeFlag.MiddleLayer))
				|| (position == (CubeFlag.BackSlice | CubeFlag.MiddleSliceSides | CubeFlag.MiddleLayer)));
		}


		/// <summary>
		/// Returns true if this CubeFlag has the given CubeFlag
		/// </summary>
		/// <param name="flag">Defines the CubeFlag which will be checked</param>
		/// <returns></returns>
		public bool HasFlag(CubeFlag flag)
		{
			return this.Flags.HasFlag(flag);
		}

		/// <summary>
		/// Returns the single flags in this CubeFlag
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Enum> GetFlags()
		{
			return CubeFlagService.GetFlags(Flags);
		}

    /// <summary>
    /// Calculates the next position after a layer rotation
    /// </summary>
    /// <param name="layer">Rotation layer</param>
    /// <param name="direction">Rotation direction</param>
    public void NextFlag(CubeFlag layer, bool direction)
    {
      CubeFlag newFlags = CubeFlagService.NextFlags(Flags, layer, direction);

      this.X = CubeFlagService.FirstXFlag(newFlags);
      this.Y = CubeFlagService.FirstYFlag(newFlags);
      this.Z = CubeFlagService.FirstZFlag(newFlags);
    }

    public override bool Equals(object obj)
    {
      CubePosition second = (CubePosition)obj;
      return this.X == second.X && this.Y == second.Y && this.Z == second.Z;
    }

    public override int GetHashCode()
    {
      return (int)this.X + (int)this.Y + (int)this.Z;
    }

    public override string ToString()
    {
      return Flags.ToString();
    }

	}
}
