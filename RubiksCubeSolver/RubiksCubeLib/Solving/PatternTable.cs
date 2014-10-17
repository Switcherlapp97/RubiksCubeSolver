using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib.Solver
{
  public abstract class PatternTable
  {
    public abstract Dictionary<Pattern, Algorithm> Patterns { get; }

    public Algorithm FindFirstMatches(Pattern p)
    {
      return Patterns.FirstOrDefault(kvp => p.IncludesAllPatternElements(kvp.Key)).Value;
    }
  }
}
