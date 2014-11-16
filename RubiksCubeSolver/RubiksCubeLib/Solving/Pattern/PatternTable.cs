using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.Solver
{
  /// <summary>
  /// Represents a pattern table to search for equivalent patterns easily
  /// </summary>
  public abstract class PatternTable
  {
    /// <summary>
    /// Collection of comparison patterns with related algorithms
    /// </summary>
    public abstract Dictionary<Pattern, Algorithm> Patterns { get; }

    /// <summary>
    /// Finds all possible algorithms for this pattern
    /// </summary>
    /// <param name="p">Current rubik pattern</param>
    /// <param name="rotationLayer">Transformation rotation</param>
    /// <returns>Returns all possible solutions for this pattern</returns>
    public Dictionary<Pattern, Algorithm> FindMatches(Pattern p, CubeFlag rotationLayer, PatternFilter filter)
    {
      Dictionary<Pattern, Algorithm> transformedPatterns = Patterns.ToDictionary(kvp => kvp.Key.DeepClone(), a => a.Value); // clone
      Dictionary<Pattern,Algorithm> filteredPatterns = transformedPatterns.Where(kvp => filter.Filter(p, kvp.Key)).ToDictionary(pa => pa.Key, a => a.Value); // filter
      filteredPatterns = filteredPatterns.OrderByDescending(k => k.Key.Probability).ToDictionary(pa => pa.Key.DeepClone(), a => a.Value); // order by probability

      Dictionary<Pattern, Algorithm> matches = new Dictionary<Pattern, Algorithm>();
      // 4 possible standard transformations
      for (int i = 0; i < 4; i++)
      {
        // Get matches
        foreach (KeyValuePair<Pattern, Algorithm> kvp in filteredPatterns.Where(pa => p.IncludesAllPatternElements(pa.Key)))
        {
          matches.Add(kvp.Key, kvp.Value); // Add to matches
        }
        if (rotationLayer == CubeFlag.None) return matches;

        if (filter.OnlyAtBeginning)
        {
          transformedPatterns = filteredPatterns.Except(matches).ToDictionary(pa => pa.Key.Transform(rotationLayer), a => a.Value.Transform(rotationLayer));
          filteredPatterns = transformedPatterns;
        }
        else
        {
          transformedPatterns = transformedPatterns.ToDictionary(pa => pa.Key.Transform(rotationLayer), a => a.Value.Transform(rotationLayer));
          filteredPatterns = transformedPatterns.Where(kvp => filter.Filter(p, kvp.Key)).ToDictionary(pa => pa.Key, a => a.Value);
        }
      }
      return matches;
    }

    /// <summary>
    /// Searches for the best algorithm for the given pattern
    /// </summary>
    /// <param name="p">Current rubik pattern</param>
    /// <param name="rotationLayer">Transformation layer</param>
    /// <returns>Returns the best match</returns>
    public Algorithm FindBestMatch(Pattern p, CubeFlag rotationLayer, PatternFilter filter)
    {
      Dictionary<Pattern, Algorithm> matches = FindMatches(p, rotationLayer, filter);
      Algorithm bestAlgo = matches.OrderByDescending(item => item.Key.Items.Count()).FirstOrDefault().Value;
      return bestAlgo;
    }
  }
}
