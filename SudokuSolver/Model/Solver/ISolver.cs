using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SudokuSolver.Model.Data;

namespace SudokuSolver.Model.Solver
{
    internal interface ISolver
    {
        public bool Solve(Sudoku s);
    }
}
