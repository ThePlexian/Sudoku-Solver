using System.Linq;

namespace SudokuSolver.Sudoku
{

	public class Cell
	{
	
		//Constructors
		public Cell(Index i)
		{
			this.Number = 0;
			this.Candidates = FullCandidates();
			this.Index = i;
			this.IsPreset = false;
		}


		//Properties
		public int Number { get; set; }

		public AutoSortList<int> Candidates { get; set; }

		public bool IsPreset { get; set; }
		
		public Index Index { get; private set; }




		//Methods
		public static AutoSortList<int> EmptyCandidates()
		{
			return new AutoSortList<int>(); 
		}

		public static AutoSortList<int> FullCandidates()
		{
			return new AutoSortList<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; 
		}

		public override string ToString()
		{
			return (this.Number != 0 ? this.Number.ToString() : "");
		}

		public static Cell Empty
		{
			get
			{
				return new Cell(new Index(-1, -1));
			}
		}




		//Compare
		public bool IsEqual(Cell c)
		{
			return (this.Number == c.Number && HaveSameCandidates(this, c) && this.IsPreset == c.IsPreset && this.Index.IsEqual(c.Index));
		}

		private static bool HaveSameCandidates(Cell c1, Cell c2)
		{
			if (c1.Candidates.Count != c2.Candidates.Count)
				return false;

			for (var i = 0; i <= c1.Candidates.Count - 1; i++)
			{
				if (c1.Candidates.ElementAt(i) != c2.Candidates.ElementAt(i))
					return false;
			}

			return true;
		}
	}
}
