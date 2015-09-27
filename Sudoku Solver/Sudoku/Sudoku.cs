namespace SudokuSolver.Sudoku
{

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Xml;

	public partial class Sudoku : ICloneable
	{

		//Fields
		private Cell[,] Cells { get; }
		public int MissingNumbers { get; private set; }



		//Constructors
		public Sudoku() : this(EmptyCellArray()) { }

		private Sudoku(string name) : this(EmptyCellArray(), name) { }

		private Sudoku(Cell[,] cells) : this(cells, "Sudoku - " + DateTime.Now) { }

		private Sudoku(Cell[,] cells, string name)
		{
			if ((cells.GetLength(0) != 9) || (cells.GetLength(1) != 9))
				return;

			Cells = cells;
			MissingNumbers = 0;
			foreach (var c in Cells)
			{
				if (c.Number != 0)
					DeleteCandidate(c.Number, c.Index);
				if (c.Number == 0)
					MissingNumbers++;
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
		public bool IsFilled
		{
			get
			{
				return (Cells != null);
			}
		}

		//Checks whether the Sudoku is completly filled - solved
		public bool IsSolved
		{
			get
			{
				return MissingNumbers == 0;
			}
		}

		//Checks whether the whole sudoku is valid
		public bool IsValid
		{
			get
			{
				//Compare each cell with another, and return false if they are equal and not zero
				var b1 = !GetCellsIterated().Any(c1 =>
												 GetConnectedCells(c1.Index).Any(c2 =>
																					  !c1.IsEqual(c2) && c1.Number == c2.Number && c1.Number != 0));
				//Check whether any cell contains zero candidates
				var b2 = GetCellsIterated().All(c =>
							c.Number != 0 || c.Candidates.Count != 0);

				return b1 && b2;
			}
		}

		#endregion




		#region **** Cell Methods ****

		//Get
		public Cell GetCell(Index i)
		{
			return GetCell(i.Row, i.Column);
		}

		public Cell GetCell(int i)
		{
			return GetCell(i / 9, i % 9);
		}

		public Cell GetCell(int r, int c)
		{
			if (r == -1 || c == -1)
				return Cell.Empty;

			return Cells[r, c];
		}


		//Set
		public bool SetValue(Index i, int value)
		{
			return SetValue(i.Row, i.Column, value);
		}

		public bool SetValue(int i, int value)
		{
			return SetValue(i / 9, i % 9, value);
		}

		private bool SetValue(int r, int c, int value)
		{
			//Adds the oldvalue as candidate in the connected cells 
			var oldvalue = Cells[r, c].Number;
			if (oldvalue != 0)
				AddCandidate(oldvalue, new Index(r, c));

			//Sets the new cell and delete the number as candidate in the connected cells
			if (value != 0)
			{
				Cells[r, c].Number = value;
				Cells[r, c].Candidates = Cell.EmptyCandidates();
				if (oldvalue == 0 && value != 0)
					MissingNumbers--;
				DeleteCandidate(value, new Index(r, c));
			}


			if (CellChanged != null && RaiseCellChangedEvent)
				CellChanged(this, new CellChangedEventArgs(Cells[r, c], CellChangedEventArgs.CellProperty.Number | CellChangedEventArgs.CellProperty.Candidates));

			return true;
		}


		//Set preset
		public void SetPresetValue(Index i, bool value)
		{
			SetPresetValue(i.Row, i.Column, value);
		}

		public void SetPresetValue(int i, bool value)
		{
			SetPresetValue(i / 9, i % 9, value);
		}

		private void SetPresetValue(int r, int c, bool value)
		{
			Cells[r, c].IsPreset = value;
			if (CellChanged != null && RaiseCellChangedEvent)
				CellChanged(this, new CellChangedEventArgs(Cells[r, c], CellChangedEventArgs.CellProperty.IsPreset));
		}

		//Reset
		public void ResetCell(Index i)
		{
			var oldvalue = Cells[i.Row, i.Column].Number;
			Cells[i.Row, i.Column] = new Cell(i);
			MissingNumbers++;

			AddCandidate(oldvalue, i);
			RefreshCandidates(i);

			if (CellChanged != null && RaiseCellChangedEvent)
				CellChanged(this, new CellChangedEventArgs(
					Cells[i.Row, i.Column], CellChangedEventArgs.CellProperty.Number | CellChangedEventArgs.CellProperty.IsPreset | CellChangedEventArgs.CellProperty.Candidates));
		}


		//Get All
		public IEnumerable<Cell> GetCellsIterated()
		{
			foreach (var cell in Cells)
				yield return cell;
		}

		#endregion




		//Return a copy (another instance) of the sudoku
		public object Clone()
		{
			var temp = new Sudoku();

			foreach (var c in Cells)
			{
				temp.SetValue(c.Index, c.Number);
				temp.SetPresetValue(c.Index, c.IsPreset);
			}

			temp.CellChanged += CellChanged;
			temp.SolvingCompleted += SolvingCompleted;

			return temp;
		}

		//Override this sudoku with another
		private void Override(Sudoku s)
		{
			OverrideValues(s);

			CellChanged = null;
			CellChanged += s.CellChanged;
			SolvingCompleted = null;
			SolvingCompleted += s.SolvingCompleted;
		}

		//Override this sudoku, but only its values
		public void OverrideValues(Sudoku s)
		{
			Name = s.Name;

			MissingNumbers = 81;
			foreach (var c in s.Cells)
			{
				Cells[c.Index.Row, c.Index.Column] = c;
				if (c.Number != 0)
					MissingNumbers--;
			}
		}

		//Returns a complete Cellarray with empty cells
		private static Cell[,] EmptyCellArray()
		{
			var temp = new Cell[9, 9];

			for (var i = 0; i <= temp.Length - 1; i++)
				temp[i / 9, i % 9] = new Cell(new Index(i / 9, i % 9));
			return temp;
		}



		//Converts this sudoku to a string
		public override string ToString()
		{
			var cells = "";

			if (!IsFilled)
				return Name + ": " + cells;

			for (var i = 0; i < 81; i++)
				cells += GetCell(i).Number.ToString().Replace("0", ".");

			return Name + ": " + cells;
		}

		private string CellsToString()
		{
			return string.Join("", GetCellsIterated()
									   .Select(c => c.Number == 0 ? "." : c.ToString()));
		}




		#region **** LOADING / SAVING ****

		public enum FileAccess { CreateOnly, CreateOrOverwrite, CreateOrAppend }
		public enum LoadingProcessResult { Success, InvalidFileContent }
		public enum SavingProcessResult { Success, FileAlreadyExists, UnauthorizedAccess }

		public static LoadingProcessResult LoadTxt(string path, out List<Sudoku> sudokus)
		{
			sudokus = new List<Sudoku>();


			//Start reading
			var text = File.ReadAllLines(path);
			var counter = 1;

			foreach (var line in text)
			{
				//Read
				var s = new Sudoku("Sudoku No. " + counter);
				if (!ReadLine(line, ref s))
					return LoadingProcessResult.InvalidFileContent;

				//Add if successful
				sudokus.Add(s);
				counter++;
			}

			return LoadingProcessResult.Success;
		}

		public static LoadingProcessResult LoadXml(string path, out List<Sudoku> sudokus)
		{
			sudokus = new List<Sudoku>();

			//Start reading
			try
			{
				var doc = new XmlDocument();
				doc.Load(path);

				var xmlNodeList = doc.SelectNodes("Sudokus");
				if (xmlNodeList != null)
				{
					var snodes = xmlNodeList[0].SelectNodes("Sudoku");

					if (snodes != null)
						foreach (XmlNode snode in snodes)
						{
							var xmlNode = snode.ChildNodes.Item(0);
							if (xmlNode == null)
								continue;

							var s = new Sudoku { Name = xmlNode.InnerText };

							//Read children
							var item1 = snode.ChildNodes.Item(1);
							var item2 = snode.ChildNodes.Item(2);
							if (item1 == null || item2 == null)
								return LoadingProcessResult.InvalidFileContent;

							var cells = item1.InnerText;
							var preset = item2.InnerText;

							//Adjust cells
							var validline = ReadLine(cells, ref s);
							for (var i = 0; i <= 80; i++)
								s.SetPresetValue(i, preset[i] == 'p');

							sudokus.Add(validline ? s : null);
						}
				}
			}
			catch { return LoadingProcessResult.InvalidFileContent; }

			return LoadingProcessResult.Success;
		}

		private static bool ReadLine(string text, ref Sudoku sudoku)
		{
			//Preset
			var oldsudoku = sudoku;
			sudoku = new Sudoku(sudoku.Name);

			text = text.Replace(".", "0");					//Replace, cause both dots and zeros are valid
			text = text.Replace(" ", "");					//Replace, cause spaces are allowed as seperator, but are not necessary
			text = text.Replace(Environment.NewLine, "");	//Linefeeds are allowed as seperator, but are not necessary
			text = text.Replace("\n", "");					//Linefeeds are allowed as seperator, but are not necessary

			//Length of text invalid
			if (text.Length != 81)
				return false;

			//Try to set cells
			var i = 0;
			foreach (var c in text)
			{
				//No number
				int n;
				if (!int.TryParse(c.ToString(), out n))
					return false;


				if (n != 0)
					if (!sudoku.SetValue(i, n))
						return false;

				sudoku.SetPresetValue(i, (n != 0));
				i++;
			}


			//Add eventhandlers afterwards
			sudoku.CellChanged += oldsudoku.CellChanged;
			sudoku.SolvingCompleted += oldsudoku.SolvingCompleted;

			return true;
		}


		public SavingProcessResult SaveTxt(string path, FileAccess fa)
		{

			//Do not overwrite
			var fileexists = File.Exists(path);
			var append = (fa == FileAccess.CreateOrAppend);

			if (fileexists && fa == FileAccess.CreateOnly)
				return SavingProcessResult.FileAlreadyExists;


			//Saving
			try
			{
				using (var sw = new StreamWriter(path, append))
				{
					if (append)
						sw.WriteLine();

					sw.Write(CellsToString());
				}
			}
			catch
			{ return SavingProcessResult.UnauthorizedAccess; }

			return SavingProcessResult.Success;
		}

		public SavingProcessResult SaveXml(string path, FileAccess fa)
		{

			//Do not overwrite
			var fileexists = File.Exists(path);
			var append = (fa == FileAccess.CreateOrAppend);

			if (fileexists && fa == FileAccess.CreateOnly)
				return SavingProcessResult.FileAlreadyExists;


			//Saving
			try
			{
				var doc = new XmlDocument();
				XmlElement body = null;
				if (!append || !fileexists)
				{
					//Create new header
					//Declaration
					var xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
					var root = doc.DocumentElement;
					doc.InsertBefore(xmlDeclaration, root);

					//Body
					body = doc.CreateElement(string.Empty, "Sudokus", string.Empty);
					doc.AppendChild(body);
				}
				else
				{
					//Just load the existing one
					doc.Load(path);
					var xmlNodeList = doc.SelectNodes("Sudokus");
					if (xmlNodeList != null)
						body = xmlNodeList[0] as XmlElement;
				}


				//Sudoku
				if (body != null)
					body.AppendChild(CreateSudokuElement(doc));

				doc.Save(path);
			}
			catch
			{ return SavingProcessResult.UnauthorizedAccess; }

			return SavingProcessResult.Success;
		}

		private XmlElement CreateSudokuElement(XmlDocument doc)
		{
			//New sudoku
			var parent = doc.CreateElement(string.Empty, "Sudoku", string.Empty);
			var date = doc.CreateAttribute("Date");
			date.Value = DateTime.Now.ToString();

			//Name
			var name = doc.CreateElement(string.Empty, "Name", string.Empty);
			name.AppendChild(doc.CreateTextNode(Name));

			//Cells
			var cells = doc.CreateElement(string.Empty, "Cells", string.Empty);
			cells.AppendChild(doc.CreateTextNode(CellsToString()));

			//Preset
			var presets = doc.CreateElement(string.Empty, "Preset", string.Empty);
			presets.AppendChild(doc.CreateTextNode(string.Join("", GetCellsIterated().ToList().Select(c => c.IsPreset ? "p" : "."))));

			parent.SetAttributeNode(date);
			parent.AppendChild(name);
			parent.AppendChild(cells);
			parent.AppendChild(presets);

			return parent;
		}

		#endregion


		//Get raised whenever a Cell changed
		public event EventHandler<CellChangedEventArgs> CellChanged;

	}
}
