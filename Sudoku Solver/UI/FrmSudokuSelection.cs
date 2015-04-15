using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace SudokuSolver.UI
{
	using SudokuSolver.Sudoku;


	public partial class FrmSudokuSelection : Form
	{

		//Constructor
		public FrmSudokuSelection()
		{
			InitializeComponent();
			this.Sudokus = new List<Sudoku>();
		}


		//Startup
		private void FrmSudokuSelection_Load(object sender, EventArgs e)
		{
			this.pbIcon.Image = SystemIcons.Information.ToBitmap();
			this.pbIcon.SizeMode = PictureBoxSizeMode.CenterImage;

			this.cbSudokus.DataSource = this.Sudokus;
			this.cbSudokus.DisplayMember = "Name";
		}



		//The sudokus to be shown
		public List<Sudoku> Sudokus { private get; set; }

		//The selected sudoku
		public Sudoku SelectedSudoku { get; private set; }




		//Item changed
		private void cbSudokus_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.SelectedSudoku = this.Sudokus[this.cbSudokus.SelectedIndex];
			this.sfPreview.Sudoku = this.SelectedSudoku;
			this.sfPreview.Invalidate();
		}



		//Dialogue
		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}


	}
}
