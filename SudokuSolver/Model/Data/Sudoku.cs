using System.Xml;

namespace SudokuSolver.Model.Data
{
    public class Sudoku : ICloneable
    {

        //Fields
        private Cell[,] Cells { get; }
        public int MissingNumbers { get; private set; }



        //Constructors
        public Sudoku() : this(EmptyCellArray()) { }

        public Sudoku(string name) : this(EmptyCellArray(), name) { }

        private Sudoku(Cell[,] cells) : this(cells, "Sudoku - " + DateTime.Now) { }

        private Sudoku(Cell[,] cells, string name)
        {
            if ((cells.GetLength(0) != 9) || (cells.GetLength(1) != 9)) {
                return;
            }

            Cells = cells;
            MissingNumbers = 0;
            foreach (var c in Cells) {
                if (c.Number != 0) {
                    DeleteCandidate(c.Number, c.Index);
                }

                if (c.Number == 0) {
                    MissingNumbers++;
                }
            }

            Name = name;
            RaiseCellChangedEvent = true;
        }




        #region **** Properties ****

        //The identifier
        public string Name { get; set; }

        //Defines whether the CellChanged Event will be raised
        public bool RaiseCellChangedEvent { get; set; }

        //Checks whether the Sudoku contains Cells
        public bool IsFilled => Cells != null;

        //Checks whether the Sudoku is completly filled - solved
        public bool IsSolved => MissingNumbers == 0;

        //Checks whether the whole sudoku is valid
        public bool IsValid {
            get {
                //Compare each cell with another, and return false if they are equal and not zero
                bool b1 = !GetCellsIterated().Any(c1 =>
                                                 GetConnectedCells(c1.Index).Any(c2 =>
                                                                                      !c1.IsEqual(c2) && c1.Number == c2.Number && c1.Number != 0));
                //Check whether any cell contains zero candidates
                bool b2 = GetCellsIterated().All(c =>
                            c.Number != 0 || c.Candidates.Count != 0);

                return b1 && b2;
            }
        }

        #endregion




        #region **** Cell Methods ****

        //Get
        public Cell GetCell(GridIndex i) => GetCell(i.Row, i.Column);

        public Cell GetCell(int i) => GetCell(i / 9, i % 9);

        public Cell GetCell(int r, int c)
        {
            if (r == -1 || c == -1) {
                return Cell.Empty;
            }

            return Cells[r, c];
        }


        //Set
        public bool SetValue(GridIndex i, int value) => SetValue(i.Row, i.Column, value);

        public bool SetValue(int i, int value) => SetValue(i / 9, i % 9, value);

        private bool SetValue(int r, int c, int value)
        {
            //Adds the oldvalue as candidate in the connected cells 
            int oldvalue = Cells[r, c].Number;
            if (oldvalue != 0) {
                AddCandidate(oldvalue, new GridIndex(r, c));
            }

            //Sets the new cell and delete the number as candidate in the connected cells
            if (value != 0) {
                Cells[r, c].Number = value;
                Cells[r, c].Candidates = Cell.EmptyCandidates();
                if (oldvalue == 0 && value != 0) {
                    MissingNumbers--;
                }

                DeleteCandidate(value, new GridIndex(r, c));
            }


            if (CellChanged != null && RaiseCellChangedEvent) {
                CellChanged(this, new CellChangedEventArgs(Cells[r, c], CellChangedEventArgs.CellProperty.Number | CellChangedEventArgs.CellProperty.Candidates));
            }

            return true;
        }


        //Set preset
        public void SetPresetValue(GridIndex i, bool value) => SetPresetValue(i.Row, i.Column, value);

        public void SetPresetValue(int i, bool value) => SetPresetValue(i / 9, i % 9, value);

        private void SetPresetValue(int r, int c, bool value)
        {
            Cells[r, c].IsPreset = value;
            if (CellChanged != null && RaiseCellChangedEvent) {
                CellChanged(this, new CellChangedEventArgs(Cells[r, c], CellChangedEventArgs.CellProperty.IsPreset));
            }
        }

        //Reset
        public void ResetCell(GridIndex i)
        {
            int oldvalue = Cells[i.Row, i.Column].Number;
            Cells[i.Row, i.Column] = new Cell(i);
            MissingNumbers++;

            AddCandidate(oldvalue, i);
            RefreshCandidates(i);

            if (CellChanged != null && RaiseCellChangedEvent) {
                CellChanged(this, new CellChangedEventArgs(
                    Cells[i.Row, i.Column], CellChangedEventArgs.CellProperty.Number | CellChangedEventArgs.CellProperty.IsPreset | CellChangedEventArgs.CellProperty.Candidates));
            }
        }


        //Get All
        public IEnumerable<Cell> GetCellsIterated()
        {
            foreach (var cell in Cells) {
                yield return cell;
            }
        }

        #endregion




        //Return a copy (another instance) of the sudoku
        public object Clone()
        {
            var temp = new Sudoku();

            foreach (var c in Cells) {
                temp.SetValue(c.Index, c.Number);
                temp.SetPresetValue(c.Index, c.IsPreset);
            }

            temp.CellChanged += CellChanged;
            return temp;
        }

        //Override this sudoku with another
        public void Override(Sudoku s)
        {
            OverrideValues(s);

            CellChanged = null;
            CellChanged += s.CellChanged;
        }

        //Override this sudoku, but only its values
        public void OverrideValues(Sudoku s)
        {
            Name = s.Name;

            MissingNumbers = 81;
            foreach (var c in s.Cells) {
                Cells[c.Index.Row, c.Index.Column] = c;
                if (c.Number != 0) {
                    MissingNumbers--;
                }
            }
        }

        //Returns a complete Cellarray with empty cells
        private static Cell[,] EmptyCellArray()
        {
            var temp = new Cell[9, 9];

            for (int i = 0; i <= temp.Length - 1; i++) {
                temp[i / 9, i % 9] = new Cell(new GridIndex(i / 9, i % 9));
            }

            return temp;
        }


        

        #region ** Get certain cell clusters **

        //Returns a cluster of cells
        public List<Cell> GetCellsInRow(GridIndex ind)
        {
            var temp = new List<Cell>();

            for (int c = 0; c <= 8; c++) {
                temp.Add(Cells[ind.Row, c]);
            }

            return temp;
        }

        public List<Cell> GetCellsInColumn(GridIndex ind)
        {
            var temp = new List<Cell>();

            for (int r = 0; r <= 8; r++) {
                temp.Add(Cells[r, ind.Column]);
            }

            return temp;
        }

        public List<Cell> GetCellsInBox(GridIndex ind)
        {
            var temp = new List<Cell>();
            var topleft = new GridIndex(ind.Row / 3 * 3, ind.Column / 3 * 3);

            for (int r = topleft.Row; r <= topleft.Row + 2; r++) {
                for (int c = topleft.Column; c <= topleft.Column + 2; c++) {
                    temp.Add(Cells[r, c]);
                }
            }

            return temp;
        }

        public List<Cell> GetConnectedCells(GridIndex ind) => GetCellsInRow(ind).Concat(GetCellsInColumn(ind)).Concat(GetCellsInBox(ind)).Where(c => !c.IsEqual(Cells[ind.Row, ind.Column])).ToList();


        //Returns a cluster of lists of cells
        public IEnumerable<List<Cell>> GetListOfRows()
        {
            var temp = new List<List<Cell>>();
            for (int r = 0; r <= 8; r++) {
                temp.Add(GetCellsInRow(new GridIndex(r, 0))); //column doesn't matter
            }

            return temp;
        }

        public IEnumerable<List<Cell>> GetListOfColumns()
        {
            var temp = new List<List<Cell>>();
            for (int c = 0; c <= 8; c++) {
                temp.Add(GetCellsInColumn(new GridIndex(0, c))); //row doesn't matter
            }

            return temp;
        }

        public IEnumerable<List<Cell>> GetListOfBoxes()
        {
            var temp = new List<List<Cell>>();
            for (int r = 0; r <= 8; r += 3) {
                for (int c = 0; c <= 8; c += 3) {
                    temp.Add(GetCellsInBox(new GridIndex(r, c)));
                }
            }

            return temp;
        }

        #endregion


        #region ** Compare cell positions **



        public static GridIndex ToBoxIndex(GridIndex i) => new(i.Row / 3 * 3, i.Column / 3 * 3);

        #endregion


        #region ** Update Candidates **

        //Removes all the given candidates out of the given Cells
        public bool RemoveCandidates(Cell c, params int[] candidates)
        {
            int countbefore = c.Candidates.Count;
            candidates.ToList().ForEach(candidate => c.Candidates.Remove(candidate));

            if (countbefore <= c.Candidates.Count) {
                return false;
            }

            if (CellChanged != null && RaiseCellChangedEvent) {
                CellChanged(this, new CellChangedEventArgs(c, CellChangedEventArgs.CellProperty.Candidates));
            }

            return true;
        }



        //Deletes the number out of the candidates of the reachable Cells
        public void DeleteCandidate(int candidate, GridIndex i)
        {
            foreach (var c in GetConnectedCells(i)) {
                if (c.Candidates.Remove(candidate)) {
                    if (CellChanged != null && RaiseCellChangedEvent) {
                        CellChanged(this, new CellChangedEventArgs(c, CellChangedEventArgs.CellProperty.Candidates));
                    }
                }
            }
        }

        //Activates the number in the candidates of the reachable Cells
        public void AddCandidate(int candidate, GridIndex i)
        {
            if (candidate == 0) {
                return;
            }

            foreach (var c in GetConnectedCells(i).Where(c => c.Number == 0)) {
                c.Candidates.Add(candidate);
                if (CellChanged != null && RaiseCellChangedEvent) {
                    CellChanged(this, new CellChangedEventArgs(c, CellChangedEventArgs.CellProperty.Candidates));
                }
            }
        }

        //Refreshes the candidates in the Cell
        public void RefreshCandidates(GridIndex i)
        {
            var candidates = new AutoSortList<int>();

            //If the cell is filled, there is nothing ti refresh
            if (Cells[i.Row, i.Column].Number != 0) {
                return;
            }

            //Get all cells connected with this cell
            var connectedcells = GetConnectedCells(i);

            //Enumerate through the candidates, and add if none of the connected cells contains the candidate as number
            for (int n = 1; n <= 9; n++) {
                if (connectedcells.All(c => c.Number != n)) {
                    candidates.Add(n);
                }
            }

            Cells[i.Row, i.Column].Candidates = candidates;
        }

        #endregion





        //Do what is neccessary when a new Cell number got found out
        public void NewNumberCalculated(Cell c, int value, ref bool successful)
        {
            if (value == 0) {
                return;
            }

            SetValue(c.Index, value);
            successful = true;
        }

        //Get raised whenever a Cell changed
        public event EventHandler<CellChangedEventArgs> CellChanged;

    }
}
