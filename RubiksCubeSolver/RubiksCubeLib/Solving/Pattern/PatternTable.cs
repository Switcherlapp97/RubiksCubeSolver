﻿using System;
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
    public Dictionary<Pattern, Algorithm> FindMatches(Pattern p, CubeFlag rotationLayer)
    {
      Dictionary<Pattern, Algorithm> patterns = Patterns.ToDictionary(pa => pa.Key.DeepClone(), a => a.Value);
      //if (regardProbability) patterns = patterns.OrderByDescending(k => k.Key.Probability).ToDictionary(pa => pa.Key.DeepClone(), a => a.Value);

      Dictionary<Pattern, Algorithm> matches = new Dictionary<Pattern, Algorithm>();
      // 4 possible transformations
      for (int i = 0; i < 4; i++)
      {
        // Get matches
        foreach (KeyValuePair<Pattern, Algorithm> kvp in patterns.Where(pa => p.IncludesAllPatternElements(pa.Key)))
        {
          matches.Add(kvp.Key, kvp.Value); // Add to matches
        }
        patterns = patterns.Except(matches).ToDictionary(pa => pa.Key.Transform(rotationLayer), a => a.Value.Transform(rotationLayer)); // transform
      }
      return matches;
    }

    /// <summary>
    /// Searches for the best algorithm for the given pattern
    /// </summary>
    /// <param name="p">Current rubik pattern</param>
    /// <param name="rotationLayer">Transformation layer</param>
    /// <returns>Returns the best match</returns>
    public Algorithm FindBestMatch(Pattern p, CubeFlag rotationLayer)
    {
      Dictionary<Pattern, Algorithm> matches = FindMatches(p, rotationLayer);
      Algorithm bestAlgo = matches.OrderByDescending(item => item.Key.Items.Count()).FirstOrDefault().Value;
      return bestAlgo;
    }
  }
}
