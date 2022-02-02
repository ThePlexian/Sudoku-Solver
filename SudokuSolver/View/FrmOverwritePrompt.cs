using SudokuSolver.Model.Data;

namespace SudokuSolver.View
{

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
        private void FrmSavePrompt_Load(object sender, EventArgs e)
        {
            lblText.Text = "The file you selected is already existing.\nHow do you want to continue?";
            pbIcon.Image = SystemIcons.Exclamation.ToBitmap();
            pbIcon.SizeMode = PictureBoxSizeMode.CenterImage;

            FileAccess = default;
        }





        //Dialogue behaviour

        public Model.IO.FileAccess FileAccess { get; private set; }


        private void BtnOverwrite_Click(object sender, EventArgs e)
        {
            FileAccess = Model.IO.FileAccess.CreateOrOverwrite;
            Close();
        }

        private void BtnAppend_Click(object sender, EventArgs e)
        {
            FileAccess = Model.IO.FileAccess.CreateOrAppend;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            FileAccess = Model.IO.FileAccess.CreateOnly;
            Close();
        }

        private void FrmOverwritePrompt_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.FormOwnerClosing) {
                FileAccess = Model.IO.FileAccess.CreateOnly;
            }
        }

    }
}
