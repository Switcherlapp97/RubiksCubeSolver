using System;

namespace VirtualRubik
{
  public class LayerMove
  {
    public Cube3D.RubikPosition Layer;
    public Boolean Direction;

    public LayerMove(Cube3D.RubikPosition layer, Boolean direction)
    {
      Layer = layer;
      Direction = direction;
    }

    public LayerMove Clone()
    {
      return new LayerMove(Layer, Direction);
    }

		public static LayerMove Parse(string notation)
		{
			string layer = notation[0].ToString();
			Cube3D.RubikPosition rotationLayer = Cube3D.RubikPosition.None;
			switch (layer)
			{
				case "R":
					rotationLayer = Cube3D.RubikPosition.RightSlice;
					break;
				case "L":
					rotationLayer = Cube3D.RubikPosition.LeftSlice;
					break;
				case "U":
					rotationLayer = Cube3D.RubikPosition.TopLayer;
					break;
				case "D":
					rotationLayer = Cube3D.RubikPosition.BottomLayer;
					break;
				case "F":
					rotationLayer = Cube3D.RubikPosition.FrontSlice;
					break;
				case "B":
					rotationLayer = Cube3D.RubikPosition.BackSlice;
					break;
				case "M":
					rotationLayer = Cube3D.RubikPosition.MiddleSlice_Sides;
					break;
				case "E":
					rotationLayer = Cube3D.RubikPosition.MiddleLayer;
					break;
				case "S":
					rotationLayer = Cube3D.RubikPosition.MiddleSlice;
					break;
			}
			bool direction = notation.Length == 1;
			return new LayerMove(rotationLayer, direction);
		}

  }
}
