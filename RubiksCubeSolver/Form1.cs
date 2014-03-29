using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace VirtualRubik
{
	public partial class Form1 : Form
	{
		private RubikRenderer rubikRenderer;
		private Color[] colors = new Color[] { Color.ForestGreen, Color.RoyalBlue, Color.White, Color.Yellow, Color.Red, Color.Orange };
		private Stack<LayerMove> moveStack = new Stack<LayerMove>();
		private int rotationTime = 200; // in ms
		RenderInfo command = new RenderInfo();


		private PositionSpec oldSelection = new PositionSpec() { CubePosition = Cube3D.RubikPosition.None, FacePosition = Face3D.FacePosition.None };
		private PositionSpec currentSelection = new PositionSpec() { CubePosition = Cube3D.RubikPosition.None, FacePosition = Face3D.FacePosition.None };

		public Form1()
		{
			InitializeComponent();
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.UserPaint, true);
			Icon = VirtualRubik.Properties.Resources.cube;
			foreach (string name in Enum.GetNames(typeof(Cube3D.RubikPosition)))
			{
				int value = (int)Enum.Parse(typeof(Cube3D.RubikPosition), name);
				if (value != 0) comboBox1.Items.Add(name);
			}
			foreach (ToolStripMenuItem menuItem in menuStrip1.Items) ((ToolStripDropDownMenu)menuItem.DropDown).ShowImageMargin = false;
			comboBox1.SelectedIndex = 0;
			foreach (Color col in colors)
			{
				Bitmap bmp = new Bitmap(16, 16);
				Graphics g = Graphics.FromImage(bmp);
				g.Clear(col);
				g.DrawRectangle(Pens.Black, 0, 0, 15, 15);
				contextMenuStrip1.Items.Add(col.Name, bmp, toolStripMenu1_Item_Click);
			}
			this.FormClosing += (sender, e) => rubikRenderer.Abort();
			ResetCube();
		}

		private void Form1_Resize(object sender, EventArgs e)
		{
			Rectangle r = new Rectangle(0, 0, this.ClientRectangle.Width - ((groupBox1.Visible) ? groupBox1.Width : 0), this.ClientRectangle.Height - ((statusStrip1.Visible) ? statusStrip1.Height : 0) - ((statusStrip1.Visible) ? statusStrip2.Height : 0) + menuStrip1.Height);
			int min = Math.Min(r.Height, r.Width);
			double factor = 3 * ((double)min / (double)400);
			if (r.Width > r.Height) r.X = (r.Width - r.Height) / 2;
			else if (r.Height > r.Width) r.Y = (r.Height - r.Width) / 2;

			groupBox1.Width = Math.Max(Math.Min((int)((double)this.ClientRectangle.Width * 0.3), 300), 220);
			rubikRenderer.SetDrawingArea(r, factor);
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			Rectangle r = new Rectangle(0, 0, this.ClientRectangle.Width - ((groupBox1.Visible) ? groupBox1.Width : 0), this.ClientRectangle.Height - ((statusStrip1.Visible) ? statusStrip1.Height : 0) - ((statusStrip1.Visible) ? statusStrip2.Height : 0) + menuStrip1.Height);
			int min = Math.Min(r.Height, r.Width);
			double factor = 3 * ((double)min / (double)400);
			if (r.Width > r.Height) r.X = (r.Width - r.Height) / 2;
			else if (r.Height > r.Width) r.Y = (r.Height - r.Width) / 2;

			rubikRenderer.SetDrawingArea(r, factor);

			RenderInfo currentCommand = command;
			if (currentCommand.FacesProjected != null)
			{
				PositionSpec selectedPos = (contextMenuStrip1.Visible) ? currentSelection : rubikRenderer.RubikManager.RubikCube.Render(e.Graphics, currentCommand, PointToClient(Cursor.Position));
				if (selectedPos.Equals(currentSelection)) rubikRenderer.RubikManager.RubikCube.Render(e.Graphics, currentCommand, PointToClient(Cursor.Position));
				rubikRenderer.RubikManager.setFaceSelection(Face3D.SelectionMode.None);
				rubikRenderer.RubikManager.setFaceSelection(oldSelection.CubePosition, oldSelection.FacePosition, Face3D.SelectionMode.Second);
				rubikRenderer.RubikManager.setFaceSelection(selectedPos.CubePosition, selectedPos.FacePosition, Face3D.SelectionMode.First);
				currentSelection = selectedPos;
				toolStripStatusLabel2.Text = "[" + selectedPos.CubePosition.ToString() + "] | " + selectedPos.FacePosition.ToString();
			}
		}

		#region Buttons

		private void button1_Click(object sender, EventArgs e)
		{
			listBox1.Items.Clear();
			moveStack.Clear();
		}
		private void button2_Click(object sender, EventArgs e)
		{
			groupBox1.Enabled = false;
			//rotationTicks = 100;
			Cube3D.RubikPosition layer = (Cube3D.RubikPosition)Enum.Parse(typeof(Cube3D.RubikPosition), comboBox1.Text);
			bool direction = (button5.Text == "CW");
			rubikRenderer.RubikManager.Rotate90(layer, direction, rotationTime);
			toolStripStatusLabel1.Text = "Rotating " + layer.ToString() + " " + ((button5.Text == "CW") ? "Clockwise" : "Counter-Clockwise");
		}
		private void button3_Click(object sender, EventArgs e)
		{
			String dir = "Clockwise";
			if (button5.Text != "CW") dir = "Counter-Clockwise";
			listBox1.Items.Add(comboBox1.Text + " " + dir);
		}
		private void button4_Click(object sender, EventArgs e)
		{
			if (listBox1.Items.Count > 0)
			{
				//rotationTicks = 10;
				groupBox1.Enabled = false;
				List<LayerMove> lms = new List<LayerMove>();
				for (int i = listBox1.Items.Count - 1; i >= 0; i--)
				{
					String[] commands = listBox1.Items[i].ToString().Split(new Char[] { Convert.ToChar(" ") });
					lms.Add(new LayerMove((Cube3D.RubikPosition)Enum.Parse(typeof(Cube3D.RubikPosition), commands[0]), (commands[1] == "Clockwise")));
				}
				moveStack = new Stack<LayerMove>(lms);
				LayerMove nextMove = moveStack.Pop();
				bool direction = nextMove.Direction;
				rubikRenderer.RubikManager.Rotate90(nextMove.Layer, direction, rotationTime);
				toolStripStatusLabel1.Text = "Rotating " + nextMove.Layer.ToString() + " " + ((nextMove.Direction) ? "Clockwise" : "Counter-Clockwise");
				listBox1.SelectedIndex = 0;
				comboBox1.Text = listBox1.SelectedItem.ToString();
			}
		}
		private void ResetCube()
		{
			Rectangle r = new Rectangle(0, 0, this.ClientRectangle.Width - ((groupBox1.Visible) ? groupBox1.Width : 0), this.ClientRectangle.Height - ((statusStrip1.Visible) ? statusStrip1.Height : 0) - ((statusStrip1.Visible) ? statusStrip2.Height : 0) + menuStrip1.Height);
			int min = Math.Min(r.Height, r.Width);
			double factor = 3 * ((double)min / (double)400);
			if (r.Width > r.Height) r.X = (r.Width - r.Height) / 2;
			else if (r.Height > r.Width) r.Y = (r.Height - r.Width) / 2;
			if (rubikRenderer != null) rubikRenderer.Abort();

			//Create render object, event handling
			rubikRenderer = new RubikRenderer(r, factor);
			rubikRenderer.OnRender += new RubikRenderer.RenderHandler(Render);
			rubikRenderer.RubikManager.OnRotatingFinished += new RubikManager.RotatingFinishedHandler(RotatingFinished);
			//Start update and render processs
			rubikRenderer.Start();

			toolStripStatusLabel1.Text = "Ready";
		}

		void Render(object sender, RenderEventArgs e)
		{
			command = e.RenderInfo;
			this.Invalidate();
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Delete)
			{
				rubikRenderer.RubikManager.RubikCube.cubes.ForEach(c => c.Faces.ToList().ForEach(f => f.Selection = Face3D.SelectionMode.None));
				oldSelection = new PositionSpec() { CubePosition = Cube3D.RubikPosition.None, FacePosition = Face3D.FacePosition.None };
				currentSelection = new PositionSpec() { CubePosition = Cube3D.RubikPosition.None, FacePosition = Face3D.FacePosition.None };
			}
		}

		private void scrambleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			groupBox1.Enabled = false;
			Random rnd = new Random();
			for (int i = 0; i < 50; i++) rubikRenderer.RubikManager.Rotate90Sync((Cube3D.RubikPosition)Math.Pow(2, rnd.Next(0, 9)), Convert.ToBoolean(rnd.Next(0, 2)));
			groupBox1.Enabled = true;
		}
		private void resetToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetCube();
		}
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("RubiksCubeSolver Beta 0.9.5 by Switcherlapp97", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		#endregion

		private void RotatingFinished(object sender)
		{
			if (moveStack.Count > 0)
			{
				LayerMove nextMove = moveStack.Pop();
				bool direction = nextMove.Direction;
				rubikRenderer.RubikManager.Rotate90(nextMove.Layer, direction, rotationTime);
				toolStripStatusLabel1.Text = "Rotating " + nextMove.Layer.ToString() + " " + ((nextMove.Direction) ? "Clockwise" : "Counter-Clockwise");
				if (listBox1.InvokeRequired) listBox1.Invoke((MethodInvoker)delegate() { listBox1.SelectedIndex++; });
				if (comboBox1.InvokeRequired) comboBox1.Invoke((MethodInvoker)delegate() { comboBox1.Text = listBox1.SelectedItem.ToString(); });
			}
			else
			{
				if (groupBox1.InvokeRequired) groupBox1.Invoke((MethodInvoker)delegate() { groupBox1.Enabled = true; });
				if (listBox1.InvokeRequired) listBox1.Invoke((MethodInvoker)delegate()
				{
					if (listBox1.SelectedIndex != listBox1.Items.Count - 1) listBox1.SelectedIndex++;
					else listBox1.SelectedIndex = -1;
				});
				toolStripStatusLabel1.Text = "Ready";
			}
		}

		#region Mouse Handling

		private Point oldMousePos = new Point(-1, -1);
		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			if (oldMousePos.X != -1 && oldMousePos.Y != -1)
			{
				if (e.Button == System.Windows.Forms.MouseButtons.Right || e.Button == System.Windows.Forms.MouseButtons.Middle)
				{
					this.Cursor = Cursors.SizeAll;
					int dX = e.X - oldMousePos.X;
					int dY = e.Y - oldMousePos.Y;
					rubikRenderer.RubikManager.RubikCube.Rotation[1] -= dX / 3;
					rubikRenderer.RubikManager.RubikCube.Rotation[0] += (dY / 3);
					//rotationAccum.Rotate(Point3D.RotationType.X, (dY));
					//rotationAccum.Rotate(Point3D.RotationType.Y, -(dX));
					//double rotY = Math.Atan2(rotationAccum.X, rotationAccum.Z);
					//Point3D pp = new Point3D(rotationAccum.X, rotationAccum.Y, rotationAccum.Z);
					//pp.Rotate(Point3D.RotationType.Y, -rotY * (180 / Math.PI));
					//double rotX = Math.Atan2(-pp.Y, pp.Z);
					//pp.Rotate(Point3D.RotationType.X, -rotX * (180 / Math.PI));
					//rubikManager.RubikCube.Rotation[0] = rotX * (180 / Math.PI);
					//rubikManager.RubikCube.Rotation[1] = rotY * (180 / Math.PI);
				}
				else
				{
					this.Cursor = Cursors.Arrow;
				}
				this.Invalidate();
			}
			oldMousePos = e.Location;
		}
		private void Form1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				if ((Control.ModifierKeys & Keys.Shift) != 0)
				{
					if (currentSelection.CubePosition != Cube3D.RubikPosition.None && currentSelection.FacePosition != Face3D.FacePosition.None)
					{
						Color clr = rubikRenderer.RubikManager.getFaceColor(currentSelection.CubePosition, currentSelection.FacePosition);
						int ind = 0;
						for (int i = 0; i < contextMenuStrip1.Items.Count; i++)
						{
							if (contextMenuStrip1.Items[i].Text == clr.Name) ind = i;
						}
						ind++;
						if (ind >= colors.Length) ind = 0;
						rubikRenderer.RubikManager.setFaceColor(currentSelection.CubePosition, currentSelection.FacePosition, colors[ind]);
					}
				}
				else
				{
					if (!contextMenuStrip1.Visible)
					{
						if (oldSelection.CubePosition == Cube3D.RubikPosition.None || oldSelection.FacePosition == Face3D.FacePosition.None)
						{
							if (currentSelection.CubePosition == Cube3D.RubikPosition.None || currentSelection.FacePosition == Face3D.FacePosition.None)
							{
								rubikRenderer.RubikManager.RubikCube.cubes.ForEach(c => c.Faces.ToList().ForEach(f => f.Selection = Face3D.SelectionMode.None));
								oldSelection = new PositionSpec() { CubePosition = Cube3D.RubikPosition.None, FacePosition = Face3D.FacePosition.None };
								currentSelection = new PositionSpec() { CubePosition = Cube3D.RubikPosition.None, FacePosition = Face3D.FacePosition.None };
							}
							else
							{
								if (!Cube3D.isCorner(currentSelection.CubePosition))
								{
									oldSelection = currentSelection;
									rubikRenderer.RubikManager.RubikCube.cubes.ForEach(c => c.Faces.ToList().ForEach(f =>
									{
										if (currentSelection.CubePosition != c.Position && !Cube3D.isCenter(c.Position) && currentSelection.FacePosition == f.Position)
										{
											Cube3D.RubikPosition assocLayer = Face3D.layerAssocToFace(currentSelection.FacePosition);
											Cube3D.RubikPosition commonLayer = Cube3D.getCommonLayer(currentSelection.CubePosition, c.Position, assocLayer);
											if (commonLayer != Cube3D.RubikPosition.None && c.Position.HasFlag(commonLayer))
											{
												f.Selection |= Face3D.SelectionMode.Possible;
											}
											else
											{
												f.Selection |= Face3D.SelectionMode.NotPossible;
											}
										}
										else
										{
											f.Selection |= Face3D.SelectionMode.NotPossible;
										}
									}));
									toolStripStatusLabel1.Text = "First selection: [" + currentSelection.CubePosition.ToString() + "] | " + currentSelection.FacePosition.ToString(); ;
								}
								else
								{
									rubikRenderer.RubikManager.RubikCube.cubes.ForEach(c => c.Faces.ToList().ForEach(f => f.Selection = Face3D.SelectionMode.None));
									toolStripStatusLabel1.Text = "Error: Invalid first selection, must not be a corner";
								}
							}
						}
						else
						{
							if (currentSelection.CubePosition == Cube3D.RubikPosition.None || currentSelection.FacePosition == Face3D.FacePosition.None)
							{
								toolStripStatusLabel1.Text = "Ready";
							}
							else
							{
								if (currentSelection.CubePosition != oldSelection.CubePosition)
								{
									if (!Cube3D.isCenter(currentSelection.CubePosition))
									{
										if (oldSelection.FacePosition == currentSelection.FacePosition)
										{
											Cube3D.RubikPosition assocLayer = Face3D.layerAssocToFace(oldSelection.FacePosition);
											Cube3D.RubikPosition commonLayer = Cube3D.getCommonLayer(oldSelection.CubePosition, currentSelection.CubePosition, assocLayer);
											Boolean direction = true;
											if (commonLayer == Cube3D.RubikPosition.TopLayer || commonLayer == Cube3D.RubikPosition.MiddleLayer || commonLayer == Cube3D.RubikPosition.BottomLayer)
											{
												if (((oldSelection.FacePosition == Face3D.FacePosition.Back) && currentSelection.CubePosition.HasFlag(Cube3D.RubikPosition.RightSlice))
												|| ((oldSelection.FacePosition == Face3D.FacePosition.Left) && currentSelection.CubePosition.HasFlag(Cube3D.RubikPosition.BackSlice))
												|| ((oldSelection.FacePosition == Face3D.FacePosition.Front) && currentSelection.CubePosition.HasFlag(Cube3D.RubikPosition.LeftSlice))
												|| ((oldSelection.FacePosition == Face3D.FacePosition.Right) && currentSelection.CubePosition.HasFlag(Cube3D.RubikPosition.FrontSlice))) direction = false;
											}
											if (commonLayer == Cube3D.RubikPosition.LeftSlice || commonLayer == Cube3D.RubikPosition.MiddleSlice_Sides || commonLayer == Cube3D.RubikPosition.RightSlice)
											{
												if (((oldSelection.FacePosition == Face3D.FacePosition.Bottom) && currentSelection.CubePosition.HasFlag(Cube3D.RubikPosition.BackSlice))
												|| ((oldSelection.FacePosition == Face3D.FacePosition.Back) && currentSelection.CubePosition.HasFlag(Cube3D.RubikPosition.TopLayer))
												|| ((oldSelection.FacePosition == Face3D.FacePosition.Top) && currentSelection.CubePosition.HasFlag(Cube3D.RubikPosition.FrontSlice))
												|| ((oldSelection.FacePosition == Face3D.FacePosition.Front) && currentSelection.CubePosition.HasFlag(Cube3D.RubikPosition.BottomLayer))) direction = false;
											}
											if (commonLayer == Cube3D.RubikPosition.BackSlice || commonLayer == Cube3D.RubikPosition.MiddleSlice || commonLayer == Cube3D.RubikPosition.FrontSlice)
											{
												if (((oldSelection.FacePosition == Face3D.FacePosition.Top) && currentSelection.CubePosition.HasFlag(Cube3D.RubikPosition.RightSlice))
												|| ((oldSelection.FacePosition == Face3D.FacePosition.Right) && currentSelection.CubePosition.HasFlag(Cube3D.RubikPosition.BottomLayer))
												|| ((oldSelection.FacePosition == Face3D.FacePosition.Bottom) && currentSelection.CubePosition.HasFlag(Cube3D.RubikPosition.LeftSlice))
												|| ((oldSelection.FacePosition == Face3D.FacePosition.Left) && currentSelection.CubePosition.HasFlag(Cube3D.RubikPosition.TopLayer))) direction = false;
											}
											if (commonLayer != Cube3D.RubikPosition.None)
											{
												if (groupBox1.Enabled)
												{
													groupBox1.Enabled = false;
													//rotationTicks = 25;
													if (commonLayer == Cube3D.RubikPosition.TopLayer || commonLayer == Cube3D.RubikPosition.LeftSlice || commonLayer == Cube3D.RubikPosition.FrontSlice) direction = !direction;
													rubikRenderer.RubikManager.Rotate90(commonLayer, direction, rotationTime);
													comboBox1.Text = commonLayer.ToString();
													toolStripStatusLabel1.Text = "Rotating " + commonLayer.ToString() + " " + ((direction) ? "Clockwise" : "Counter-Clockwise");
												}
											}
											else
											{
												toolStripStatusLabel1.Text = "Error: Invalid second selection, does not specify distinct layer";
											}
										}
										else
										{
											toolStripStatusLabel1.Text = "Error: Invalid second selection, must match orientation of first selection";
										}
									}
									else
									{
										toolStripStatusLabel1.Text = "Error: Invalid second selection, must not be a center";
									}
								}
								else
								{
									toolStripStatusLabel1.Text = "Error: Invalid second selection, must not be first selection";
								}
							}
							rubikRenderer.RubikManager.RubikCube.cubes.ForEach(c => c.Faces.ToList().ForEach(f => f.Selection = Face3D.SelectionMode.None));
							oldSelection = new PositionSpec() { CubePosition = Cube3D.RubikPosition.None, FacePosition = Face3D.FacePosition.None };
							currentSelection = new PositionSpec() { CubePosition = Cube3D.RubikPosition.None, FacePosition = Face3D.FacePosition.None };
						}
					}
				}
			}
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				if ((Control.ModifierKeys & Keys.Shift) != 0)
				{
					if (currentSelection.CubePosition != Cube3D.RubikPosition.None && currentSelection.FacePosition != Face3D.FacePosition.None)
					{
						Color clr = rubikRenderer.RubikManager.getFaceColor(currentSelection.CubePosition, currentSelection.FacePosition);
						for (int i = 0; i < contextMenuStrip1.Items.Count; i++)
						{
							((ToolStripMenuItem)contextMenuStrip1.Items[i]).Checked = (contextMenuStrip1.Items[i].Text == clr.Name);
						}
						contextMenuStrip1.Show(Cursor.Position);
					}
				}
			}
		}
		private void groupBox1_EnabledChanged(object sender, EventArgs e)
		{
			if (!groupBox1.Enabled)
			{
				rubikRenderer.RubikManager.RubikCube.cubes.ForEach(c => c.Faces.ToList().ForEach(f => f.Selection = Face3D.SelectionMode.None));
				oldSelection = new PositionSpec() { CubePosition = Cube3D.RubikPosition.None, FacePosition = Face3D.FacePosition.None };
				currentSelection = new PositionSpec() { CubePosition = Cube3D.RubikPosition.None, FacePosition = Face3D.FacePosition.None };
			}
		}

		#endregion

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (toolStripMenuItem1.Text == "Controls <<")
			{
				toolStripMenuItem1.Text = "Controls >>";
				statusStrip1.Visible = true;
				statusStrip2.Visible = true;
				groupBox1.Visible = true;
			}
			else
			{
				toolStripMenuItem1.Text = "Controls <<";
				statusStrip1.Visible = false;
				statusStrip2.Visible = false;
				groupBox1.Visible = false;
			}
		}

		private void button5_Click(object sender, EventArgs e)
		{
			if (button5.Text == "CW")
			{
				button5.Text = "CCW";
			}
			else
			{
				button5.Text = "CW";
			}
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex != -1)
				comboBox1.Text = listBox1.SelectedItem.ToString().Split(new Char[] { Convert.ToChar(" ") })[0];
		}

		private void button6_Click(object sender, EventArgs e)
		{
			listBox1.Items.Clear();
			CubeSolverBeginner cs = new CubeSolverBeginner(rubikRenderer.RubikManager);
			if (cs.CanSolve())
			{
				RubikManager ma = cs.ReturnRubik();
				ma.OnRotatingFinished += RotatingFinished;
				ma.Moves.ForEach(m =>
				{
					if (m.Layer == Cube3D.RubikPosition.TopLayer || m.Layer == Cube3D.RubikPosition.LeftSlice || m.Layer == Cube3D.RubikPosition.FrontSlice) m.Direction = !m.Direction;
					listBox1.Items.Add(m.Layer.ToString() + " " + ((m.Direction) ? "Clockwise" : "Counter-Clockwise"));
				});
				if (listBox1.Items.Count > 0) listBox1.SelectedIndex = 0;
				listBox1.Focus();
			}
			else MessageBox.Show("Insoluble cube");
			this.Invalidate();
		}

		private void solveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CubeSolverBeginner cs = new CubeSolverBeginner(rubikRenderer.RubikManager);
			if (cs.CanSolve())
			{
				rubikRenderer.RubikManager = cs.ReturnRubik().Clone();
				rubikRenderer.RubikManager.Moves.Clear();
			}
			else MessageBox.Show("Insoluble cube");
		}

		private void toolStripMenu1_Item_Click(object sender, EventArgs e)
		{
			rubikRenderer.RubikManager.setFaceColor(currentSelection.CubePosition, currentSelection.FacePosition, Color.FromName(((ToolStripMenuItem)sender).Text));
		}

		private void listBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space)
			{
				//rotationTicks = 25;
				if (listBox1.SelectedIndex == -1 & listBox1.Items.Count > 0) listBox1.SelectedIndex = 0;
				if (listBox1.Items.Count > 0)
				{
					string layerString = listBox1.SelectedItem.ToString().Split(new Char[] { Convert.ToChar(" ") })[0];
					Cube3D.RubikPosition layer = (Cube3D.RubikPosition)Enum.Parse(typeof(Cube3D.RubikPosition), layerString);

					string directionString = listBox1.SelectedItem.ToString().Split(new Char[] { Convert.ToChar(" ") })[1];
					bool direction = (directionString == "Clockwise") ? true : false;

					rubikRenderer.RubikManager.Rotate90(layer, direction, rotationTime);
				}
			}
		}

	}

}
