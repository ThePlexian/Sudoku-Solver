namespace SudokuSolver.Sudoku
{
	public struct Index
	{

		public Index(int r, int c)
		{
			this.Row = r;
			this.Column = c;
		}

		public readonly int Row;
		public readonly int Column;


		public bool IsEqual(Index i)
		{
			return (this.Row == i.Row && this.Column == i.Column);
		}

		public override string ToString()
		{
			return "{ " + this.Row + " | " + this.Column + " }";
		}

	}
}
