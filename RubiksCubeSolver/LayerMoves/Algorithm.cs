using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualRubik
{
	public class Algorithm
	{
		public List<LayerMove> Moves;

		public Algorithm(string moves)
		{
			Moves = new List<LayerMove>();
			foreach (string s in moves.Split(char.Parse(" ")))
			{
				Moves.Add(LayerMove.Parse(s));
			}
		}
	}
}
