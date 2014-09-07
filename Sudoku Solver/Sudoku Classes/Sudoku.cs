using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SudokuSolver
{
	public partial class Sudoku : ICloneable
	{

		//Fields
		private Cell[,] Cells { get; set; }
		public int MissingNumbers { get; private set; }

		//Initialize
		public Sudoku() : this(Sudoku.EmptyCellArray())
		{
		}
		public Sudoku(Cell[,] cells)
		{
			if ((cells.GetLength(0) == 9) && (cells.GetLength(1) == 9))
			{
				this.Cells = cells;
				this.MissingNumbers = 0;
				foreach (Cell c in this.Cells)
				{
					if (c.Number != 0)
						DeleteCandidate(c.Number, c.Index);
					if (c.Number == 0)
						this.MissingNumbers++;
				}
			}
				
		}



		//**** Cell Methods ****
		public Cell GetCell(Index i)
		{
			return GetCell(i.Row, i.Column);
		}
		public Cell GetCell(int r, int c)
		{
			if (r == -1 || c == -1)
				return Cell.Empty();

			return this.Cells[r, c];
		}

		public bool SetValue(Index i, int value)
		{
			return SetValue(i.Row, i.Column, value);
		}
		public bool SetValue(int r, int c, int value)
		{
			if (!IsValid(value, new Index(r, c)) || this.Cells[r, c].Number == value)
				return false;

			//Adds the oldvalue as candidate in the connected cells 
			int oldvalue = this.Cells[r, c].Number;
			if (oldvalue != 0)
				AddCandidate(oldvalue, new Index(r, c));

			//Sets the new cell and delete the number as candidate in the connected cells
			this.Cells[r, c].Number = value;
			this.Cells[r, c].Candidates = Cell.EmptyCandidates();
			if (oldvalue == 0)
				this.MissingNumbers--;
			DeleteCandidate(value, new Index(r, c));

			if (CellChanged != null)
				CellChanged(this, new CellChangedEventArgs(this.Cells[r, c], CellChangedEventArgs.CellProperty.Number | CellChangedEventArgs.CellProperty.Candidates));
			return true;
		}

		public void SetPresetValue(Index i, bool value)
		{
			SetPresetValue(i.Row, i.Column, value);
		}
		public void SetPresetValue(int r, int c, bool value)
		{
			this.Cells[r, c].IsPreset = value;
			if (CellChanged != null)
				CellChanged(this, new CellChangedEventArgs(this.Cells[r, c], CellChangedEventArgs.CellProperty.IsPreset));
		}

		public void ResetCell(int r, int c)
		{
			ResetCell(new Index(r, c));
		}
		public void ResetCell(Index i)
		{
			int oldvalue = this.Cells[i.Row, i.Column].Number;
			this.Cells[i.Row, i.Column] = new Cell(i);
			this.MissingNumbers++;
			AddCandidate(oldvalue, i);
			RefreshCandidates(i);
			if (CellChanged != null)
				CellChanged(this, new CellChangedEventArgs(
					this.Cells[i.Row, i.Column], CellChangedEventArgs.CellProperty.Number | CellChangedEventArgs.CellProperty.IsPreset | CellChangedEventArgs.CellProperty.Candidates));
		}

		public Cell[,] GetAllCells()
		{
			return this.Cells;
		}



		//Return a copy (another instance) of the sudoku
		public object Clone()
		{
			Sudoku temp = new Sudoku();

			foreach (Cell c in this.Cells)
			{
				temp.SetValue(c.Index, c.Number);
				temp.SetPresetValue(c.Index, c.IsPreset);
			}

			temp.CellChanged += this.CellChanged;
			temp.SolvingCompleted += this.SolvingCompleted;

			return temp;
		}

		//Override this sudoku with another
		public void Override(Sudoku s)
		{
			foreach (Cell c in s.Cells)
			{
				this.Cells[c.Index.Row, c.Index.Column] = c;
			}

			this.CellChanged = null;
			this.CellChanged += s.CellChanged;
			this.SolvingCompleted = null;
			this.SolvingCompleted += s.SolvingCompleted;
		}

		//Returns a complete Cellarray with empty cells
		public static Cell[,] EmptyCellArray()
		{
			Cell[,] temp = new Cell[9, 9];

			for (int i = 0; i <= temp.Length - 1; i++)
				temp[i / 9, i % 9] = new Cell(new Index(i / 9, i % 9));
			return temp;
		}

		//Checks whether the Sudoku contains Cells
		public bool IsFilled()
		{
			return (this.Cells != null);
		}

		//Checks whether the value at the given index is valid for the sudoku
		public bool IsValid(int value, Index i)
		{
			//Check connected cells
			foreach (Cell c in GetConnectedCells(i))
				if (c.Number == value)
					return false;

			//Valid
			return true;
		}


		
		//Converts this sudoku to a string
		public override string ToString()
		{
			string s = "";

			if (this.IsFilled())
				for (int i = 0; i < 81; i++)
					s += this.Cells[i / 9, i % 9].Number.ToString().Replace("0", ".");

			return s;
		}



		// **** LOADING / SAVING ****
		public static bool Load(string path, ref Sudoku sudoku)
		{
			//Preset
			Sudoku oldsudoku = sudoku;
			sudoku = new Sudoku();
			sudoku.CellChanged += oldsudoku.CellChanged;
			sudoku.SolvingCompleted += oldsudoku.SolvingCompleted;

			//File doesn't exist
			if (!File.Exists(path))
				return false;

			//Start reading
			string text;
			using (StreamReader sr = new StreamReader(path))
				text = sr.ReadToEnd();
	
			//Read in
			return Sudoku.Read(text, ref sudoku);
		}

		public static bool Read(string text, ref Sudoku sudoku)
		{
			//Preset
			Sudoku oldsudoku = sudoku;
			sudoku = new Sudoku();
			sudoku.CellChanged += oldsudoku.CellChanged;
			sudoku.SolvingCompleted += oldsudoku.SolvingCompleted;

			text = text.Replace(".", "0");					//Replace, cause both dots and zeros are valid
			text = text.Replace(" ", "");					//Replace, cause spaces are allowed as seperator, but are not necessary
			text = text.Replace(Environment.NewLine, "");	//Linefeeds are allowed as seperator, but are not necessary
			text = text.Replace("\n", "");					//Linefeeds are allowed as seperator, but are not necessary

			//Length of text invalid
			if (text.Length != 81)
				return false;

			//Try to set cells
			int i = 0;
			foreach (char c in text.ToCharArray())
			{
				//No number
				int n = 0;
				if (!int.TryParse(c.ToString(), out n))
					return false;


				if (n != 0)
					if (!sudoku.SetValue(new Index(i / 9, i % 9), n))
						return false;

				sudoku.SetPresetValue(new Index(i / 9, i % 9), (n != 0));
				i++;
			}

			return true;
		}

		public bool Save(string path, bool overwrite)
		{

			//Do not overwrite
			if (File.Exists(path) && !overwrite)
				return false;


			//Saving
			try
			{
				using (StreamWriter sw = new StreamWriter(path))
				{
					for (int r = 0; r <= 8; r++)
					{
						for (int c = 0; c <= 8; c++)
						{
							sw.Write((this.Cells[r, c].Number == 0 ? "." : this.Cells[r, c].Number.ToString()));
						}
					}
						
				}
			}
			catch
			{ return false; }

			return true;
		}




		//Get raised whenever a Cell changed
		public delegate void CellChangedEventHandler(object sender, CellChangedEventArgs e);
		public event CellChangedEventHandler CellChanged;

		public class CellChangedEventArgs : EventArgs
		{
			public CellChangedEventArgs(Cell c, CellProperty cp)
			{
				this.Cell = c;
				this.ChangedProperty = cp;
			}

			public Cell Cell { get; set; }
			public CellProperty ChangedProperty { get; set; }

			[Flags]
			public enum CellProperty { Number = 1, Candidates = 2, IsPreset = 4 }
		}
	}
}
