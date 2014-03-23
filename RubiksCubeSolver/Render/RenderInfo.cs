using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VirtualRubik
{
	struct RenderInfo
	{
		public Point MousePosition { get; set; }
		public PositionSpec SelectedPos { get; set; }
		public IEnumerable<Face3D> FacesProjected { get; set; }
	}
}
