using SudokuSolver.Model.Data;

namespace SudokuSolver.Model.Solver
{
    internal class LogicSolver : ISolver
    {
        public virtual bool Solve(Sudoku s) 
        {
            //Solve
            while (s.MissingNumbers != 0) {
                bool successful = false;

                //Use the solving methods
                successful |= NakedSingle(s);
                successful |= HiddenSingle(s);
                Enumerable.Range(2, 3).ToList().ForEach(i => successful |= NakedSubset(s, i));
                Enumerable.Range(2, 3).ToList().ForEach(i => successful |= HiddenSubset(s, i));
                successful |= LockedCandidatesType1(s);
                successful |= LockedCandidatesType2(s);


                //If this round no number got calculated - failed
                if (!successful) {
                    return false;
                }
            }

            return true;
        }


        #region ** Single solving methods **

        private bool NakedSingle(Sudoku s)
        {
            bool successful = false;

            foreach (var c in s.GetCellsIterated()) {
                //If the Cell is empty and has only one possible candidate --> set
                if (c.Number == 0 && c.Candidates.Count == 1) {
                    s.NewNumberCalculated(c, c.Candidates[0], ref successful);
                }
            }

            return successful;
        }

        private bool HiddenSingle(Sudoku s)
        {
            bool successful = false;
            var clusters = GetAllClusters(s);

            //Iterate through the clusters
            foreach (var cluster in clusters) {
                //Iterate all candidates
                for (int n = 1; n <= 9; n++) {
                    //If this candidate is only available in one cell of this cluster - set
                    var candidatecells = cluster.Where(c => c.Number == 0 && c.Candidates.Contains(n)).ToList();

                    if (candidatecells.Count == 1) {
                        s.NewNumberCalculated(candidatecells[0], n, ref successful);
                    }
                }
            }

            return successful;
        }

        private bool NakedSubset(Sudoku s, int n)
        {
            bool successful = false;

            //All rows, columns and boxes
            var clusters = GetAllClusters(s);

            //Iterate through the clusters
            foreach (var cluster in clusters) {
                //List of cells with a amount of candidates lower than or equal to n
                var selected = cluster.Where(c => c.Candidates.Count <= n && c.Candidates.Count != 0).Select(c => c).ToList();

                //Only if there are enough tuples
                if (selected.Count < n) {
                    continue;
                }

                //Iterate through all possible ntuples in selected (i.e. if n == 3 but selected contains 5 cells,
                //all possible combinations of length n in selected will be iterated
                foreach (var iesubset in selected.Combinations(n)) {
                    var nsubset = iesubset.ToList();

                    //List of all candidates appearing in the Cells of the list 
                    var candidatelists = new List<List<int>>();
                    nsubset.ForEach(c => candidatelists.Add(c.Candidates.ToList()));
                    var combinedcandidates = candidatelists.CombineSortDeduplicate().ToList();


                    //If the amount of the total candidates is equal to n - delete them from other cells
                    if (combinedcandidates.Count != n) {
                        continue;
                    }

                    //A list containing all Cells of cluster except the cells in selected
                    var shortedcluster = cluster.Where(c => !nsubset.Any(c.IsEqual) && c.Number == 0).Select(c => c).ToList();
                    //No ||, because otherwise the method wouldnt be called if successful == true
                    successful |= RemoveCandidates(s, shortedcluster, combinedcandidates.ToArray());
                }
            }

            return successful;
        }

        private bool HiddenSubset(Sudoku s, int n)
        {
            bool successful = false;

            //Iterate through all clusters
            var clusters = GetAllClusters(s);

            foreach (var cluster in clusters) {
                //Get all candidates occuring max n times (and min 1 time) in the cluster
                var selectedcandidates = Cell.FullCandidates().Where(i => cluster.Count(c => c.Candidates.Contains(i)) <= n && cluster.Any(c => c.Candidates.Contains(i))).ToList();


                //Iterate thorugh all subsets of size n
                foreach (var iesubset in selectedcandidates.Combinations(n)) {
                    var nsubset = iesubset.ToList();

                    //Get a list of all cells the candidates of nsubset are in
                    var selectedcells = cluster.Where(c => nsubset.Any(i => c.Candidates.Contains(i))).ToList();

                    //If the list of cells of both candidates are equal, delete the other candidates from the cells
                    if (selectedcells.Count != n) {
                        continue;
                    }

                    successful = selectedcells.Aggregate(successful, (current, c) => current | s.RemoveCandidates(c, c.Candidates.Where(i => !nsubset.Contains(i)).ToArray()));
                }
            }

            return successful;
        }

        private bool LockedCandidatesType1(Sudoku s) //aka BoxLineIntersection / Pointing
        {
            bool successful = false;

            //Iterate through all boxes
            var boxes = s.GetListOfBoxes();


            foreach (var box in boxes) {
                //Get all candidates occuring at least 1 time in the box
                var selectedcandidates = Cell.FullCandidates().Where(i => box.Any(c => c.Candidates.Contains(i))).ToList();

                //Iterate through all candidates
                foreach (int n in selectedcandidates) {
                    //Get cells containing the current candidate n
                    var selectedcells = box.Where(c => c.Candidates.Contains(n)).ToList();

                    //Check whether the cells are in the same line
                    if (IsIdenticalRow(selectedcells, out int rowindex)) {
                        var row = s.GetCellsInRow(new GridIndex(rowindex, 0));
                        successful |= RemoveCandidates(s, row.Where(c => !selectedcells.Any(c.IsEqual)), n);
                    }
                    else if (IsIdenticalColumn(selectedcells, out int columnindex)) {
                        var column = s.GetCellsInColumn(new GridIndex(0, columnindex));
                        successful |= RemoveCandidates(s, column.Where(c => !selectedcells.Any(c.IsEqual)), n);
                    }
                }
            }

            return successful;
        }

        private bool LockedCandidatesType2(Sudoku s) //aka LineBoxIntersection / Claiming
        {
            bool successful = false;

            //Iterate through all lines
            var lines = s.GetListOfRows().Concat(s.GetListOfColumns());


            foreach (var line in lines) {
                //Get all candidates occuring at least 1 time in the line
                var selectedcandidates = Cell.FullCandidates().Where(i => line.Any(c => c.Candidates.Contains(i))).ToList();

                //Iterate through all candidates
                foreach (int n in selectedcandidates) {
                    //Get cells containing the current candidate n
                    var selectedcells = line.Where(c => c.Candidates.Contains(n)).ToList();

                    //Check whether the cells are in the same line
                    if (!IsIdenticalBox(selectedcells, out var boxindex)) {
                        continue;
                    }

                    var box = s.GetCellsInBox(boxindex);
                    successful |= RemoveCandidates(s, box.Where(c => !selectedcells.Any(c.IsEqual)).ToList(), n);
                }
            }


            return successful;
        }

        #endregion


        #region Helper

        public bool RemoveCandidates(Sudoku s, IEnumerable<Cell> cells, params int[] candidates) 
            => cells.Aggregate(false, (current, c) => current | s.RemoveCandidates(c, candidates));


        //Returns whether a cell cluster is in the same line / box
        public static bool IsIdenticalRow(IList<Cell> cells, out int rowindex)
        {
            rowindex = -1;

            if (cells.Any(c => c.Index.Row != cells[0].Index.Row)) {
                return false;
            }

            rowindex = cells[0].Index.Row;
            return true;
        }

        public static bool IsIdenticalColumn(IList<Cell> cells, out int columnindex)
        {
            columnindex = -1;

            if (cells.Any(c => c.Index.Column != cells[0].Index.Column)) {
                return false;
            }

            columnindex = cells[0].Index.Column;
            return true;
        }

        public static bool IsIdenticalBox(IList<Cell> cells, out GridIndex boxindex)
        {
            boxindex = new GridIndex(-1, -1);

            if (!cells.All(c => Sudoku.ToBoxIndex(c.Index).IsEqual(Sudoku.ToBoxIndex(cells[0].Index)))) {
                return false;
            }

            boxindex = Sudoku.ToBoxIndex(cells[0].Index);
            return true;
        }


        public IEnumerable<List<Cell>> GetAllClusters(Sudoku s) 
            => s.GetListOfRows().Concat(s.GetListOfColumns()).Concat(s.GetListOfBoxes()).ToList();


        #endregion

    }
}
