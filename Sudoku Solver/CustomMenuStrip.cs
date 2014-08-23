using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SudokuSolver
{
	class CustomMenuStrip : System.Windows.Forms.MenuStrip
	{

		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaintBackground(e);	

			e.Graphics.FillRectangle(new SolidBrush(this.BackColor), new Rectangle(0, 0, this.Width, this.Height));
			Pen p = new Pen(Extensions.MixColors(this.BackColor, Color.Black, 0.8));
			e.Graphics.DrawLine(p, 5, this.Height-1, this.Width-5, this.Height-1);
		}

	}
}
