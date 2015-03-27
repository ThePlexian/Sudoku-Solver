using System;
using System.Windows.Forms;

namespace SudokuSolver.UI
{
	public partial class FrmSudokuName : Form
	{

		//Constructor
		public FrmSudokuName()
		{
			InitializeComponent();
		}

		public FrmSudokuName(Sudoku.Sudoku s)
		{
			InitializeComponent();

			_sudoku = s;
			_tmpname = s.Name;

			tbName.Text = _tmpname;
			tbName.SelectAll();
		}


		//The sudoku
		private readonly Sudoku.Sudoku _sudoku;
		private string _tmpname = "";



		//Dialog behaviour
		private void btnReset_Click(object sender, EventArgs e)
		{
			this.tbName.Text = _sudoku.Name;
		}

		private void btnApply_Click(object sender, EventArgs e)
		{
			_sudoku.Name = _tmpname;
		}

		private void tbName_TextChanged(object sender, EventArgs e)
		{
			_tmpname = tbName.Text;
		}

	}
}
