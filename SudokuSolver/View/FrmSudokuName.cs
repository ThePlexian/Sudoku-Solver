namespace SudokuSolver.View
{
    public partial class FrmSudokuName : Form
    {

        //Constructor
        public FrmSudokuName()
        {
            InitializeComponent();
        }

        public FrmSudokuName(Model.Data.Sudoku s)
        {
            InitializeComponent();

            _sudoku = s;
            _tmpname = s.Name;

            tbName.Text = _tmpname;
            tbName.SelectAll();
        }


        //The sudoku
        private readonly Model.Data.Sudoku _sudoku;
        private string _tmpname = "";



        //Dialog behaviour
        private void BtnReset_Click(object sender, EventArgs e) => tbName.Text = _sudoku.Name;

        private void BtnApply_Click(object sender, EventArgs e) => _sudoku.Name = _tmpname;

        private void TbName_TextChanged(object sender, EventArgs e) => _tmpname = tbName.Text;

    }
}
