using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SudokuSolver
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();
		}

		//The Sudoku
		public Sudoku _sudoku;


		//Startup
		private void Form1_Load(object sender, EventArgs e)
		{
			_sudoku = new Sudoku();

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

			Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			//this.Text += "vers. " + v.Major.ToString() + "." + v.Minor.ToString();
			this.Text += "vers. " + v.ToString(); 
		}

		private void Form1_Shown(object sender, EventArgs e)
		{
			sudokuField1.Focus();
		}


	
		// **** Solve ****
		private void btnSolve_Click(object sender, EventArgs e)
		{
			_sudoku.Solve();
		}








		// **** Menustrip *****

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_sudoku = new Sudoku();
			this.sudokuField1.Sudoku = _sudoku;
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
					if (!Sudoku.Load(ofd.FileName, out _sudoku))
					{
						MessageBox.Show("The selected file does not contain a valid sudoku text. Please select another file.", "Invalid content", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
					this.sudokuField1.Sudoku = _sudoku;
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
					_sudoku.Save(sfd.FileName, false);
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
				}
			}
		}

		private void resetToInputToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Sudoku temp = new Sudoku();

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


		private void showCandidatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem item = (this.customMenuStrip1.Items.Find("showCandidatesToolStripMenuItem", true)[0] as ToolStripMenuItem);
			item.Checked = !item.Checked;
			this.sudokuField1.ShowCandidates = item.Checked;
			this.sudokuField1.Invalidate();
		}

		private void allowEditingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem item = (this.customMenuStrip1.Items.Find("allowEditingToolStripMenuItem", true)[0] as ToolStripMenuItem);
			item.Checked = !item.Checked;
			this.sudokuField1.EditingEnabled = item.Checked;
			this.sudokuField1.Invalidate();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}


	}

}
