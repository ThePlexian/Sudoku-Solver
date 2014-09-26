using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
