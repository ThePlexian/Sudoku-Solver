namespace SudokuSolver.Sudoku
{
	public struct Index
	{

		public Index(int r, int c)
		{
			Row = r;
			Column = c;
		}

		public readonly int Row;
		public readonly int Column;


		public bool IsEqual(Index i)
		{
			return (Row == i.Row && Column == i.Column);
		}

		public override string ToString()
		{
			return "{ " + Row + " | " + Column + " }";
		}

	}
}
