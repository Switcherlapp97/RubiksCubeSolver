using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RubiksCubeLib.RubiksCube;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using RubiksCubeLib.Solver;

namespace RubiksCubeLib.CubeModel
{

  /// <summary>
  /// Represents a 3D rubiks cube
  /// </summary>
  public partial class CubeModel : UserControl
  {

    // *** CONSTRUCTORS ***

    /// <summary>
    /// Initializes a new instance of the CubeModel class
    /// </summary>
    public CubeModel() : this(new Rubik()) { }

    /// <summary>
    /// Initializes a new instance of the CubeModel class
    /// </summary>
    /// <param name="rubik">Rubik's Cube that has to be drawn</param>
    public CubeModel(Rubik rubik)
    {
      this.Rubik = rubik;
      this.Rotation = new double[] { 0, 0, 0 };
      this.Moves = new Queue<RotationInfo>();
      this.MouseHandling = true;
      this.DrawingMode = RubiksCubeLib.CubeModel.DrawingMode.ThreeDimensional;
      this.State = "Ready";
      this.RotationSpeed = 250;

      InitColorPicker();
      ResetLayerRotation();
      InitSelection();
      InitRenderer();

      InitializeComponent();
      SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      SetStyle(ControlStyles.DoubleBuffer, true);
      SetStyle(ControlStyles.SupportsTransparentBackColor, true);
      SetStyle(ControlStyles.UserPaint, true);
    }


    // *** PRIVATE FIELDS ***

    private SelectionCollection _selections;


    // *** PROPERTIES ***

    /// <summary>
    /// Gets the rotation angles in the direction of x, y and z
    /// </summary>
    public double[] Rotation { get; private set; }

    /// <summary>
    /// Gets the rotation angles of the specific layers
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Dictionary<CubeFlag, double> LayerRotation { get; private set; }

    /// <summary>
    /// Gets the moves in the queue
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Queue<RotationInfo> Moves { get; private set; }

    public BindingList<RotationInfo> MovesList
    {
      get
      {
        BindingList<RotationInfo> moves = new BindingList<RotationInfo>();
        foreach (RotationInfo info in this.Moves)
          moves.Add(info);
        return moves;
      }
    }

    /// <summary>
    /// Gets the information about the drawn Rubik's Cube
    /// </summary>
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Rubik Rubik { get; private set; }

    /// <summary>
    /// Gets the current state of the rubik
    /// </summary>
    public string State { get; private set; }

    /// <summary>
    /// Gets or sets the drawing mode of the rubik
    /// </summary>
    public DrawingMode DrawingMode { get; set; }

    /// <summary>
    /// Gets or sets how fast the layer rotations will be performed
    /// </summary>
    public int RotationSpeed { get; set; }

    // *** METHODS ***

    /// <summary>
    /// Resets the Rubik object and reset all cube and layer rotations and all selections
    /// </summary>
    public void ResetCube()
    {
      this.Rubik = new Rubik();
      this.Rotation = new double[] { 0, 0, 0 };
      this.Moves = new Queue<RotationInfo>();
      this.MouseHandling = true;
      this.State = "Ready";
      this.Zoom = 1.0;
      ResetLayerRotation();
      InitSelection();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
      this.SetDrawingArea(this.ClientRectangle);
      this.Invalidate();
      base.OnSizeChanged(e);
    }

    

    public void LoadPattern(string path)
    {
      this.Rubik = Rubik.FromPattern(Pattern.FromXml(path));
    }

    public void SavePattern(string path)
    {
      Pattern.FromRubik(this.Rubik).SaveXML(path);
    }

    /// <summary>
    /// Resets the rotation of specific layers
    /// </summary>
    private void ResetLayerRotation()
    {
      this.LayerRotation = new Dictionary<CubeFlag, double>();
      foreach (CubeFlag rp in (CubeFlag[])Enum.GetValues(typeof(CubeFlag)))
      {
        if (rp != CubeFlag.XFlags && rp != CubeFlag.YFlags && rp != CubeFlag.ZFlags)
          this.LayerRotation[rp] = 0;
      }
    }

    /// <summary>
    /// Executes the rotation on the real Rubik when the animation finished
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnRotatingFinished(object sender, RotationFinishedEventArgs e)
    {
      ResetLayerRotation();
      foreach (AnimatedLayerMove m in e.Info.Moves)
      {
        this.Rubik.RotateLayer(new LayerMove(m.Move.Layer, m.Move.Direction, m.Move.Twice));
      }
      _selections.Reset();
    }




    // ** SELECTION **

    /// <summary>
    /// Add for all 54 sub faces of the cube an entry to a selection collection and set the selection to None
    /// </summary>
    private void InitSelection()
    {
      _selections = new SelectionCollection();
      this.Rubik.Cubes.ForEach(c => c.Faces.ToList().ForEach(f =>
        {
          if (f.Color != Color.Black)
            _selections.Add(new PositionSpec() { CubePosition = c.Position.Flags, FacePosition = f.Position }, Selection.None);
        }));
    }

    /// <summary>
    /// Set a selection to all entries in the selection collection
    /// </summary>
    /// <param name="selection">New selection</param>
    private void ResetFaceSelection(Selection selection)
    {
      this.Rubik.Cubes.ForEach(c => c.Faces.Where(f => f.Color != Color.Black).ToList().ForEach(f =>
        {
          PositionSpec pos = new PositionSpec() { FacePosition = f.Position, CubePosition = c.Position.Flags };

          if (_selections[pos].HasFlag(Selection.Possible))
          {
            _selections[pos] = Selection.Possible | selection;
          }
          else if (_selections[pos].HasFlag(Selection.NotPossible))
          {
            _selections[pos] = selection | Selection.NotPossible;
          }
          else
          {
            _selections[pos] = selection;
          }
        }));
    }

    /// <summary>
    /// Updates the selection
    /// </summary>
    /// <param name="e"></param>
    protected override void OnKeyDown(KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
      {
        _selections.Reset();
        _oldSelection = PositionSpec.Default;
        _currentSelection = PositionSpec.Default;
      }
      base.OnKeyDown(e);
    }




    // ** COLOR PICKER **

    private void InitColorPicker()
    {
      this.ContextMenuStrip = new ContextMenuStrip();
      foreach (Color col in Rubik.Colors)
      {
        Bitmap bmp = new Bitmap(16, 16);
        Graphics g = Graphics.FromImage(bmp);
        g.Clear(col);
        g.DrawRectangle(Pens.Black, 0, 0, 15, 15);
        this.ContextMenuStrip.Items.Add(col.Name, bmp);
      }
      this.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
      this.ContextMenuStrip.ItemClicked += ContextMenuStrip_ItemClicked;
      this.ContextMenuStrip.Closed += ((s, e) => this.MouseHandling = true);
    }

    private void ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      Color col = this.Rubik.Colors.First();
      for (int i = 0; i < this.Rubik.Colors.Length; i++)
      {
        if (e.ClickedItem.Text == this.Rubik.Colors[i].Name)
          col = this.Rubik.Colors[i];
      }
      this.Rubik.SetFaceColor(_currentSelection.CubePosition, _currentSelection.FacePosition, col);
    }

    private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      if ((Control.ModifierKeys & Keys.Shift) != 0 && !_currentSelection.IsDefault)
      {
        Color clr = this.Rubik.GetFaceColor(_currentSelection.CubePosition, _currentSelection.FacePosition);
        for (int i = 0; i < this.ContextMenuStrip.Items.Count; i++)
        {
          ((ToolStripMenuItem)this.ContextMenuStrip.Items[i]).Checked = (this.ContextMenuStrip.Items[i].Text == clr.Name);
        }
        this.MouseHandling = false;
        _oldMousePos = new Point(-1, -1);
        this.ContextMenuStrip.Show(Cursor.Position);
      }
      else
        e.Cancel = true;
    }




    // ** RENDERING **

    /// <summary>
    /// Updates the cubeModel (including the selection)
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPaint(PaintEventArgs e)
    {
      if (_buffer[_currentBufferIndex] != null)
      {
        bool threeD = this.DrawingMode == RubiksCubeLib.CubeModel.DrawingMode.ThreeDimensional;
        PositionSpec selectedPos = threeD ? Render(e.Graphics,_buffer[_currentBufferIndex], PointToClient(Cursor.Position)) 
          : Render2D(e.Graphics, _buffer[_currentBufferIndex], PointToClient(Cursor.Position));

        // disallow changes of current selection while color picker is visible
        if (!this.ContextMenuStrip.Visible && this.MouseHandling)
        {
          ResetFaceSelection(Selection.None);

          // set selections
          _selections[_oldSelection] = Selection.Second;
          _selections[selectedPos] |= Selection.First;
          _currentSelection = selectedPos;
        }
      }
    }

    /// <summary>
    /// Renders the current frame
    /// </summary>
    /// <param name="g">Graphics</param>
    /// <param name="frame">Frame to render</param>
    /// <param name="mousePos">Current mouse position</param>
    /// <returns></returns>
    private PositionSpec Render(Graphics g, IEnumerable<Face3D> frame, Point mousePos)
    {
      g.SmoothingMode = SmoothingMode.AntiAlias;
      PositionSpec pos = PositionSpec.Default;

      foreach (Face3D face in frame)
      {
        PointF[] parr = face.Vertices.Select(p => new PointF((float)p.X, (float)p.Y)).ToArray();
        Brush b = new SolidBrush(face.Color);
        double factor = ((Math.Sin((double)Environment.TickCount / (double)200) + 1) / 4) + 0.75;
        PositionSpec facePos = new PositionSpec() { FacePosition = face.Position, CubePosition = face.MasterPosition };

        if (this.MouseHandling)
        {
          if (_selections[facePos].HasFlag(Selection.Second))
            b = new HatchBrush(HatchStyle.Percent75, Color.Black, face.Color);
          else if (_selections[facePos].HasFlag(Selection.NotPossible))
            b = new SolidBrush(Color.FromArgb(face.Color.A, (int)(face.Color.R * 0.3), (int)(face.Color.G * 0.3), (int)(face.Color.B * 0.3)));
          else if (_selections[facePos].HasFlag(Selection.First))
            b = new HatchBrush(HatchStyle.Percent30, Color.Black, face.Color);
          else if (_selections[facePos].HasFlag(Selection.Possible))
            b = new SolidBrush(Color.FromArgb(face.Color.A, (int)(Math.Min(face.Color.R * factor, 255)), (int)(Math.Min(face.Color.G * factor, 255)), (int)(Math.Min(face.Color.B * factor, 255))));
          else b = new SolidBrush(face.Color);
        }
        else
          b = new SolidBrush(face.Color);

        g.FillPolygon(b, parr);
        g.DrawPolygon(new Pen(Color.Black, 1), parr);

        GraphicsPath gp = new GraphicsPath();
        gp.AddPolygon(parr);
        if (gp.IsVisible(mousePos))
          pos = facePos;
      }

      g.FillRectangle(new SolidBrush(this.BackColor), 0, this.Height - 25, this.Width - 1, 24);
      g.DrawRectangle(Pens.Black, 0, this.Height - 25, this.Width - 1, 24);
      g.DrawString(string.Format("[{0}] | {1}", _currentSelection.CubePosition, _currentSelection.FacePosition), this.Font, Brushes.Black, 5, this.Height - 20);

      g.FillRectangle(new SolidBrush(this.BackColor), 0, this.Height - 50, this.Width - 1, 25);
      g.DrawRectangle(Pens.Black, 0, this.Height - 50, this.Width - 1, 25);
      g.DrawString(this.State, this.Font, Brushes.Black, 5, this.Height - 45);

      g.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 50);
     
      return pos;
    }

    public PositionSpec Render2D(Graphics g, IEnumerable<Face3D> frame, Point mousePos)
    {
      g.SmoothingMode = SmoothingMode.AntiAlias;
      PositionSpec pos = PositionSpec.Default;

      int square = 0, borderX = 5, borderY = 5;
      if (((double)(this.Screen.Width - 10) / (double)(this.Screen.Height - 10)) > (4.0 / 3.0))
      {
        square = (int)(this.Screen.Height / 9.0);
        borderX = (this.Screen.Width - 12 * square) / 2;
      }
      else
      {
        square = (int)(this.Screen.Width / 12.0);
        borderY = (this.Screen.Height - 9 * square) / 2;
      }

      List<Face3D> faces = new List<Face3D>();
      foreach (Cube c in this.Rubik.Cubes)
        faces.AddRange(c.Faces.Where(f => c.Position.Flags.HasFlag(CubeFlagService.FromFacePosition(f.Position))).Select(f => new Face3D(null, f.Color, f.Position, c.Position.Flags)));
      frame = faces;

      foreach (Face3D face in frame)
      {
        #region CalculatePoints

        int x = 0, y = 0;
        int xOffs = borderX, yOffs = borderY;

        if (face.Position.HasFlag(FacePosition.Front))
        {
          xOffs += 3 * square; yOffs += 3 * square;
          CubePosition cubePos = new CubePosition(face.MasterPosition);
          x = xOffs + (CubeFlagService.ToInt(cubePos.X) + 1) * square;
          y = yOffs + (CubeFlagService.ToInt(cubePos.Y) * (-1) + 1) * square;
        }

        if (face.Position.HasFlag(FacePosition.Top))
        {
          xOffs += 3 * square;
          CubePosition cubePos = new CubePosition(face.MasterPosition);
          x = xOffs + (CubeFlagService.ToInt(cubePos.X) + 1) * square;
          y = yOffs + (CubeFlagService.ToInt(cubePos.Z) + 1) * square;
        }

        if (face.Position.HasFlag(FacePosition.Bottom))
        {
          xOffs += 3 * square; yOffs += 6 * square;
          CubePosition cubePos = new CubePosition(face.MasterPosition);
          x = xOffs + (CubeFlagService.ToInt(cubePos.X) + 1) * square;
          y = yOffs + (CubeFlagService.ToInt(cubePos.Z) * (-1) + 1) * square;
        }

        if (face.Position.HasFlag(FacePosition.Left))
        {
          yOffs += 3 * square;
          CubePosition cubePos = new CubePosition(face.MasterPosition);
          x = xOffs + (CubeFlagService.ToInt(cubePos.Z) + 1) * square;
          y = yOffs + (CubeFlagService.ToInt(cubePos.Y) * (-1) + 1) * square;
        }

        if (face.Position.HasFlag(FacePosition.Right))
        {
          xOffs += 6 * square; yOffs += 3 * square;
          CubePosition cubePos = new CubePosition(face.MasterPosition);
          x = xOffs + (CubeFlagService.ToInt(cubePos.Z) * (-1) + 1) * square;
          y = yOffs + (CubeFlagService.ToInt(cubePos.Y) * (-1) + 1) * square;
        }

        if (face.Position.HasFlag(FacePosition.Back))
        {
          xOffs += 9 * square; yOffs += 3 * square;
          CubePosition cubePos = new CubePosition(face.MasterPosition);
          x = xOffs + (CubeFlagService.ToInt(cubePos.X) * (-1) + 1) * square;
          y = yOffs + (CubeFlagService.ToInt(cubePos.Y) * (-1) + 1) * square;
        }
        #endregion

        Point[] parr = new Point[] { new Point(x, y), new Point(x, y + square), new Point(x + square, y + square), new Point(x + square, y)  };

        Brush b = new SolidBrush(face.Color);
        double factor = ((Math.Sin((double)Environment.TickCount / (double)200) + 1) / 4) + 0.75;
        PositionSpec facePos = new PositionSpec() { FacePosition = face.Position, CubePosition = face.MasterPosition };

        if (this.MouseHandling)
        {
          if (_selections[facePos].HasFlag(Selection.Second))
            b = new HatchBrush(HatchStyle.Percent75, Color.Black, face.Color);
          else if (_selections[facePos].HasFlag(Selection.NotPossible))
            b = new SolidBrush(Color.FromArgb(face.Color.A, (int)(face.Color.R * 0.3), (int)(face.Color.G * 0.3), (int)(face.Color.B * 0.3)));
          else if (_selections[facePos].HasFlag(Selection.First))
            b = new HatchBrush(HatchStyle.Percent30, Color.Black, face.Color);
          else if (_selections[facePos].HasFlag(Selection.Possible))
            b = new SolidBrush(Color.FromArgb(face.Color.A, (int)(Math.Min(face.Color.R * factor, 255)), (int)(Math.Min(face.Color.G * factor, 255)), (int)(Math.Min(face.Color.B * factor, 255))));
          else b = new SolidBrush(face.Color);
        }
        else
          b = new SolidBrush(face.Color);

        g.FillPolygon(b, parr);
        g.DrawPolygon(new Pen(Color.Black, 1), parr);


        GraphicsPath gp = new GraphicsPath();
        gp.AddPolygon(parr);
        if (gp.IsVisible(mousePos))
          pos = facePos;
      }

      g.DrawRectangle(Pens.Black, 0, this.Height - 25, this.Width - 1, 24);
      //g.DrawLine(Pens.Black, 0, this.Height - 25, this.Width, this.Height - 25);
      g.DrawString(string.Format("[{0}] | {1}", _currentSelection.CubePosition, _currentSelection.FacePosition), this.Font, Brushes.Black, 5, this.Height - 20);

      g.DrawRectangle(Pens.Black, 0, this.Height - 50, this.Width - 1, 25);
      g.DrawString(this.State, this.Font, Brushes.Black, 5, this.Height - 45);

      g.DrawRectangle(Pens.Black, 0, 0, this.Width - 1, this.Height - 50);

      return pos;
    }


    /// <summary>
    /// Generates 3D cubes from the Rubik cubes
    /// </summary>
    /// <returns>Cubes from the Rubik converted to 3D cubes</returns>
    private List<Cube3D> GenCubes3D()
    {
      List<Cube> cubes = this.Rubik.Cubes;

      List<Cube3D> cubes3D = new List<Cube3D>();
      double d = 2.0 / 3.0;
      foreach (Cube c in cubes)
      {
        Cube3D cr = new Cube3D(new Point3D(d * CubeFlagService.ToInt(c.Position.X), d * CubeFlagService.ToInt(c.Position.Y), d * CubeFlagService.ToInt(c.Position.Z)), d / 2, c.Position.Flags, c.Faces);
        if (cr.Position.HasFlag(CubeFlag.TopLayer))
          cr = cr.Rotate(RotationType.Y, LayerRotation[CubeFlag.TopLayer], new Point3D(0, d, 0));
        if (cr.Position.HasFlag(CubeFlag.MiddleLayer))
          cr = cr.Rotate(RotationType.Y, LayerRotation[CubeFlag.MiddleLayer], new Point3D(0, 0, 0));
        if (cr.Position.HasFlag(CubeFlag.BottomLayer))
          cr = cr.Rotate(RotationType.Y, LayerRotation[CubeFlag.BottomLayer], new Point3D(0, -d, 0));
        if (cr.Position.HasFlag(CubeFlag.FrontSlice))
          cr = cr.Rotate(RotationType.Z, LayerRotation[CubeFlag.FrontSlice], new Point3D(0, 0, d));
        if (cr.Position.HasFlag(CubeFlag.MiddleSlice))
          cr = cr.Rotate(RotationType.Z, LayerRotation[CubeFlag.MiddleSlice], new Point3D(0, 0, 0));
        if (cr.Position.HasFlag(CubeFlag.BackSlice))
          cr = cr.Rotate(RotationType.Z, LayerRotation[CubeFlag.BackSlice], new Point3D(0, 0, -d));
        if (cr.Position.HasFlag(CubeFlag.LeftSlice))
          cr = cr.Rotate(RotationType.X, LayerRotation[CubeFlag.LeftSlice], new Point3D(-d, 0, 0));
        if (cr.Position.HasFlag(CubeFlag.MiddleSliceSides))
          cr = cr.Rotate(RotationType.X, LayerRotation[CubeFlag.MiddleSliceSides], new Point3D(0, 0, 0));
        if (cr.Position.HasFlag(CubeFlag.RightSlice))
          cr = cr.Rotate(RotationType.X, LayerRotation[CubeFlag.RightSlice], new Point3D(d, 0, 0));

        cr = cr.Rotate(RotationType.Y, this.Rotation[1], new Point3D(0, 0, 0));
        cr = cr.Rotate(RotationType.Z, this.Rotation[2], new Point3D(0, 0, 0));
        cr = cr.Rotate(RotationType.X, this.Rotation[0], new Point3D(0, 0, 0));
        cubes3D.Add(cr);
      }
      return cubes3D;
    }

    /// <summary>
    /// Projects the 3D cubes to 2D view for rendering
    /// </summary>
    /// <param name="screen">Render screen</param>
    /// <param name="scale">Scale</param>
    /// <returns></returns>
    public IEnumerable<Face3D> GenFacesProjected(Rectangle screen, double scale)
    {
      List<Cube3D> cubesRender = GenCubes3D();
      IEnumerable<Face3D> facesProjected = cubesRender.Select(c => c.Project(screen.Width, screen.Height, 100, 4, scale).Faces).Aggregate((a, b) => a.Concat(b));
      facesProjected = facesProjected.OrderBy(f => f.Vertices.ElementAt(0).Z).Reverse();
      return facesProjected;
    }


    // ** ENLARGE RENDER QUEUE **

    /// <summary>
    /// Registers a new animated rotation
    /// </summary>
    /// <param name="layer">Rotation layer</param>
    /// <param name="direction">Direction of rotation</param>
    public void RotateLayerAnimated(CubeFlag layer, bool direction)
    {
      RotateLayerAnimated(new LayerMove(layer, direction), this.RotationSpeed);
    }


    /// <summary>
    /// Registers a new single animated move or a collection of new animated rotations
    /// </summary>
    /// <param name="move">Movement that will be performed</param>
    public void RotateLayerAnimated(IMove move)
    {
      RotateLayerAnimated(move, this.RotationSpeed);
    }

    /// <summary>
    /// Registers a collection of new animated moves
    /// </summary>
    /// <param name="moves">Collection of moves</param>
    /// <param name="milliseconds">Duration of animated rotation</param>
    public void RotateLayerAnimated(IMove moves, int milliseconds)
    { 
      this.MouseHandling = false;

      if (this.DrawingMode == RubiksCubeLib.CubeModel.DrawingMode.TwoDimensional)
        milliseconds = 0;

      this.Moves.Enqueue(new RotationInfo(moves, milliseconds));
      this.MovesList.Add(new RotationInfo(moves, milliseconds));
      this.State = string.Format("Rotating {0}", Moves.Peek().Name);
    }

  }
}
