namespace SudokuSolver.Model.Data
{
    public class AutoSortList<T> : List<T>
    {
        public new void Add(T item)
        {
            base.Add(item);
            Sort();
        }
    }
}
