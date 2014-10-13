using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RubiksCubeLib.RubiksCube;
using System.Diagnostics;

namespace RubiksCubeLib.CubeModel
{
  /// <summary>
  /// Represents a 3D rubiks cube
  /// </summary>
  public partial class CubeModel
  {
    /// <summary>
    /// Gets or sets the permission to perform mouse activities
    /// </summary>
    public bool MouseHandling { get; set; }

    private PositionSpec oldSelection = PositionSpec.Default;
    private PositionSpec currentSelection = PositionSpec.Default;

    /// <summary>
    /// Converts negative angles to positive
    /// </summary>
    /// <param name="angleInDeg"></param>
    /// <returns></returns>
    private double ToPositiveAngle(double angleInDeg)
    {
      return angleInDeg < 0 ? 360 + angleInDeg : angleInDeg;
    }

    private Point oldMousePos = new Point(-1, -1);

    /// <summary>
    /// Detection and execution of the rotation of the whole cube
    /// </summary>
    protected override void OnMouseMove(MouseEventArgs e)
    {
      if (MouseHandling)
      {
        if (oldMousePos.X != -1 && oldMousePos.Y != -1)
        {
          if (e.Button == System.Windows.Forms.MouseButtons.Right)
          {
            this.Cursor = Cursors.SizeAll;
            int dx = e.X - oldMousePos.X;
            int dy = e.Y - oldMousePos.Y;

            int min = Math.Min(ClientRectangle.Height, ClientRectangle.Width);
            double scale = ((double)min / (double)400) * 6.0;

            Rotation[0] = ToPositiveAngle(Rotation[0] + dy / scale) % 360; // y rotation
            Rotation[1] = ToPositiveAngle(Rotation[1] - dx / scale) % 360;
           
          }
          else this.Cursor = Cursors.Arrow;
        }
      }
      oldMousePos = e.Location;
      base.OnMouseMove(e);
    }

    /// <summary>
    /// Detection and execution of mouse-controlled layer rotations
    /// </summary>
    protected override void OnMouseClick(MouseEventArgs e)
    {
      if (MouseHandling)
      {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
          if (oldSelection.IsDefault)
          {
            if (currentSelection.IsDefault)
            {
              selections.Reset();
              oldSelection = PositionSpec.Default;
              currentSelection = PositionSpec.Default;
            }
            else
            {
              if (!CubePosition.IsCorner(currentSelection.CubePosition))
              {
                oldSelection = currentSelection;
                Rubik.Cubes.ForEach(c => c.Faces.Where(f => f.Color != Color.Black).ToList().ForEach(f =>
                  {
                    PositionSpec pos = new PositionSpec() { CubePosition = c.Position.Flags, FacePosition = f.Position };

                    if (currentSelection.CubePosition != c.Position.Flags && !CubePosition.IsCenter(c.Position.Flags) && currentSelection.FacePosition == f.Position)
                    {
                      CubeFlag assocLayer = CubeFlagService.FromFacePosition(currentSelection.FacePosition);
                      CubeFlag commonLayer = CubeFlagService.GetFirstNotInvalidCommonFlag(currentSelection.CubePosition, c.Position.Flags, assocLayer);

                      if (commonLayer != CubeFlag.None && c.Position.HasFlag(commonLayer))
                      {
                        selections[pos] |= Selection.Possible;
                      }
                      else
                      {
                        selections[pos] |= Selection.NotPossible;
                      }
                    }
                    else
                    {
                      selections[pos] |= Selection.NotPossible;
                    }
                  }));
              }
              else
              {
                selections.Reset();
              }
            }
          }
          else
          {
            if (!currentSelection.IsDefault)
            {
              if (currentSelection.CubePosition != oldSelection.CubePosition)
              {
                if (!CubePosition.IsCenter(currentSelection.CubePosition))
                {
                  if (oldSelection.FacePosition == currentSelection.FacePosition)
                  {
                    CubeFlag assocLayer = CubeFlagService.FromFacePosition(oldSelection.FacePosition);
                    CubeFlag commonLayer = CubeFlagService.GetFirstNotInvalidCommonFlag(oldSelection.CubePosition, currentSelection.CubePosition, assocLayer);
                    bool direction = true;

                    if (commonLayer == CubeFlag.TopLayer || commonLayer == CubeFlag.MiddleLayer || commonLayer == CubeFlag.BottomLayer)
                    {
                      if (((oldSelection.FacePosition == FacePosition.Back) && currentSelection.CubePosition.HasFlag(CubeFlag.RightSlice))
                      || ((oldSelection.FacePosition == FacePosition.Left) && currentSelection.CubePosition.HasFlag(CubeFlag.BackSlice))
                      || ((oldSelection.FacePosition == FacePosition.Front) && currentSelection.CubePosition.HasFlag(CubeFlag.LeftSlice))
                      || ((oldSelection.FacePosition == FacePosition.Right) && currentSelection.CubePosition.HasFlag(CubeFlag.FrontSlice))) direction = false;
                      if (commonLayer == CubeFlag.TopLayer || commonLayer == CubeFlag.MiddleLayer) direction = !direction;
                    }

                    if (commonLayer == CubeFlag.LeftSlice || commonLayer == CubeFlag.MiddleSliceSides || commonLayer == CubeFlag.RightSlice)
                    {
                      if (((oldSelection.FacePosition == FacePosition.Bottom) && currentSelection.CubePosition.HasFlag(CubeFlag.BackSlice))
                      || ((oldSelection.FacePosition == FacePosition.Back) && currentSelection.CubePosition.HasFlag(CubeFlag.TopLayer))
                      || ((oldSelection.FacePosition == FacePosition.Top) && currentSelection.CubePosition.HasFlag(CubeFlag.FrontSlice))
                      || ((oldSelection.FacePosition == FacePosition.Front) && currentSelection.CubePosition.HasFlag(CubeFlag.BottomLayer))) direction = false;
                      if (commonLayer == CubeFlag.LeftSlice) direction = !direction;
                    }

                    if (commonLayer == CubeFlag.BackSlice || commonLayer == CubeFlag.MiddleSlice || commonLayer == CubeFlag.FrontSlice)
                    {
                      if (((oldSelection.FacePosition == FacePosition.Top) && currentSelection.CubePosition.HasFlag(CubeFlag.RightSlice))
                      || ((oldSelection.FacePosition == FacePosition.Right) && currentSelection.CubePosition.HasFlag(CubeFlag.BottomLayer))
                      || ((oldSelection.FacePosition == FacePosition.Bottom) && currentSelection.CubePosition.HasFlag(CubeFlag.LeftSlice))
                      || ((oldSelection.FacePosition == FacePosition.Left) && currentSelection.CubePosition.HasFlag(CubeFlag.TopLayer))) direction = false;
                      if (commonLayer == CubeFlag.FrontSlice || commonLayer == CubeFlag.MiddleSlice) direction = !direction;
                    }

                    if (commonLayer != CubeFlag.None)
                    {
                      RotateLayerAnimated(commonLayer, direction);
                    }
                    else
                    {
                      Debug.WriteLine("Error: Invalid second selection, does not specify distinct layer");
                    }
                  }
                  else
                  {
                    Debug.WriteLine("Error: Invalid second selection, must match orientation of first selection");
                  }
                }
                else
                {
                  Debug.WriteLine("Error: Invalid second selection, must not be a center");
                }
              }
              else
              {
                Debug.WriteLine("Error: Invalid second selection, must not be first selection");
              }
            }
            selections.Reset();
            oldSelection = PositionSpec.Default;
            currentSelection = PositionSpec.Default;
          }
        }
      }

      base.OnMouseClick(e);
    }
  }
}
