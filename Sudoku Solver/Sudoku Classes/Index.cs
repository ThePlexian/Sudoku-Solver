using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuSolver
{
	public class Index
	{
		public Index(int r, int c)
		{
			this.Row = r;
			this.Column = c;
		}

		public int Row { get; set; }
		public int Column { get; set; }


		public bool IsEqual(Index i)
		{
			return (this.Row == i.Row && this.Column == i.Column);
		}

		public override string ToString()
		{
			return "{ " + this.Row.ToString() + " | " + this.Column.ToString() + " }";
		}
	}
}
