using RubiksCubeLib.RubiksCube;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace RubiksCubeLib.Solver
{
  /// <summary>
  /// Represents a cube pattern
  /// </summary>
  [Serializable]
  public class Pattern
  {
    #region Position definitions
    /// <summary>
    /// Gets all possible edge positions
    /// </summary>
    public static IEnumerable<CubePosition> EdgePositions
    {
      get
      {
        return new List<CubePosition>()
        {
          new CubePosition(CubeFlag.MiddleSliceSides,CubeFlag.TopLayer, CubeFlag.BackSlice),
          new CubePosition(CubeFlag.RightSlice,CubeFlag.TopLayer, CubeFlag.MiddleSlice),
          new CubePosition(CubeFlag.MiddleSliceSides,CubeFlag.TopLayer, CubeFlag.FrontSlice),
          new CubePosition(CubeFlag.LeftSlice,CubeFlag.TopLayer, CubeFlag.MiddleSlice),

          new CubePosition(CubeFlag.LeftSlice,CubeFlag.MiddleLayer, CubeFlag.BackSlice),
          new CubePosition(CubeFlag.RightSlice,CubeFlag.MiddleLayer, CubeFlag.BackSlice),
          new CubePosition(CubeFlag.RightSlice,CubeFlag.MiddleLayer, CubeFlag.FrontSlice),
          new CubePosition(CubeFlag.LeftSlice,CubeFlag.MiddleLayer, CubeFlag.FrontSlice),

          new CubePosition(CubeFlag.MiddleSliceSides,CubeFlag.BottomLayer, CubeFlag.BackSlice),
          new CubePosition(CubeFlag.RightSlice,CubeFlag.BottomLayer, CubeFlag.MiddleSlice),
          new CubePosition(CubeFlag.MiddleSliceSides,CubeFlag.BottomLayer, CubeFlag.FrontSlice),
          new CubePosition(CubeFlag.LeftSlice,CubeFlag.BottomLayer, CubeFlag.MiddleSlice),
        };
      }
    }

    /// <summary>
    /// Gets all possible corner positions
    /// </summary>
    public static IEnumerable<CubePosition> CornerPositions
    {
      get
      {
        return new List<CubePosition>()
        {
          new CubePosition(CubeFlag.LeftSlice,CubeFlag.TopLayer, CubeFlag.BackSlice),
          new CubePosition(CubeFlag.RightSlice,CubeFlag.TopLayer, CubeFlag.BackSlice),
          new CubePosition(CubeFlag.RightSlice,CubeFlag.TopLayer, CubeFlag.FrontSlice),
          new CubePosition(CubeFlag.LeftSlice,CubeFlag.TopLayer, CubeFlag.FrontSlice),

          new CubePosition(CubeFlag.LeftSlice,CubeFlag.BottomLayer, CubeFlag.BackSlice),
          new CubePosition(CubeFlag.RightSlice,CubeFlag.BottomLayer, CubeFlag.BackSlice),
          new CubePosition(CubeFlag.RightSlice,CubeFlag.BottomLayer, CubeFlag.FrontSlice),
          new CubePosition(CubeFlag.LeftSlice,CubeFlag.BottomLayer, CubeFlag.FrontSlice),
        };
      }
    }

    /// <summary>
    /// Gets all possible cube positions
    /// </summary>
    public static IEnumerable<CubePosition> Positions { get { return CornerPositions.Union(EdgePositions); } }

    #endregion

    /// <summary>
    /// Gets or sets the specific pattern elements
    /// </summary>
    public List<PatternItem> Items { get; set; }

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
        List<CubeFlag> newOrder = new List<CubeFlag>();
        foreach (CubePosition p in CornerPositions)
        {
          PatternItem affected = Items.FirstOrDefault(i => i.TargetPosition == p.Flags);
          CubePosition pos = affected != null ? affected.CurrentPosition : p;
          newOrder.Add(pos.Flags);
        }

        return CountInversions(CornerPositions.Select(pos => pos.Flags).ToList(), newOrder);
      }
    }

    /// <summary>
    /// Gets the number of required edge inversions
    /// </summary>
    public int EdgeInversions
    {
      get
      {
        List<CubeFlag> newOrder = new List<CubeFlag>();
        foreach (CubePosition p in EdgePositions)
        {
          PatternItem affected = Items.FirstOrDefault(i => i.TargetPosition == p.Flags);
          CubePosition pos = affected != null ? affected.CurrentPosition : p;
          newOrder.Add(pos.Flags);
        }

        return CountInversions(EdgePositions.Select(pos => pos.Flags).ToList(), newOrder);
      }
    }

    /// <summary>
    /// Gets the number of required 120° corner rotations
    /// </summary>
    public int CornerRotations { get { return Items.Where(i => CornerPositions.Contains(i.CurrentPosition)).Sum(c => (int)c.CurrentOrientation); } }

    /// <summary>
    /// Gets the number of flipped edges
    /// </summary>
    public int EdgeFlips { get { return Items.Where(i => EdgePositions.Contains(i.CurrentPosition)).Sum(c => (int)c.CurrentOrientation); } }

    /// <summary>
    /// True, if pattern is possible
    /// </summary>
    public bool IsPossible { get { return Inversions % 2 == 0 && CornerRotations % 3 == 0 && EdgeFlips % 2 == 0; } }

    /// <summary>
    /// Gets or sets the probalitiy of the pattern
    /// </summary>
    public double Probability { get; set; }

    /// <summary>
    /// Initializes a new instance of the Pattern class
    /// </summary>
    /// <param name="pattern">
    /// All pattern elements in the following format:
    /// Cube positions as string: "RFT" => Right | Front | Top
    /// Orientations as string "0" => 0 (max value = 2)
    /// 1: "currentPos, targetPos, currentOrientation"
    /// 2: "currentPos, currentOrientation" => any target position
    /// 3: "currentPos, targetPos" => any orientation
    /// </param>
    public Pattern(string[] pattern, double probability = 0)
    {
      this.Probability = probability;
      List<PatternItem> newItems = new List<PatternItem>();

      foreach (string item in pattern)
      {
        newItems.Add(PatternItem.Parse(item));
      }
      Items = Order(Positions, newItems).ToList();
    }

    /// <summary>
    /// Initializes a new instance of the pattern class
    /// </summary>
    public Pattern() { }

    public Pattern(IEnumerable<PatternItem> items, double probability = 0)
    {
      this.Probability = probability;
      Items = Order(Positions, items).ToList();
    }

    /// <summary>
    /// Converts a rubik to a pattern
    /// </summary>
    /// <param name="r">Rubik </param>
    /// <returns>The pattern of the given rubik</returns>
    public static Pattern FromRubik(Rubik r)
    {
      Pattern p = new Pattern();
      List<PatternItem> newItems = new List<PatternItem>();
      foreach (CubePosition pos in Positions)
      {
        Cube cube = r.Cubes.First(c => r.GetTargetFlags(c) == pos.Flags);
        newItems.Add(new PatternItem(cube.Position, Solvability.GetOrientation(r, cube), pos.Flags));
      }
      p.Items = newItems;
      return p;
    }

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
      foreach (PatternItem item in pattern.Items)
      {
        if (!Items.Any(i => i.Equals(item))) return false;
      }
      return true;
    }

    /// <summary>
    /// True, if this pattern has exactly the same pattern elemnts as the other pattern
    /// </summary>
    /// <param name="pattern">Pattern to compare</param>
    public bool Equals(Pattern pattern)
    {
      return CollectionMethods.ScrambledEquals(pattern.Items, Items);
    }

    /// <summary>
    /// Put to normal form
    /// </summary>
    /// <param name="standard">Normal form</param>
    /// <param name="newOrder">New order</param>
    /// <returns></returns>
    private static IEnumerable<PatternItem> Order(IEnumerable<CubePosition> standard, IEnumerable<PatternItem> newOrder)
    {
      List<PatternItem> result = new List<PatternItem>();
      foreach (CubePosition p in standard)
      {
        PatternItem affected = newOrder.FirstOrDefault(i => i.TargetPosition == p.Flags);
        if (affected != null) result.Add(affected);
      }

      foreach (PatternItem i in newOrder)
      {
        if (i.TargetPosition == CubeFlag.None) result.Add(i);
      }
      return result;
    }

    public Pattern DeepClone()
    {
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, this);
				ms.Position = 0;

				return (Pattern)formatter.Deserialize(ms);
			}
    }

    /// <summary>
    /// Transforms the pattern
    /// </summary>
    /// <param name="type">Transformation axis</param>
    public Pattern Transform(CubeFlag rotationLayer)
    {
      Pattern newPattern = DeepClone();
      foreach (PatternItem item in newPattern.Items)
      {
        item.Transform(rotationLayer);
      }
      return newPattern;
    }

    public void SaveXML(string path)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(Pattern));
      using (StreamWriter sw = new StreamWriter(path))
      {
        serializer.Serialize(sw, this);
      }
    }

    public static Pattern FromXml(string path)
    {
      Pattern pattern = null;
      try
      {
        XmlSerializer deserializer = new XmlSerializer(typeof(Pattern));
        using (StreamReader sr = new StreamReader(path))
        {
          pattern = (Pattern)deserializer.Deserialize(sr);
        }
      }
      catch
      {
        System.Windows.Forms.MessageBox.Show("Xml input error");
      }
      return pattern;
    }
  }
}
