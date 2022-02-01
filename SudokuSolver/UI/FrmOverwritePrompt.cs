using System;
using System.Drawing;
using System.Windows.Forms;

namespace SudokuSolver.UI
{
	using Sudoku;

	public partial class FrmOverwritePrompt : Form
	{

		//Constructor
		public FrmOverwritePrompt()
		{
			InitializeComponent();

			AcceptButton = btnOverwrite;
			CancelButton = btnCancel;
		}

		//Startup
		private void frmSavePrompt_Load(object sender, EventArgs e)
		{
			lblText.Text = "The file you selected is already existing.\nHow do you want to continue?";
			pbIcon.Image = SystemIcons.Exclamation.ToBitmap();
			pbIcon.SizeMode = PictureBoxSizeMode.CenterImage;

			FileAccess = default(Sudoku.FileAccess);
		}





		//Dialogue behaviour

		public Sudoku.FileAccess FileAccess { get; private set; }


		private void btnOverwrite_Click(object sender, EventArgs e)
		{
			FileAccess = Sudoku.FileAccess.CreateOrOverwrite;
			Close();
		}

		private void btnAppend_Click(object sender, EventArgs e)
		{
			FileAccess = Sudoku.FileAccess.CreateOrAppend;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			FileAccess = Sudoku.FileAccess.CreateOnly;
			Close();
		}

		private void FrmOverwritePrompt_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.FormOwnerClosing)
				FileAccess = Sudoku.FileAccess.CreateOnly;
		}

	}
}
