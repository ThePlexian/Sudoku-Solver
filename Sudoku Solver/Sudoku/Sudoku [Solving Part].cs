using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuSolver.Sudoku
{
	public partial class Sudoku
	{



		//**** MAIN SOLVING PROCEDURE ****
		public bool Solve(SolvingTechnique st)
		{
			bool success;
			switch (st)
			{
				case SolvingTechnique.HumanSolvingTechnique:
					success = SolveHumanLike();
					break;
				case SolvingTechnique.BackTracking:
					success = SolveBackTracking();
					break;
				default:
					success = false;
					break;
			}

			if (this.SolvingCompleted != null)
				this.SolvingCompleted(this, EventArgs.Empty);

			return success;
		}

		public enum SolvingTechnique
		{
			HumanSolvingTechnique,
			BackTracking
		}

		public event EventHandler SolvingCompleted;




		// **** BACKTRACKING ****
		private bool SolveBackTracking() //aka TrialAndError + Human
		{
			//Use human solving in the beginning
			this.SolveHumanLike();


			//Get missing numbers
			var tmp = new Cell[this.Cells.Length];
			for (var i = 0; i <= this.Cells.Length - 1; i++)
				tmp[i] = GetCell(i);
			var missing = tmp.Where(c => c.Number == 0).ToList();


			//If the sudoku is filled or unsolvable - exit
			if (missing.Count == 0)
				return true;

			if (missing.Count >= 81 - 17 || !this.IsValid)
				return false;


			//Try each candidate
			foreach (var n in missing[0].Candidates)
			{
				var copy = (Sudoku)this.Clone();

				//Set candidate
				copy.SetValue(missing[0].Index, n);
				if (this.CellChanged != null && RaiseCellChangedEvent)
					this.CellChanged(this, new CellChangedEventArgs(copy.GetCell(missing[0].Index), CellChangedEventArgs.CellProperty.Number));

				//If solving wasn't successful
				if (!copy.SolveBackTracking())
					continue;

				this.Override(copy);
				return true;
			}

			//No solution found
			return false;
		}





		// **** HUMAN SOLVING TECHNIQUES ****

		private bool SolveHumanLike()
		{
			//Solve
			while (this.MissingNumbers != 0)
			{
				var successful = false;

				//Use the solving methods
				successful |= NakedSingle();
				successful |= HiddenSingle(); 
				Enumerable.Range(2, 3).ToList().ForEach(i => successful |= NakedSubset(i));
				Enumerable.Range(2, 3).ToList().ForEach(i => successful |= HiddenSubset(i));
				successful |= LockedCandidatesType1();
				successful |= LockedCandidatesType2();


				//If this round no number got calculated - failed
				if (!successful)
					return false;
			}

			return true;
		}


		#region ** Single solving methods **

		private bool NakedSingle()
		{
			var successful = false;

			foreach (var c in GetCellsIterated())
			{
				//If the Cell is empty and has only one possible candidate --> set
				if (c.Number == 0 && c.Candidates.Count == 1)
					NewNumberCalculated(c, c.Candidates[0], ref successful);
			}

			return successful;
		}

		private bool HiddenSingle()
		{
			var successful = false;
			var clusters = GetAllClusters();

			//Iterate through the clusters
			foreach (var cluster in clusters)
			{
				//Iterate all candidates
				for (var n = 1; n <= 9; n++)
				{
					//If this candidate is only available in one cell of this cluster - set
					var candidatecells = cluster.Where(c => c.Number == 0 && c.Candidates.Contains(n)).ToList();

					if (candidatecells.Count == 1)
						NewNumberCalculated(candidatecells[0], n, ref successful);
				}
			}

			return successful;
		}

		private bool NakedSubset(int n)
		{
			var successful = false;

			//All rows, columns and boxes
			var clusters = GetAllClusters();

			//Iterate through the clusters
			foreach (var cluster in clusters)
			{
				//List of cells with a amount of candidates lower than or equal to n
				var selected = cluster.Where(c => c.Candidates.Count <= n && c.Candidates.Count != 0).Select(c => c).ToList();

				//Only if there are enough tuples
				if (selected.Count < n)
					continue;

				//Iterate through all possible ntuples in selected (i.e. if n == 3 but selected contains 5 cells,
				//all possible combinations of length n in selected will be iterated
				foreach (var iesubset in selected.Combinations(n))
				{
					var nsubset = iesubset.ToList();

					//List of all candidates appearing in the Cells of the list 
					var candidatelists = new List<List<int>>();
					nsubset.ForEach(c => candidatelists.Add(c.Candidates.ToList()));
					var combinedcandidates = candidatelists.CombineSortDeduplicate().ToList();


					//If the amount of the total candidates is equal to n - delete them from other cells
					if (combinedcandidates.Count != n)
						continue;

					//A list containing all Cells of cluster except the cells in selected
					var shortedcluster = cluster.Where(c => !nsubset.Any(c.IsEqual) && c.Number == 0).Select(c => c).ToList();
					//No ||, because otherwise the method wouldnt be called if successful == true
					successful |= RemoveCandidates(shortedcluster, combinedcandidates.ToArray());
				}
			}

			return successful;
		}

		private bool HiddenSubset(int n)
		{
			var successful = false;

			//Iterate through all clusters
			var clusters = GetAllClusters();

			foreach (var cluster in clusters)
			{
				//Get all candidates occuring max n times (and min 1 time) in the cluster
				var selectedcandidates = Cell.FullCandidates().Where(i => cluster.Count(c => c.Candidates.Contains(i)) <= n && cluster.Any(c => c.Candidates.Contains(i))).ToList();


				//Iterate thorugh all subsets of size n
				foreach (var iesubset in selectedcandidates.Combinations(n))
				{
					var nsubset = iesubset.ToList();

					//Get a list of all cells the candidates of nsubset are in
					var selectedcells = cluster.Where(c => nsubset.Any(i => c.Candidates.Contains(i))).ToList();

					//If the list of cells of both candidates are equal, delete the other candidates from the cells
					if (selectedcells.Count != n)
						continue;

					successful = selectedcells.Aggregate(successful, (current, c) => current | RemoveCandidates(c, c.Candidates.Where(i => !nsubset.Contains(i)).ToArray()));
				}
			}

			return successful;
		}

		private bool LockedCandidatesType1() //aka BoxLineIntersection / Pointing
		{
			var successful = false;

			//Iterate through all boxes
			var boxes = GetListOfBoxes();


			foreach (var box in boxes)
			{
				//Get all candidates occuring at least 1 time in the box
				var selectedcandidates = Cell.FullCandidates().Where(i => box.Any(c => c.Candidates.Contains(i))).ToList();

				//Iterate through all candidates
				foreach (var n in selectedcandidates)
				{
					//Get cells containing the current candidate n
					var selectedcells = box.Where(c => c.Candidates.Contains(n)).ToList();

					//Check whether the cells are in the same line
					int rowindex, columnindex;
					if (IsIdenticalRow(selectedcells, out rowindex))
					{
						var row = GetCellsInRow(new Index(rowindex, 0));
						successful |= RemoveCandidates(row.Where(c => !selectedcells.Any(c.IsEqual)), n);
					}
					else if (IsIdenticalColumn(selectedcells, out columnindex))
					{
						var column = GetCellsInColumn(new Index(0, columnindex));
						successful |= RemoveCandidates(column.Where(c => !selectedcells.Any(c.IsEqual)), n);
					}
				}
			}

			return successful;
		}

		private bool LockedCandidatesType2() //aka LineBoxIntersection / Claiming
		{
			var successful = false;

			//Iterate through all lines
			var lines = GetListOfRows().Concat(GetListOfColumns());


			foreach (var line in lines)
			{
				//Get all candidates occuring at least 1 time in the line
				var selectedcandidates = Cell.FullCandidates().Where(i => line.Any(c => c.Candidates.Contains(i))).ToList();

				//Iterate through all candidates
				foreach (var n in selectedcandidates)
				{
					//Get cells containing the current candidate n
					var selectedcells = line.Where(c => c.Candidates.Contains(n)).ToList();

					//Check whether the cells are in the same line
					Index boxindex;
					if (!IsIdenticalBox(selectedcells, out boxindex))
						continue;

					var box = GetCellsInBox(boxindex);
					successful |= RemoveCandidates(box.Where(c => !selectedcells.Any(c.IsEqual)).ToList(), n);
				}
			}


			return successful;
		}

		#endregion



		#region ** Get certain cell clusters **

		//Returns a cluster of cells
		private List<Cell> GetCellsInRow(Index ind)
		{
			var temp = new List<Cell>();

			for (var c = 0; c <= 8; c++)
				temp.Add(this.Cells[ind.Row, c]);

			return temp;
		}

		private List<Cell> GetCellsInColumn(Index ind)
		{
			var temp = new List<Cell>();

			for (var r = 0; r <= 8; r++)
				temp.Add(this.Cells[r, ind.Column]);

			return temp;
		}

		private List<Cell> GetCellsInBox(Index ind)
		{
			var temp = new List<Cell>();
			var topleft = new Index((ind.Row / 3) * 3, (ind.Column / 3) * 3);

			for (var r = topleft.Row; r <= topleft.Row + 2; r++)
				for (var c = topleft.Column; c <= topleft.Column + 2; c++)
					temp.Add(this.Cells[r, c]);

			return temp;
		}

		private List<Cell> GetConnectedCells(Index ind)
		{
			return GetCellsInRow(ind).Concat(GetCellsInColumn(ind)).Concat(GetCellsInBox(ind)).Where(c => !c.IsEqual(this.Cells[ind.Row, ind.Column])).ToList();
		}


		//Returns a cluster of lists of cells
		private IEnumerable<List<Cell>> GetListOfRows()
		{
			var temp = new List<List<Cell>>();
			for (var r = 0; r <= 8; r++)
				temp.Add(GetCellsInRow(new Index(r, 0))); //column doesn't matter
			return temp;
		}

		private IEnumerable<List<Cell>> GetListOfColumns()
		{
			var temp = new List<List<Cell>>();
			for (var c = 0; c <= 8; c++)
				temp.Add(GetCellsInColumn(new Index(0, c))); //row doesn't matter
			return temp;
		}

		private IEnumerable<List<Cell>> GetListOfBoxes()
		{
			var temp = new List<List<Cell>>();
			for (var r = 0; r <= 8; r += 3)
				for (var c = 0; c <= 8; c += 3)
					temp.Add(GetCellsInBox(new Index(r, c)));
			return temp;
		}

		private IEnumerable<List<Cell>> GetAllClusters()
		{
			return GetListOfRows().Concat(GetListOfColumns()).Concat(GetListOfBoxes()).ToList();
		}

		#endregion


		#region ** Compare cell positions **

		//Returns whether a cell cluster is in the same line / box
		private static bool IsIdenticalRow(IList<Cell> cells, out int rowindex)
		{
			rowindex = -1;

			if (cells.Any(c => c.Index.Row != cells[0].Index.Row))
				return false;

			rowindex = cells[0].Index.Row;
			return true;
		}

		private static bool IsIdenticalColumn(IList<Cell> cells, out int columnindex)
		{
			columnindex = -1;

			if (cells.Any(c => c.Index.Column != cells[0].Index.Column))
				return false;

			columnindex = cells[0].Index.Column;
			return true;
		}

		private static bool IsIdenticalBox(IList<Cell> cells, out Index boxindex)
		{
			boxindex = new Index(-1, -1);

			if (!cells.All(c => ToBoxIndex(c.Index).IsEqual(ToBoxIndex(cells[0].Index))))
				return false;

			boxindex = ToBoxIndex(cells[0].Index);
			return true;
		}

		private static Index ToBoxIndex(Index i)
		{
			return new Index(i.Row / 3 * 3, i.Column / 3 * 3);
		}

		#endregion


		#region ** Update Candidates **

		//Removes all the given candidates out of the given Cells
		private bool RemoveCandidates(Cell c, params int[] candidates)
		{
			var countbefore = c.Candidates.Count;
			candidates.ToList().ForEach(candidate => c.Candidates.Remove(candidate));

			if (countbefore <= c.Candidates.Count)
				return false;


			if (this.CellChanged != null && RaiseCellChangedEvent)
				this.CellChanged(this, new CellChangedEventArgs(c, CellChangedEventArgs.CellProperty.Candidates));

			return true;
		}

		private bool RemoveCandidates(IEnumerable<Cell> cells, params int[] candidates)
		{
			return cells.Aggregate(false, (current, c) => current | RemoveCandidates(c, candidates));
		}


		//Deletes the number out of the candidates of the reachable Cells
		private void DeleteCandidate(int candidate, Index i)
		{
			foreach (var c in GetConnectedCells(i))
				if (c.Candidates.Remove(candidate))
					if (this.CellChanged != null && RaiseCellChangedEvent)
						this.CellChanged(this, new CellChangedEventArgs(c, CellChangedEventArgs.CellProperty.Candidates));
		}

		//Activates the number in the candidates of the reachable Cells
		private void AddCandidate(int candidate, Index i)
		{
			if (candidate == 0)
				return;

			foreach (var c in GetConnectedCells(i).Where(c => c.Number == 0))
			{
				c.Candidates.Add(candidate);
				if (this.CellChanged != null && RaiseCellChangedEvent)
					this.CellChanged(this, new CellChangedEventArgs(c, CellChangedEventArgs.CellProperty.Candidates));
			}
		}

		//Refreshes the candidates in the Cell
		private void RefreshCandidates(Index i)
		{
			var candidates = new AutoSortList<int>();

			//If the cell is filled, there is nothing ti refresh
			if (this.Cells[i.Row, i.Column].Number != 0)
				return;

			//Get all cells connected with this cell
			var connectedcells = GetConnectedCells(i);

			//Enumerate through the candidates, and add if none of the connected cells contains the candidate as number
			for (var n = 1; n <= 9; n++)
				if (connectedcells.All(c => c.Number != n))
					candidates.Add(n);

			this.Cells[i.Row, i.Column].Candidates = candidates;
		}

		#endregion





		//Do what is neccessary when a new Cell number got found out
		private void NewNumberCalculated(Cell c, int value, ref bool successful)
		{
			if (value == 0)
				return;

			this.SetValue(c.Index, value);
			successful = true;
		}

	}
}
