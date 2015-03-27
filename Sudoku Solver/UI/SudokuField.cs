using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace SudokuSolver.UI
{
	using System.Linq;
	using SudokuSolver.Sudoku;

	public class SudokuField : Control
	{

		//Constructor
		public SudokuField()
		{
			this.Size = new Size(400, 400);
			this.GridWidth = 1;
			this.GridInnerBorderWidth = 3;
			this.GridBorderWidth = 5;
			this.GridColor = Color.Orange;
			this.HoveredCellColor = Color.FromArgb(127, 200, 200, 200);
			this.SelectedCellColor = Color.FromArgb(255, 255, 120, 80);
			this.PresetCellBackColor = Color.FromArgb(127, 200, 200, 200);
			this.PresetCellForeColor = Color.FromArgb(255, 0, 0, 0);
			this.NonPresetCellForeColor = Color.FromArgb(255, 80, 80, 80);

			this.HoverActivated = true;
			this.ShowCandidates = true;
			this.EditingEnabled = true;

			SelectedCell = Cell.Empty;
			HoveredCell = Cell.Empty;

			this.Sudoku = new Sudoku();
		}



		#region **** Properties / Fields ****

		private Sudoku _sudoku;

		[Category("Custom")]
		public Sudoku Sudoku
		{
			get { return _sudoku; }
			set
			{
				if (value == null)
					return;

				var oldsudoku = _sudoku;
				_sudoku = value;

				//Invalidate
				if (oldsudoku == null || this._sudoku == null)
					return;

				foreach (var c in this._sudoku.GetCellsIterated().Where(c => !c.IsEqual(oldsudoku.GetCell(c.Index))))
					this.Invalidate(GetRectangle(c));

				this.ConditionalInvoke(this.Update);
			}
		}


		private int _gridwidth, _gridborderwidth, _gridinnerborderwidth;

		[Category("Custom")]
		public int GridWidth
		{
			get { return _gridwidth; }
			set
			{
				_gridwidth = value;
				this.Invalidate();
				CheckSize();
			}
		}
		[Category("Custom")]
		public int GridInnerBorderWidth
		{
			get { return _gridinnerborderwidth; }
			set
			{
				_gridinnerborderwidth = value;
				this.Invalidate();
				CheckSize();
			}
		}
		[Category("Custom")]
		public int GridBorderWidth
		{
			get { return _gridborderwidth; }
			set
			{
				_gridborderwidth = value;
				this.Invalidate();
				CheckSize();
			}
		}
		[Category("Custom")]
		public Color GridColor { get; set; }

		[Category("Custom")]
		public Color HoveredCellColor { get; set; }
		[Category("Custom")]
		public Color SelectedCellColor { get; set; }
		[Category("Custom")]
		public Color PresetCellBackColor { get; set; }

		[Category("Custom")]
		public Color PresetCellForeColor { get; set; }
		[Category("Custom")]
		public Color NonPresetCellForeColor { get; set; }


		private bool _editingenabled;
		private Cell _lastselectedcell;

		[Category("Custom")]
		public bool HoverActivated { get; set; }

		[Category("Custom")]
		public bool ShowCandidates { get; set; }

		[Category("Custom")]
		public bool EditingEnabled
		{
			get { return _editingenabled; }
			set
			{
				_editingenabled = value;
				if (_editingenabled)
				{
					SelectedCell = (_lastselectedcell ?? Cell.Empty);
				}
				else
				{
					_lastselectedcell = SelectedCell;
					SelectedCell = Cell.Empty;
				}
			}
		}

		[Category("Custom")]
		public bool IsAlwaysFocused { get; set; }

		#endregion



		//Painting
		[System.Diagnostics.DebuggerStepThrough]
		protected override void OnPaint(PaintEventArgs e)
		{
			//Set quality flags
			e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
			e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			e.Graphics.SmoothingMode = SmoothingMode.Default;
			e.Graphics.InterpolationMode = InterpolationMode.Default;
			e.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

			//Drawing the grid
			var curr = GetGridWidth(0) / 2;
			var onecell = GetSizeOfOneCell().Width;

			for (var s = 0; s <= 9; s++)
			{
				e.Graphics.DrawLine(new Pen(GridColor, GetGridWidth(s)), new Point(0, curr), new Point(this.Width, curr));
				e.Graphics.DrawLine(new Pen(GridColor, GetGridWidth(s)), new Point(curr, 0), new Point(curr, this.Height));

				curr += (int)Math.Ceiling((double)GetGridWidth(s) / 2) + onecell + (int)Math.Floor((double)GetGridWidth(s + 1) / 2);
			}


			//Drawing the clicked Cell
			if (SelectedCell != null)
			{
				var r = GetRectangle(SelectedCell);
				r.Inflate(-1, -1);
				e.Graphics.FillRectangle(new SolidBrush(this.SelectedCellColor), r);
			}


			//Drawing the hovered Cell
			if (this.HoverActivated && HoveredCell != null)
			{
				var r = GetRectangle(HoveredCell);
				r.Inflate(-1, -1);
				e.Graphics.FillRectangle(new SolidBrush(this.HoveredCellColor), r);
			}




			//Drawing the numbers
			if (Sudoku != null && Sudoku.IsFilled)
			{
				var rects = GetListOfRectangles();
				var i = 0;
				var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

				Brush presetcellbackbrush = new SolidBrush(this.PresetCellBackColor);
				Brush presetcellforebrush = new SolidBrush(this.PresetCellForeColor);
				Brush nonpresetbrush = new SolidBrush(this.NonPresetCellForeColor);
				Brush surroundingbrush = new SolidBrush(Extensions.MixColors(this.PresetCellForeColor, Color.Black, 0.7));

				foreach (var r in rects)
				{

					var txt = Sudoku.GetCell(i).ToString();

					if (Sudoku.GetCell(i).IsPreset)
					{
						//Background
						e.Graphics.FillRectangle(presetcellbackbrush, r);

						//Surrounding
						var newr = r;
						newr.Offset(1, 1);
						e.Graphics.DrawString(txt, this.Font, surroundingbrush, newr, sf);

						//Number
						e.Graphics.DrawString(txt, this.Font, presetcellforebrush, r, sf);
					}
					else
					{
						//Number
						e.Graphics.DrawString(txt, this.Font, nonpresetbrush, r, sf);
					}


					//Candidates
					if (this.ShowCandidates && Sudoku.GetCell(i).Number == 0)
					{
						var reducedfont = new Font(this.Font.FontFamily, 8);

						var n = 1;
						for (var y = 0; y <= 2; y++)
						{
							for (var x = 0; x <= 2; x++)
							{
								var srect = new RectangleF(r.X + x * r.Width / 3 + 1, r.Y + y * r.Height / 3 + 1, r.Width / 3f, r.Height / 3f);
								if (Sudoku.GetCell(i).Candidates.Contains(n))
									e.Graphics.DrawString(n.ToString(), reducedfont, nonpresetbrush, srect, sf);
								n++;
							}
						}
					}


					i++;
				}
			}

			base.OnPaint(e);
		}




		#region **** Mouse interactions ****
		public Cell HoveredCell;
		public Cell SelectedCell;

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			var lastr = GetRectangle(HoveredCell);
			HoveredCell = Cell.Empty;
			this.Invalidate(lastr);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (!this.EditingEnabled)
				return;


			var lastr = GetRectangle(this.SelectedCell);
			this.SelectedCell = this.Sudoku.GetCell(GetIndex(e.Location));
			var newr = GetRectangle(this.SelectedCell);

			if (lastr != newr)
			{
				this.Invalidate(lastr);
				this.Invalidate(newr);
			}
			else	//Same field clicked
			{
				this.SelectedCell = Cell.Empty;
				this.Invalidate(newr);
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			//Get last and new rectangle
			var lastr = GetRectangle(HoveredCell);
			HoveredCell = Sudoku.GetCell(GetIndex(e.Location));
			var newr = GetRectangle(HoveredCell);

			//Only invalidate if the rects aren't the same
			if (!this.HoverActivated || lastr == newr)
				return;

			this.Invalidate(lastr);
			this.Invalidate(newr);

			//Change cursor if necessary
			this.Cursor = (newr != new Rectangle(-1, -1, 1, 1)) ? Cursors.Hand : Cursors.Default;
		}

		#endregion


		#region **** Keyboard interactions ****

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if (SelectedCell.IsEqual(Cell.Empty))
				return;

			var i = this.SelectedCell.Index;

			//Check whether a number is pressed
			var pressedKey = e.KeyChar.ToString();
			int number;
			if (!int.TryParse(pressedKey, out number))
				return;

			if (number == 0)
			{
				this.Sudoku.ResetCell(i);
			}
			else
			{
				if (this.Sudoku.SetValue(i, number))
					this.Sudoku.SetPresetValue(i, true);
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			//No number, but perhaps Arrowkeys or Tab etc.
			var i = SelectedCell.Index;

			//Cancel if no cell is selected
			if (i.Column == -1 || i.Row == -1)
			{
				if (!e.KeyData.IsArrowKey())
					return;
				this.SelectedCell = this.Sudoku.GetCell(0, 0);
				this.Invalidate(GetRectangle(this.SelectedCell));
				return;
			}

			switch (e.KeyData)
			{
				case Keys.Up:	//One step up
					ChangeIndex(ref SelectedCell, Direction.Up);
					break;
				case Keys.Down: //One step down
					ChangeIndex(ref SelectedCell, Direction.Down);
					break;
				case Keys.Left:	//One step left
					ChangeIndex(ref SelectedCell, Direction.Left);
					break;
				case Keys.Tab: //One block right
					ChangeIndex(ref SelectedCell, Direction.TabRight);
					break;
				case Keys.Right: //One step right
					ChangeIndex(ref SelectedCell, Direction.Right);
					break;
				case Keys.Delete: //Delete the number
					this.Sudoku.ResetCell(i);
					break;
			}
		}

		protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
		{
			base.OnPreviewKeyDown(e);
			e.IsInputKey = true;	//Now the OnKeyDownEvent gets raised on ArrowKeys etc.
		}


		private enum Direction { Up, Down, Left, Right, TabRight }
		private void ChangeIndex(ref Cell cell, Direction d)
		{

			if (cell.IsEqual(Cell.Empty))
				return;

			var lastindex = cell.Index;
			var newindex = lastindex;
			var lastr = GetRectangle(cell);

			int r, c;
			int summand;

			switch (d)
			{
				case Direction.Up:
					r = Extensions.SetInRange(lastindex.Row - 1, 0, 8, out summand);
					c = lastindex.Column - summand;
					if (c >= 0)
						newindex = new Index(r, c);
					break;

				case Direction.Down:
					r = Extensions.SetInRange(lastindex.Row + 1, 0, 8, out summand);
					c = lastindex.Column + summand;
					if (c <= 8)
						newindex = new Index(r, c);
					break;

				case Direction.Left:
					c = Extensions.SetInRange(lastindex.Column - 1, 0, 8, out summand);
					r = lastindex.Row - summand;
					if (r >= 0)
						newindex = new Index(r, c);
					break;

				case Direction.Right:
					c = Extensions.SetInRange(lastindex.Column + 1, 0, 8, out summand);
					r = lastindex.Row + summand;
					if (r <= 8)
						newindex = new Index(r, c);
					break;

				case Direction.TabRight:
					c = Extensions.SetInRange(lastindex.Column + 3, 0, 8, out summand);
					r = lastindex.Row + summand * 3;
					if (r <= 8)
						newindex = new Index(r, c);
					break;
			}

			//Set and invalidate
			if (newindex.IsEqual(lastindex))
				return;

			cell = this.Sudoku.GetCell(newindex);
			this.Invalidate(lastr);
			this.Invalidate(GetRectangle(cell));
		}

		#endregion



		//Resizing
		[System.Diagnostics.DebuggerStepThrough]
		protected override void OnResize(EventArgs eventargs)
		{
			base.OnResize(eventargs);

			CheckSize();
		}

		private void CheckSize()
		{
			//Calculating
			var onecell = GetSizeOfOneCell();

			var newsize = new Size(9 * onecell.Width + 2 * GridBorderWidth + 2 * GridInnerBorderWidth + 6 * GridWidth,
				9 * onecell.Height + 2 * GridBorderWidth + 2 * GridInnerBorderWidth + 6 * GridWidth);


			//Square
			if (newsize.Width != newsize.Height)
			{
				var meanvalue = (newsize.Width + newsize.Height) / 2;
				newsize = new Size(meanvalue, meanvalue);
			}

			//Conditional resize to avoid looping
			this.Size = newsize;
		}



		//Handle focus
		[System.Diagnostics.DebuggerStepThrough]
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			if (this.IsAlwaysFocused)
				this.Focus();
		}





		//Useful
		public Size GetSizeOfOneCell()
		{
			return new Size((int)Math.Floor((double)(this.Width - 2 * GridBorderWidth - 2 * GridInnerBorderWidth - 6 * GridWidth) / 9),
				(int)Math.Floor((double)(this.Height - 2 * GridBorderWidth - 2 * GridInnerBorderWidth - 6 * GridWidth) / 9));
		}

		public int GetGridWidth(int step)
		{
			switch (step)
			{
				case 0:
				case 9:
					return GridBorderWidth;
				case 1:
				case 2:
				case 4:
				case 5:
				case 7:
				case 8:
					return GridWidth;
				case 3:
				case 6:
					return GridInnerBorderWidth;
				default:
					return 0;
			}
		}

		public List<Rectangle> GetListOfRectangles()
		{
			var onecell = GetSizeOfOneCell().Width;
			var currcoor = GetGridWidth(0);

			//Get toplefts
			var toplefts = new List<int>();
			for (var s = 0; s <= 8; s++)
			{
				toplefts.Add(currcoor);
				currcoor += onecell + GetGridWidth(s + 1);
			}


			//Get rects
			return (from top in toplefts from left in toplefts select new Rectangle(left, top, onecell, onecell)).ToList();
		}


		public Rectangle GetRectangle(Cell c)
		{
			return GetRectangle(c.Index.Row, c.Index.Column);
		}

		public Rectangle GetRectangle(int r, int c)
		{
			if (r == -1 && c == -1)
				return new Rectangle(-1, -1, 1, 1);

			var currcoor = GetGridWidth(0);
			var onecell = GetSizeOfOneCell().Width;
			var left = 0;	//currcoor with column
			var top = 0;	//currcoor with row

			//Get top/left
			for (var s = 0; s <= Math.Max(r, c); s++)
			{
				if (s == c)
					left = currcoor;
				if (s == r)
					top = currcoor;
				currcoor += onecell + GetGridWidth(s + 1);
			}

			return new Rectangle(left, top, onecell, onecell);
		}

		public Index GetIndex(Point pt)
		{
			var allrects = GetListOfRectangles();

			var i = allrects.TakeWhile(r => !r.Contains(pt)).Count();

			return (i == allrects.Count ? new Index(-1, -1) : new Index(i / 9, i % 9));
		}
	}
}
