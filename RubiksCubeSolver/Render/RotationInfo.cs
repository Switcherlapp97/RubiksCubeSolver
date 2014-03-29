using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualRubik
{
	public struct RotationInfo
	{
		public bool Rotating { get; set; }
		public int Milliseconds { get; set; } //in ms
		public bool Direction { get; set; }
		public Cube3D.RubikPosition Layer { get; set; }
		public int Target { get; set; }
	}
}
