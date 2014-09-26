using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SudokuSolver
{
	public static class Extensions
	{

		public static int SetInRange(int n, int min, int max, out int distance)
		{
			distance = 0;

			//Borders wrong way
			if (min > max)
				Extensions.Swap(ref min, ref max);

			//Already in range
			if (n >= min && n <= max)
				return n;

			//Higher
			if (n >= max)
			{
				int difference = n - max;
				int rangewidth = max - min + 1;
				distance = (int)Math.Ceiling((double)difference / rangewidth);
				return n - distance * rangewidth;
			}
			
			//Lower
			if (n <= min)
			{
				int difference = Math.Abs(min - n);
				int rangewidth = max - min + 1;
				distance = (int)Math.Ceiling((double)difference / rangewidth);
				return n +  distance * rangewidth;
			}

			return 0;
		}

		public static void Swap(ref int a, ref int b)
		{
			int temp = a;
			a = b;
			b = temp;
		}

		public static Color MixColors(Color c1, Color c2) 
		{
			return MixColors(c1, c2, 0.5);
		}

		public static Color MixColors(Color c1, Color c2, double alphavalue)
		{
			int a = (int)Math.Round(c1.A * alphavalue + c2.A * (1 - alphavalue));
			int r = (int)Math.Round(c1.R * alphavalue + c2.R * (1 - alphavalue));
			int g = (int)Math.Round(c1.G * alphavalue + c2.G * (1 - alphavalue));
			int b = (int)Math.Round(c1.B * alphavalue + c2.B * (1 - alphavalue));

			return Color.FromArgb(a, r, g, b);
		}

		//algorithm to return all combinations of k elements from n
		public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
		{
			return k == 0 ? new[] { new T[0] } :
			  elements.SelectMany((e, i) =>
				elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
		}

		public static object ConditionalInvoke(this System.Windows.Forms.Control ctrl, Action a)
		{
			if (ctrl.InvokeRequired)
			{
				ctrl.BeginInvoke(a);
			}
			else
			{
				a();
			}
			return null;
		}
	}
}

