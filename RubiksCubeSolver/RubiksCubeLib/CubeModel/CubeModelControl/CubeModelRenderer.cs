using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RubiksCubeLib.CubeModel
{

	/// <summary>
	/// Represents the central render operations to render the 3D model of the rubiks cube without possible delays
	/// </summary>
	[Serializable]
	public class CubeModelRenderer : IDisposable
	{

		// ** CONSTRUCTOR **

		public CubeModelRenderer(CubeModel model, Rectangle screen)
		{
			_cmodel = model;
			InitRenderer();
			SetDrawingArea(screen);
		}




		// ** PRIVATE FIELDS **

		private double _scale;
		private Rectangle _screen;

		private List<double> _frameTimes;
		private IEnumerable<Face3D>[] _buffer;
		private Thread _updateThread, _renderThread;
		private AutoResetEvent[] _updateHandle;
		private AutoResetEvent[] _renderHandle;
		private int _currentBufferIndex;
		private CubeModel _cmodel;



		// ** EVENTS **

		//Gets raised when a rotation animation finished
		public delegate void RotatingFinishedHandler(object sender, RotationFinishedEventArgs e);
		public event RotatingFinishedHandler OnRotatingFinished;
		private void BroadcastRotatingFinished()
		{
			if (OnRotatingFinished == null)
				return;
			OnRotatingFinished(this, new RotationFinishedEventArgs(_cmodel.Moves.Dequeue()));
		}

		//Gets raised when the renderloop gets executed
		public delegate void RenderHandler(object sender, IEnumerable<Face3D> e);
		public event RenderHandler OnRender;
		private void BroadcastRender(int bufferIndex)
		{
			if (OnRender == null)
				return;
			OnRender(this, _buffer[bufferIndex]);
		}




		// ** PROPERTIES **

		/// <summary>
		/// Gets or sets the Fps limit
		/// </summary>
		public double MaxFps { get; set; }

		/// <summary>
		/// Gets the current FPS
		/// </summary>
		public double Fps { get; private set; }

		/// <summary>
		/// Gets if the rendering cycle is active
		/// </summary>
		public bool IsRunning { get; private set; }




		private void InitRenderer()
		{
			_frameTimes = new List<double>();
			this.IsRunning = false;

			_updateHandle = new AutoResetEvent[2];
			for (int i = 0; i < _updateHandle.Length; i++)
				_updateHandle[i] = new AutoResetEvent(false);

			_renderHandle = new AutoResetEvent[2];
			for (int i = 0; i < _renderHandle.Length; i++)
				_renderHandle[i] = new AutoResetEvent(true);

			_buffer = new IEnumerable<Face3D>[2];
			for (int i = 0; i < _buffer.Length; i++)
				_buffer[i] = new List<Face3D>();
		}

		/// <summary>
		/// Reset the rendering screen
		/// </summary>
		/// <param name="screen">Screen measures</param>
		public void SetDrawingArea(Rectangle screen)
		{
			_screen = screen;
			int min = Math.Min(screen.Height, screen.Width);
			_scale = 3 * ((double)min / (double)400);
			if (screen.Width > screen.Height)
				screen.X = (screen.Width - screen.Height) / 2;
			else if (screen.Height > screen.Width)
				screen.Y = (screen.Height - screen.Width) / 2;
		}



		// ** EDIT THE RENDER CYCLE **

		/// <summary>
		/// Starts the render cycle
		/// </summary>
		public void StartRender()
		{
			if (!this.IsRunning)
			{
				this.IsRunning = true;
				_updateThread = new Thread(UpdateLoop);
				_updateThread.Start();
				_renderThread = new Thread(RenderLoop);
				_renderThread.Start();
			}
		}

		/// <summary>
		/// Stops the render cycle
		/// </summary>
		public void StopRender()
		{
			if (this.IsRunning)
			{
				this.IsRunning = false;
				_updateThread.Join();
				_renderThread.Join();
				this.Fps = 0;
				_frameTimes.Clear();
			}
		}

		/// <summary>
		/// Aborts the render cycle
		/// </summary>
		public void AbortRender()
		{
			if (this.IsRunning)
			{
				this.IsRunning = false;
				_updateThread.Abort();
				_renderThread.Abort();
			}
		}





		// ** RENDERING AND UPDATING **

		private Stopwatch _sw = new Stopwatch();

		private void RenderLoop()
		{
			int bufferIndex = 0x0;

			while (this.IsRunning)
			{
				_sw.Restart();
				Render(bufferIndex);
				bufferIndex ^= 0x1;

				double minTime = 1000.0 / this.MaxFps;
				while (_sw.Elapsed.TotalMilliseconds < minTime) { }

				_sw.Stop();


				_frameTimes.Add(_sw.Elapsed.TotalMilliseconds);
				int counter = 0;
				int index = _frameTimes.Count - 1;
				double ms = 0;
				while (index >= 0 && ms + _frameTimes[index] <= 1000)
				{
					ms += _frameTimes[index];
					counter++;
					index--;
				}
				if (index > 0)
					_frameTimes.RemoveRange(0, index);
				this.Fps = counter + ((1000 - ms) / _frameTimes[0]);
			}
		}

		private void UpdateLoop()
		{
			int bufferIndex = 0x0;
			_currentBufferIndex = 0x1;

			while (this.IsRunning)
			{
				Update(bufferIndex);
				_currentBufferIndex = bufferIndex;
				bufferIndex ^= 0x1;
			}
		}


		private void Render(int bufferIndex)
		{
			_updateHandle[bufferIndex].WaitOne();
			BroadcastRender(bufferIndex);
			_renderHandle[bufferIndex].Set();
		}

		private void Update(int bufferIndex)
		{
			_renderHandle[bufferIndex].WaitOne();

			if (_cmodel.Moves.Count > 0)
			{
				RotationInfo currentRotation = _cmodel.Moves.Peek();

				foreach (AnimatedLayerMove rotation in currentRotation.Moves)
				{
					double step = (double)rotation.Target / (double)((double)(currentRotation.Milliseconds / 1000.0) * (double)(this.Fps));
					_cmodel.LayerRotation[rotation.Move.Layer] += step;
				}

				if (RotationIsFinished(currentRotation.Moves))
					BroadcastRotatingFinished();
			}
			_buffer[bufferIndex] = _cmodel.GenFacesProjected(_screen, _scale);
			_updateHandle[bufferIndex].Set();
		}

		
		private bool RotationIsFinished(List<AnimatedLayerMove> moves)
		{
			foreach (AnimatedLayerMove m in moves)
			{
				if (!(m.Target > 0 && _cmodel.LayerRotation[m.Move.Layer] >= m.Target) && !(m.Target < 0 && _cmodel.LayerRotation[m.Move.Layer] <= m.Target))
					return false;
			}
			return true;
		}





		//Disposing

		/// <summary>
		/// Disposes the render object
		/// </summary>
		public void Dispose()
		{
			AbortRender();
		}

	}
}
