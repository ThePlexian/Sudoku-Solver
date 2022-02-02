namespace SudokuSolver.Model.Notifier
{
    public static class Notifications
    {

        //Constructor
        static Notifications()
        {
            //Add notifications to list
            _components = new List<Notification>
                          {
                              new Notification("Solved", "Sudoku solved in [0]ms.",
                                               Notification.MessageType.Information, 2, 8000),
                              new Notification("Unsolved", "Sudoku could not be solved - [0] numbers remaining.",
                                               Notification.MessageType.Error, 2, 8000),
                              new Notification("Invalid", "The entered sudoku is invalid.",
                                               Notification.MessageType.Error, 1),
                              new Notification("Unambiguous",
                                               "You have to enter more than 17 numbers, otherwise the sudoku is not ambiguous solvable.",
                                               Notification.MessageType.Warning, 3),
                              new Notification("Invalid File Content",
                                               "The file you selected doesn't contain a valid sudoku.\nPlease select another one.",
                                               Notification.MessageType.Error, 1, 8000),
                              new Notification("Saving Path Exists",
                                               "The path you selected for saving already exists.\nPlease select another one.",
                                               Notification.MessageType.Error, 1, 8000),
                              new Notification("Unauthorized File Access",
                                               "You don't have the necessary access permissions to save the file at the selected path.",
                                               Notification.MessageType.Error, 1, 8000),
                              new Notification("Saved", "File saved successfully.",
                                               Notification.MessageType.Information, 2, 8000),
                              new Notification("Loaded", "Sudoku \"[0]\" successfully loaded.",
                                               Notification.MessageType.Information, 2, 8000)
                          };


            //Raise the Notify Event if the timer of any notification started / expired
            static void notifyAction(object s, EventArgs e) => Notify?.Invoke(s, e);
            foreach (var n in _components) {
                n.TimerStarted += (s, e) => notifyAction(s, e);
                n.TimerExpired += (s, e) => notifyAction(s, e);
            }

        }


        //Member
        private static List<Notification> _components;


        //Returns the topmost notification
        public static Notification TopMost => _components.FirstOrDefault(n => n.IsActive);






        //Edit list
        public static void ChangeState(string key, bool state)
        {
            var n = GetNotification(key);

            if (n == null) {
                return;
            }

            //Double change to reactivate the timer
            if (n.IsActive == state) {
                n.IsActive = !state;
            }



            //Change
            n.IsActive = state;

            //Set as topmost
            _components = _components.OrderBy(noti => noti.IsActive)
                                     .ThenBy(noti => noti.RankValue)
                                     .ToList();


            Notify?.Invoke(null, EventArgs.Empty);
        }

        public static void Reset()
        {
            _components.ToList().ForEach(n => n.IsActive = false);

            Notify?.Invoke(null, null);
        }

        private static Notification GetNotification(string key) => _components.FirstOrDefault(n => n.Tag == key);





        //Event
        public static event EventHandler Notify;

    }
}
