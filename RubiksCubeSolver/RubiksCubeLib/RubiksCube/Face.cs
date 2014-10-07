using System;
using System.Drawing;

namespace RubiksCubeLib.RubiksCube
{
  [Serializable]
  public class Face
  {
    #region Properties
    public Color Color { get; set; }
    public FacePosition Position { get; set; }
    #endregion

    #region Constructor
    public Face() : this(Color.Black,FacePosition.None) { }
    public Face(Color color, FacePosition position)
    {
      Color = color;
      Position = position;
    }
    #endregion

  }
}
