using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
  public static class CubeFlagService
  {
    public static bool IsPossibleMovement(CubeFlag flags)
    {
      return GetFlags(flags).Where(x => IsXFlag((CubeFlag)x)).Count() == CountFlags(flags) ||
        GetFlags(flags).Where(y => IsYFlag((CubeFlag)y)).Count() == CountFlags(flags) ||
        GetFlags(flags).Where(z => IsZFlag((CubeFlag)z)).Count() == CountFlags(flags);
    }

    public static bool IsXFlag(CubeFlag flag)
    {
      return CubeFlag.XFlags.HasFlag(flag) && CountFlags(flag) == 1;
    }

    public static bool IsYFlag(CubeFlag flag)
    {
      return CubeFlag.YFlags.HasFlag(flag) && CountFlags(flag) == 1;
    }

    public static bool IsZFlag(CubeFlag flag)
    {
      return CubeFlag.ZFlags.HasFlag(flag) && CountFlags(flag) == 1;
    }

    public static CubeFlag FirstXFlag(CubeFlag flags)
    {
      return FirstNotInvalidFlag(flags, CubeFlag.YFlags | CubeFlag.ZFlags);
    }

    public static CubeFlag FirstYFlag(CubeFlag flags)
    {
      return FirstNotInvalidFlag(flags, CubeFlag.XFlags | CubeFlag.ZFlags);
    }

    public static CubeFlag FirstZFlag(CubeFlag flags)
    {
      return FirstNotInvalidFlag(flags, CubeFlag.XFlags | CubeFlag.YFlags);
    }

    /// <summary>
    /// Convert a cubeflag into values from -1 to 1
    /// </summary>
    public static int ToInt(CubeFlag flag)
    {
      if (IsXFlag(flag))
      {
        switch (flag)
        {
          case CubeFlag.RightSlice: return 1;
          case CubeFlag.MiddleSliceSides: return 0;
          default: return -1;
        }
      }

      else if (IsYFlag(flag))
      {
        switch (flag)
        {
          case CubeFlag.TopLayer: return -1;
          case CubeFlag.MiddleLayer: return 0;
          default: return 1;
        }
      }
      else if (IsZFlag(flag))
      {
        switch (flag)
        {
          case CubeFlag.BackSlice: return 1;
          case CubeFlag.MiddleSlice: return 0;
          default: return -1;
        }
      }
      else if (flag == CubeFlag.None) return 0;
      else throw new Exception("Flag can not be converted to an integer");
    }

    public static int CountFlags(CubeFlag flag)
    {
      return GetFlags(flag).Count() - 1;
    }

    public static IEnumerable<Enum> GetFlags(CubeFlag flags)
    {
      foreach (Enum value in Enum.GetValues(flags.GetType()))
      {
        if (flags.HasFlag(value)) yield return value;
      }
    }

    public static CubeFlag FirstNotInvalidFlag(CubeFlag flags, CubeFlag invalid)
    {
      foreach (CubeFlag f in GetFlags(flags))
      {
        if (!invalid.HasFlag(f)) return f;
      }
      return CubeFlag.None;
    }

    public static CubeFlag ExceptFlag(CubeFlag flags, CubeFlag invalid)
    {
      CubeFlag pos = CubeFlag.None;
      foreach (CubeFlag p in GetFlags(flags))
      {
        if (!invalid.HasFlag(p)) pos |= p;
      }
      return pos;
    }

    public static CubeFlag CommonFlags(CubeFlag first, CubeFlag second)
    {
      CubeFlag commonFlags = CubeFlag.None;
      foreach (CubeFlag flag in GetFlags(first))
      {
        if (second.HasFlag(flag)) commonFlags |= flag;
      }
      return commonFlags;
    }

    public static CubeFlag FromFacePosition(FacePosition facePos)
    {
      switch (facePos)
      {
        case FacePosition.Top: return CubeFlag.TopLayer;
        case FacePosition.Bottom: return CubeFlag.BottomLayer;
        case FacePosition.Left: return CubeFlag.LeftSlice;
        case FacePosition.Right: return CubeFlag.RightSlice;
        case FacePosition.Back: return CubeFlag.BackSlice;
        case FacePosition.Front: return CubeFlag.FrontSlice;
        default: return CubeFlag.None;
      }
    }

    public static FacePosition ToFacePosition(CubeFlag cubePos)
    {
      switch (cubePos)
      {
        case CubeFlag.TopLayer: return FacePosition.Top;
        case CubeFlag.BottomLayer: return FacePosition.Bottom;
        case CubeFlag.LeftSlice: return FacePosition.Left;
        case CubeFlag.RightSlice: return FacePosition.Right;
        case CubeFlag.BackSlice: return FacePosition.Back;
        case CubeFlag.FrontSlice: return FacePosition.Front;
        default: return FacePosition.None;
      }
    }

    public static CubeFlag GetFirstNotInvalidCommonFlag(CubeFlag a, CubeFlag b, CubeFlag exclude)
    {
      return FirstNotInvalidFlag(CommonFlags(a, b), exclude);
    }

    public static CubeFlag RemoveFlag(CubeFlag flags, CubeFlag flagToRemove)
    {
      return flags &= ~flagToRemove;
    }

    public static CubeFlag GetOppositeFlag(CubeFlag flag)
    {
      switch (flag)
      {
        case CubeFlag.TopLayer: return CubeFlag.BottomLayer;
        case CubeFlag.BottomLayer: return CubeFlag.TopLayer;
        case CubeFlag.FrontSlice: return CubeFlag.BackSlice;
        case CubeFlag.BackSlice: return CubeFlag.FrontSlice;
        case CubeFlag.LeftSlice: return CubeFlag.RightSlice;
        case CubeFlag.RightSlice: return CubeFlag.LeftSlice;
        default: return CubeFlag.None;
      }
    }

    public static FacePosition GetOppositeFace(FacePosition position)
    {
      switch (position)
      {
        case FacePosition.Top:
          return FacePosition.Bottom;
        case FacePosition.Bottom:
          return FacePosition.Top;
        case FacePosition.Left:
          return FacePosition.Right;
        case FacePosition.Right:
          return FacePosition.Left;
        case FacePosition.Back:
          return FacePosition.Front;
        case FacePosition.Front:
          return FacePosition.Back;
        default:
          return FacePosition.None;
      }
    }

    public static string CubeFlagToString(CubeFlag flag)
    {
      switch (flag)
      {
        case CubeFlag.TopLayer:
          return "U";
        case CubeFlag.MiddleLayer:
          return "E";
        case CubeFlag.BottomLayer:
          return "D";
        case CubeFlag.FrontSlice:
          return "F";
        case CubeFlag.MiddleSlice:
          return "S";
        case CubeFlag.BackSlice:
          return "B";
        case CubeFlag.LeftSlice:
          return "L";
        case CubeFlag.MiddleSliceSides:
          return "M";
        case CubeFlag.RightSlice:
          return "R";
        default:
          return string.Empty;
      }
    }
  }
}
