using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace SudokuSolver
{
	public class SudokuField : System.Windows.Forms.Control
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

			SelectedCell = Cell.Empty();
			HoveredCell = Cell.Empty();
		}



		//Properties
		private Sudoku _sudoku;
 
		[Category("Custom")]
		public Sudoku Sudoku 
		{
			get { return _sudoku; }
			set
			{
				_sudoku = value;
				if (_sudoku != null)
					this.Sudoku.CellChanged += new Sudoku.CellChangedEventHandler(CellChanged);
				this.Invalidate();
			}
		}


		private int _gridwidth, _gridborderwidth, _gridinnerborderwidth;

		[Category("Custom")]
		public int GridWidth { 
			get { return _gridwidth; } 
			set 
			{ 
				_gridwidth = value;
				this.Invalidate();
				CheckSize(); 
			} 
		}
		[Category("Custom")]
		public int GridInnerBorderWidth { 
			get { return _gridinnerborderwidth; } 
			set
			{ 
				_gridinnerborderwidth = value;
				this.Invalidate();
				CheckSize(); 
			} 
		}
		[Category("Custom")]
		public int GridBorderWidth { 
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
					SelectedCell = (_lastselectedcell != null ? _lastselectedcell : Cell.Empty());
				}
				else
				{
					_lastselectedcell = SelectedCell;
					SelectedCell = Cell.Empty();
				}
			}
		}



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
			int curr = GetGridWidth(0) / 2;
			int onecell = GetSizeOfOneCell().Width;

			for (int s = 0; s <= 9; s++)
			{
				e.Graphics.DrawLine(new Pen(GridColor, GetGridWidth(s)), new Point(0, curr), new Point(this.Width, curr));
				e.Graphics.DrawLine(new Pen(GridColor, GetGridWidth(s)), new Point(curr, 0), new Point(curr, this.Height));

				curr += (int)Math.Ceiling((double)GetGridWidth(s) / 2) + onecell + (int)Math.Floor((double)GetGridWidth(s + 1) / 2);
			}


			//Drawing the clicked Cell
			if (SelectedCell != null) 
			{
				Rectangle r = GetRectangle(SelectedCell);
				r.Inflate(-1, -1);
				e.Graphics.FillRectangle(new SolidBrush(this.SelectedCellColor), r);
			}


			//Drawing the hovered Cell
			if (this.HoverActivated && HoveredCell != null)
			{
				Rectangle r = GetRectangle(HoveredCell);
				r.Inflate(-1, -1);
				e.Graphics.FillRectangle(new SolidBrush(this.HoveredCellColor), r);
			}
				



			//Drawing the numbers
			if (Sudoku != null && Sudoku.IsFilled())
			{
				List<Rectangle> rects = GetListOfRectangles();
				int i = 0;
				StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

				Brush _presetcellbackbrush = new SolidBrush(this.PresetCellBackColor);		
				Brush _presetcellforebrush = new SolidBrush(this.PresetCellForeColor);
				Brush _nonpresetbrush = new SolidBrush(this.NonPresetCellForeColor);
				Brush _surroundingbrush = new SolidBrush(Extensions.MixColors(this.PresetCellForeColor, Color.Black, 0.7));

				foreach (Rectangle r in rects)
				{

					string txt = Sudoku.GetCell(i / 9, i % 9).ToString();
					
					if (Sudoku.GetCell(i / 9, i % 9).IsPreset)
					{
						//Background
						e.Graphics.FillRectangle(_presetcellbackbrush, r);
						
						//Surrounding
						Rectangle newr = r;
						newr.Offset(1, 1);
						e.Graphics.DrawString(txt, this.Font, _surroundingbrush, newr, sf);

						//Number
						e.Graphics.DrawString(txt, this.Font, _presetcellforebrush, r, sf);
					}
					else
					{
						//Number
						e.Graphics.DrawString(txt, this.Font, _nonpresetbrush, r, sf);
					}


					//Candidates
					if (this.ShowCandidates && Sudoku.GetCell(i / 9, i % 9).Number == 0)
					{
						Font _reducedfont = new Font(this.Font.FontFamily, 8);

						int n = 1;
						for (int y = 0; y <= 2; y++)
						{
							for (int x = 0; x <= 2; x++)
							{
								RectangleF srect = new RectangleF(r.X + x * r.Width / 3 + 1, r.Y + y * r.Height / 3 + 1, r.Width / 3, r.Height / 3);
								if(Sudoku.GetCell(i / 9, i % 9).Candidates.Contains(n))
									e.Graphics.DrawString(n.ToString(), _reducedfont , _nonpresetbrush, srect, sf);
								n++;
							}
						}
					}
					

					i++;
				}
			}

			base.OnPaint(e);
		}


		

		//Mouse interactions
		private Cell HoveredCell;
		public Cell SelectedCell;

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			Rectangle lastr = GetRectangle(HoveredCell);
			HoveredCell = Cell.Empty();
			this.Invalidate(lastr);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
 			base.OnMouseDown(e);

			if (this.EditingEnabled)
			{
				Rectangle lastr = GetRectangle(SelectedCell);
				SelectedCell = Sudoku.GetCell(GetIndex(e.Location));
				Rectangle newr = GetRectangle(SelectedCell);

				if (lastr != newr)
				{
					this.Invalidate(lastr);
					this.Invalidate(newr);
				}
				else	//Same field clicked
				{
					SelectedCell = Cell.Empty();
					this.Invalidate(newr);
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			//Get last and new rectangle
			Rectangle lastr = GetRectangle(HoveredCell);
			HoveredCell = Sudoku.GetCell(GetIndex(e.Location));
			Rectangle newr = GetRectangle(HoveredCell);

			//Only invalidate if the rects aren't the same
			if (this.HoverActivated && lastr != newr)
			{
				this.Invalidate(lastr);
				this.Invalidate(newr);

				//Change cursor if necessary
				if (newr != new Rectangle(-1, -1, 1, 1))
				{
					this.Cursor = Cursors.Hand;
				}
				else
				{
					this.Cursor = Cursors.Default;
				}

			}
		}

		

		//Keyboard interactoons
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if (!SelectedCell.IsEqual(Cell.Empty()))
			{
				Index i = SelectedCell.Index;

				//Check whether a number is pressed
				string PressedKey = e.KeyChar.ToString();
				int number;
				if (int.TryParse(PressedKey, out number))
				{
					if (number == 0)
					{
						Sudoku.ResetCell(i);
					}
					else
					{
						if (this.Sudoku.SetValue(i, number))
							this.Sudoku.SetPresetValue(i, true);
					}
				}
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			//No number, but perhaps Arrowkeys or Tab etc.
			Rectangle lastr = GetRectangle(SelectedCell);
			Index i = SelectedCell.Index;

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
				case Keys.Tab:
					ChangeIndex(ref SelectedCell, Direction.TabRight);
					break;
				case Keys.Right:	//One step right
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


		private enum Direction {Up, Down, Left, Right, TabRight}
		private void ChangeIndex(ref Cell cell, Direction d)
		{
			Index lastindex = cell.Index;
			Index newindex = lastindex;
			Rectangle lastr = GetRectangle(cell);

			int r, c = 0;
			int summand = 0;

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
			if (!newindex.IsEqual(lastindex))
			{
				cell = this.Sudoku.GetCell(newindex);
				this.Invalidate(lastr);
				this.Invalidate(GetRectangle(cell));
			}

		}



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
			Size onecell = GetSizeOfOneCell();

			Size newsize = new Size(9 * onecell.Width + 2 * GridBorderWidth + 2 * GridInnerBorderWidth + 6 * GridWidth,
				9 * onecell.Height + 2 * GridBorderWidth + 2 * GridInnerBorderWidth + 6 * GridWidth);


			//Square
			if (newsize.Width != newsize.Height)
			{
				int meanvalue = (int)((newsize.Width + newsize.Height) / 2);
				newsize = new Size(meanvalue, meanvalue);
			}

			//Conditional resize to avoid looping
			if (this.Size != newsize)
				this.Size = newsize;
		}



		//Handle focus
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.Focus();
		}



		//Cell changed
		private void CellChanged(Cell c)
		{
			this.Invalidate(GetRectangle(c));
		}



		//Useful
		private Size GetSizeOfOneCell()
		{
			return new Size((int)Math.Floor((double)(this.Width - 2 * GridBorderWidth - 2 * GridInnerBorderWidth - 6 * GridWidth) / 9),
				(int)Math.Floor((double)(this.Height - 2 * GridBorderWidth - 2 * GridInnerBorderWidth - 6 * GridWidth) / 9));
		}

		private int GetGridWidth(int step)
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

		private List<Rectangle> GetListOfRectangles()
		{
			int onecell = GetSizeOfOneCell().Width;
			int currcoor = GetGridWidth(0);

			//Get toplefts
			List<int> toplefts = new List<int>();
			for (int s = 0; s <= 8; s++)
			{
				toplefts.Add(currcoor);
				currcoor += onecell + GetGridWidth(s + 1);
			}


			//Get rects
			List<Rectangle> rects = new List<Rectangle>();
			foreach (int top in toplefts)	//y
			{
				foreach (int left in toplefts)	//x
				{
					rects.Add(new Rectangle(left, top, onecell, onecell));
				}
			}

			return rects;
		}


		private Rectangle GetRectangle(Cell c)
		{
			return GetRectangle(c.Index.Row, c.Index.Column);
		}

		private Rectangle GetRectangle(Index i)
		{
			return GetRectangle(i.Row, i.Column);
		}

		private Rectangle GetRectangle(int r, int c)
		{
			if (r == -1 && c == -1)
				return new Rectangle(-1, -1, 1, 1);

			int currcoor = GetGridWidth(0);
			int onecell = GetSizeOfOneCell().Width;
			int left = 0;	//currcoor with column
			int top = 0;	//currcoor with row

			//Get top/left
			for (int s = 0; s <= Math.Max(r, c); s++)
			{
				if (s == c)
					left = currcoor;
				if (s == r)
					top = currcoor;
				currcoor += onecell + GetGridWidth(s + 1);
			}

			return new Rectangle(left, top, onecell, onecell);
		}

		private Index GetIndex(Point pt)
		{
			List<Rectangle> allrects = GetListOfRectangles();

			int i = 0;
			foreach (Rectangle r in allrects)
			{
				if (r.Contains(pt))
					break;
				i++;
			}

			return (i == allrects.Count ? new Index(-1, -1) : new Index(i / 9, i % 9));
		}


	}
}
