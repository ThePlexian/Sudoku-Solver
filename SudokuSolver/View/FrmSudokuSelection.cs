using SudokuSolver.Model.Data;

namespace SudokuSolver.View
{


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
        private void CbSudokus_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedSudoku = Sudokus[cbSudokus.SelectedIndex];
            sfPreview.Sudoku = SelectedSudoku;
            sfPreview.Invalidate();
        }



        //Dialogue
        private void BtnCancel_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;

        private void BtnSelect_Click(object sender, EventArgs e) => DialogResult = DialogResult.OK;


    }
}
