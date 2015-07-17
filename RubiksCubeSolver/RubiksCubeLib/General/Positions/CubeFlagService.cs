using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{

  /// <summary>
  /// Represents a service to analyze CubeFlags, CubePositions and FacePositions
  /// </summary>
  public static class CubeFlagService
  {

    // **** METHODS ****

    /// <summary>
    /// Returns true if the given CubeFlag contains a valid move
    /// </summary>
    /// <param name="flags">Defines the CubeFlag to be analyzed</param>
    /// <returns></returns>
    public static bool IsPossibleMove(CubeFlag flags)
    {
      flags = ExceptFlag(flags, CubeFlag.None);
      return GetFlags(flags).All(f => IsXFlag((CubeFlag)f)) ||
        GetFlags(flags).All(f => IsYFlag((CubeFlag)f)) ||
        GetFlags(flags).All(f => IsZFlag((CubeFlag)f));
    }

    /// <summary>
    /// Returns true if the flag is an XFlag only
    /// </summary>
    /// <param name="flag">Defines the CubeFlag to be analyzed</param>
    /// <returns></returns>
    public static bool IsXFlag(CubeFlag flag)
    {
      return CubeFlag.XFlags.HasFlag(flag);
    }

    /// <summary>
    /// Returns true if the flag is a YFlag only
    /// </summary>
    /// <param name="flag">Defines the CubeFlag to be analyzed</param>
    /// <returns></returns>
    public static bool IsYFlag(CubeFlag flag)
    {
      return CubeFlag.YFlags.HasFlag(flag);
    }

    /// <summary>
    /// Returns true if the flag is a ZFlag only
    /// </summary>
    /// <param name="flag">Defines the CubeFlag to be analyzed</param>
    /// <returns></returns>
    public static bool IsZFlag(CubeFlag flag)
    {
      return CubeFlag.ZFlags.HasFlag(flag);
    }

    /// <summary>
    /// Returns the first XFlag in the given CubeFlag
    /// </summary>
    /// <param name="flags">Defines the CubeFlag to be analyzed</param>
    /// <returns></returns>
    public static CubeFlag FirstXFlag(CubeFlag flags)
    {
      return FirstNotInvalidFlag(flags, CubeFlag.YFlags | CubeFlag.ZFlags);
    }

    /// <summary>
    /// Returns the first YFlag in the given CubeFlag
    /// </summary>
    /// <param name="flags">Defines the CubeFlag to be analyzed</param>
    /// <returns></returns>
    public static CubeFlag FirstYFlag(CubeFlag flags)
    {
      return FirstNotInvalidFlag(flags, CubeFlag.XFlags | CubeFlag.ZFlags);
    }

    /// <summary>
    /// Returns the first ZFlag in the given CubeFlag
    /// </summary>
    /// <param name="flags">Defines the CubeFlag to be analyzed</param>
    /// <returns></returns>
    public static CubeFlag FirstZFlag(CubeFlag flags)
    {
      return FirstNotInvalidFlag(flags, CubeFlag.XFlags | CubeFlag.YFlags);
    }



    /// <summary>
    /// Converts a CubeFlag into values from -1 to 1
    /// </summary>
    /// <exception cref="System.Exception">Thrown when the CubeFlag is either invalid or has more than one flag</exception>
    public static int ToInt(CubeFlag flag)
    {
      if (IsXFlag(flag))
      {
        switch (flag)
        {
          case CubeFlag.RightSlice:
            return 1;
          case CubeFlag.MiddleSliceSides:
            return 0;
          default:
            return -1;
        }
      }

      else if (IsYFlag(flag))
      {
        switch (flag)
        {
          case CubeFlag.TopLayer:
            return -1;
          case CubeFlag.MiddleLayer:
            return 0;
          default:
            return 1;
        }
      }
      else if (IsZFlag(flag))
      {
        switch (flag)
        {
          case CubeFlag.BackSlice:
            return 1;
          case CubeFlag.MiddleSlice:
            return 0;
          default:
            return -1;
        }
      }
      else if (flag == CubeFlag.None)
        return 0;
      else
        throw new Exception("Flag can not be converted to an integer");
    }

    /// <summary>
    /// Returns the amount of flags in the parameter
    /// </summary>
    /// <param name="flag">Defines the flag to be counted out</param>
    /// <returns></returns>
    public static int CountFlags(CubeFlag flag)
    {
      return GetFlags(flag).Count() - 1;
    }

    /// <summary>
    /// Yields each single flag within the parameter
    /// </summary>
    /// <param name="flags">Defines the flag to be 'splitted'</param>
    /// <returns></returns>
    public static IEnumerable<Enum> GetFlags(CubeFlag flags)
    {
      foreach (Enum value in Enum.GetValues(flags.GetType()))
      {
        if (flags.HasFlag(value))
          yield return value;
      }
    }



    /// <summary>
    /// Returns the first flag in the first parameter which the second parameter does not contain
    /// </summary>
    /// <param name="flags">Defines the posiible flags to be returned</param>
    /// <param name="invalid">Defines the invalid flags</param>
    /// <returns></returns>
    public static CubeFlag FirstNotInvalidFlag(CubeFlag flags, CubeFlag invalid)
    {
      foreach (CubeFlag f in GetFlags(flags))
      {
        if (!invalid.HasFlag(f))
          return f;
      }
      return CubeFlag.None;
    }

    /// <summary>
    /// Returns a ClubFlag which contains all single flags in the first parameter which don't exist in the second parameter
    /// </summary>
    /// <param name="flags">Defines all possible flags</param>
    /// <param name="invalid">Defines the flags to be filtered out of the first parameter</param>
    /// <returns></returns>
    public static CubeFlag ExceptFlag(CubeFlag flags, CubeFlag invalid)
    {
      CubeFlag pos = CubeFlag.None;
      foreach (CubeFlag p in GetFlags(flags))
      {
        if (!invalid.HasFlag(p))
          pos |= p;
      }
      return pos;
    }

    /// <summary>
    /// Returns a CubeFlag which contains all flags which exist in both the first and the second parameter
    /// </summary>
    /// <param name="first">Defines the first CubeFlag</param>
    /// <param name="second">Defines the second CubeFlag</param>
    /// <returns></returns>
    public static CubeFlag CommonFlags(CubeFlag first, CubeFlag second)
    {
      CubeFlag commonFlags = CubeFlag.None;
      foreach (CubeFlag flag in GetFlags(first))
      {
        if (second.HasFlag(flag))
          commonFlags |= flag;
      }
      return commonFlags;
    }

    /// <summary>
    /// Converts a FacePosition into a CubeFlag
    /// </summary>
    /// <param name="facePos">Defines the FacePostion to be converted</param>
    /// <returns></returns>
    public static CubeFlag FromFacePosition(FacePosition facePos)
    {
      switch (facePos)
      {
        case FacePosition.Top:
          return CubeFlag.TopLayer;
        case FacePosition.Bottom:
          return CubeFlag.BottomLayer;
        case FacePosition.Left:
          return CubeFlag.LeftSlice;
        case FacePosition.Right:
          return CubeFlag.RightSlice;
        case FacePosition.Back:
          return CubeFlag.BackSlice;
        case FacePosition.Front:
          return CubeFlag.FrontSlice;
        default:
          return CubeFlag.None;
      }
    }

    /// <summary>
    /// Converts a CubeFlag into a FacePosition
    /// </summary>
    /// <param name="cubePos">Defines the CubeFlag to be converted</param>
    /// <returns></returns>
    public static FacePosition ToFacePosition(CubeFlag cubePos)
    {
      switch (cubePos)
      {
        case CubeFlag.TopLayer:
          return FacePosition.Top;
        case CubeFlag.BottomLayer:
          return FacePosition.Bottom;
        case CubeFlag.LeftSlice:
          return FacePosition.Left;
        case CubeFlag.RightSlice:
          return FacePosition.Right;
        case CubeFlag.BackSlice:
          return FacePosition.Back;
        case CubeFlag.FrontSlice:
          return FacePosition.Front;
        default:
          return FacePosition.None;
      }
    }

    /// <summary>
    /// Returns the first flag which exists in the first and second parameter but not in the third parameter
    /// </summary>
    /// <param name="a">Defines the first CubeFlag</param>
    /// <param name="b">Defines the second CubeFlag</param>
    /// <param name="exclude">Defines the CubeFlag which is not allowed</param>
    /// <returns></returns>
    public static CubeFlag GetFirstNotInvalidCommonFlag(CubeFlag a, CubeFlag b, CubeFlag exclude)
    {
      return FirstNotInvalidFlag(CommonFlags(a, b), exclude);
    }


    /// <summary>
    /// Returns a CubeFlag which contains all flags of the first parameter except the ones of the second one
    /// </summary>
    /// <param name="flags">Defines all flags</param>
    /// <param name="flagToRemove">Defines the flags to be removed</param>
    /// <returns></returns>
    public static CubeFlag RemoveFlag(CubeFlag flags, CubeFlag flagToRemove)
    {
      return flags &= ~flagToRemove;
    }

    /// <summary>
    /// Returns a CubeFlag which is the opposite of the parameter
    /// </summary>
    /// <param name="flag">Defines the parameter to be analyzed</param>
    /// <returns></returns>
    public static CubeFlag GetOppositeFlag(CubeFlag flag)
    {
      switch (flag)
      {
        case CubeFlag.TopLayer:
          return CubeFlag.BottomLayer;
        case CubeFlag.BottomLayer:
          return CubeFlag.TopLayer;
        case CubeFlag.FrontSlice:
          return CubeFlag.BackSlice;
        case CubeFlag.BackSlice:
          return CubeFlag.FrontSlice;
        case CubeFlag.LeftSlice:
          return CubeFlag.RightSlice;
        case CubeFlag.RightSlice:
          return CubeFlag.LeftSlice;
        case CubeFlag.MiddleLayer:
        case CubeFlag.MiddleSlice:
        case CubeFlag.MiddleSliceSides:
          return flag;
        default:
          return CubeFlag.None;
      }
    }

    /// <summary>
    /// Returns a FacePosition which is the opposite of the parameter
    /// </summary>
    /// <param name="flag">Defines the parameter to be analyzed</param>
    /// <returns></returns>
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



    /// <summary>
    /// Converts the given CubeFlag into a notation string
    /// </summary>
    public static string ToNotationString(CubeFlag flag)
    {
      string notation = string.Empty;

      if (flag.HasFlag(CubeFlag.TopLayer)) notation += "U";
      if (flag.HasFlag(CubeFlag.BottomLayer)) notation += "D";
      if (flag.HasFlag(CubeFlag.FrontSlice)) notation += "F";
      if (flag.HasFlag(CubeFlag.RightSlice)) notation += "R";
      if (flag.HasFlag(CubeFlag.BackSlice)) notation += "B";
      if (flag.HasFlag(CubeFlag.LeftSlice)) notation += "L";
      if (flag.HasFlag(CubeFlag.MiddleSlice)) notation += "S";
      if (flag.HasFlag(CubeFlag.MiddleSliceSides)) notation += "M";
      if (flag.HasFlag(CubeFlag.MiddleLayer)) notation += "E";

      return notation;
    }

    /// <summary>
    /// Parses a notation into a collection of cube flags
    /// </summary>
    public static CubeFlag Parse(string notation)
    {
      CubeFlag flags = CubeFlag.None;
      for (int i = 0; i < notation.Length; i++)
      {
        switch (notation[i])
        {
          case 'R':
            flags |= CubeFlag.RightSlice;
            break;
          case 'L':
            flags |= CubeFlag.LeftSlice;
            break;
          case 'U':
            flags |= CubeFlag.TopLayer;
            break;
          case 'D':
            flags |= CubeFlag.BottomLayer;
            break;
          case 'F':
            flags |= CubeFlag.FrontSlice;
            break;
          case 'B':
            flags |= CubeFlag.BackSlice;
            break;
          case 'M':
            flags |= CubeFlag.MiddleSliceSides;
            break;
          case 'E':
            flags |= CubeFlag.MiddleLayer;
            break;
          case 'S':
            flags |= CubeFlag.MiddleSlice;
            break;
        }
      }
      return flags;
    }

    /// <summary>
    /// Returns the next flag after a layer rotation
    /// </summary>
    /// <param name="rotationLayer">Rotation layer</param>
    /// <param name="direction">Rotation direction</param>
    /// <returns>Next cube flag</returns>
    public static CubeFlag NextCubeFlag(CubeFlag flag, CubeFlag rotationLayer, bool direction = true)
    {
      CubeFlag nextFlag = CubeFlag.None;

      if (CountFlags(flag) == 1)
      {
        if (CubeFlagService.IsXFlag(rotationLayer))
        {
          if (rotationLayer == CubeFlag.LeftSlice) direction = !direction;
          if (!direction && !IsXFlag(flag)) flag = CubeFlagService.GetOppositeFlag(flag);
          switch (flag)
          {
            case CubeFlag.FrontSlice: nextFlag = CubeFlag.TopLayer; break;
            case CubeFlag.MiddleSlice: nextFlag = CubeFlag.MiddleLayer; break;
            case CubeFlag.BackSlice: nextFlag = CubeFlag.BottomLayer; break;
            case CubeFlag.TopLayer: nextFlag = CubeFlag.BackSlice; break;
            case CubeFlag.MiddleLayer: nextFlag = CubeFlag.MiddleSlice; break;
            case CubeFlag.BottomLayer: nextFlag = CubeFlag.FrontSlice; break;
            default: nextFlag = flag; break;
          }
        }
        else if (CubeFlagService.IsYFlag(rotationLayer))
        {
          if (rotationLayer == CubeFlag.BottomLayer) direction = !direction;
          if (!direction && !IsYFlag(flag)) flag = CubeFlagService.GetOppositeFlag(flag);
          switch (flag)
          {
            case CubeFlag.FrontSlice: nextFlag = CubeFlag.LeftSlice; break;
            case CubeFlag.MiddleSlice: nextFlag = CubeFlag.MiddleSliceSides; break;
            case CubeFlag.BackSlice: nextFlag = CubeFlag.RightSlice; break;
            case CubeFlag.LeftSlice: nextFlag = CubeFlag.BackSlice; break;
            case CubeFlag.MiddleSliceSides: nextFlag = CubeFlag.MiddleSlice; break;
            case CubeFlag.RightSlice: nextFlag = CubeFlag.FrontSlice; break;
            default: nextFlag = flag; break;
          }
        }
        else if (CubeFlagService.IsZFlag(rotationLayer))
        {
          if (rotationLayer == CubeFlag.BackSlice) direction = !direction;
          if (!direction && !IsZFlag(flag)) flag = CubeFlagService.GetOppositeFlag(flag);
          switch (flag)
          {
            case CubeFlag.TopLayer: nextFlag = CubeFlag.RightSlice; break;
            case CubeFlag.MiddleLayer: nextFlag = CubeFlag.MiddleSliceSides; break;
            case CubeFlag.BottomLayer: nextFlag = CubeFlag.LeftSlice; break;
            case CubeFlag.LeftSlice: nextFlag = CubeFlag.TopLayer; break;
            case CubeFlag.MiddleSliceSides: nextFlag = CubeFlag.MiddleLayer; break;
            case CubeFlag.RightSlice: nextFlag = CubeFlag.BottomLayer; break;
            default: nextFlag = flag; break;
          }
        }
      }
      return CubeFlagService.ExceptFlag(nextFlag, CubeFlag.None);
    }

    /// <summary>
    /// Returns the next flags of the old flags after a layer rotation
    /// </summary>
    /// <param name="rotationLayer">Rotation layer</param>
    /// <param name="direction">Rotation direction</param>
    /// <returns>Next cube flags</returns>
    public static CubeFlag NextFlags(CubeFlag flags, CubeFlag rotationLayer, bool direction = true)
    {
      CubeFlag newFlags = CubeFlag.None;
      IEnumerable<Enum> lstFlags = GetFlags(flags);
      foreach (CubeFlag flag in lstFlags)
      {
        newFlags |= CubeFlagService.NextCubeFlag(flag, rotationLayer, direction);
      }

      return newFlags;
    }
  }
}
