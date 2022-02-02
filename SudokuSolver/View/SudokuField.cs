using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using SudokuSolver.Model.Data;

namespace SudokuSolver.View
{
    public class SudokuField : Control
    {

        //Constructor
        public SudokuField()
        {
            Size = new Size(400, 400);
            GridWidth = 1;
            GridInnerBorderWidth = 3;
            GridBorderWidth = 5;
            GridColor = Color.Orange;
            HoveredCellColor = Color.FromArgb(127, 200, 200, 200);
            SelectedCellColor = Color.FromArgb(255, 255, 120, 80);
            PresetCellBackColor = Color.FromArgb(127, 200, 200, 200);
            PresetCellForeColor = Color.FromArgb(255, 0, 0, 0);
            NonPresetCellForeColor = Color.FromArgb(255, 80, 80, 80);

            HoverActivated = true;
            ShowCandidates = true;
            EditingEnabled = true;

            _selectedCell = Cell.Empty;
            _hoveredCell = Cell.Empty;

            Sudoku = new Sudoku();
        }



        #region **** Properties / Fields ****

        private Sudoku _sudoku;

        [Category("Custom")]
        public Sudoku Sudoku {
            private get => _sudoku;
            set {
                if (value == null) {
                    return;
                }

                var oldsudoku = _sudoku;
                _sudoku = value;

                //Invalidate
                if (oldsudoku == null || _sudoku == null) {
                    return;
                }

                foreach (var c in _sudoku.GetCellsIterated().Where(c => !c.IsEqual(oldsudoku.GetCell(c.Index)))) {
                    Invalidate(GetRectangle(c));
                }

                this.ConditionalInvoke(Update);
            }
        }


        private int _gridwidth, _gridborderwidth, _gridinnerborderwidth;

        [Category("Custom")]
        public int GridWidth {
            private get => _gridwidth;
            set {
                _gridwidth = value;
                Invalidate();
                CheckSize();
            }
        }
        [Category("Custom")]
        public int GridInnerBorderWidth {
            private get => _gridinnerborderwidth;
            set {
                _gridinnerborderwidth = value;
                Invalidate();
                CheckSize();
            }
        }
        [Category("Custom")]
        public int GridBorderWidth {
            private get => _gridborderwidth;
            set {
                _gridborderwidth = value;
                Invalidate();
                CheckSize();
            }
        }
        [Category("Custom")]
        public Color GridColor { private get; set; }

        [Category("Custom")]
        public Color HoveredCellColor { private get; set; }
        [Category("Custom")]
        public Color SelectedCellColor { private get; set; }
        [Category("Custom")]
        public Color PresetCellBackColor { private get; set; }

        [Category("Custom")]
        public Color PresetCellForeColor { private get; set; }
        [Category("Custom")]
        public Color NonPresetCellForeColor { private get; set; }


        private bool _editingenabled;
        private Cell _lastselectedcell;

        [Category("Custom")]
        public bool HoverActivated { private get; set; }

        [Category("Custom")]
        public bool ShowCandidates { get; set; }

        [Category("Custom")]
        public bool EditingEnabled {
            private get => _editingenabled;
            set {
                _editingenabled = value;
                if (_editingenabled) {
                    _selectedCell = _lastselectedcell ?? Cell.Empty;
                }
                else {
                    _lastselectedcell = _selectedCell;
                    _selectedCell = Cell.Empty;
                }
            }
        }

        [Category("Custom")]
        public bool IsAlwaysFocused { private get; set; }

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
            int curr = GetGridWidth(0) / 2;
            int onecell = GetSizeOfOneCell().Width;

            for (int s = 0; s <= 9; s++) {
                e.Graphics.DrawLine(new Pen(GridColor, GetGridWidth(s)), new Point(0, curr), new Point(Width, curr));
                e.Graphics.DrawLine(new Pen(GridColor, GetGridWidth(s)), new Point(curr, 0), new Point(curr, Height));

                curr += (int)Math.Ceiling((double)GetGridWidth(s) / 2) + onecell + (int)Math.Floor((double)GetGridWidth(s + 1) / 2);
            }


            //Drawing the clicked Cell
            if (_selectedCell != null) {
                var r = GetRectangle(_selectedCell);
                r.Inflate(-1, -1);
                e.Graphics.FillRectangle(new SolidBrush(SelectedCellColor), r);
            }


            //Drawing the hovered Cell
            if (HoverActivated && _hoveredCell != null) {
                var r = GetRectangle(_hoveredCell);
                r.Inflate(-1, -1);
                e.Graphics.FillRectangle(new SolidBrush(HoveredCellColor), r);
            }




            //Drawing the numbers
            if (Sudoku != null && Sudoku.IsFilled) {
                var rects = GetListOfRectangles();
                int i = 0;
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

                Brush presetcellbackbrush = new SolidBrush(PresetCellBackColor);
                Brush presetcellforebrush = new SolidBrush(PresetCellForeColor);
                Brush nonpresetbrush = new SolidBrush(NonPresetCellForeColor);
                Brush surroundingbrush = new SolidBrush(Extensions.MixColors(PresetCellForeColor, Color.Black, 0.7));

                foreach (var r in rects) {

                    string? txt = Sudoku.GetCell(i).ToString();

                    if (Sudoku.GetCell(i).IsPreset) {
                        //Background
                        e.Graphics.FillRectangle(presetcellbackbrush, r);

                        //Surrounding
                        var newr = r;
                        newr.Offset(1, 1);
                        e.Graphics.DrawString(txt, Font, surroundingbrush, newr, sf);

                        //Number
                        e.Graphics.DrawString(txt, Font, presetcellforebrush, r, sf);
                    }
                    else {
                        //Number
                        e.Graphics.DrawString(txt, Font, nonpresetbrush, r, sf);
                    }


                    //Candidates
                    if (ShowCandidates && Sudoku.GetCell(i).Number == 0) {
                        var reducedfont = new Font(Font.FontFamily, 8);

                        int n = 1;
                        for (int y = 0; y <= 2; y++) {
                            for (int x = 0; x <= 2; x++) {
                                var srect = new RectangleF(r.X + (x * r.Width / 3) + 1, r.Y + (y * r.Height / 3) + 1, r.Width / 3f, r.Height / 3f);
                                if (Sudoku.GetCell(i).Candidates.Contains(n)) {
                                    e.Graphics.DrawString(n.ToString(), reducedfont, nonpresetbrush, srect, sf);
                                }

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
        private Cell _hoveredCell;
        private Cell _selectedCell;

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            var lastr = GetRectangle(_hoveredCell);
            _hoveredCell = Cell.Empty;
            Invalidate(lastr);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!EditingEnabled) {
                return;
            }

            var lastr = GetRectangle(_selectedCell);
            _selectedCell = Sudoku.GetCell(GetIndex(e.Location));
            var newr = GetRectangle(_selectedCell);

            if (lastr != newr) {
                Invalidate(lastr);
                Invalidate(newr);
            }
            else    //Same field clicked
            {
                _selectedCell = Cell.Empty;
                Invalidate(newr);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            //Get last and new rectangle
            var lastr = GetRectangle(_hoveredCell);
            _hoveredCell = Sudoku.GetCell(GetIndex(e.Location));
            var newr = GetRectangle(_hoveredCell);

            //Only invalidate if the rects aren't the same
            if (!HoverActivated || lastr == newr) {
                return;
            }

            Invalidate(lastr);
            Invalidate(newr);

            //Change cursor if necessary
            Cursor = (newr != new Rectangle(-1, -1, 1, 1)) ? Cursors.Hand : Cursors.Default;
        }

        #endregion


        #region **** Keyboard interactions ****

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (_selectedCell.IsEqual(Cell.Empty)) {
                return;
            }

            var i = _selectedCell.Index;

            //Check whether a number is pressed
            string? pressedKey = e.KeyChar.ToString();
            if (!int.TryParse(pressedKey, out int number)) {
                return;
            }

            if (number == 0) {
                Sudoku.ResetCell(i);
            }
            else {
                if (Sudoku.SetValue(i, number)) {
                    Sudoku.SetPresetValue(i, true);
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            //No number, but perhaps Arrowkeys or Tab etc.
            var i = _selectedCell.Index;

            //Cancel if no cell is selected
            if (i.Column == -1 || i.Row == -1) {
                if (!e.KeyData.IsArrowKey()) {
                    return;
                }

                _selectedCell = Sudoku.GetCell(0, 0);
                Invalidate(GetRectangle(_selectedCell));
                return;
            }

            switch (e.KeyData) {
                case Keys.Up:   //One step up
                    ChangeIndex(ref _selectedCell, Direction.Up);
                    break;
                case Keys.Down: //One step down
                    ChangeIndex(ref _selectedCell, Direction.Down);
                    break;
                case Keys.Left: //One step left
                    ChangeIndex(ref _selectedCell, Direction.Left);
                    break;
                case Keys.Tab: //One block right
                    ChangeIndex(ref _selectedCell, Direction.TabRight);
                    break;
                case Keys.Right: //One step right
                    ChangeIndex(ref _selectedCell, Direction.Right);
                    break;
                case Keys.Delete: //Delete the number
                    Sudoku.ResetCell(i);
                    break;
            }
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            e.IsInputKey = true;    //Now the OnKeyDownEvent gets raised on ArrowKeys etc.
        }


        private enum Direction { Up, Down, Left, Right, TabRight }
        private void ChangeIndex(ref Cell cell, Direction d)
        {

            if (cell.IsEqual(Cell.Empty)) {
                return;
            }

            var lastindex = cell.Index;
            var newindex = lastindex;
            var lastr = GetRectangle(cell);

            int r, c;
            int summand;

            switch (d) {
                case Direction.Up:
                    r = Extensions.SetInRange(lastindex.Row - 1, 0, 8, out summand);
                    c = lastindex.Column - summand;
                    if (c >= 0) {
                        newindex = new GridIndex(r, c);
                    }

                    break;

                case Direction.Down:
                    r = Extensions.SetInRange(lastindex.Row + 1, 0, 8, out summand);
                    c = lastindex.Column + summand;
                    if (c <= 8) {
                        newindex = new GridIndex(r, c);
                    }

                    break;

                case Direction.Left:
                    c = Extensions.SetInRange(lastindex.Column - 1, 0, 8, out summand);
                    r = lastindex.Row - summand;
                    if (r >= 0) {
                        newindex = new GridIndex(r, c);
                    }

                    break;

                case Direction.Right:
                    c = Extensions.SetInRange(lastindex.Column + 1, 0, 8, out summand);
                    r = lastindex.Row + summand;
                    if (r <= 8) {
                        newindex = new GridIndex(r, c);
                    }

                    break;

                case Direction.TabRight:
                    c = Extensions.SetInRange(lastindex.Column + 3, 0, 8, out summand);
                    r = lastindex.Row + (summand * 3);
                    if (r <= 8) {
                        newindex = new GridIndex(r, c);
                    }

                    break;
            }

            //Set and invalidate
            if (newindex.IsEqual(lastindex)) {
                return;
            }

            cell = Sudoku.GetCell(newindex);
            Invalidate(lastr);
            Invalidate(GetRectangle(cell));
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

            var newsize = new Size((9 * onecell.Width) + (2 * GridBorderWidth) + (2 * GridInnerBorderWidth) + (6 * GridWidth),
                (9 * onecell.Height) + (2 * GridBorderWidth) + (2 * GridInnerBorderWidth) + (6 * GridWidth));


            //Square
            if (newsize.Width != newsize.Height) {
                int meanvalue = (newsize.Width + newsize.Height) / 2;
                newsize = new Size(meanvalue, meanvalue);
            }

            //Conditional resize to avoid looping
            Size = newsize;
        }



        //Handle focus
        [System.Diagnostics.DebuggerStepThrough]
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (IsAlwaysFocused) {
                Focus();
            }
        }





        //Useful
        private Size GetSizeOfOneCell()
        {
            return new Size((int)Math.Floor((double)(Width - (2 * GridBorderWidth) - (2 * GridInnerBorderWidth) - (6 * GridWidth)) / 9),
                (int)Math.Floor((double)(Height - (2 * GridBorderWidth) - (2 * GridInnerBorderWidth) - (6 * GridWidth)) / 9));
        }

        private int GetGridWidth(int step)
        {
            return step switch {
                0 or 9 => GridBorderWidth,
                1 or 2 or 4 or 5 or 7 or 8 => GridWidth,
                3 or 6 => GridInnerBorderWidth,
                _ => 0,
            };
        }

        private List<Rectangle> GetListOfRectangles()
        {
            int onecell = GetSizeOfOneCell().Width;
            int currcoor = GetGridWidth(0);

            //Get toplefts
            var toplefts = new List<int>();
            for (int s = 0; s <= 8; s++) {
                toplefts.Add(currcoor);
                currcoor += onecell + GetGridWidth(s + 1);
            }


            //Get rects
            return (from top in toplefts from left in toplefts select new Rectangle(left, top, onecell, onecell)).ToList();
        }


        public Rectangle GetRectangle(Cell c) => GetRectangle(c.Index.Row, c.Index.Column);

        private Rectangle GetRectangle(int r, int c)
        {
            if (r == -1 && c == -1) {
                return new Rectangle(-1, -1, 1, 1);
            }

            int currcoor = GetGridWidth(0);
            int onecell = GetSizeOfOneCell().Width;
            int left = 0;   //currcoor with column
            int top = 0;    //currcoor with row

            //Get top/left
            for (int s = 0; s <= Math.Max(r, c); s++) {
                if (s == c) {
                    left = currcoor;
                }

                if (s == r) {
                    top = currcoor;
                }

                currcoor += onecell + GetGridWidth(s + 1);
            }

            return new Rectangle(left, top, onecell, onecell);
        }

        private Model.Data.GridIndex GetIndex(Point pt)
        {
            var allrects = GetListOfRectangles();

            int i = allrects.TakeWhile(r => !r.Contains(pt)).Count();

            return i == allrects.Count ? new GridIndex(-1, -1) : new GridIndex(i / 9, i % 9);
        }
    }
}
