using System;
using System.Drawing;
using System.Windows.Forms;

namespace SudokuSolver.UI
{
	using SudokuSolver.Sudoku;

	public partial class FrmOverwritePrompt : Form
	{

		//Constructor
		public FrmOverwritePrompt()
		{
			InitializeComponent();

			this.AcceptButton = btnOverwrite;
			this.CancelButton = btnCancel;
		}

		//Startup
		private void frmSavePrompt_Load(object sender, EventArgs e)
		{
			this.lblText.Text = "The file you selected is already existing.\nHow do you want to continue?";
			this.pbIcon.Image = SystemIcons.Exclamation.ToBitmap();
			this.pbIcon.SizeMode = PictureBoxSizeMode.CenterImage;

			this.FileAccess = default(Sudoku.FileAccess);
		}





		//Dialogue behaviour

		public Sudoku.FileAccess FileAccess { get; private set; }


		private void btnOverwrite_Click(object sender, EventArgs e)
		{
			this.FileAccess = Sudoku.FileAccess.CreateOrOverwrite;
			this.Close();
		}

		private void btnAppend_Click(object sender, EventArgs e)
		{
			this.FileAccess = Sudoku.FileAccess.CreateOrAppend;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.FileAccess = Sudoku.FileAccess.CreateOnly;
			this.Close();
		}

		private void FrmOverwritePrompt_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.FormOwnerClosing)
				this.FileAccess = Sudoku.FileAccess.CreateOnly;
		}

	}
}
