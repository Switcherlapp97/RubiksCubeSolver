using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
  [Serializable]
  public class CubePosition
  {
    public CubeFlag X { get; set; }
    public CubeFlag Y { get; set; }
    public CubeFlag Z { get; set; }
    public CubeFlag Flags { get { return X | Y | Z; } }

    public CubePosition() { }

    public CubePosition(CubeFlag x, CubeFlag y, CubeFlag z)
    {
      this.X = x;
      this.Y = y;
      this.Z = z;
    }

    public CubePosition(CubeFlag flags) : this(CubeFlagService.FirstXFlag(flags), CubeFlagService.FirstYFlag(flags), CubeFlagService.FirstZFlag(flags)) { }

    public static bool IsCorner(CubeFlag Position)
    {
      return ((Position == (CubeFlag.TopLayer | CubeFlag.FrontSlice | CubeFlag.LeftSlice))
          || (Position == (CubeFlag.TopLayer | CubeFlag.FrontSlice | CubeFlag.RightSlice))
          || (Position == (CubeFlag.TopLayer | CubeFlag.BackSlice | CubeFlag.LeftSlice))
          || (Position == (CubeFlag.TopLayer | CubeFlag.BackSlice | CubeFlag.RightSlice))
          || (Position == (CubeFlag.BottomLayer | CubeFlag.FrontSlice | CubeFlag.LeftSlice))
          || (Position == (CubeFlag.BottomLayer | CubeFlag.FrontSlice | CubeFlag.RightSlice))
          || (Position == (CubeFlag.BottomLayer | CubeFlag.BackSlice | CubeFlag.LeftSlice))
          || (Position == (CubeFlag.BottomLayer | CubeFlag.BackSlice | CubeFlag.RightSlice)));
    }

    public static bool IsEdge(CubeFlag Position)
    {
      return ((Position == (CubeFlag.TopLayer | CubeFlag.FrontSlice | CubeFlag.MiddleSliceSides))
        || (Position == (CubeFlag.TopLayer | CubeFlag.BackSlice | CubeFlag.MiddleSliceSides))
        || (Position == (CubeFlag.TopLayer | CubeFlag.RightSlice | CubeFlag.MiddleSlice))
        || (Position == (CubeFlag.TopLayer | CubeFlag.LeftSlice | CubeFlag.MiddleSlice))
        || (Position == (CubeFlag.MiddleLayer | CubeFlag.FrontSlice | CubeFlag.RightSlice))
        || (Position == (CubeFlag.MiddleLayer | CubeFlag.FrontSlice | CubeFlag.LeftSlice))
        || (Position == (CubeFlag.MiddleLayer | CubeFlag.BackSlice | CubeFlag.RightSlice))
        || (Position == (CubeFlag.MiddleLayer | CubeFlag.BackSlice | CubeFlag.LeftSlice))
        || (Position == (CubeFlag.BottomLayer | CubeFlag.FrontSlice | CubeFlag.MiddleSliceSides))
        || (Position == (CubeFlag.BottomLayer | CubeFlag.BackSlice | CubeFlag.MiddleSliceSides))
        || (Position == (CubeFlag.BottomLayer | CubeFlag.RightSlice | CubeFlag.MiddleSlice))
        || (Position == (CubeFlag.BottomLayer | CubeFlag.LeftSlice | CubeFlag.MiddleSlice)));
    }

    public static bool IsCenter(CubeFlag Position)
    {
      return ((Position == (CubeFlag.TopLayer | CubeFlag.MiddleSlice | CubeFlag.MiddleSliceSides))
          || (Position == (CubeFlag.BottomLayer | CubeFlag.MiddleSlice | CubeFlag.MiddleSliceSides))
          || (Position == (CubeFlag.LeftSlice | CubeFlag.MiddleSlice | CubeFlag.MiddleLayer))
          || (Position == (CubeFlag.RightSlice | CubeFlag.MiddleSlice | CubeFlag.MiddleLayer))
          || (Position == (CubeFlag.FrontSlice | CubeFlag.MiddleSliceSides | CubeFlag.MiddleLayer))
          || (Position == (CubeFlag.BackSlice | CubeFlag.MiddleSliceSides | CubeFlag.MiddleLayer)));
    }

    public bool HasFlag(CubeFlag flag)
    {
      return Flags.HasFlag(flag);
    }

    public IEnumerable<Enum> GetFlags()
    {
      return CubeFlagService.GetFlags(Flags);
    }
  }
}
