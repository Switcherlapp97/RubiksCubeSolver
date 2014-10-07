using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
  public class Algorithm
  {
    public List<LayerMove> Moves { get; set; }

    public Algorithm()
    {
      Moves = new List<LayerMove>();
    }

    public Algorithm(string format, params object[] args) : this(string.Format(format, args)) { }

    /// <summary>
    /// Converts the notation of an algorithm in a collection of layer moves
    /// </summary>
    /// <param name="algorithm">Notation: separator = " " (space); counter-clockwise = any character (', i)</param>
    /// 
    public Algorithm(string algorithm)
    {
      Moves = new List<LayerMove>();
      foreach (string s in algorithm.Split((char.Parse(" "))))
      {
        Moves.Add(LayerMove.Parse(s));
      }
    }

    public override string ToString()
    {
      string algorithm = string.Empty;
      foreach (LayerMove move in Moves)
      {
        algorithm = (Moves.IndexOf(move) != 0) ? string.Join(" ", algorithm, move.ToString()) : move.ToString();
      }
      return algorithm;
    }
  }
}
