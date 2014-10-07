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
  [Serializable]
  public class CubeModelRenderer : IDisposable
  {
    private double scale;
    private Rectangle screen;

    private List<double> frameTimes;
    private IEnumerable<Face3D>[] buffer;
    private Thread updateThread, renderThread;
    private AutoResetEvent[] updateHandle;
    private AutoResetEvent[] renderHandle;
    private int currentBufferIndex;
    private CubeModel model;

    public delegate void RotatingFinishedHandler(object sender, RotationFinishedEventArgs e);
    public event RotatingFinishedHandler OnRotatingFinished;
    private void BroadcastRotatingFinished()
    {
      if (OnRotatingFinished == null) return;
      OnRotatingFinished(this, new RotationFinishedEventArgs(model.Moves.Dequeue()));
    }

    public delegate void RenderHandler(object sender, IEnumerable<Face3D> e);
    public event RenderHandler OnRender;
    private void BroadcastRender(int bufferIndex)
    {
      if (OnRender == null) return;
      OnRender(this, buffer[bufferIndex]);
    }

    public CubeModelRenderer(CubeModel model, Rectangle screen)
    {
      this.model = model;
      InitRenderer();
      SetDrawingArea(screen);
    }

    private double maxFps = 60;
    public double MaxFps { get { return maxFps; } set { maxFps = value; } }

    public double Fps { get; private set; }
    public bool IsRunning { get; private set; }

    private void InitRenderer()
    {
      frameTimes = new List<double>();
      IsRunning = false;

      updateHandle = new AutoResetEvent[2];
      for (int i = 0; i < updateHandle.Length; i++) updateHandle[i] = new AutoResetEvent(false);

      renderHandle = new AutoResetEvent[2];
      for (int i = 0; i < renderHandle.Length; i++) renderHandle[i] = new AutoResetEvent(true);

      buffer = new IEnumerable<Face3D>[2];
      for (int i = 0; i < buffer.Length; i++) buffer[i] = new List<Face3D>();
    }

    public void SetDrawingArea(Rectangle screen)
    {
      this.screen = screen;
      int min = Math.Min(screen.Height, screen.Width);
      scale = 3 * ((double)min / (double)400);
      if (screen.Width > screen.Height) screen.X = (screen.Width - screen.Height) / 2;
      else if (screen.Height > screen.Width) screen.Y = (screen.Height - screen.Width) / 2;
    }

    public void StartRender()
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

    public void StopRender()
    {
      if (IsRunning)
      {
        IsRunning = false;
        updateThread.Join();
        renderThread.Join();
        Fps = 0;
        frameTimes.Clear();
      }
    }

    public void AbortRender()
    {
      if (IsRunning)
      {
        IsRunning = false;
        this.IsRunning = false;
        updateThread.Abort();
        renderThread.Abort();
      }
    }

    private void UpdateLoop()
    {
      int bufferIndex = 0x0;
      currentBufferIndex = 0x1;

      while (IsRunning)
      {
        Update(bufferIndex);
        currentBufferIndex = bufferIndex;
        bufferIndex ^= 0x1;
      }
    }

    private void Update(int bufferIndex)
    {
      renderHandle[bufferIndex].WaitOne();

      if (model.Moves.Count > 0)
      {
        RotationInfo currentRotation = model.Moves.Peek();

        foreach (AnimatedLayerMove rotation in currentRotation.Moves)
        {
          double step = (double)rotation.Target / (double)((double)(currentRotation.Milliseconds / 1000.0) * (double)(Fps));
          model.LayerRotation[rotation.Move.Layer] += step;
        }

        if (RotationIsFinished(currentRotation.Moves)) BroadcastRotatingFinished();
      }
      buffer[bufferIndex] = model.GenFacesProjected(screen, scale);
      updateHandle[bufferIndex].Set();
    }

    private bool RotationIsFinished(List<AnimatedLayerMove> moves)
    {
      foreach (AnimatedLayerMove m in moves)
      {
        if (!(m.Target > 0 && model.LayerRotation[m.Move.Layer] >= m.Target) && !(m.Target < 0 && model.LayerRotation[m.Move.Layer] <= m.Target)) return false;
      }
      return true;
    }

    private Stopwatch sw = new Stopwatch();
    public void RenderLoop()
    {
      int bufferIndex = 0x0;

      while (IsRunning)
      {
        sw.Restart();
        Render(bufferIndex);
        bufferIndex ^= 0x1;

        double minTime = 1000.0 / maxFps;
        if (sw.ElapsedMilliseconds < minTime) Thread.Sleep(System.Math.Max((int)(minTime - sw.ElapsedMilliseconds), 0));
        while (sw.Elapsed.TotalMilliseconds < minTime) { }

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
      BroadcastRender(bufferIndex);
      renderHandle[bufferIndex].Set();
    }

    public void Dispose()
    {
      AbortRender();
    }
  }
}
