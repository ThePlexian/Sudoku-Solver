using System.Collections.Generic;


namespace SudokuSolver.Sudoku
{
	public class AutoSortList<T> : List<T>
	{
		public new void Add(T item)
		{
			base.Add(item);
			this.Sort();
		}
	}
}
