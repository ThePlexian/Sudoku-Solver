using System.Drawing;

namespace SudokuSolver.UI
{
	class CustomMenuStrip : System.Windows.Forms.MenuStrip
	{

		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaintBackground(e);	

			e.Graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(0, 0, Width, Height));
			var p = new Pen(Extensions.MixColors(BackColor, Color.Black, 0.8));
			e.Graphics.DrawLine(p, 5, Height-1, Width-5, Height-1);
		}

	}
}
