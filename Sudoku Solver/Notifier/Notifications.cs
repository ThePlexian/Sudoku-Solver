using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuSolver.Notifier
{

	public static class Notifications
	{

		//Constructor
		static Notifications()
		{
			//Add notifications to list
			Components = new ObservableCollection<Notification>
			                 {
				                 new Notification("Solved",
				                                  "Sudoku solved in [0]ms.",
				                                  Notification.MessageType.Information, 8000),
				                 new Notification("Unsolved",
				                                  "Sudoku could not be solved - [0] numbers remaining.",
				                                  Notification.MessageType.Error, 8000),
				                 new Notification("Unambiguous",
				                                  "You have to enter more than 17 numbers, otherwise the sudoku is not ambiguous solvable.",
				                                  Notification.MessageType.Warning),
				                 new Notification("Invalid",
				                                  "The entered sudoku is invalid.",
				                                  Notification.MessageType.Error),
				                 new Notification("Invalid File",
				                                  "The file you selected doesn't contain a valid sudoku.\nPlease select another one.",
				                                  Notification.MessageType.Error, 8000),
				                 new Notification("Saved",
				                                  "File saved successfully.",
				                                  Notification.MessageType.Information, 8000),
								 new Notification("Loaded",
												  "Sudoku \"[0]\" successfully loaded.",
												  Notification.MessageType.Information, 8000)
			                 };


			//Raise the Notify Event if the timer of any notification started / expired
			foreach (var n in Components)
			{
				n.TimerStarted += (s, ne) =>
								  {
									  if (Notify != null)
										  Notify(s, ne);
								  };

				n.TimerExpired += (s, ne) =>
								  {
									  if (Notify != null)
										  Notify(s, ne);
								  };

			}

			//When the collection changes, notify
			Components.CollectionChanged += (s, e) =>
											{
												if (Notify != null)
													Notify(s, null);
											};
		}


		//Member
		private static readonly ObservableCollection<Notification> Components;


		//Returns the topmost notification
		public static Notification TopMost { get { return Components.FirstOrDefault(n => n.IsActive); } }






		//Edit list
		public static void ChangeState(string key, bool state)
		{
			var n = GetNotification(key);

			if (n == null)
				return;

			//Double change to reactivate the timer
			if (n.IsActive == state)
				n.IsActive = !state;



			//Change
			n.IsActive = state;

			//Set as topmost
			Components.Move(Components.IndexOf(n), 0);

			if (Notify != null)
				Notify(null, new NotifyEventArgs(n));
		}

		public static void Reset()
		{
			Components.ToList().ForEach(n => n.IsActive = false);
		}

		private static Notification GetNotification(string key)
		{
			return Components.FirstOrDefault(n => n.Tag == key);
		}





		//Event
		public delegate void NotifyEventHandler(object sender, NotifyEventArgs ne);
		public static event NotifyEventHandler Notify;

	}
}
