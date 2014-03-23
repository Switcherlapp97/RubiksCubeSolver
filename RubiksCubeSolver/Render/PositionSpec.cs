using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualRubik
{
	struct PositionSpec
	{
		public Cube3D.RubikPosition CubePosition { get; set; }
		public Face3D.FacePosition FacePosition { get; set; }
	}
}
