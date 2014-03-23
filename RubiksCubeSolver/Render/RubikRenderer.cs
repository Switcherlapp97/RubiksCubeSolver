using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;

namespace VirtualRubik
{
	class RubikRenderer
	{
		List<double> frameTimes;
		List<RenderInfo> buffers;
		Thread updateThread, renderThread;
		Rectangle screen;
		double scale;
		double rotationTime;
		public RubikManager RubikManager;

		public delegate void RenderHandler(object sender, RenderEventArgs e);
		public event RenderHandler OnRender;

		public double Fps { get; private set; }
		public bool IsRunning { get; private set; }

		private void BroadcastRender()
		{
			if (OnRender == null) return;
			if (buffers.Count != 0) OnRender(this, new RenderEventArgs(buffers[buffers.Count - 1]));
		}

		public RubikRenderer(Rectangle screen, double scale)
		{
			RubikManager = new RubikManager();
			this.scale = scale;
			this.screen = screen;
			frameTimes = new List<double>();
			buffers = new List<RenderInfo>();
			IsRunning = false;
			rotationTime = 2000; //in ms
		}

		public void Start()
		{
			if (!IsRunning)
			{
				IsRunning = true;
				updateThread = new Thread(UpdateLoop);
				updateThread.Start();
				renderThread = new Thread(RenderLoop);
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
			Update();
			renderThread.Start();
			while (IsRunning)
			{
				Update();
			}
		}

		private void Update()
		{
			RotationInfo rotationInfo = RubikManager.GetRotationInfo();
			if (rotationInfo.Rotating)
			{
				double rotationStep = (double)90 / (double)((double)(rotationTime / 1000) * (double)(Fps / 1000));
				if (rotationInfo.Direction) rotationStep *= -1;

				RubikManager.RubikCube.LayerRotation[RubikManager.rotationLayer] += rotationStep;
				if ((rotationInfo.Target > 0 && RubikManager.RubikCube.LayerRotation[rotationInfo.Layer] >= rotationInfo.Target) || (rotationInfo.Target < 0 && RubikManager.RubikCube.LayerRotation[rotationInfo.Layer] <= rotationInfo.Target))
				{
					RubikManager.resetFlags(true);
				}
			}

			RenderInfo newRenderInfo = RubikManager.RubikCube.NewRender(screen, scale);
			buffers.Add(newRenderInfo);
		}

		public void RenderLoop()
		{
			Stopwatch sw = new Stopwatch();
			while (IsRunning)
			{
				sw.Restart();
				Render();
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

		private void Render()
		{
			BroadcastRender();
		}
	}
}
