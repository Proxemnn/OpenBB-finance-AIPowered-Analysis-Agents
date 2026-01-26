using Services.Models;
using System.Data.Common;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Services
{
	public class ClickService : IDisposable
	{
		private Thread ClickingThread { get; set; }

		private bool IsAlive { get; set; }
		private int Interval { get; set; }

		private const int MOUSEEVENTF_LEFTDOWN = 0x02;
		private const int MOUSEEVENTF_LEFTUP = 0x04;
		//private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
		//private const int MOUSEEVENTF_RIGHTUP = 0x10;

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern bool GetCursorPos(out POINT lpPoint);

		public ClickService() : base()
		{
			IsAlive = false;
			this.Interval = 20;
			ClickingThread = new Thread(this.Run);
			ClickingThread.Start();
		}

		public ClickService(int interval) : base()
		{
			IsAlive = false;
			this.Interval = interval;
			ClickingThread = new Thread(this.Run);
			ClickingThread.Start();
		}

		private void StartClicking()
		{
			IsAlive = true;
		}

		private void Stop()
		{
			
			IsAlive = false;
		}

		public void Switch()
		{
			if (IsAlive)
			{
				Stop();
				return;
			}

			StartClicking();
		}

		private void Run()
		{
			while (true)
			{
				if (IsAlive)
				{

					Point currentCursorPosition = GetCursorPosition();

					uint X = (uint)currentCursorPosition.X;
					uint Y = (uint)currentCursorPosition.Y;

					mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
					Thread.Sleep(Interval);
				} else
				{
					Thread.Sleep(500);
				}
			}
		}

		private static Point GetCursorPosition()
		{
			GetCursorPos(out POINT lpPoint);
			return lpPoint;
		}

		public void UpdateInterval(int value)
		{
			IsAlive = false;
			Interval = value;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && ClickingThread != null)
			{
				ClickingThread.Join();
			}
		}
	}
}
