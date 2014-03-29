using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualRubik
{
	public struct PositionSpec
	{
		public Cube3D.RubikPosition CubePosition { get; set; }
		public Face3D.FacePosition FacePosition { get; set; }

		public bool Equals(PositionSpec compare)
		{
			return (CubePosition == compare.CubePosition && FacePosition == compare.FacePosition);
		}
	}
}
