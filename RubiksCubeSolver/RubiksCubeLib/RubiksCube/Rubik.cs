using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RubiksCubeLib.ScanInput;
using System.Xml.Serialization;
using RubiksCubeLib.Solver;

namespace RubiksCubeLib.RubiksCube
{

  /// <summary>
  /// Defines a Rubik's Cube
  /// </summary>
  [Serializable]
  public class Rubik
  {

    // **** CONSTRUCTORS ****

    /// <summary>
    /// Empty constructor (default colors will be set)
    /// </summary>
    public Rubik() : this(Color.Orange, Color.Red, Color.Yellow, Color.White, Color.Blue, Color.Green) { }

    /// <summary>
    /// Constructor with the colors of the faces
    /// </summary>
    /// <param name="cfront">Defines the color of the front face</param>
    /// <param name="cback">Defines the color of the back face</param>
    /// <param name="ctop">Defines the color of the top face</param>
    /// <param name="cbottom">Defines the color of the bot face</param>
    /// <param name="cright">Defines the color of the right face</param>
    /// <param name="cleft">Defines the color of the left face</param>
    public Rubik(Color cfront, Color cback, Color ctop, Color cbottom, Color cright, Color cleft)
    {
      Colors = new Color[] { cfront, cback, ctop, cbottom, cright, cleft };
      Cubes = new List<Cube>();
      for (int i = -1; i <= 1; i++)
      {
        for (int j = -1; j <= 1; j++)
        {
          for (int k = -1; k <= 1; k++)
          {
            Cubes.Add(new Cube(GenSideFlags(i, j, k)));
          }
        }
      }
      SetFaceColor(CubeFlag.FrontSlice, FacePosition.Front, cfront);
      SetFaceColor(CubeFlag.BackSlice, FacePosition.Back, cback);
      SetFaceColor(CubeFlag.TopLayer, FacePosition.Top, ctop);
      SetFaceColor(CubeFlag.BottomLayer, FacePosition.Bottom, cbottom);
      SetFaceColor(CubeFlag.RightSlice, FacePosition.Right, cright);
      SetFaceColor(CubeFlag.LeftSlice, FacePosition.Left, cleft);
    }



    // **** PROPERTIES ****

    /// <summary>
    /// The collection of cubes of this Rubik
    /// </summary>
    public List<Cube> Cubes { get; private set; }

    /// <summary>
    /// The colors of this Rubik
    /// </summary>
    public Color[] Colors { get; private set; }

    /// <summary>
    /// Returns the color of the front face
    /// </summary>
    public Color FrontColor { get { return GetFaceColor(CubeFlag.FrontSlice | CubeFlag.MiddleSliceSides | CubeFlag.MiddleLayer, FacePosition.Front); } }

    /// <summary>
    /// Returns the color of the back face
    /// </summary>
    public Color BackColor { get { return GetFaceColor(CubeFlag.BackSlice | CubeFlag.MiddleSliceSides | CubeFlag.MiddleLayer, FacePosition.Back); } }

    /// <summary>
    /// Returns the color of the top face
    /// </summary>
    public Color TopColor { get { return GetFaceColor(CubeFlag.TopLayer | CubeFlag.MiddleSliceSides | CubeFlag.MiddleSlice, FacePosition.Top); } }

    /// <summary>
    /// Returns the color of the bottom face
    /// </summary>
    public Color BottomColor { get { return GetFaceColor(CubeFlag.BottomLayer | CubeFlag.MiddleSliceSides | CubeFlag.MiddleSlice, FacePosition.Bottom); } }

    /// <summary>
    /// Returns the color of the right face
    /// </summary>
    public Color RightColor { get { return GetFaceColor(CubeFlag.RightSlice | CubeFlag.MiddleLayer | CubeFlag.MiddleSlice, FacePosition.Right); } }

    /// <summary>
    /// Returns the color of the left face
    /// </summary>
    public Color LeftColor { get { return GetFaceColor(CubeFlag.LeftSlice | CubeFlag.MiddleSlice | CubeFlag.MiddleLayer, FacePosition.Left); } }






    // **** METHODS ****

    public static Rubik FromPattern(Pattern pattern)
    {
      Rubik rubik = new Rubik();
      foreach (PatternItem item in pattern.Items)
      {
        if (CubePosition.IsCorner(item.CurrentPosition.Flags))
        {
          bool xyzCorrect = !((item.CurrentPosition.Y == CubeFlag.TopLayer ^ (item.CurrentPosition.X == CubeFlag.RightSlice ^ item.CurrentPosition.Z == CubeFlag.FrontSlice))
              ^ (CubeFlagService.FirstYFlag(item.TargetPosition) == CubeFlag.TopLayer ^ (CubeFlagService.FirstXFlag(item.TargetPosition) == CubeFlag.RightSlice ^ CubeFlagService.FirstZFlag(item.TargetPosition) == CubeFlag.FrontSlice)));

          if (item.CurrentOrientation == Orientation.Correct)
          {
            rubik.SetFaceColor(item.CurrentPosition.Flags, CubeFlagService.ToFacePosition(item.CurrentPosition.Y), rubik.GetSliceColor(CubeFlagService.FirstYFlag(item.TargetPosition)));

            FacePosition x = xyzCorrect ? CubeFlagService.ToFacePosition(item.CurrentPosition.X) : CubeFlagService.ToFacePosition(item.CurrentPosition.Z);
            FacePosition z = xyzCorrect ? CubeFlagService.ToFacePosition(item.CurrentPosition.Z) : CubeFlagService.ToFacePosition(item.CurrentPosition.X);

            rubik.SetFaceColor(item.CurrentPosition.Flags, x, rubik.GetSliceColor(CubeFlagService.FirstXFlag(item.TargetPosition)));
            rubik.SetFaceColor(item.CurrentPosition.Flags, z, rubik.GetSliceColor(CubeFlagService.FirstZFlag(item.TargetPosition)));
          }
          else
          {
            bool corr = (item.CurrentPosition.X == CubeFlag.RightSlice ^ item.CurrentPosition.Z == CubeFlag.BackSlice) ^ item.CurrentPosition.Y == CubeFlag.BottomLayer;
            FacePosition x = (corr ^ item.CurrentOrientation == Orientation.Clockwise) ? CubeFlagService.ToFacePosition(item.CurrentPosition.X) : CubeFlagService.ToFacePosition(item.CurrentPosition.Y);
            FacePosition y = (corr ^ item.CurrentOrientation == Orientation.Clockwise) ? CubeFlagService.ToFacePosition(item.CurrentPosition.Z) : CubeFlagService.ToFacePosition(item.CurrentPosition.X);
            FacePosition z = (corr ^ item.CurrentOrientation == Orientation.Clockwise) ? CubeFlagService.ToFacePosition(item.CurrentPosition.Y) : CubeFlagService.ToFacePosition(item.CurrentPosition.Z);

            rubik.SetFaceColor(item.CurrentPosition.Flags, x, rubik.GetSliceColor(CubeFlagService.FirstXFlag(item.TargetPosition)));
            rubik.SetFaceColor(item.CurrentPosition.Flags, y, rubik.GetSliceColor(CubeFlagService.FirstYFlag(item.TargetPosition)));
            rubik.SetFaceColor(item.CurrentPosition.Flags, z, rubik.GetSliceColor(CubeFlagService.FirstZFlag(item.TargetPosition)));
          }
        }

        if (CubePosition.IsEdge(item.CurrentPosition.Flags))
        {
          CubeFlag flagOne = CubeFlagService.FirstNotInvalidFlag(item.CurrentPosition.Flags, CubeFlag.MiddleLayer | CubeFlag.MiddleSlice | CubeFlag.MiddleSliceSides);
          FacePosition faceOne = CubeFlagService.ToFacePosition(flagOne);
          FacePosition faceTwo = CubeFlagService.ToFacePosition(CubeFlagService.FirstNotInvalidFlag(item.CurrentPosition.Flags, CubeFlag.MiddleLayer | CubeFlag.MiddleSlice | CubeFlag.MiddleSliceSides | flagOne));

          CubeFlag tFlagOne = CubeFlagService.FirstNotInvalidFlag(item.TargetPosition, CubeFlag.MiddleLayer | CubeFlag.MiddleSlice | CubeFlag.MiddleSliceSides);

          rubik.SetFaceColor(item.CurrentPosition.Flags, faceOne, rubik.GetSliceColor(tFlagOne));
          rubik.SetFaceColor(item.CurrentPosition.Flags, faceTwo, rubik.GetSliceColor(CubeFlagService.FirstNotInvalidFlag(item.TargetPosition, CubeFlag.MiddleLayer | CubeFlag.MiddleSlice | CubeFlag.MiddleSliceSides | tFlagOne)));

          if (Solvability.GetOrientation(rubik, rubik.Cubes.First(c => c.Position.Flags == item.CurrentPosition.Flags)) != item.CurrentOrientation)
          {
            rubik.SetFaceColor(item.CurrentPosition.Flags, faceTwo, rubik.GetSliceColor(tFlagOne));
            rubik.SetFaceColor(item.CurrentPosition.Flags, faceOne, rubik.GetSliceColor(CubeFlagService.FirstNotInvalidFlag(item.TargetPosition, CubeFlag.MiddleLayer | CubeFlag.MiddleSlice | CubeFlag.MiddleSliceSides | tFlagOne)));
          }
        }

        if (CubePosition.IsCenter(item.CurrentPosition.Flags))
        {
          CubeFlag flag = CubeFlagService.FirstNotInvalidFlag(item.CurrentPosition.Flags, CubeFlag.MiddleLayer | CubeFlag.MiddleSlice | CubeFlag.MiddleSliceSides);
          CubeFlag tFlag = CubeFlagService.FirstNotInvalidFlag(item.TargetPosition, CubeFlag.MiddleLayer | CubeFlag.MiddleSlice | CubeFlag.MiddleSliceSides);

          rubik.SetFaceColor(item.CurrentPosition.Flags, CubeFlagService.ToFacePosition(flag), rubik.GetSliceColor(tFlag));
        }
      }
      return rubik;
    }

    /// <summary>
    /// Returns a clone of this cube (i.e. same properties but new instance)
    /// </summary>
    /// <returns></returns>
    public Rubik DeepClone()
    {
      using (var ms = new MemoryStream())
      {
        var formatter = new BinaryFormatter();
        formatter.Serialize(ms, this);
        ms.Position = 0;

        return (Rubik)formatter.Deserialize(ms);
      }
    }

    /// <summary>
    /// Returns the CubeFlag of the Cube at the given 3D position 
    /// </summary>
    /// <param name="i">Defines the XFlag (left to right)</param>
    /// <param name="j">Defines the YFlag (top to bottom)</param>
    /// <param name="k">Defines the ZFlag (front to back)</param>
    /// <returns></returns>
    public CubeFlag GenSideFlags(int i, int j, int k)
    {
      CubeFlag p = new CubeFlag();
      switch (i)
      {
        case -1:
          p |= CubeFlag.LeftSlice;
          break;
        case 0:
          p |= CubeFlag.MiddleSliceSides;
          break;
        case 1:
          p |= CubeFlag.RightSlice;
          break;
      }
      switch (j)
      {
        case -1:
          p |= CubeFlag.TopLayer;
          break;
        case 0:
          p |= CubeFlag.MiddleLayer;
          break;
        case 1:
          p |= CubeFlag.BottomLayer;
          break;
      }
      switch (k)
      {
        case -1:
          p |= CubeFlag.FrontSlice;
          break;
        case 0:
          p |= CubeFlag.MiddleSlice;
          break;
        case 1:
          p |= CubeFlag.BackSlice;
          break;
      }
      return p;
    }

    /// <summary>
    /// Sets the facecolor of the given cubes and faces with the given color
    /// </summary>
    /// <param name="affected">Defines the cubes to be changed</param>
    /// <param name="face">Defines the face of the cubes to be changed</param>
    /// <param name="color">Defines the color to be set</param>
    public void SetFaceColor(CubeFlag affected, FacePosition face, Color color)
    {
      Cubes.Where(c => c.Position.HasFlag(affected)).ToList().ForEach(c => c.SetFaceColor(face, color));
      Cubes.ToList().ForEach(c => { c.Colors.Clear(); c.Faces.ToList().ForEach(f => c.Colors.Add(f.Color)); });
    }

    /// <summary>
    /// Returns the color of the face of the first cube with the given CubeFlag
    /// </summary>
    /// <param name="position">Defines the CubeFlag which filters this cubes</param>
    /// <param name="face">Defines the face to be analyzed</param>
    /// <returns></returns>
    public Color GetFaceColor(CubeFlag position, FacePosition face)
    {
      return Cubes.First(c => c.Position.Flags == position).GetFaceColor(face);
    }



    /// <summary>
    /// Executes the given move (rotation)
    /// </summary>
    /// <param name="move">Defines the move to be executed</param>
    public void RotateLayer(IMove move)
    {
      if (move.MultipleLayers)
      {
        LayerMoveCollection moves = (LayerMoveCollection)move;
        foreach (LayerMove m in moves)
        {
          RotateLayer(m);
        }
      }
      else
        RotateLayer((LayerMove)move);
    }

    /// <summary>
    /// Executes the given LayerMove
    /// </summary>
    /// <param name="move">Defines the LayerMove to be executed</param>
    private void RotateLayer(LayerMove move)
    {
      int repetitions = move.Twice ? 2 : 1;
      for (int i = 0; i < repetitions; i++)
      {
        IEnumerable<Cube> affected = Cubes.Where(c => c.Position.HasFlag(move.Layer));
        affected.ToList().ForEach(c => c.NextPos(move.Layer, move.Direction));
      }
    }

    /// <summary>
    /// Rotates a layer of the Rubik
    /// </summary>
    /// <param name="layer">Defines the layer to be rotated on</param>
    /// <param name="direction">Defines the direction of the rotation (true == clockwise)</param>
    public void RotateLayer(CubeFlag layer, bool direction)
    {
      IEnumerable<Cube> affected = Cubes.Where(c => c.Position.HasFlag(layer));
      affected.ToList().ForEach(c => c.NextPos(layer, direction));
    }

    /// <summary>
    /// Execute a rotation of the whole cube
    /// </summary>
    /// <param name="type">Defines the axis to be rotated on</param>
    /// <param name="direction">Defines the direction of the rotation (true == clockwise)</param>
    public void RotateCube(RotationType type, bool direction)
    {
      switch (type)
      {
        case RotationType.X:
          RotateLayer(new LayerMove(CubeFlag.RightSlice, direction) & new LayerMove(CubeFlag.MiddleSliceSides, direction) & new LayerMove(CubeFlag.LeftSlice, !direction));
          break;
        case RotationType.Y:
          RotateLayer(new LayerMove(CubeFlag.TopLayer, direction) & new LayerMove(CubeFlag.MiddleLayer, direction) & new LayerMove(CubeFlag.BottomLayer, !direction));
          break;
        case RotationType.Z:
          RotateLayer(new LayerMove(CubeFlag.FrontSlice, direction) & new LayerMove(CubeFlag.MiddleSlice, direction) & new LayerMove(CubeFlag.BackSlice, !direction));
          break;
      }
    }



    /// <summary>
    /// Execute random LayerMoves on the cube to scramble it
    /// </summary>
    /// <param name="moves">Defines the amount of moves</param>
    public void Scramble(int moves)
    {
      Random rnd = new Random();
      for (int i = 0; i < moves; i++)
        RotateLayer(new LayerMove((CubeFlag)Math.Pow(2, rnd.Next(0, 9)), Convert.ToBoolean(rnd.Next(0, 2))));
    }



    /// <summary>
    /// Returns the amount of corners of the top layer which have the right orientation
    /// </summary>
    /// <returns></returns>
    public int CountCornersWithCorrectOrientation()
    {
      Color topColor = GetFaceColor(CubeFlag.TopLayer | CubeFlag.MiddleSliceSides | CubeFlag.MiddleSlice, FacePosition.Top);
      return Cubes.Count(c => c.IsCorner && c.Faces.First(f => f.Position == FacePosition.Top).Color == topColor);
    }

    /// <summary>
    /// Returns the amount of edges of the top layer which have the right orientation
    /// </summary>
    /// <returns></returns>
    public int CountEdgesWithCorrectOrientation()
    {
      Color topColor = GetFaceColor(CubeFlag.TopLayer | CubeFlag.MiddleSliceSides | CubeFlag.MiddleSlice, FacePosition.Top);
      return Cubes.Count(c => c.IsEdge && c.Faces.First(f => f.Position == FacePosition.Top).Color == topColor);
    }

    /// <summary>
    /// Returns a solved Rubik 
    /// </summary>
    /// <returns></returns>
    public Rubik GenStandardCube()
    {
      Rubik standardCube = new Rubik();
      standardCube.SetFaceColor(CubeFlag.TopLayer, FacePosition.Top, TopColor);
      standardCube.SetFaceColor(CubeFlag.BottomLayer, FacePosition.Bottom, BottomColor);
      standardCube.SetFaceColor(CubeFlag.RightSlice, FacePosition.Right, RightColor);
      standardCube.SetFaceColor(CubeFlag.LeftSlice, FacePosition.Left, LeftColor);
      standardCube.SetFaceColor(CubeFlag.FrontSlice, FacePosition.Front, FrontColor);
      standardCube.SetFaceColor(CubeFlag.BackSlice, FacePosition.Back, BackColor);

      return standardCube;
    }

    /// <summary>
    /// Returns the position of given cube where it has to be when the Rubik is solved
    /// </summary>
    /// <param name="cube">Defines the cube to be analyzed</param>
    /// <returns></returns>
    public CubeFlag GetTargetFlags(Cube cube)
    {
      return GenStandardCube().Cubes.First(cu => CollectionMethods.ScrambledEquals(cu.Colors, cube.Colors)).Position.Flags;
    }

    /// <summary>
    /// Returns the center color of a slice
    /// </summary>
    /// <param name="slice">Defines the slice whose center color will be returned</param>
    public Color GetSliceColor(CubeFlag slice)
    {
      switch (slice)
      {
        case CubeFlag.TopLayer: return this.TopColor;
        case CubeFlag.BottomLayer: return this.BottomColor;
        case CubeFlag.FrontSlice: return this.FrontColor;
        case CubeFlag.BackSlice: return this.BackColor;
        case CubeFlag.LeftSlice: return this.LeftColor;
        case CubeFlag.RightSlice: return this.RightColor;
        default: return Color.Black;
      }
    }

    public bool IsCorrect(Cube cube)
    {
      return cube.Position.Flags == GetTargetFlags(cube) && Solvability.GetOrientation(this, cube) == 0;
    }

    /// <summary>
    /// Returns a scanned Rubik
    /// </summary>
    /// <param name="scanner">Defines the scanner to be used</param>
    /// <returns></returns>
    public static Rubik Scan(CubeScanner scanner)
    {
      return scanner.Scan();
    }
  }
}
