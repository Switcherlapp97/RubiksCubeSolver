using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace VirtualRubik
{
    public partial class Form1 : Form
    {
        private RubikManager rubikManager;
        private Stack<LayerMove> moveStack = new Stack<LayerMove>();
        private int rotationTicks;
        private Timer timer;
        //private Point3D rotationAccum;

        private RubikManager.PositionSpec oldSelection = new RubikManager.PositionSpec() { cubePos = Cube3D.RubikPosition.None, facePos = Face3D.FacePosition.None };
        private RubikManager.PositionSpec currentSelection = new RubikManager.PositionSpec() { cubePos = Cube3D.RubikPosition.None, facePos = Face3D.FacePosition.None };

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
            ResetCube();
            timer = new Timer();
            timer.Interval = 10;
            timer.Tick += timer_Tick;
            timer.Start();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            groupBox1.Width = Math.Max(Math.Min((int)((double)this.ClientRectangle.Width * 0.3), 300), 220);
            this.Invalidate();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Rectangle r = new Rectangle(0, 0, this.ClientRectangle.Width - ((groupBox1.Visible)? groupBox1.Width:0), this.ClientRectangle.Height - ((statusStrip1.Visible)? statusStrip1.Height:0) - ((statusStrip1.Visible)? statusStrip2.Height:0) + menuStrip1.Height);
            int min = Math.Min(r.Height, r.Width);
            double factor = 3 * ((double)min / (double)400);
            if (r.Width > r.Height) r.X = (r.Width - r.Height) / 2;
            else if (r.Height > r.Width) r.Y = (r.Height - r.Width) / 2;
            RubikManager.PositionSpec selectedPos = rubikManager.Render(e.Graphics, r, factor, PointToClient(Cursor.Position));
            rubikManager.setFaceSelection(Face3D.SelectionMode.None);
            rubikManager.setFaceSelection(oldSelection.cubePos, oldSelection.facePos, Face3D.SelectionMode.Second);
            rubikManager.setFaceSelection(selectedPos.cubePos, selectedPos.facePos, Face3D.SelectionMode.First);
            currentSelection = selectedPos;
            toolStripStatusLabel2.Text = "[" + selectedPos.cubePos.ToString() + "] | " + selectedPos.facePos.ToString();
        }
        //private void dpp(Point3D p, Graphics g, Brush c)
        //{
        //    Point3D proj = p.Project(400, 400, 100, 4, 3);
        //    int size = (int)((double)3 * (3 - (proj.Z - 1.5)));
        //    g.FillEllipse(c, new Rectangle((int)proj.X-(size/2), (int)proj.Y-(size/2), size, size));
        //}

        #region Buttons

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            moveStack.Clear();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            rotationTicks = 25;
            Cube3D.RubikPosition layer = (Cube3D.RubikPosition)Enum.Parse(typeof(Cube3D.RubikPosition), comboBox1.Text);
            bool direction = (button5.Text == "CW");
            rubikManager.Rotate90(layer, direction, rotationTicks);
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
                rotationTicks = 25;
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
                if (nextMove.Layer == Cube3D.RubikPosition.TopLayer || nextMove.Layer == Cube3D.RubikPosition.LeftSlice || nextMove.Layer == Cube3D.RubikPosition.FrontSlice) direction = !direction;
                rubikManager.Rotate90(nextMove.Layer, direction, rotationTicks);
                toolStripStatusLabel1.Text = "Rotating " + nextMove.Layer.ToString() + " " + ((nextMove.Direction) ? "Clockwise" : "Counter-Clockwise");
                listBox1.SelectedIndex = 0;
                comboBox1.Text = listBox1.SelectedItem.ToString();
            }
        }
        private void ResetCube()
        {
            rubikManager = new RubikManager();
            rubikManager.OnRotatingFinished += new RubikManager.RotatingFinishedHandler(RotatingFinished);
            //rotationAccum = new Point3D(Math.Sqrt(0.5), Math.Sqrt(0.5), Math.Sqrt(0.5));
            toolStripStatusLabel1.Text = "Ready";
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Delete)
            {
                rubikManager.RubikCube.cubes.ForEach(c => c.Faces.ToList().ForEach(f => f.Selection = Face3D.SelectionMode.None));
                oldSelection = new RubikManager.PositionSpec() { cubePos = Cube3D.RubikPosition.None, facePos = Face3D.FacePosition.None };
                currentSelection = new RubikManager.PositionSpec() { cubePos = Cube3D.RubikPosition.None, facePos = Face3D.FacePosition.None };
            }
        }
        private void scrambleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = false;
            Random rnd = new Random();
            for (int i = 0; i < 50; i++) rubikManager.Rotate90Sync((Cube3D.RubikPosition)Math.Pow(2, rnd.Next(0, 9)), Convert.ToBoolean(rnd.Next(0, 2)));
            groupBox1.Enabled = true;
        }
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetCube();
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("VirtualRubik Version " + Application.ProductVersion, "VirtualRubik - About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        private void RotatingFinished(object sender)
        {
            if (moveStack.Count > 0)
            {
                LayerMove nextMove = moveStack.Pop();
                bool direction = nextMove.Direction;
                if (nextMove.Layer == Cube3D.RubikPosition.TopLayer || nextMove.Layer == Cube3D.RubikPosition.LeftSlice || nextMove.Layer == Cube3D.RubikPosition.FrontSlice) direction = !direction;
                rubikManager.Rotate90(nextMove.Layer, direction, rotationTicks);
                toolStripStatusLabel1.Text = "Rotating " + nextMove.Layer.ToString() + " " + ((nextMove.Direction) ? "Clockwise" : "Counter-Clockwise");
                listBox1.SelectedIndex++;
                comboBox1.Text = listBox1.SelectedItem.ToString();
            }
            else
            {
                groupBox1.Enabled = true;
                listBox1.SelectedIndex = -1;
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
                    rubikManager.RubikCube.Rotation[1] -= dX / 3;
                    rubikManager.RubikCube.Rotation[0] += (dY / 3);

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
            }
            oldMousePos = e.Location;

        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (oldSelection.cubePos == Cube3D.RubikPosition.None || oldSelection.facePos == Face3D.FacePosition.None)
                {
                    if (currentSelection.cubePos == Cube3D.RubikPosition.None || currentSelection.facePos == Face3D.FacePosition.None)
                    {
                        rubikManager.RubikCube.cubes.ForEach(c => c.Faces.ToList().ForEach(f => f.Selection = Face3D.SelectionMode.None));
                        oldSelection = new RubikManager.PositionSpec() { cubePos = Cube3D.RubikPosition.None, facePos = Face3D.FacePosition.None };
                        currentSelection = new RubikManager.PositionSpec() { cubePos = Cube3D.RubikPosition.None, facePos = Face3D.FacePosition.None };
                    }
                    else
                    {
                        if (!Cube3D.isCorner(currentSelection.cubePos))
                        {
                            oldSelection = currentSelection;
                            rubikManager.RubikCube.cubes.ForEach(c => c.Faces.ToList().ForEach(f =>
                            {
                                if (currentSelection.cubePos != c.Position && !Cube3D.isCenter(c.Position) && currentSelection.facePos == f.Position)
                                {
                                    Cube3D.RubikPosition assocLayer = Face3D.layerAssocToFace(currentSelection.facePos);
                                    Cube3D.RubikPosition commonLayer = Cube3D.getCommonLayer(currentSelection.cubePos, c.Position, assocLayer);
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
                            toolStripStatusLabel1.Text = "First selection: [" + currentSelection.cubePos.ToString() + "] | " + currentSelection.facePos.ToString(); ;
                        }
                        else
                        {
                            rubikManager.RubikCube.cubes.ForEach(c => c.Faces.ToList().ForEach(f => f.Selection = Face3D.SelectionMode.None));
                            toolStripStatusLabel1.Text = "Error: Invalid first selection, must not be a corner";
                        }
                    }
                }
                else
                {
                    if (currentSelection.cubePos == Cube3D.RubikPosition.None || currentSelection.facePos == Face3D.FacePosition.None)
                    {
                        toolStripStatusLabel1.Text = "Ready";
                    }
                    else
                    {
                        if (currentSelection.cubePos != oldSelection.cubePos)
                        {
                            if (!Cube3D.isCenter(currentSelection.cubePos))
                            {
                                if (oldSelection.facePos == currentSelection.facePos)
                                {
                                    Cube3D.RubikPosition assocLayer = Face3D.layerAssocToFace(oldSelection.facePos);
                                    Cube3D.RubikPosition commonLayer = Cube3D.getCommonLayer(oldSelection.cubePos, currentSelection.cubePos, assocLayer);
                                    Boolean direction = true;
                                    if (commonLayer == Cube3D.RubikPosition.TopLayer || commonLayer == Cube3D.RubikPosition.MiddleLayer || commonLayer == Cube3D.RubikPosition.BottomLayer)
                                    {
                                      if (((oldSelection.facePos == Face3D.FacePosition.Back) && currentSelection.cubePos.HasFlag(Cube3D.RubikPosition.RightSlice))
                                      || ((oldSelection.facePos == Face3D.FacePosition.Left) && currentSelection.cubePos.HasFlag(Cube3D.RubikPosition.BackSlice))
                                      || ((oldSelection.facePos == Face3D.FacePosition.Front) && currentSelection.cubePos.HasFlag(Cube3D.RubikPosition.LeftSlice))
                                      || ((oldSelection.facePos == Face3D.FacePosition.Right) && currentSelection.cubePos.HasFlag(Cube3D.RubikPosition.FrontSlice))) direction = false;
                                    }
                                    if (commonLayer == Cube3D.RubikPosition.LeftSlice || commonLayer == Cube3D.RubikPosition.MiddleSlice_Sides || commonLayer == Cube3D.RubikPosition.RightSlice)
                                    {
                                      if (((oldSelection.facePos == Face3D.FacePosition.Bottom) && currentSelection.cubePos.HasFlag(Cube3D.RubikPosition.BackSlice))
                                      || ((oldSelection.facePos == Face3D.FacePosition.Back) && currentSelection.cubePos.HasFlag(Cube3D.RubikPosition.TopLayer))
                                      || ((oldSelection.facePos == Face3D.FacePosition.Top) && currentSelection.cubePos.HasFlag(Cube3D.RubikPosition.FrontSlice))
                                      || ((oldSelection.facePos == Face3D.FacePosition.Front) && currentSelection.cubePos.HasFlag(Cube3D.RubikPosition.BottomLayer))) direction = false;
                                    }
                                    if (commonLayer == Cube3D.RubikPosition.BackSlice || commonLayer == Cube3D.RubikPosition.MiddleSlice || commonLayer == Cube3D.RubikPosition.FrontSlice)
                                    {
                                      if (((oldSelection.facePos == Face3D.FacePosition.Top) && currentSelection.cubePos.HasFlag(Cube3D.RubikPosition.RightSlice))
                                      || ((oldSelection.facePos == Face3D.FacePosition.Right) && currentSelection.cubePos.HasFlag(Cube3D.RubikPosition.BottomLayer))
                                      || ((oldSelection.facePos == Face3D.FacePosition.Bottom) && currentSelection.cubePos.HasFlag(Cube3D.RubikPosition.LeftSlice))
                                      || ((oldSelection.facePos == Face3D.FacePosition.Left) && currentSelection.cubePos.HasFlag(Cube3D.RubikPosition.TopLayer))) direction = false;
                                    }
                                    if (commonLayer != Cube3D.RubikPosition.None)
                                    {
                                        if (groupBox1.Enabled)
                                        {
                                            groupBox1.Enabled = false;
                                            rotationTicks = 25;
                                            if (commonLayer == Cube3D.RubikPosition.TopLayer || commonLayer == Cube3D.RubikPosition.LeftSlice || commonLayer == Cube3D.RubikPosition.FrontSlice) direction = !direction;
                                            rubikManager.Rotate90(commonLayer, direction, rotationTicks);
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
                    rubikManager.RubikCube.cubes.ForEach(c => c.Faces.ToList().ForEach(f => f.Selection = Face3D.SelectionMode.None));
                    oldSelection = new RubikManager.PositionSpec() { cubePos = Cube3D.RubikPosition.None, facePos = Face3D.FacePosition.None };
                    currentSelection = new RubikManager.PositionSpec() { cubePos = Cube3D.RubikPosition.None, facePos = Face3D.FacePosition.None };
                }
            }
        }
        private void groupBox1_EnabledChanged(object sender, EventArgs e)
        {
            if (!groupBox1.Enabled)
            {
                rubikManager.RubikCube.cubes.ForEach(c => c.Faces.ToList().ForEach(f => f.Selection = Face3D.SelectionMode.None));
                oldSelection = new RubikManager.PositionSpec() { cubePos = Cube3D.RubikPosition.None, facePos = Face3D.FacePosition.None };
                currentSelection = new RubikManager.PositionSpec() { cubePos = Cube3D.RubikPosition.None, facePos = Face3D.FacePosition.None };
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
            if(listBox1.SelectedIndex != -1)
            comboBox1.Text = listBox1.SelectedItem.ToString().Split(new Char[] { Convert.ToChar(" ") })[0];
        }

        private void button6_Click(object sender, EventArgs e)
        {
          CubeSolver cs = new CubeSolver(rubikManager);
          cs.SolveFirstCross();
        }

    }

}
