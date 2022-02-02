using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using SudokuSolver.Model.Data;
using SudokuSolver.Model.Notifier;

namespace SudokuSolver.View
{


    public partial class FrmMain : Form
    {

        //Constructor
        public FrmMain()
        {
            InitializeComponent();
        }



        //The Sudoku + animative stuff
        private Sudoku _sudoku;

        private Thread _solvingthread;
        private Sudoku.SolvingTechnique _solvingmethod;



        #region **** Form Handling ****

        //Startup + End
        private void FrmMain_Load(object sender, EventArgs e)
        {
            //Sudoku
            _sudoku = new Sudoku();
            _sudoku.CellChanged += CellChanged;
            _sudoku.SolvingCompleted += SudokuSolvingCompleted;
            _solvingmethod = Sudoku.SolvingTechnique.HumanSolvingTechnique;

            //Sudoku control
            sudokuField1.Sudoku = _sudoku;

            //The button
            btnSolve.Location = new Point((ClientSize.Width - btnSolve.Width) / 2, btnSolve.Location.Y);

            //Notifier
            Notifications.Notify += Notified;
            Notifications.Reset();
            _notirectangle = new Rectangle(10, customMenuStrip1.Location.Y + customMenuStrip1.Height + 10, ClientSize.Width - 20, 35);
            _notiparam = string.Empty;
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            Notifications.ChangeState("Unambiguous", true);
            sudokuField1.Focus();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_solvingthread != null) {
                if (_solvingthread.IsAlive) {
                    _solvingthread.Abort();
                }
            }

            _solvingthread = null;
        }




        //Painting
        [DebuggerStepThrough]
        private void FrmMain_Paint(object sender, PaintEventArgs e)
        {
            //Set the qualities
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            e.Graphics.SmoothingMode = SmoothingMode.Default;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //The notification to be displayed

            var n = Notifications.TopMost;

            if (n != null) {
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                Brush b = new SolidBrush(Color.FromKnownColor(KnownColor.Control));
                switch (n.Type) {
                    case Notification.MessageType.Information:
                        b = new SolidBrush(Color.FromArgb(100, 255, 100));
                        break;
                    case Notification.MessageType.Warning:
                        b = new SolidBrush(Color.FromArgb(255, 255, 50));
                        break;
                    case Notification.MessageType.Error:
                        b = new SolidBrush(Color.FromArgb(255, 100, 100));
                        break;
                    case Notification.MessageType.None:
                        return;     //Exit
                }

                e.Graphics.FillRectangle(b, _notirectangle);
                e.Graphics.DrawString(n.Message.Replace("[0]", _notiparam.ToString().Replace(',', '.')), new Font(Font.FontFamily, 9F, FontStyle.Bold), Brushes.Black, _notirectangle, sf);
            }

            e.Graphics.DrawRectangle(Pens.Black, _notirectangle.X + 1, _notirectangle.Y + 1, _notirectangle.Width - 1, _notirectangle.Height - 1);
        }

        #endregion



        #region **** Notification Handling ****

        // **** Notifications ****
        private Rectangle _notirectangle;
        private object _notiparam;


        //Notification started / expired
        private void Notified(object sender, EventArgs e) => this.ConditionalInvoke(() => Invalidate(_notirectangle));

        #endregion



        #region **** Solving ****

        // **** Solve ****
        private Stopwatch _swsolving;

        //Start Solving
        private void BtnSolve_Click(object sender, EventArgs e)
        {
            //Deactivate UI Control
            customMenuStrip1.Enabled = false;
            btnSolve.Enabled = false;
            sudokuField1.EditingEnabled = false;

            //Start solving
            _swsolving = Stopwatch.StartNew();
            _sudoku.SolvingCompleted += SudokuSolvingCompleted;
            _solvingthread = new Thread(() => _sudoku.Solve(_solvingmethod));
            _solvingthread.Start();
        }



        //Cell changed
        private void CellChanged(object sender, CellChangedEventArgs e)
        {
            //Update notifier
            if (_swsolving == null || !_swsolving.IsRunning || _solvingmethod == Sudoku.SolvingTechnique.HumanSolvingTechnique) {
                Notifications.ChangeState("Unambiguous", _sudoku.MissingNumbers > 81 - 17);
                Notifications.ChangeState("Invalid", !_sudoku.IsValid);
            }


            //Update sudoku field
            if (_solvingmethod != Sudoku.SolvingTechnique.HumanSolvingTechnique) {
                return;
            }

            if (e == null) {
                return;
            }

            var cp = e.ChangedProperty;
            if (!cp.HasFlag(CellChangedEventArgs.CellProperty.Number) &&        /*no Number change*/
                !cp.HasFlag(CellChangedEventArgs.CellProperty.IsPreset) &&      /*no Preset change*/
                (!sudokuField1.ShowCandidates || !cp.HasFlag(CellChangedEventArgs.CellProperty.Candidates)))    /*no candidate change or candidates not visible*/
{
                return;
            }

            if (e.Cell.IsEqual(_sudoku.GetCell(e.Cell.Index))) {
                sudokuField1.Invalidate(sudokuField1.GetRectangle(e.Cell));
            }
        }

        //Sudoku solving procedure finished
        private void SudokuSolvingCompleted(object sender, EventArgs e)
        {
            //Show result
            if (_sudoku.IsSolved) {
                Notifications.ChangeState("Solved", true);
                Notifications.ChangeState("Unsolved", false);
                _notiparam = Math.Round(_swsolving.Elapsed.TotalMilliseconds, 2);
            }
            else {
                Notifications.ChangeState("Solved", false);
                Notifications.ChangeState("Unsolved", true);
                _notiparam = _sudoku.MissingNumbers;
            }

            //Reactivate UI Control
            this.ConditionalInvoke(() => {
                customMenuStrip1.Enabled = true;
                btnSolve.Enabled = true;
                sudokuField1.EditingEnabled = true;
                sudokuField1.Invalidate();
            });
        }

        #endregion



        #region **** Menustrip ****

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sudoku = new Sudoku();
            _sudoku.CellChanged += CellChanged;
            _sudoku.SolvingCompleted += SudokuSolvingCompleted;
            sudokuField1.Sudoku = _sudoku;

            Notifications.Reset();
            Notifications.ChangeState("Unambiguous", true);
            Invalidate(_notirectangle);
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            //Initialize OFD
            ofd.Title = "Open from file";
            ofd.CheckPathExists = true;
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ofd.Filter = "Text files (*.txt)|*.txt|XML files (*.xml)|*.xml|All files (*.*)|*.*";

            //Show
            if (ofd.ShowDialog(this) != DialogResult.OK) {
                return;
            }

            //Load
            var sudokus = new List<Sudoku>();
            var result = default(Sudoku.LoadingProcessResult);

            switch (new FileInfo(ofd.FileName).Extension.ToLower()) {
                case ".txt":
                    result = Sudoku.LoadTxt(ofd.FileName, out sudokus);
                    break;
                case ".xml":
                    result = Sudoku.LoadXml(ofd.FileName, out sudokus);
                    break;
            }

            //Loading not sucessful
            if (result != Sudoku.LoadingProcessResult.Success) {
                //Add notification
                if (result == Sudoku.LoadingProcessResult.InvalidFileContent) {
                    Notifications.ChangeState("Invalid File Content", true);
                }

                //Reset sudoku
                _sudoku = new Sudoku();
                _sudoku.CellChanged += CellChanged;
                _sudoku.SolvingCompleted += SudokuSolvingCompleted;
                sudokuField1.Sudoku = _sudoku;

                return;
            }

            //Loading successful
            if (sudokus.Count > 1) {
                //Select sudoku, cause the file contains more than 1 sudoku
                using var fss = new FrmSudokuSelection { Sudokus = sudokus.Where(s => s != null).ToList(), StartPosition = FormStartPosition.CenterParent };
                if (fss.ShowDialog(this) != DialogResult.OK) {
                    return;
                }

                _sudoku.OverrideValues(fss.SelectedSudoku);
            }
            else //Just load the one
{
                _sudoku.OverrideValues(sudokus[0]);
            }

            //Apply
            sudokuField1.Sudoku = _sudoku;
            sudokuField1.Invalidate();
            CellChanged(this, CellChangedEventArgs.Empty);
            _notiparam = _sudoku.Name;
            Notifications.ChangeState("Loaded", true);
        }


        private void SaveAsTextfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog();
            sfd.Title = "Save As Textfile";
            sfd.AddExtension = true;
            sfd.DefaultExt = "txt";
            sfd.CheckPathExists = true;
            sfd.OverwritePrompt = false;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sfd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (sfd.ShowDialog() != DialogResult.OK) {
                return;
            }

            //Check for overwriting, or appending
            var fa = default(Sudoku.FileAccess);

            if (File.Exists(sfd.FileName)) {
                using var fop = new FrmOverwritePrompt();
                fop.ShowDialog();
                fa = fop.FileAccess;

            }


            var res = _sudoku.SaveTxt(sfd.FileName, fa);
            switch (res) {
                case Sudoku.SavingProcessResult.Success:
                    Notifications.ChangeState("Saved", true);
                    break;
                case Sudoku.SavingProcessResult.FileAlreadyExists:
                    Notifications.ChangeState("Saving Path Exists", true);
                    break;
                case Sudoku.SavingProcessResult.UnauthorizedAccess:
                    Notifications.ChangeState("Unauthorized File Access", true);
                    break;
            }
        }

        private void SaveAsBitmapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var sfd = new SaveFileDialog();
            sfd.Title = "Save As Bitmap";
            sfd.AddExtension = true;
            sfd.DefaultExt = "bmp";
            sfd.CheckPathExists = true;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sfd.Filter = "Bitmaps (*.bmp)|*.bmp|All files (*.*)|*.*";

            if (sfd.ShowDialog() != DialogResult.OK) {
                return;
            }

            var sudokupic = new Bitmap(sudokuField1.Width, sudokuField1.Height);
            sudokuField1.DrawToBitmap(sudokupic, new Rectangle(new Point(0, 0), sudokuField1.Size));

            const int borderwidth = 5;
            var result = new Bitmap(sudokupic.Width + (2 * borderwidth), sudokupic.Height + (2 * borderwidth));
            using (var g = Graphics.FromImage(result)) {
                g.Clear(Color.FromArgb(245, 245, 245));
                g.DrawImage(sudokupic, new Point(borderwidth, borderwidth));
            }

            result.Save(sfd.FileName);
            Notifications.ChangeState("Saved", true);
        }

        private void SaveAsXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using var sfd = new SaveFileDialog();
            sfd.Title = "Save As XML";
            sfd.AddExtension = true;
            sfd.DefaultExt = "xml";
            sfd.CheckPathExists = true;
            sfd.OverwritePrompt = false;
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sfd.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";

            if (sfd.ShowDialog() != DialogResult.OK) {
                return;
            }


            //Check for overwriting or appending
            var fa = default(Sudoku.FileAccess);

            if (File.Exists(sfd.FileName)) {
                using var fop = new FrmOverwritePrompt();
                fop.ShowDialog();
                fa = fop.FileAccess;

            }


            //Change name
            using (var fsn = new FrmSudokuName(_sudoku)) {
                fsn.ShowDialog();
            }

            var res = _sudoku.SaveXml(sfd.FileName, fa);
            switch (res) {
                case Sudoku.SavingProcessResult.Success:
                    Notifications.ChangeState("Saved", true);
                    break;
                case Sudoku.SavingProcessResult.FileAlreadyExists:
                    Notifications.ChangeState("Saving Path Exists", true);
                    break;
                case Sudoku.SavingProcessResult.UnauthorizedAccess:
                    Notifications.ChangeState("Unauthorized File Access", true);
                    break;
            }
        }


        private void DeleteNonPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var temp = new Sudoku();
            temp.CellChanged += CellChanged;
            temp.SolvingCompleted += SudokuSolvingCompleted;

            for (int i = 0; i < 81; i++) {
                var c = _sudoku.GetCell(i);
                if (!c.IsPreset) {
                    continue;
                }

                temp.SetValue(i, c.Number);
                temp.SetPresetValue(i, true);
            }

            _sudoku = temp;
            sudokuField1.Sudoku = _sudoku;
            sudokuField1.Invalidate();
        }


        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ShowCandidatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sudokuField1.ShowCandidates = ((ToolStripMenuItem)sender).Checked;
            sudokuField1.Invalidate();
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();


        private void HumanSolvingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UncheckOtherSolvingTechnique((ToolStripMenuItem)sender);
            _solvingmethod = Sudoku.SolvingTechnique.HumanSolvingTechnique;

            btnSolve.Text = "Solve - Human Solving";
        }

        private void BackTrackingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UncheckOtherSolvingTechnique((ToolStripMenuItem)sender);
            _solvingmethod = Sudoku.SolvingTechnique.BackTracking;

            btnSolve.Text = "Solve - BackTracking";
        }

        private static void UncheckOtherSolvingTechnique(ToolStripMenuItem checkedone)
        {
            foreach (var tsmi in checkedone.Owner.Items.Cast<ToolStripMenuItem>()
                                           .Where(itm => !checkedone.Equals(itm))) {
                tsmi.Checked = false;
            }

            // Keeps the menu visible
            checkedone.OwnerItem.OwnerItem.PerformClick();
            checkedone.OwnerItem.Owner.Show();
            checkedone.Owner.Show();
        }


        private void AnimatedSolvingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sudoku.RaiseCellChangedEvent = !_sudoku.RaiseCellChangedEvent;

            // Keeps the menu visible
            ((ToolStripMenuItem)sender).Owner.Show();
        }

        #endregion



    }

}
