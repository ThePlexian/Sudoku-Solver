using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;


namespace SudokuSolver.UI
{
	using Sudoku;


	public partial class FrmSudokuSelection : Form
	{

		//Constructor
		public FrmSudokuSelection()
		{
			InitializeComponent();
			Sudokus = new List<Sudoku>();
		}


		//Startup
		private void FrmSudokuSelection_Load(object sender, EventArgs e)
		{
			pbIcon.Image = SystemIcons.Information.ToBitmap();
			pbIcon.SizeMode = PictureBoxSizeMode.CenterImage;

			cbSudokus.DataSource = Sudokus;
			cbSudokus.DisplayMember = "Name";
		}



		//The sudokus to be shown
		public List<Sudoku> Sudokus { private get; set; }

		//The selected sudoku
		public Sudoku SelectedSudoku { get; private set; }




		//Item changed
		private void cbSudokus_SelectedIndexChanged(object sender, EventArgs e)
		{
			SelectedSudoku = Sudokus[cbSudokus.SelectedIndex];
			sfPreview.Sudoku = SelectedSudoku;
			sfPreview.Invalidate();
		}



		//Dialogue
		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void btnSelect_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}


	}
}
