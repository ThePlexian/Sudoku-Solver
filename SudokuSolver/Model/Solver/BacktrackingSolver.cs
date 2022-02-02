using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SudokuSolver.Model.Data;

namespace SudokuSolver.Model.Solver
{
    internal class BacktrackingSolver : LogicSolver
    {
        public override bool Solve(Sudoku s)
        {
            base.Solve(s);

            //Get missing numbers
            var tmp = new Cell[81];
            for (int i = 0; i < 81; i++) {
                tmp[i] = s.GetCell(i);
            }

            var missing = tmp.Where(c => c.Number == 0).ToList();


            //If the sudoku is filled or unsolvable - exit
            if (missing.Count == 0) {
                return true;
            }

            if (missing.Count >= 81 - 17 || !s.IsValid) {
                return false;
            }


            //Try each candidate
            foreach (int n in missing[0].Candidates) {
                var copy = (Sudoku)s.Clone();

                //Set candidate
                copy.SetValue(missing[0].Index, n);
                //if (s.CellChanged != null && s.RaiseCellChangedEvent) {
                //    s.CellChanged(this, new CellChangedEventArgs(copy.GetCell(missing[0].Index), CellChangedEventArgs.CellProperty.Number));
                //}

                //If solving wasn't successful
                if (!Solve(copy)) {
                    continue;
                }

                s.Override(copy);
                return true;
            }

            //No solution found
            return false;
        }
    }
}
