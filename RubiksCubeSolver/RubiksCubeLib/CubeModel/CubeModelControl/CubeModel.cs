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

namespace RubiksCubeLib.CubeModel
{
	/// <summary>
	/// Represents a 3D rubiks cube
	/// </summary>
	public partial class CubeModel : UserControl
	{
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

		/// <summary>
		/// Gets the information about the drawn Rubik's Cube
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Rubik Rubik { get; private set; }

		private CubeModelRenderer renderer;
		private IEnumerable<Face3D> currentFrame;

		/// <summary>
		/// Occurs when the selection of any sub face has changed
		/// </summary>
		/// <param name="sender">The source of the event</param>
		/// <param name="e">Selection changed event data</param>
		public delegate void SelectionChangedHandler(object sender, SelectionChangedEventArgs e);
		public event SelectionChangedHandler OnSelectionChanged;
		private void BroadCastSelectionChanged()
		{
			if (OnSelectionChanged == null)
				return;
			OnSelectionChanged(this, new SelectionChangedEventArgs(currentSelection.CubePosition, currentSelection.FacePosition));
		}

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
			Rubik = rubik;
			Rotation = new double[] { 0, 0, 0 };
			Moves = new Queue<RotationInfo>();
			MouseHandling = true;

			InitColorPicker();
			ResetLayerRotation();
			InitSelection();

			renderer = new CubeModelRenderer(this, this.ClientRectangle);
			renderer.OnRender += OnRender;
			renderer.OnRotatingFinished += OnRotatingFinished;

			int min = Math.Min(ClientRectangle.Height, ClientRectangle.Width);
			currentFrame = GenFacesProjected(this.ClientRectangle, 3 * ((double)min / (double)400));
			renderer.StartRender();

			InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.UserPaint, true);
		}

		#region Selection
		private SelectionCollection selections;

		/// <summary>
		/// Add for all 54 sub faces of the cube an entry to a selection collection and set the selection to None
		/// </summary>
		private void InitSelection()
		{
			selections = new SelectionCollection();
			Rubik.Cubes.ForEach(c => c.Faces.ToList().ForEach(f =>
			  {
				  if (f.Color != Color.Black)
					  selections.Add(new PositionSpec() { CubePosition = c.Position.Flags, FacePosition = f.Position }, Selection.None);
			  }));
		}

		/// <summary>
		/// Set a selection to all entries in the selection collection
		/// </summary>
		/// <param name="selection">New selection</param>
		private void ResetFaceSelection(Selection selection)
		{
			Rubik.Cubes.ForEach(c => c.Faces.Where(f => f.Color != Color.Black).ToList().ForEach(f =>
			  {
				  PositionSpec pos = new PositionSpec() { FacePosition = f.Position, CubePosition = c.Position.Flags };

				  if (selections[pos].HasFlag(Selection.Possible))
				  {
					  selections[pos] = Selection.Possible | selection;
				  }
				  else if (selections[pos].HasFlag(Selection.NotPossible))
				  {
					  selections[pos] = selection | Selection.NotPossible;
				  }
				  else
				  {
					  selections[pos] = selection;
				  }
			  }));
		}
		#endregion

		#region ContextMenuStrip ColorPicker
		/// <summary>
		/// Generates a context menu strip to allow users to change face colors
		/// </summary>
		private void InitColorPicker()
		{
			ContextMenuStrip = new ContextMenuStrip();
			foreach (Color col in Rubik.Colors)
			{
				Bitmap bmp = new Bitmap(16, 16);
				Graphics g = Graphics.FromImage(bmp);
				g.Clear(col);
				g.DrawRectangle(Pens.Black, 0, 0, 15, 15);
				ContextMenuStrip.Items.Add(col.Name, bmp);
			}
			ContextMenuStrip.Opening += ContextMenuStrip_Opening;
			ContextMenuStrip.ItemClicked += ContextMenuStrip_ItemClicked;
			ContextMenuStrip.Closed += ContextMenuStrip_Closed;
		}

		private void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			MouseHandling = true;
		}

		private void ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			Color col = Rubik.Colors.First();
			for (int i = 0; i < Rubik.Colors.Length; i++)
			{
				if (e.ClickedItem.Text == Rubik.Colors[i].Name)
					col = Rubik.Colors[i];
			}
			Rubik.SetFaceColor(currentSelection.CubePosition, currentSelection.FacePosition, col);
		}

		private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			if ((Control.ModifierKeys & Keys.Shift) != 0 && !currentSelection.IsDefault)
			{
				Color clr = Rubik.GetFaceColor(currentSelection.CubePosition, currentSelection.FacePosition);
				for (int i = 0; i < ContextMenuStrip.Items.Count; i++)
				{
					((ToolStripMenuItem)ContextMenuStrip.Items[i]).Checked = (ContextMenuStrip.Items[i].Text == clr.Name);
				}
				MouseHandling = false;
				oldMousePos = new Point(-1, -1);
				ContextMenuStrip.Show(Cursor.Position);
			}
			else
				e.Cancel = true;
		}
		#endregion

		/// <summary>
		/// Resets the rotation of specific layers
		/// </summary>
		private void ResetLayerRotation()
		{
			LayerRotation = new Dictionary<CubeFlag, double>();
			foreach (CubeFlag rp in (CubeFlag[])Enum.GetValues(typeof(CubeFlag)))
			{
				LayerRotation[rp] = 0;
			}
		}

		/// <summary>
		/// Start render cycle
		/// </summary>
		public void StartRender()
		{
			renderer.StartRender();
		}

		/// <summary>
		/// Stop render cycle
		/// </summary>
		public void StopRender()
		{
			renderer.StopRender();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				selections.Reset();
				oldSelection = PositionSpec.Default;
				currentSelection = PositionSpec.Default;
			}
			base.OnKeyDown(e);
		}

		private void OnRotatingFinished(object sender, RotationFinishedEventArgs e)
		{
			ResetLayerRotation();
			foreach (AnimatedLayerMove m in e.Info.Moves)
			{
				Rubik.RotateLayer(m.Move.Layer, m.Move.Direction);
			}
			selections.Reset();
		}

		private void OnRender(object sender, IEnumerable<Face3D> e)
		{
			currentFrame = e;
			Invalidate();
		}

		/// <summary>
		/// Registers a new animated rotation
		/// </summary>
		/// <param name="move">Movement that will be performed</param>
		/// <param name="milliseconds">Duration of the animatied rotation</param>
		public void RotateLayerAnimated(LayerMove move, int milliseconds)
		{
			Moves.Enqueue(new RotationInfo(move, milliseconds));
		}

		/// <summary>
		/// Registers a new animated rotation
		/// </summary>
		/// <param name="layer">Rotation layer</param>
		/// <param name="direction">Direction of rotation</param>
		public void RotateLayerAnimated(CubeFlag layer, bool direction)
		{
			RotateLayerAnimated(new LayerMove(layer, direction), 200);
		}

		/// <summary>
		/// Registers a collection of new animated moves
		/// </summary>
		/// <param name="moves">Collection of moves</param>
		/// <param name="milliseconds">Duration of animated rotation</param>
		public void RotateLayersAnimated(LayerMoveCollection moves, int milliseconds)
		{
			Moves.Enqueue(new RotationInfo(moves, milliseconds));
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

		/// <summary>
		/// Generates 3D cubes from the Rubik cubes
		/// </summary>
		/// <returns>Cubes from the Rubik converted to 3D cubes</returns>
		private List<Cube3D> GenCubes3D()
		{
			List<Cube> cubes = Rubik.Cubes;

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

				cr = cr.Rotate(RotationType.Z, Rotation[2], new Point3D(0, 0, 0));
				cr = cr.Rotate(RotationType.X, Rotation[0], new Point3D(0, 0, 0));
				cr = cr.Rotate(RotationType.Y, Rotation[1], new Point3D(0, 0, 0));
				cubes3D.Add(cr);
			}
			return cubes3D;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			if (currentFrame != null)
			{
				PositionSpec selectedPos = Render(e.Graphics, currentFrame, PointToClient(Cursor.Position));

				// disallow changes of current selection while color picker is visible
				if (!ContextMenuStrip.Visible)
				{
					ResetFaceSelection(Selection.None);

					// set selections
					selections[oldSelection] = Selection.Second;
					selections[selectedPos] |= Selection.First;
					currentSelection = selectedPos;
					BroadCastSelectionChanged();
				}
			}
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			renderer.SetDrawingArea(ClientRectangle);
			Invalidate();
			base.OnSizeChanged(e);
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
				GraphicsPath gp = new GraphicsPath();
				gp.AddPolygon(parr);

				double fak = ((Math.Sin((double)Environment.TickCount / (double)200) + 1) / 4) + 0.75;
				PositionSpec facePos = new PositionSpec() { FacePosition = face.Position, CubePosition = face.MasterPosition };
				if (gp.IsVisible(mousePos))
					pos = facePos;
				if (selections[facePos].HasFlag(Selection.Second))
					g.FillPolygon(new HatchBrush(HatchStyle.Percent75, Color.Black, face.Color), parr);
				else if (selections[facePos].HasFlag(Selection.NotPossible))
					g.FillPolygon(new SolidBrush(Color.FromArgb(face.Color.A, (int)(face.Color.R * 0.15), (int)(face.Color.G * 0.15), (int)(face.Color.B * 0.15))), parr);
				else if (selections[facePos].HasFlag(Selection.First))
					g.FillPolygon(new HatchBrush(HatchStyle.Percent30, Color.Black, face.Color), parr);
				else if (selections[facePos].HasFlag(Selection.Possible))
					g.FillPolygon(new SolidBrush(Color.FromArgb(face.Color.A, (int)(Math.Min(face.Color.R * fak, 255)), (int)(Math.Min(face.Color.G * fak, 255)), (int)(Math.Min(face.Color.B * fak, 255)))), parr);
				else
					g.FillPolygon(new SolidBrush(face.Color), parr);

				g.DrawPolygon(Pens.Black, parr);
			}

			g.DrawString(string.Format("X: {0:f1} | Y: {1:f1} | Z: {2:f1}", Rotation[0], Rotation[1], Rotation[2]), this.Font, Brushes.Black, 5, this.Height - 20);
			return pos;
		}

		/// <summary>
		/// Resets the Rubik object and reset all cube and layer rotations and all selections
		/// </summary>
		public void ResetCube()
		{
			Rubik = new Rubik();
			Rotation = new double[] { 0, 0, 0 };
			Moves = new Queue<RotationInfo>();
			MouseHandling = true;
			ResetLayerRotation();
			InitSelection();
		}
	}
}
