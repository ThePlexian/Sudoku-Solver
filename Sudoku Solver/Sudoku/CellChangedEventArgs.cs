using System;

namespace SudokuSolver.Sudoku
{
	public class CellChangedEventArgs : EventArgs
	{
		public CellChangedEventArgs(Cell c, CellProperty cp)
		{
			Cell = c;
			ChangedProperty = cp;
		}

		public Cell Cell { get; private set; }
		public CellProperty ChangedProperty { get; private set; }
		public static new CellChangedEventArgs Empty => new CellChangedEventArgs(Cell.Empty, CellProperty.None);

		[Flags]
		public enum CellProperty { None = 0, Number = 1, Candidates = 2, IsPreset = 4 }
	}
}
