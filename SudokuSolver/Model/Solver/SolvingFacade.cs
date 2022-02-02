using SudokuSolver.Model.Data;

namespace SudokuSolver.Model.Solver
{
    public static class SolvingFacade
    {
        public static bool Solve(Sudoku s, SolvingTechnique st)
        {
            ISolver solver = st switch {
                SolvingTechnique.HumanSolvingTechnique => new LogicSolver(),
                SolvingTechnique.BackTracking => new BacktrackingSolver(),
                _ => throw new ArgumentException("Unknown solving technique", nameof(st)),
            };

            bool success = solver.Solve(s);
            SolvingCompleted?.Invoke(null, EventArgs.Empty);

            return success;
        }

        public enum SolvingTechnique
        {
            HumanSolvingTechnique,
            BackTracking
        }

        public static event EventHandler SolvingCompleted;
    }
}
