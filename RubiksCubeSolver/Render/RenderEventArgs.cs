using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualRubik
{
	public class RenderEventArgs: EventArgs
	{
		public RenderInfo RenderInfo { get; private set; }
		public RenderEventArgs(RenderInfo renderInfo)
		{
			RenderInfo = renderInfo;
		}
	}
}
