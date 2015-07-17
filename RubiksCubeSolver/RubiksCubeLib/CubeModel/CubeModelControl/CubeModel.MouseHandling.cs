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

    // *** PROPERTIES ***

    /// <summary>
    /// Gets or sets the permission to perform mouse activities
    /// </summary>
    public bool MouseHandling { get; set; }



    // *** PRIVATE FIELDS ***

    private PositionSpec _oldSelection = PositionSpec.Default;
    private PositionSpec _currentSelection = PositionSpec.Default;

    private Point _oldMousePos = new Point(-1, -1);


    // *** METHODS ***


    // Editing angles

    /// <summary>
    /// Converts negative angles to positive
    /// </summary>
    /// <param name="angleInDeg"></param>
    /// <returns></returns>
    private double ToPositiveAngle(double angleInDeg)
    {
      return angleInDeg < 0 ? 360 + angleInDeg : angleInDeg;
    }

    /// <summary>
    /// Rounds a angle to 90 deg up
    /// </summary>
    /// <param name="angleInDeg"></param>
    /// <returns></returns>
    private double ToNextQuarter(double angleInDeg)
    {
      return Math.Round(angleInDeg / 90) * 90;
    }



    // Handle mouse interactions

    /// <summary>
    /// Detection and execution of the rotation of the whole cube
    /// </summary>
    protected override void OnMouseMove(MouseEventArgs e)
    {
      if (_oldMousePos.X != -1 && _oldMousePos.Y != -1)
      {
        if (e.Button == System.Windows.Forms.MouseButtons.Right)
        {
          this.Cursor = Cursors.SizeAll;
          int dx = e.X - _oldMousePos.X;
          int dy = e.Y - _oldMousePos.Y;

          int min = Math.Min(ClientRectangle.Height, ClientRectangle.Width);
          double scale = ((double)min / (double)400) * 6.0;

          this.Rotation[1] -= (dx / scale) % 360;
          this.Rotation[0] += (dy / scale) % 360;

        }
        else
          this.Cursor = Cursors.Arrow;
      }

      _oldMousePos = e.Location;
      base.OnMouseMove(e);
    }


    /// <summary>
    /// Returns true if the value is between the given bounds
    /// </summary>
    /// <param name="value">Defines the value to be checked</param>
    /// <param name="lbound">Defines the left bound (inclusive)</param>
    /// <param name="rbound">Defines the right bound (exclusive)</param>
    /// <returns></returns>
    private bool IsInRange(double value, double lbound, double rbound)
    {
      return value >= lbound && value < rbound;
    }


    /// <summary>
    /// Detection and execution of mouse-controlled layer rotations
    /// </summary>
    protected override void OnMouseClick(MouseEventArgs e)
    {
      if (this.MouseHandling)
      {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
        {
          if (_oldSelection.IsDefault)
          {
            if (_currentSelection.IsDefault)
            {
              _selections.Reset();
              _oldSelection = PositionSpec.Default;
              _currentSelection = PositionSpec.Default;
            }
            else
            {
              if (!CubePosition.IsCorner(_currentSelection.CubePosition))
              {
                _oldSelection = _currentSelection;
                this.Rubik.Cubes.ForEach(c => c.Faces.Where(f => f.Color != Color.Black).ToList().ForEach(f =>
                  {
                    PositionSpec pos = new PositionSpec() { CubePosition = c.Position.Flags, FacePosition = f.Position };

                    if (_currentSelection.CubePosition != c.Position.Flags && !CubePosition.IsCenter(c.Position.Flags) && _currentSelection.FacePosition == f.Position)
                    {
                      CubeFlag assocLayer = CubeFlagService.FromFacePosition(_currentSelection.FacePosition);
                      CubeFlag commonLayer = CubeFlagService.GetFirstNotInvalidCommonFlag(_currentSelection.CubePosition, c.Position.Flags, assocLayer);

                      if (commonLayer != CubeFlag.None && c.Position.HasFlag(commonLayer))
                      {
                        _selections[pos] |= Selection.Possible;
                      }
                      else
                      {
                        _selections[pos] |= Selection.NotPossible;
                      }
                    }
                    else
                    {
                      _selections[pos] |= Selection.NotPossible;
                    }
                  }));
                this.State = string.Format("First selection [{0}] | {1}", _currentSelection.CubePosition, _currentSelection.FacePosition);
              }
              else
              {
                _selections.Reset();
                this.State = "Error: Invalid first selection, must not be a corner";
              }
            }
          }
          else
          {
            if (_currentSelection.IsDefault)
            {
              this.State = "Ready";
            }
            else
            {
              if (_currentSelection.CubePosition != _oldSelection.CubePosition)
              {
                if (!CubePosition.IsCenter(_currentSelection.CubePosition))
                {
                  if (_oldSelection.FacePosition == _currentSelection.FacePosition)
                  {
                    CubeFlag assocLayer = CubeFlagService.FromFacePosition(_oldSelection.FacePosition);
                    CubeFlag commonLayer = CubeFlagService.GetFirstNotInvalidCommonFlag(_oldSelection.CubePosition, _currentSelection.CubePosition, assocLayer);
                    bool direction = true;

                    if (commonLayer == CubeFlag.TopLayer || commonLayer == CubeFlag.MiddleLayer || commonLayer == CubeFlag.BottomLayer)
                    {
                      if (((_oldSelection.FacePosition == FacePosition.Back) && _currentSelection.CubePosition.HasFlag(CubeFlag.RightSlice))
                      || ((_oldSelection.FacePosition == FacePosition.Left) && _currentSelection.CubePosition.HasFlag(CubeFlag.BackSlice))
                      || ((_oldSelection.FacePosition == FacePosition.Front) && _currentSelection.CubePosition.HasFlag(CubeFlag.LeftSlice))
                      || ((_oldSelection.FacePosition == FacePosition.Right) && _currentSelection.CubePosition.HasFlag(CubeFlag.FrontSlice)))
                        direction = false;
                      if (commonLayer == CubeFlag.TopLayer || commonLayer == CubeFlag.MiddleLayer)
                        direction = !direction;
                    }

                    if (commonLayer == CubeFlag.LeftSlice || commonLayer == CubeFlag.MiddleSliceSides || commonLayer == CubeFlag.RightSlice)
                    {
                      if (((_oldSelection.FacePosition == FacePosition.Bottom) && _currentSelection.CubePosition.HasFlag(CubeFlag.BackSlice))
                      || ((_oldSelection.FacePosition == FacePosition.Back) && _currentSelection.CubePosition.HasFlag(CubeFlag.TopLayer))
                      || ((_oldSelection.FacePosition == FacePosition.Top) && _currentSelection.CubePosition.HasFlag(CubeFlag.FrontSlice))
                      || ((_oldSelection.FacePosition == FacePosition.Front) && _currentSelection.CubePosition.HasFlag(CubeFlag.BottomLayer)))
                        direction = false;
                      if (commonLayer == CubeFlag.LeftSlice)
                        direction = !direction;
                    }

                    if (commonLayer == CubeFlag.BackSlice || commonLayer == CubeFlag.MiddleSlice || commonLayer == CubeFlag.FrontSlice)
                    {
                      if (((_oldSelection.FacePosition == FacePosition.Top) && _currentSelection.CubePosition.HasFlag(CubeFlag.RightSlice))
                      || ((_oldSelection.FacePosition == FacePosition.Right) && _currentSelection.CubePosition.HasFlag(CubeFlag.BottomLayer))
                      || ((_oldSelection.FacePosition == FacePosition.Bottom) && _currentSelection.CubePosition.HasFlag(CubeFlag.LeftSlice))
                      || ((_oldSelection.FacePosition == FacePosition.Left) && _currentSelection.CubePosition.HasFlag(CubeFlag.TopLayer)))
                        direction = false;
                      if (commonLayer == CubeFlag.FrontSlice || commonLayer == CubeFlag.MiddleSlice)
                        direction = !direction;
                    }

                    if (commonLayer != CubeFlag.None)
                    {
                      RotateLayerAnimated(commonLayer, direction);
                    }
                    else
                    {
                      this.State = "Error: Invalid second selection, does not specify distinct layer";
                    }
                  }
                  else
                  {
                    this.State = "Error: Invalid second selection, must match orientation of first selection";
                  }
                }
                else
                {
                  this.State = "Error: Invalid second selection, must not be a center";
                }
              }
              else
              {
                this.State = "Error: Invalid second selection, must not be first selection";
              }
            }
            _selections.Reset();
            _oldSelection = PositionSpec.Default;
            _currentSelection = PositionSpec.Default;
          }
        }
      }

      base.OnMouseClick(e);
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
      zoom = Math.Min(Math.Max(0.2, zoom + e.Delta / 100.0), 10.0);
      base.OnMouseWheel(e);
    }
  }
}
