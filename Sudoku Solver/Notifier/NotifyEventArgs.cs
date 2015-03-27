using System;

namespace SudokuSolver.Notifier
{
	public class NotifyEventArgs : EventArgs
	{
		public NotifyEventArgs(Notification n) 
		{
			this.Notification = n;
		}

		public Notification Notification { get; set; }
	}
}
