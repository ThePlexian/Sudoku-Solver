using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Threading;
using System.Timers;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

namespace SudokuSolver
{
	public partial class frmMain : Form
	{
		
		public frmMain()
		{
			InitializeComponent();
		}



		//The Sudoku + animative stuff
		public Sudoku _sudoku;

		public Thread _solvingthread;
		public Sudoku.SolvingTechnique _solvingmethod;


		//Startup
		private void frmMain_Load(object sender, EventArgs e)
		{
			//Sudoku
			_sudoku = new Sudoku();
			_sudoku.CellChanged += this.CellChanged;
			_sudoku.SolvingCompleted += this.SudokuSolvingCompleted;
			_solvingmethod = Sudoku.SolvingTechnique.HumanSolvingTechnique;

			//Sudoku control
			this.sudokuField1.Sudoku = _sudoku;
			this.sudokuField1.GridWidth = 1;
			this.sudokuField1.GridInnerBorderWidth = 3;
			this.sudokuField1.GridBorderWidth = 5;
			this.sudokuField1.GridColor = Color.Orange;
			this.sudokuField1.HoveredCellColor = Color.FromArgb(127, 200, 200, 200);
			this.sudokuField1.SelectedCellColor = Color.FromArgb(255, 255, 100, 80);
			this.sudokuField1.Font = new Font("Calibri", 13, FontStyle.Bold);
			this.sudokuField1.ForeColor = Color.FromArgb(50, 50, 50);
			this.sudokuField1.HoverActivated = true;
			this.sudokuField1.ShowCandidates = false;

			//The button
			this.btnSolve.Location = new Point((this.ClientSize.Width - this.btnSolve.Width) / 2, this.btnSolve.Location.Y);

			//Notifier
			Notifier.Notifications.Notify += this.Notified;
			Notifier.Notifications.Reset();
			_notirectangle = new Rectangle(10, this.customMenuStrip1.Location.Y + this.customMenuStrip1.Height + 10, this.ClientSize.Width - 20, 35);

			//Title
			Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			this.Text += "vers. " + v.ToString(); 
		}

		private void frmMain_Shown(object sender, EventArgs e)
		{
			sudokuField1.Focus();
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (_solvingthread != null)
				if (_solvingthread.IsAlive)
					_solvingthread.Abort();

			_solvingthread = null;
		}


		//Painting
		private void frmMain_Paint(object sender, PaintEventArgs e)
		{
			//Set the qualities
			e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
			e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
			e.Graphics.SmoothingMode = SmoothingMode.Default;
			e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
			e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

			//The notification to be displayed
			Notifier.Notification n = GetHighestNotification();

			if (n != null)
			{
				StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
				Brush b = new SolidBrush(Color.FromKnownColor(KnownColor.Control));
				switch (n.Type)
				{
					case Notifier.Notification.MessageType.Information:	
						b = new SolidBrush(Color.FromArgb(100, 255, 100));
						break;
					case Notifier.Notification.MessageType.Warning:
						b = new SolidBrush(Color.FromArgb(255, 255, 50));
						break;
					case Notifier.Notification.MessageType.Error:
						b = new SolidBrush(Color.FromArgb(255, 100, 100));
						break;
					case Notifier.Notification.MessageType.None:
						return;		//Exit
				}

				e.Graphics.FillRectangle(b, _notirectangle);
				e.Graphics.DrawString(n.Message.Replace("[0]", notiparam.ToString().Replace(",", ".")), new Font(this.Font.FontFamily, 9.25F, FontStyle.Bold), Brushes.Black, _notirectangle, sf);
			}

			e.Graphics.DrawRectangle(Pens.Black, _notirectangle.X + 1, _notirectangle.Y + 1, _notirectangle.Width - 1, _notirectangle.Height - 1);
		}




		// **** Notifications ****
		private Rectangle _notirectangle;
		private object notiparam = 0;

		//Notification started / expired
		private void Notified(object sender, Notifier.NotifyEventArgs ne)
		{
			this.ConditionalInvoke(new Action(() =>
			{
				this.Invalidate(_notirectangle);
			}));
		}

		//Returns the most important notification
		private Notifier.Notification GetHighestNotification()
		{
			string[] keys = {"Invalid", "Invalid File", "Saved", "Unambiguous", "Solved", "Unsolved"};
			foreach (string key in keys)
			{
				Notifier.Notification n = Notifier.Notifications.GetNotification(key);
				if (n.IsActive)
					return n;
			}

			return null;
		}





	
		// **** Solve ****
		System.Diagnostics.Stopwatch _swsolving;

		//Start Solving
		private void btnSolve_Click(object sender, EventArgs e)
		{
			//Deactivate UI Control
			this.customMenuStrip1.Enabled = false;
			this.btnSolve.Enabled = false;
			this.sudokuField1.EditingEnabled = false;

			//Start solving
			_swsolving = System.Diagnostics.Stopwatch.StartNew();
			_solvingthread = new Thread(() => _sudoku.Solve(_solvingmethod));
			_solvingthread.Start();
		}

		//Cell changed
		private void CellChanged(object sender, Sudoku.CellChangedEventArgs e)
		{
			Notifier.Notifications.ChangeState("Invalid", !_sudoku.IsValid);
			Notifier.Notifications.ChangeState("Unambiguous", _sudoku.MissingNumbers > 81 - 17);
		}

		//Sudoku solving procedure finished
		private void SudokuSolvingCompleted(object sender, EventArgs e)
		{
			//Show result
			if (_sudoku.IsSolved) 
			{
				Notifier.Notifications.ChangeState("Solved", true);
				Notifier.Notifications.ChangeState("Unsolved", false);
				notiparam = Math.Round(_swsolving.Elapsed.TotalMilliseconds, 2);
			}
			else 
			{
				Notifier.Notifications.ChangeState("Solved", false);
				Notifier.Notifications.ChangeState("Unsolved", true);
				notiparam = _sudoku.MissingNumbers;
			}

			//Reactivate UI Control
			this.ConditionalInvoke(new Action(() =>
			{
				this.customMenuStrip1.Enabled = true;
				this.btnSolve.Enabled = true;
				this.sudokuField1.EditingEnabled = true;
				this.sudokuField1.Invalidate();
			}));
		}






		//Solve 1011 Sudokus
		private void Solve1011()
		{
			List<string> sudokus = File.ReadAllLines(@"C:\Users\Marek\Desktop\1011sudokus.txt").ToList();
			int win = 0, total = 0;
			List<double> time = new List<double>();

			foreach (string s in sudokus)
			{
				Sudoku su = new Sudoku();
				Sudoku.Read(s, ref su);
				total++;

				System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
				int lastwin = win;

				if (su.Solve(Sudoku.SolvingTechnique.BackTracking))
					win++;

				time.Add(sw.Elapsed.TotalMilliseconds);
				System.Diagnostics.Debug.Print("{2}: {0} - {1}", lastwin != win, sw.Elapsed.TotalMilliseconds, total);

			}

			System.IO.File.WriteAllText(@"C:\Users\Marek\Desktop\Sudoku Tests\" + DateTime.UtcNow.ToString("u").Replace(":", "-").Replace("Z", "h") + ".txt",
				string.Format("Solved {0} out of {1} total sudokus within a total time of {2}{3}Max time: {4}{3}Min time: {5}{3}Average time: {6}", win, total,
				time.Sum(), Environment.NewLine, time.Max(), time.Min(), time.Average()));

			System.Diagnostics.Debug.Print("Solved {0} out of {1} total sudokus within a total time of {2}{3}Max time: {4}{3}Min time: {5}{3}Average time: {6}", win, total, 
				time.Sum(), Environment.NewLine, time.Max(), time.Min(), time.Average());
		}





		// **** Menustrip *****

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_sudoku = new Sudoku();
			_sudoku.CellChanged += this.CellChanged;
			_sudoku.SolvingCompleted += this.SudokuSolvingCompleted;
			this.sudokuField1.Sudoku = _sudoku;

			Notifier.Notifications.Reset();
			this.Invalidate(_notirectangle);
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog ofd = new OpenFileDialog())
			{
				ofd.Title = "Open from Textfile";
				ofd.AddExtension = true;
				ofd.DefaultExt = "txt";
				ofd.CheckPathExists = true;
				ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

				if (ofd.ShowDialog() == DialogResult.OK) 
				{
					Notifier.Notifications.Reset();
					this.Invalidate(_notirectangle);

					if (!Sudoku.Load(ofd.FileName, ref _sudoku))
					{
						//Add notification
						Notifier.Notifications.ChangeState("Invalid File", true);

						//Reset sudoku
						_sudoku = new Sudoku();
						_sudoku.CellChanged += this.CellChanged;
						_sudoku.SolvingCompleted += this.SudokuSolvingCompleted;
						this.sudokuField1.Sudoku = _sudoku;

						return;
					}
					this.sudokuField1.Sudoku = _sudoku;
					this.CellChanged(null, null);

				}
					
			}
		}

		private void saveAsTextfileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog sfd = new SaveFileDialog())
			{
				sfd.Title = "Save As Textfile";
				sfd.AddExtension = true;
				sfd.DefaultExt = "txt";
				sfd.CheckPathExists = true;
				sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				sfd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

				if (sfd.ShowDialog() == DialogResult.OK)
				{
					_sudoku.Save(sfd.FileName, false);
					Notifier.Notifications.ChangeState("Saved", true);
				}

			}
		}

		private void saveAsBitmapToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog sfd = new SaveFileDialog())
			{
				sfd.Title = "Save As Bitmap";
				sfd.AddExtension = true;
				sfd.DefaultExt = "bmp";
				sfd.CheckPathExists = true;
				sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				sfd.Filter = "Bitmaps (*.bmp)|*.bmp|All files (*.*)|*.*";

				if (sfd.ShowDialog() == DialogResult.OK)
				{
					Bitmap sudokupic = new Bitmap(this.sudokuField1.Width, this.sudokuField1.Height);
					this.sudokuField1.DrawToBitmap(sudokupic, new Rectangle(new Point(0, 0), this.sudokuField1.Size));

					int borderwidth = 5;
					Bitmap result = new Bitmap(sudokupic.Width + 2 * borderwidth, sudokupic.Height + 2 * borderwidth);
					using (Graphics g = Graphics.FromImage(result))
					{
						g.Clear(Color.FromArgb(245, 245, 245));
						g.DrawImage(sudokupic, new Point(borderwidth, borderwidth));
					}

					result.Save(sfd.FileName);
					Notifier.Notifications.ChangeState("Saved", true);
				}
			}
		}

		private void deleteNonPresetToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Sudoku temp = new Sudoku();
			temp.CellChanged += this.CellChanged;
			temp.SolvingCompleted += this.SudokuSolvingCompleted;

			for (int i = 0; i < 81; i++)
			{
				Cell c = _sudoku.GetCell(i / 9, i % 9);
				if (c.IsPreset) 
				{
					temp.SetValue(i / 9, i % 9, c.Number);
					temp.SetPresetValue(i / 9, i % 9, true);
				}

			}

			_sudoku = temp;
			this.sudokuField1.Sudoku = _sudoku;
		}

		
		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void showCandidatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem item = (this.customMenuStrip1.Items.Find("showCandidatesToolStripMenuItem", true)[0] as ToolStripMenuItem);
			item.Checked = !item.Checked;
			this.sudokuField1.ShowCandidates = item.Checked;
			this.sudokuField1.Invalidate();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}


		private void humanSolvingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(this.customMenuStrip1.Items.Find("humanSolvingToolStripMenuItem", true)[0] as ToolStripMenuItem).Checked = true;
			(this.customMenuStrip1.Items.Find("backTrackingToolStripMenuItem", true)[0] as ToolStripMenuItem).Checked = false;

			_solvingmethod = Sudoku.SolvingTechnique.HumanSolvingTechnique;
		}

		private void backTrackingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(this.customMenuStrip1.Items.Find("humanSolvingToolStripMenuItem", true)[0] as ToolStripMenuItem).Checked = false;
			(this.customMenuStrip1.Items.Find("backTrackingToolStripMenuItem", true)[0] as ToolStripMenuItem).Checked = true;

			_solvingmethod = Sudoku.SolvingTechnique.BackTracking;
		}
	}

}
