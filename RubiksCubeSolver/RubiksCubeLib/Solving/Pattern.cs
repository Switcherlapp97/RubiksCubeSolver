using RubiksCubeLib.RubiksCube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.Solver
{
  /// <summary>
  /// Represents a pattern of cubes at a specific position and a specific orientation
  /// </summary>
  public class Pattern
  {
    #region Position definitions
    /// <summary>
    /// Returns all possible edge positions
    /// </summary>
    public static List<PositionOrientation> EdgePositions
    {
      get
      {
        return new List<PositionOrientation>()
        {
          new PositionOrientation() {Position = CubeFlag.MiddleSliceSides| CubeFlag.BackSlice|CubeFlag.TopLayer, Orientation = 0},
          new PositionOrientation() {Position = CubeFlag.RightSlice| CubeFlag.MiddleSlice| CubeFlag.TopLayer, Orientation = 0},
          new PositionOrientation() {Position =  CubeFlag.MiddleSliceSides| CubeFlag.FrontSlice| CubeFlag.TopLayer, Orientation = 0},
          new PositionOrientation() {Position = CubeFlag.LeftSlice| CubeFlag.MiddleSlice|CubeFlag.TopLayer, Orientation = 0},
       
          new PositionOrientation() {Position = CubeFlag.LeftSlice| CubeFlag.BackSlice|CubeFlag.MiddleLayer, Orientation = 0},
          new PositionOrientation() {Position = CubeFlag.RightSlice| CubeFlag.BackSlice| CubeFlag.MiddleLayer, Orientation = 0},
          new PositionOrientation() {Position = CubeFlag.RightSlice| CubeFlag.FrontSlice| CubeFlag.MiddleLayer, Orientation = 0},
          new PositionOrientation() {Position = CubeFlag.LeftSlice| CubeFlag.FrontSlice|CubeFlag.MiddleLayer, Orientation = 0},

          new PositionOrientation() {Position = CubeFlag.MiddleSliceSides| CubeFlag.BackSlice|CubeFlag.BottomLayer, Orientation = 0},
          new PositionOrientation() {Position = CubeFlag.RightSlice| CubeFlag.MiddleSlice| CubeFlag.BottomLayer, Orientation = 0},
          new PositionOrientation() {Position = CubeFlag.MiddleSliceSides| CubeFlag.FrontSlice| CubeFlag.BottomLayer, Orientation = 0},
          new PositionOrientation() {Position = CubeFlag.LeftSlice| CubeFlag.MiddleSlice|CubeFlag.BottomLayer, Orientation = 0},
        };
      }
    }

    /// <summary>
    /// Returns all possible corner positions
    /// </summary>
    public static List<PositionOrientation> CornerPositions
    {
      get
      {
        return new List<PositionOrientation>()
        {
          new PositionOrientation() {Position = CubeFlag.LeftSlice|CubeFlag.BackSlice|CubeFlag.TopLayer, Orientation = 0},
          new PositionOrientation() {Position = CubeFlag.RightSlice|CubeFlag.BackSlice|CubeFlag.TopLayer, Orientation = 0},
          new PositionOrientation() {Position = CubeFlag.RightSlice|CubeFlag.FrontSlice|CubeFlag.TopLayer, Orientation = 0},
          new PositionOrientation() {Position = CubeFlag.LeftSlice|CubeFlag.FrontSlice|CubeFlag.TopLayer, Orientation = 0},
          new PositionOrientation() {Position = CubeFlag.LeftSlice|CubeFlag.BackSlice|CubeFlag.BottomLayer, Orientation = 0},
          new PositionOrientation() {Position = CubeFlag.RightSlice|CubeFlag.BackSlice|CubeFlag.BottomLayer, Orientation = 0},
          new PositionOrientation() {Position = CubeFlag.RightSlice|CubeFlag.FrontSlice|CubeFlag.BottomLayer, Orientation = 0},
          new PositionOrientation() {Position = CubeFlag.LeftSlice|CubeFlag.FrontSlice|CubeFlag.BottomLayer, Orientation = 0},
        };
      }
    }

    /// <summary>
    /// Returns all possible cube positions
    /// </summary>
    public static List<PositionOrientation> CubePositions
    {
      get
      {
        List<PositionOrientation> flags = new List<PositionOrientation>();
        flags.AddRange(CornerPositions);
        flags.AddRange(EdgePositions);
        return flags;
      }
    }
    #endregion

    /// <summary>
    /// Collection of all pattern elements
    /// Key: { Current position, current orientation }
    /// Value: { Target position, target orientation = 0 }
    /// </summary>
    public Dictionary<PositionOrientation, PositionOrientation> AffectedCubes { get; set; }

    /// <summary>
    /// Gets the number of required inversions
    /// </summary>
    public int Inversions { get { return CornerInversions + EdgeInversions; } }

    /// <summary>
    /// Gets the number of required corner inversions
    /// </summary>
    public int CornerInversions
    {
      get
      {
        return CountInversions(CornerPositions.Select(pos => pos.Position).ToList(),
          Order(CornerPositions, AffectedCubes).Keys.Select(pos => pos.Position).ToList());
      }
    }

    /// <summary>
    /// Gets the number of required edge inversions
    /// </summary>
    public int EdgeInversions
    {
      get
      {
        return CountInversions(EdgePositions.Select(pos => pos.Position).ToList(),
          Order(EdgePositions, AffectedCubes).Keys.Select(pos => pos.Position).ToList());
      }
    }

    /// <summary>
    /// Gets the number of required 120° corner rotations
    /// </summary>
    public int CornerRotations { get { return Order(CornerPositions, AffectedCubes).Sum(c => c.Key.Orientation); } }

    /// <summary>
    /// Gets the number of flipped edges
    /// </summary>
    public int EdgeFlips { get { return Order(EdgePositions, AffectedCubes).Sum(c => c.Key.Orientation); } }

    /// <summary>
    /// True, if pattern is possible
    /// </summary>
    public bool IsPossible { get { return Inversions % 2 == 0 && CornerRotations % 3 == 0 && EdgeFlips % 2 == 0; } }

    /// <summary>
    /// Initializes a new instance of the Pattern class
    /// </summary>
    /// <param name="pattern">
    /// All pattern elements in the following format:
    /// Cube positions as string: "RFT" => Right | Front | Top
    /// Orientations as string "0" => 0 (max value = 2)
    /// 1: { currentPos, targetPos, currentOrientation }
    /// 2: { currentPos, currentOrientation } => any target position
    /// 3: { currentPos, targetPos } => any orientation
    /// </param>
    public Pattern(string[,] pattern)
    {
      AffectedCubes = new Dictionary<PositionOrientation, PositionOrientation>();

      if (pattern.GetLength(1) == 2)
      {
        for (int i = 0; i < pattern.GetLength(0); i++)
        {
          CubeFlag currPos = CubeFlagService.Parse(pattern[i, 0]);
          CubeFlag pos = CubeFlagService.Parse(pattern[i, 1]);
          int orientation = 0;
          if (!int.TryParse(pattern[i, 1], out orientation) && !CubePositions.Contains(new PositionOrientation() { Position = pos })) throw new Exception("At least one orientation or position is not possible");
          AffectedCubes.Add(new PositionOrientation() { Position = currPos, Orientation = orientation },
          new PositionOrientation() { Position = pos, Orientation = 0 });
        }
      }
      else if (pattern.GetLength(1) == 3)
      {
        for (int i = 0; i < pattern.GetLength(0); i++)
        {
          // check valid cube position
          CubeFlag currPos = CubeFlagService.Parse(pattern[i, 0]);
          CubeFlag targetPos = CubeFlagService.Parse(pattern[i, 1]);
          if (!CubePositions.Contains(new PositionOrientation() { Position = currPos }) || !CubePositions.Contains(new PositionOrientation() { Position = targetPos }))
            throw new Exception("At least one position does not exist");

          // check valid orientation
          int orientation = 0;
          if (!int.TryParse(pattern[i, 2], out orientation)) throw new Exception("At least one orientation is not possible");

          AffectedCubes.Add(new PositionOrientation() { Position = currPos, Orientation = orientation },
          new PositionOrientation() { Position = targetPos, Orientation = 0 });
        }
      }
      else throw new Exception("Invalid pattern");

      AffectedCubes = Order(CubePositions, AffectedCubes);
    }

    /// <summary>
    /// Initializes a new instance of the Pattern class
    /// </summary>
    public Pattern() { }


    /// <summary>
    /// Counts the required inversions
    /// </summary>
    /// <param name="standard">Standard order of positions</param>
    /// <param name="input">Current Order of positions</param>
    /// <returns>Number of required inversions</returns>
    private int CountInversions(List<CubeFlag> standard, List<CubeFlag> input)
    {
      int inversions = 0;
      for (int i = 0; i < input.Count; i++)
      {
        int index = standard.IndexOf(input[i]);
        for (int j = 0; j < input.Count; j++)
        {
          int index2 = standard.IndexOf(input[j]);
          if (index2 > index && j < i)
          {
            inversions++;
          }
        }
      }
      return inversions;
    }

    /// <summary>
    /// True, if this pattern includes all the pattern elements of another pattern
    /// </summary>
    /// <param name="pattern">Pattern to compare</param>
    public bool IncludesAllPatternElements(Pattern pattern)
    {
      foreach (KeyValuePair<PositionOrientation, PositionOrientation> p in pattern.AffectedCubes)
      {
        if (!AffectedCubes.Contains(p) && !(p.Value.Position == CubeFlag.None && AffectedCubes.ContainsKey(p.Key))) return false;
      }
      return true;
    }

    /// <summary>
    /// True, if this pattern has exactly the same pattern elemnts as the other pattern
    /// </summary>
    /// <param name="pattern">Pattern to compare</param>
    public bool Equals(Pattern pattern)
    {
      return CollectionMethods.ScrambledEquals(pattern.AffectedCubes, AffectedCubes);
    }

    /// <summary>
    /// Converts a rubik to a pattern
    /// </summary>
    /// <param name="r">Rubik's Cube</param>
    /// <returns>The pattern of the rubik</returns>
    public static Pattern FromRubik(Rubik r)
    {
      Pattern p = new Pattern();
      p.AffectedCubes = new Dictionary<PositionOrientation, PositionOrientation>();
      foreach (PositionOrientation pos in CubePositions)
      {
        Cube cube = r.Cubes.First(c => r.GetTargetFlags(c) == pos.Position);
        p.AffectedCubes.Add(new PositionOrientation() { Position = cube.Position.Flags, Orientation = PositionOrientation.GetOrientation(r, cube)}, pos);
      }
      return p;
    }

    /// <summary>
    /// Put in normal form
    /// </summary>
    /// <param name="standard">Normal form</param>
    /// <param name="newOrder">New order</param>
    /// <returns></returns>
    private static Dictionary<PositionOrientation, PositionOrientation> Order(IEnumerable<PositionOrientation> standard, Dictionary<PositionOrientation, PositionOrientation> newOrder)
    {
      Dictionary<PositionOrientation, PositionOrientation> result = new Dictionary<PositionOrientation, PositionOrientation>();

      foreach (PositionOrientation t in standard)
      {
        bool contains = newOrder.Values.Select(c => c.Position).ToList().Contains(t.Position);
        PositionOrientation affected = newOrder.FirstOrDefault(kvp => kvp.Value.Position == t.Position).Key;
        if (contains) result.Add(affected, t);
      }

      foreach (KeyValuePair<PositionOrientation, PositionOrientation> t in newOrder)
      {
        if (t.Value.Position == CubeFlag.None) result.Add(t.Key,t.Value);
      }
      return result;
    }

  }
}
