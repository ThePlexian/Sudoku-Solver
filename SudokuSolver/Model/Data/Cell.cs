namespace SudokuSolver.Model.Data
{

    public class Cell
    {

        //Constructors
        public Cell(GridIndex i)
        {
            Number = 0;
            Candidates = FullCandidates();
            Index = i;
            IsPreset = false;
        }


        //Properties
        public int Number { get; set; }

        public AutoSortList<int> Candidates { get; set; }

        public bool IsPreset { get; set; }

        public GridIndex Index { get; }




        //Methods
        public static AutoSortList<int> EmptyCandidates() => new();

        public static AutoSortList<int> FullCandidates() => new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public override string ToString() => Number != 0 ? Number.ToString() : "";

        public static Cell Empty => new(new GridIndex(-1, -1));




        //Compare
        public bool IsEqual(Cell c) => Number == c.Number && HaveSameCandidates(this, c) && IsPreset == c.IsPreset && Index.IsEqual(c.Index);

        private static bool HaveSameCandidates(Cell c1, Cell c2)
        {
            if (c1.Candidates.Count != c2.Candidates.Count) {
                return false;
            }

            for (int i = 0; i <= c1.Candidates.Count - 1; i++) {
                if (c1.Candidates.ElementAt(i) != c2.Candidates.ElementAt(i)) {
                    return false;
                }
            }

            return true;
        }
    }
}
