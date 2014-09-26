using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SudokuSolver
{
	public partial class Sudoku
	{

		//**** MAIN SOLVING PROCEDURE ****
		public bool Solve(SolvingTechnique st)
		{
			bool success;
			switch(st) 
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

			if (SolvingCompleted != null)
				SolvingCompleted(this, EventArgs.Empty);
				
			return success;
		}

		public enum SolvingTechnique { HumanSolvingTechnique, BackTracking}

		public event EventHandler SolvingCompleted;



		// **** HUMAN SOLVING TECHNIQUES ****
		private bool SolveHumanLike()
		{

			//Solve
			while (this.MissingNumbers != 0)
			{
				bool successful = false;

				//Use the solving methods
				successful |= NakedSingle();													//1
				successful |= HiddenSingle();													//211
				Enumerable.Range(2, 3).ToList().ForEach(i => successful |= NakedSubset(i));		//296
				Enumerable.Range(2, 3).ToList().ForEach(i => successful |= HiddenSubset(i));	//296
				successful |= LockedCandidatesType1();											//316
				successful |= LockedCandidatesType2();											//319



				//If this round no number got calculated - failed
				if (!successful)
					return false;
			}

			return true;
		}


		// Different solving methods 
		private bool NakedSingle()
		{
			bool successful = false;

			foreach (Cell c in this.Cells)
			{
				//If the Cell is empty and has only one possible candidate --> set
				if (c.Number == 0 && c.Candidates.Count == 1)
					NewNumberCalculated(c, c.Candidates[0], ref successful);	
			}

			return successful;
		}

		private bool HiddenSingle()
		{
			bool successful = false;

			var clusters = GetAllClusters();

			//Iterate through the clusters
			foreach (List<Cell> cluster in clusters)
			{
				//Iterate all candidates
				for (int n = 1; n <= 9; n++)
				{
					//If this candidate is only available in one cell of this cluster - set
					List<Cell> candidatecells = cluster.Where(c => c.Number == 0 && c.Candidates.Contains(n)).ToList();

					if (candidatecells.Count == 1)
						NewNumberCalculated(candidatecells[0], n, ref successful);
				}
			}

			return successful;
		}

		private bool NakedSubset(int n)
		{
			bool successful = false;

			//All rows, columns and boxes
			var clusters = GetAllClusters();

			//Iterate through the clusters
			foreach (List<Cell> cluster in clusters)
			{
				//List of cells with a amount of candidates lower than or equal to n
				List<Cell> selected = cluster.Where(c => c.Candidates.Count <= n && c.Candidates.Count != 0).Select(c => c).ToList();

				//Only if there are enough tuples
				if (selected.Count >= n)
				{
					//Iterate through all possible ntuples in selected (i.e. if n == 3 but selected contains 5 cells,
					//all possible combinations of length n in selected will be iterated
					foreach (IEnumerable<Cell> iesubset in selected.Combinations(n))
					{
						List<Cell> nsubset = iesubset.ToList();

						//List of all candidates appearing in the Cells of the list 
						var candidatelists = new List<List<int>>();
						nsubset.ForEach(c => candidatelists.Add(c.Candidates.ToList()));
						List<int> combinedcandidates = CombineSortDeduplicate(candidatelists.ToArray());


						//If the amount of the total candidates is equal to n - delete them from other cells
						if (combinedcandidates.Count == n)
						{
							//A list containing all Cells of cluster except the cells in selected
							List<Cell> shortedcluster = cluster.Where(c => !nsubset.Any(s => c.IsEqual(s)) && c.Number == 0).Select(c => c).ToList();
							//No ||, because otherwise the method wouldnt be called if successful == true
							successful |= RemoveCandidates(shortedcluster, combinedcandidates);
						}
					}
				}
			}

			return successful;
		}

		private bool HiddenSubset(int n)
		{
			bool successful = false;

			//Iterate through all clusters
			var clusters = GetAllClusters();

			foreach (List<Cell> cluster in clusters)
			{
				//Get all candidates occuring max n times (and min 1 time) in the cluster
				List<int> selectedcandidates = Cell.FullCandidates().Where(
					i => cluster.Where(c => c.Candidates.Contains(i)).Count() <= n && cluster.Where(c => c.Candidates.Contains(i)).Count() > 0).ToList();


				//Iterate thorugh all subsets of size n
				foreach (IEnumerable<int> iesubset in selectedcandidates.Combinations(n))
				{
					List<int> nsubset = iesubset.ToList();

					//Get a list of all cells the candidates of nsubset are in
					List<Cell> selectedcells = cluster.Where(c => nsubset.Any(i => c.Candidates.Contains(i))).ToList();

					//If the list of cells of both candidates are equal, delete the other candidates from the cells
					if (selectedcells.Count == n)
					{
						foreach (Cell c in selectedcells)
						{
							//No ||, because otherwise the method wouldnt be called if successful == true
							successful |= RemoveCandidates(new List<Cell>() { c }, c.Candidates.Where(i => !nsubset.Contains(i)).ToList());
						}
					}
				}
			}
			return successful;
		}

		private bool LockedCandidatesType1()	//aka BoxLineIntersection / Pointing
		{
			bool successful = false;

			//Iterate through all boxes
			var boxes = GetListOfBoxes();
			foreach (List<Cell> box in boxes)
			{
				//Get all candidates occuring at least 1 time in the box
				List<int> selectedcandidates = Cell.FullCandidates().Where(i => box.Any(c => c.Candidates.Contains(i))).ToList();

				//Iterate through all candidates
				foreach (int n in selectedcandidates)
				{
					//Get cells containing the current candidate n
					List<Cell> selectedcells = box.Where(c => c.Candidates.Contains(n)).ToList();

					//Check whether the cells are in the same line
					int rowindex, columnindex;
					if (IsIdenticalRow(selectedcells, out rowindex))
					{
						List<Cell> row = GetCellsInRow(new Index(rowindex, 0));
						successful |= RemoveCandidates(row.Where(c => !selectedcells.Any(sc => c.IsEqual(sc))).ToList(), new List<int> { n });
					}
					else if(IsIdenticalColumn(selectedcells, out columnindex))
					{
						List<Cell> column = GetCellsInColumn(new Index(0, columnindex));
						successful |= RemoveCandidates(column.Where(c => !selectedcells.Any(sc => c.IsEqual(sc))).ToList(), new List<int> { n });
					}
				}
			}

			return successful;
		}

		private bool LockedCandidatesType2()	//aka LineBoxIntersection / Claiming
		{
			bool successful = false;

			//Iterate through all lines
			var lines = GetListOfRows().Concat(GetListOfColumns());
			foreach (List<Cell> line in lines)
			{
				//Get all candidates occuring at least 1 time in the line
				List<int> selectedcandidates = Cell.FullCandidates().Where(i => line.Any(c => c.Candidates.Contains(i))).ToList();

				//Iterate through all candidates
				foreach (int n in selectedcandidates)
				{
					//Get cells containing the current candidate n
					List<Cell> selectedcells = line.Where(c => c.Candidates.Contains(n)).ToList();

					//Check whether the cells are in the same line
					Index boxindex;
					if (IsIdenticalBox(selectedcells, out boxindex))
					{
						List<Cell> box = GetCellsInBox(boxindex);
						successful |= RemoveCandidates(box.Where(c => !selectedcells.Any(sc => c.IsEqual(sc))).ToList(), new List<int> { n });
					}
				}
			}


			return successful;
		}





		// **** BACKTRACKING****
		private bool SolveBackTracking()		//aka TrialAndError + Human
		{
			//Use human solving in the beginning
			this.SolveHumanLike();


			//Get missing numbers
			Cell[] tmp = new Cell[this.Cells.Length];
			for (int i = 0; i <= this.Cells.Length - 1; i++)
				tmp[i] = this.Cells[i / 9, i % 9]; 
			List<Cell> missing = tmp.Where(c => c.Number == 0).ToList();



			//If the sudoku is filled or unsolvable - exit
			if (missing.Count == 0)
				return true;
			
			if (missing.Count >= 81 - 17 || !this.IsValid)
				return false;



			//Try each candidate
			foreach (int n in missing[0].Candidates)
			{
				Sudoku copy = (this.Clone() as Sudoku);

				//Set candidate
				copy.SetValue(missing[0].Index, n);
				if (CellChanged != null)
					CellChanged(this, new CellChangedEventArgs(copy.GetCell(missing[0].Index), CellChangedEventArgs.CellProperty.Number));

				//If solving was successful
				if (copy.SolveBackTracking())
				{
					this.Override(copy);
					return true;
				}
			}

			//No solution found
			return false;
		}




		



		//Do what is neccessary when a new Cell number got found out
		private void NewNumberCalculated(Cell c, int value)
		{
			bool b = false;
			NewNumberCalculated(c, value, ref b);
		}

		private void NewNumberCalculated(Cell c, int value, ref bool successful) 
		{
			if (value == 0)
				return;

			this.SetValue(c.Index, value);
			successful = true;
		}



		//Returns a cluster of cells
		public List<Cell> GetCellsInRow(Index ind)
		{
			List<Cell> temp = new List<Cell>();

			for (int c = 0; c <= 8; c++)
				temp.Add(this.Cells[ind.Row, c]);

			return temp;
		}

		public List<Cell> GetCellsInColumn(Index ind)
		{
			List<Cell> temp = new List<Cell>();

			for (int r = 0; r <= 8; r++)
				temp.Add(this.Cells[r, ind.Column]);

			return temp;
		}

		public List<Cell> GetCellsInBox(Index ind)
		{
			List<Cell> temp = new List<Cell>();
			Index topleft = new Index((int)(ind.Row / 3) * 3, (int)(ind.Column / 3) * 3);

			for (int r = topleft.Row; r <= topleft.Row + 2; r++)
				for (int c = topleft.Column; c <= topleft.Column + 2; c++)
					temp.Add(this.Cells[r, c]);

			return temp;
		}

		public List<Cell> GetConnectedCells(Index ind)
		{
			return GetCellsInRow(ind).Concat(GetCellsInColumn(ind)).Concat(GetCellsInBox(ind)).
				Where(c => !c.IsEqual(this.Cells[ind.Row, ind.Column])).ToList();
		}


		//Returns a cluster of lists of cells
		private List<List<Cell>> GetListOfRows()
		{
			var temp = new List<List<Cell>>();
			for (int r = 0; r <= 8; r++)
				temp.Add(GetCellsInRow(new Index(r, 0)));	//column doesn't matter
			return temp;
		}

		private List<List<Cell>> GetListOfColumns()
		{
			var temp = new List<List<Cell>>();
			for (int c = 0; c <= 8; c++)
				temp.Add(GetCellsInColumn(new Index(0, c)));	//row doesn't matter
			return temp;
		}

		private List<List<Cell>> GetListOfBoxes()
		{
			var temp = new List<List<Cell>>();
			for (int r = 0; r <= 8; r += 3)
				for (int c = 0; c <= 8; c += 3)
					temp.Add(GetCellsInBox(new Index(r, c)));
			return temp;
		}

		private List<List<Cell>> GetAllClusters()
		{
			return GetListOfRows().Concat(GetListOfColumns()).Concat(GetListOfBoxes()).ToList();
		}


		//Returns whether a cell cluster is in the same line / box
		private bool IsIdenticalRow(List<Cell> cells, out int rowindex) 
		{
			rowindex = -1;

			if(cells.All(c => c.Index.Row == cells[0].Index.Row))
			{
				rowindex = cells[0].Index.Row;
				return true;
			}
			return false;
		}

		private bool IsIdenticalColumn(List<Cell> cells, out int columnindex) 
		{
			columnindex = -1;

			if (cells.All(c => c.Index.Column == cells[0].Index.Column))
			{
				columnindex = cells[0].Index.Column;
				return true;
			}
			return false;
		}

		private bool IsIdenticalBox(List<Cell> cells, out Index boxindex)
		{
			boxindex = new Index(-1, -1);

			if (cells.All(c => ToBoxIndex(c.Index).IsEqual(ToBoxIndex(cells[0].Index))))
			{
				boxindex = ToBoxIndex(cells[0].Index);
				return true;
			}
			return false;
		}

		private Index ToBoxIndex(Index i)
		{
			return new Index(i.Row / 3 * 3, i.Column / 3 * 3);
		}



		//Summarize all the items of the lists to one list and remove double entries
		private List<int> CombineSortDeduplicate(List<int>[] lists)
		{
			List<int> temp = new List<int>();
			foreach (List<int> list in lists)
				temp.AddRange(list);

			temp.Sort();
			return temp.Distinct().ToList();
		}

		//Removes all the given candidates out of the given Cells
		private bool RemoveCandidates(List<Cell> cells, List<int> candidates)
		{
			bool successful = false;

			foreach (Cell c in cells)
			{
				int countbefore = c.Candidates.Count;
				candidates.ForEach(candidate => c.Candidates.Remove(candidate));

				if (countbefore > c.Candidates.Count)
				{
					successful = true;
					if (CellChanged != null)
						CellChanged(this, new CellChangedEventArgs(c, CellChangedEventArgs.CellProperty.Candidates));
				}
			}

			return successful;
		}



		//Deletes the number out of the candidates of the reachable Cells
		public void DeleteCandidate(int candidate, Index i)
		{
			foreach (Cell c in GetConnectedCells(i))
				if (c.Candidates.Remove(candidate))
					if (CellChanged != null)
						CellChanged(this, new CellChangedEventArgs(c, CellChangedEventArgs.CellProperty.Candidates));
		}

		//Activates the number in the candidates of the reachable Cells
		public void AddCandidate(int candidate, Index i)
		{
			if (candidate != 0)
			{
				foreach (Cell c in GetConnectedCells(i))
				{
					if (c.Number == 0)
					{
						c.Candidates.Add(candidate);
						if (CellChanged != null)
							CellChanged(this, new CellChangedEventArgs(c, CellChangedEventArgs.CellProperty.Candidates));
					}
				}
			}
		}

		//Refreshes the candidates in the Cell
		public void RefreshCandidates(Index i)
		{
			AutoSortList<int> candidates = new AutoSortList<int>();

			//If the cell is filled, there is nothing ti refresh
			if (this.Cells[i.Row, i.Column].Number != 0)
				return;

			//Get all cells connected with this cell
			List<Cell> connectedcells = GetConnectedCells(i);

			//Enumerate through the candidates, and add if none of the connected cells contains the candidate as number
			for (int n = 1; n <= 9; n++)
				if (!connectedcells.Any(c => c.Number == n))
					candidates.Add(n);

			this.Cells[i.Row, i.Column].Candidates = candidates;
		}

	}
}
