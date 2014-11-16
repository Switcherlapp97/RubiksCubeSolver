using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.Solver
{
  /// <summary>
  /// Represents a filter to prevent unnecessary pattern equality checks
  /// </summary>
  public class PatternFilter
  {
    /// <summary>
    /// Equality check between two pattrns
    /// </summary>
    public Func<Pattern, Pattern, bool> Filter { get; set; }

    /// <summary>
    /// Gets or sets whether the filter is used only in the beginning
    /// </summary>
    public bool OnlyAtBeginning { get; set; }

    /// <summary>
    /// Initializes a new instance of the PatternFilter class
    /// </summary>
    /// <param name="filter">Equality check between two patterns</param>
    public PatternFilter(Func<Pattern, Pattern, bool> filter, bool onlyAtBeginning = false)
    {
      this.Filter = filter;
      this.OnlyAtBeginning = onlyAtBeginning;
    }

    #region Predefined filters
    /// <summary>
    /// True, if both patterns have the equivalent count of edge and corner inversions
    /// </summary>
    public static PatternFilter SameInversionCount
    {
      get
      {
        return new PatternFilter(new Func<Pattern, Pattern, bool>(delegate(Pattern p1, Pattern p2)
          {
            return p1.EdgeInversions == p2.EdgeInversions && p1.CornerInversions == p2.CornerInversions;
          }));
      }
    }

    /// <summary>
    /// True if both patterns have equivalent count of edge flips and corner rotations
    /// </summary>
    public static PatternFilter SameFlipCount
    {
      get
      {
        return new PatternFilter(new Func<Pattern, Pattern, bool>(delegate(Pattern p1, Pattern p2)
        {
          return p1.EdgeFlips == p2.EdgeFlips && p1.CornerRotations == p2.CornerRotations;
        }), true);
      }
    }

    public static PatternFilter None
    {
      get
      {
        return new PatternFilter(new Func<Pattern, Pattern, bool>(delegate(Pattern p1, Pattern p2)
        {
          return true;
        }), true);
      }
    }
    #endregion
  }
}
