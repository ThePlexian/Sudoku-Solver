using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Notifier
{
	public class Notification
	{
		public Notification(string t, string m, MessageType ty, int d)
		{
			//Set properties
			Tag = t;
			Message = m;
			Type = ty;
			Duration = d;

			//Add timer
			this.InitTimer();
			this.IsActive = false;
		}


		public string Tag { get; private set; }
		public string Message { get; set; }

		public enum MessageType { Error, Information, Warning, None }
		public MessageType Type { get; private set; }

		public int Duration { get; private set; }

		private bool _isactive;
		public bool IsActive
		{
			get
			{
				return _isactive;
			}
			set
			{
				if (_isactive != value)
				{
					_isactive = value;
					this.InitTimer();

					if (value)
					{
						_timer.Start();
						if (TimerStarted != null)
							TimerStarted(this, new NotifyEventArgs(this));
					}

				}
			}
		}



		//The timer
		private System.Timers.Timer _timer;

		public void InitTimer()
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
					TimerExpired(this, new NotifyEventArgs(this));
			};
		}

		public delegate void TimerStatusChangedEventHandler(object sender, NotifyEventArgs ne);
		public event TimerStatusChangedEventHandler TimerStarted;
		public event TimerStatusChangedEventHandler TimerExpired;
	}
}
