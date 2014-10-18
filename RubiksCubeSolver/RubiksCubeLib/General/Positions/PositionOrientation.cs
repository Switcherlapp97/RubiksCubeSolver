using RubiksCubeLib.RubiksCube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeLib
{
	public struct PositionOrientation
	{
		public CubeFlag Position { get; set; }
		public int Orientation { get; set; }

		public static int GetOrientation(Rubik rubik, Cube c)
		{
			int orientation = 0;
			if (c.IsEdge)
			{
				CubeFlag targetFlags = rubik.GetTargetFlags(c);
				Rubik clone = rubik.DeepClone();

				if (!targetFlags.HasFlag(CubeFlag.MiddleLayer))
				{
					while (RefreshCube(clone, c).Position.HasFlag(CubeFlag.MiddleLayer))
						clone.RotateLayer(c.Position.X, true);

					Cube clonedCube = RefreshCube(clone, c);
					Face yFace = clonedCube.Faces.First(f => f.Color == rubik.TopColor || f.Color == rubik.BottomColor);
					if (!FacePosition.YPos.HasFlag(yFace.Position))
						orientation = 1;
				}
				else
				{
					Face zFace = c.Faces.First(f => f.Color == rubik.FrontColor || f.Color == rubik.BackColor);
					if (c.Position.HasFlag(CubeFlag.MiddleLayer))
					{
						if (!FacePosition.ZPos.HasFlag(zFace.Position))
							orientation = 1;
					}
					else
					{
						if (!FacePosition.YPos.HasFlag(zFace.Position))
							orientation = 1;
					}
				}
			}
			else if (c.IsCorner)
			{
				Face face = c.Faces.First(f => f.Color == rubik.TopColor || f.Color == rubik.BottomColor);
				if (!FacePosition.YPos.HasFlag(face.Position))
				{
					if (FacePosition.XPos.HasFlag(face.Position) ^ !((c.Position.HasFlag(CubeFlag.BottomLayer) ^ (c.Position.HasFlag(CubeFlag.FrontSlice) ^ c.Position.HasFlag(CubeFlag.RightSlice)))))
					{
						orientation = 1;
					}
					else
						orientation = 2;
				}
			}

			return orientation;
		}

		private static Cube RefreshCube(Rubik r, Cube c)
		{
			return r.Cubes.First(cu => CollectionMethods.ScrambledEquals(cu.Colors, c.Colors));
		}
	}
}
