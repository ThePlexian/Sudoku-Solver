namespace SudokuSolver.Notifier
{
	using System;

	public class Notification : IDisposable
	{

		//Constructors
		public Notification(string t, string m, MessageType ty, int r, int d = int.MaxValue)
		{
			//Set properties
			Tag = t;
			Message = m;
			Type = ty;
			RankValue = r;
			Duration = d;

			//Add timer
			this.InitTimer();
			this.IsActive = false;
		}


		//Properties
		public string Tag { get; private set; }
		public string Message { get; private set; }

		public enum MessageType { Error, Information, Warning, None }
		public MessageType Type { get; private set; }

		private int Duration { get; set; }

		public int RankValue { get; private set; }

		private bool _isactive;
		public bool IsActive
		{
			get
			{
				return _isactive;
			}
			set
			{
				if (_isactive == value)
					return;

				_isactive = value;
				this.InitTimer();

				if (!value)
					return;

				_timer.Start();
				if (this.TimerStarted != null)
					this.TimerStarted(this, EventArgs.Empty);
			}
		}



		//The timer
		private System.Timers.Timer _timer;

		private void InitTimer()
		{
			if (_timer != null)
				_timer.Stop();
			_timer = new System.Timers.Timer(this.Duration);
			_timer.Elapsed += (s, e) =>
			{
				//Deactivate this
				this.IsActive = false;
				_timer.Stop();

				if (TimerExpired != null)
					TimerExpired(this, EventArgs.Empty);
			};
		}

		public event EventHandler TimerStarted;
		public event EventHandler TimerExpired;


		//IDisposable
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!disposing)
				return;

			if (_timer == null)
				return;

			_timer.Dispose();
			_timer = null;
		}
	}
}
