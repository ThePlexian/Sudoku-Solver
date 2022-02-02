namespace SudokuSolver.Model.Data
{
    public struct GridIndex
    {

        public GridIndex(int r, int c)
        {
            Row = r;
            Column = c;
        }

        public readonly int Row;
        public readonly int Column;


        public bool IsEqual(GridIndex i) => Row == i.Row && Column == i.Column;

        public override string ToString() => "{ " + Row + " | " + Column + " }";

    }
}
