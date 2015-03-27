using System.Drawing;

namespace SudokuSolver.UI
{
	class CustomMenuStrip : System.Windows.Forms.MenuStrip
	{

		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaintBackground(e);	

			e.Graphics.FillRectangle(new SolidBrush(this.BackColor), new Rectangle(0, 0, this.Width, this.Height));
			var p = new Pen(Extensions.MixColors(this.BackColor, Color.Black, 0.8));
			e.Graphics.DrawLine(p, 5, this.Height-1, this.Width-5, this.Height-1);
		}

	}
}
