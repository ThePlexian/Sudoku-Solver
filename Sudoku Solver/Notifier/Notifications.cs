using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver.Notifier
{
	public static class Notifications
	{

		static Notifications() 
		{
			//Add notifications to list
			_notifications = new List<Notification>();

			_notifications.Add(new Notification("Solved", 
				"Sudoku solved in [0]ms.", 
				Notification.MessageType.Information, 5000));

			_notifications.Add(new Notification("Unsolved", 
				"Sudoku could not be solved - [0] numbers remaining.",
				Notifier.Notification.MessageType.Error, 5000));

			_notifications.Add(new Notification("Unambiguous", 
				"You have to enter more then 17 numbers, otherwise the sudoku is not ambiguous solvable.", 
				Notification.MessageType.Warning, int.MaxValue));

			_notifications.Add(new Notification("Invalid", 
				"The entered sudoku is invalid.", 
				Notifier.Notification.MessageType.Error, int.MaxValue));

			_notifications.Add(new Notification("Invalid File",
				"The file you selected doesn't contain a valid sudoku.\nPlease select another one.",
				Notifier.Notification.MessageType.Error, int.MaxValue));


			_notifications.Add(new Notification("Saved",
				"File saved successfully.",
				Notifier.Notification.MessageType.Information, 8000));


			//Raise the Notify Event if the timer of any notification started / expired
			foreach (Notification n in _notifications)
			{
				n.TimerStarted += (s, ne) => {
					if (Notify != null) 
						Notify(s, ne);
				};

				n.TimerExpired += (s, ne) =>
				{
					if (Notify != null)
						Notify(s, ne);
				};
			}
		}

		
		//Member
		public static List<Notification> _notifications;

		//Notifications
		public static Notification GetNotification(string key) 
		{
			switch(key) 
			{
				case "Solved":
					return Solved;
				case "Unsolved":
					return Unsolved;
				case "Unambiguous":
					return Unambiguous;
				case "Invalid":
					return Invalid;
				case "Invalid File":
					return InvalidFile;
				case "Saved":
					return Saved;
				default:
					return null;
			}
		}


		private static Notification Solved
		{
			get
			{
				return _notifications.FirstOrDefault(n => n.Tag == "Solved");
			}
		}

		private static Notification Unsolved
		{
			get
			{
				return _notifications.FirstOrDefault(n => n.Tag == "Unsolved");
			}
		}

		private static Notification Unambiguous
		{
			get
			{
				return _notifications.FirstOrDefault(n => n.Tag == "Unambiguous");
			}
		}

		private static Notification Invalid
		{
			get
			{
				return _notifications.FirstOrDefault(n => n.Tag == "Invalid");
			}
		}

		private static Notification InvalidFile
		{
			get
			{
				return _notifications.FirstOrDefault(n => n.Tag == "Invalid File");
			}
		}

		private static Notification Saved
		{
			get
			{
				return _notifications.FirstOrDefault(n => n.Tag == "Saved");
			}
		}



		//Edit list
		public static void ChangeState(string key, bool state)
		{
			Notification n = GetNotification(key);

			if (n.IsActive != state)
			{
				n.IsActive = state;

				if (Notify != null)
					Notify(null, new Notifier.NotifyEventArgs(n));
			}
		}

		public static void Reset()
		{
			_notifications.ForEach(n => n.IsActive = false);
		}

		


		//Event
		public delegate void NotifyEventHandler(object sender, Notifier.NotifyEventArgs ne);
		public static event NotifyEventHandler Notify;
	}
}
