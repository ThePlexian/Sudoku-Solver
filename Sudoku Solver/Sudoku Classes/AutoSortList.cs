using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
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
