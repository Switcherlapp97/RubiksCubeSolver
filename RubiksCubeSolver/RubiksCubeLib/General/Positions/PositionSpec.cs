using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
  public struct PositionSpec
  {
    public CubeFlag CubePosition { get; set; }
    public FacePosition FacePosition { get; set; }

    public static PositionSpec Default
    {
      get
      {
        return new PositionSpec() { CubePosition = CubeFlag.None, FacePosition = FacePosition.None };
      }
    }

    public bool Equals(PositionSpec compare)
    {
      return (compare.CubePosition == CubePosition && compare.FacePosition == FacePosition);
    }

    public bool IsDefault
    {
      get
      {
        return (CubePosition == CubeFlag.None || FacePosition == FacePosition.None);
      }
    }
  }
}
