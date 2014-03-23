using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

namespace VirtualRubik
{
	class RubikRenderer
	{
		private List<double> frameTimes;
		private RenderInfo[] buffer;
		private Thread updateThread, renderThread;
		private Rectangle screen;
		private double scale;
		public RubikManager RubikManager;
		private AutoResetEvent[] updateHandle;
		private AutoResetEvent[] renderHandle;
		private AutoResetEvent resourceUpdateHandle;
		private int currentBufferIndex;

		public delegate void RenderHandler(object sender, RenderEventArgs e);
		public event RenderHandler OnRender;

		public double Fps { get; private set; }
		public double MaxFps { get; private set; }
		public bool IsRunning { get; private set; }

		private void BroadcastRender(int currentBufferIndex)
		{
			if (OnRender == null) return;
			OnRender(this, new RenderEventArgs(buffer[currentBufferIndex]));
		}

		public RubikRenderer(Rectangle screen, double scale)
		{
			RubikManager = new RubikManager();
			this.scale = scale;
			this.screen = screen;
			frameTimes = new List<double>();
			IsRunning = false;

			updateHandle = new AutoResetEvent[2];
			for (int i = 0; i < updateHandle.Length; i++)
				updateHandle[i] = new AutoResetEvent(false);

			renderHandle = new AutoResetEvent[2];
			for (int i = 0; i < renderHandle.Length; i++)
				renderHandle[i] = new AutoResetEvent(true);

			resourceUpdateHandle = new AutoResetEvent(true);

			buffer = new RenderInfo[2];
			for (int i = 0; i < buffer.Length; i++)
				buffer[i] = new RenderInfo();
		}

		public void SetDrawingArea(Rectangle area, double factor)
		{
			screen = area;
			scale = factor;
		}

		public void Start()
		{
			if (!IsRunning)
			{
				IsRunning = true;
				updateThread = new Thread(UpdateLoop);
				updateThread.Start();
				renderThread = new Thread(RenderLoop);
				renderThread.Start();
			}
		}

		public void Stop()
		{
			if (IsRunning)
			{
				IsRunning = false;
				this.IsRunning = false;
				updateThread.Join();
				renderThread.Join();
				Fps = 0;
				frameTimes.Clear();
			}
		}

		public void Abort()
		{
			if (IsRunning)
			{
				IsRunning = false;
				this.IsRunning = false;
				updateThread.Abort();
				renderThread.Abort();
			}
		}
		public void UpdateLoop()
		{
			Stopwatch sw = new Stopwatch();
			TimeSpan elapsed = default(TimeSpan);
			int bufferIndex = 0x0;
			currentBufferIndex = 0x1;

			while (IsRunning)
			{
				sw.Start();
				Update(bufferIndex, elapsed);
				currentBufferIndex = bufferIndex;
				bufferIndex ^= 0x1;
				elapsed = sw.Elapsed;
				sw.Reset();
			}
		}

		private void Update(int bufferIndex, TimeSpan elapsed)
		{
			renderHandle[bufferIndex].WaitOne();
			resourceUpdateHandle.WaitOne();

			//Update
			RotationInfo rotationInfo = RubikManager.GetRotationInfo();
			if (rotationInfo.Rotating)
			{
				double rotationStep = (double)rotationInfo.Target / (double)((double)(rotationInfo.Milliseconds / 1000.0) * (double)(Fps));

				RubikManager.RubikCube.LayerRotation[RubikManager.rotationLayer] += rotationStep;
				if ((rotationInfo.Target > 0 && RubikManager.RubikCube.LayerRotation[rotationInfo.Layer] >= rotationInfo.Target) || (rotationInfo.Target < 0 && RubikManager.RubikCube.LayerRotation[rotationInfo.Layer] <= rotationInfo.Target))
				{
					RubikManager.resetFlags(true);
				}
			}
			RenderInfo newRenderInfo = RubikManager.RubikCube.NewRender(screen, scale);
			buffer[bufferIndex] = newRenderInfo;

			updateHandle[bufferIndex].Set();
		}

		public void RenderLoop()
		{
			Stopwatch sw = new Stopwatch();
			int bufferIndex = 0x0;

			while (IsRunning)
			{
				sw.Restart();
				Render(bufferIndex);
				bufferIndex ^= 0x1;
				sw.Stop();

				frameTimes.Add(sw.Elapsed.TotalMilliseconds);
				int counter = 0;
				int index = frameTimes.Count - 1;
				double ms = 0;
				while (index >= 0 && ms + frameTimes[index] <= 1000)
				{
					ms += frameTimes[index];
					counter++;
					index--;
				}
				if (index > 0) frameTimes.RemoveRange(0, index);
				Fps = counter + ((1000 - ms) / frameTimes[0]);
			}
		}

		private void Render(int bufferIndex)
		{
			updateHandle[bufferIndex].WaitOne();
			resourceUpdateHandle.Set();
			BroadcastRender(bufferIndex);
			renderHandle[bufferIndex].Set();
		}
	}
}
