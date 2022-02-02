namespace SudokuSolver
{
    public static class Extensions
    {

        public static int SetInRange(int n, int min, int max, out int distance)
        {
            distance = 0;

            //Borders wrong way
            if (min > max) {
                Swap(ref min, ref max);
            }

            //Already in range
            if (n >= min && n <= max) {
                return n;
            }

            //Higher
            if (n >= max) {
                int difference = n - max;
                int rangewidth = max - min + 1;
                distance = (int)Math.Ceiling((double)difference / rangewidth);
                return n - (distance * rangewidth);
            }

            //Lower
            if (n <= min) {
                int difference = Math.Abs(min - n);
                int rangewidth = max - min + 1;
                distance = (int)Math.Ceiling((double)difference / rangewidth);
                return n + (distance * rangewidth);
            }

            return 0;
        }
        private static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        public static Color MixColors(Color c1, Color c2, double alphavalue = 0.5)
        {
            int a = (int)Math.Round((c1.A * alphavalue) + (c2.A * (1 - alphavalue)));
            int r = (int)Math.Round((c1.R * alphavalue) + (c2.R * (1 - alphavalue)));
            int g = (int)Math.Round((c1.G * alphavalue) + (c2.G * (1 - alphavalue)));
            int b = (int)Math.Round((c1.B * alphavalue) + (c2.B * (1 - alphavalue)));

            return Color.FromArgb(a, r, g, b);
        }

        //algorithm to return all combinations of k elements from n
        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        {
            var enumerable = elements as IList<T> ?? elements.ToList();
            return k == 0 ? new[] { Array.Empty<T>() } :
              enumerable.SelectMany((e, i) =>
                enumerable.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }

        public static void ConditionalInvoke(this Control ctrl, Action a)
        {
            if (ctrl.InvokeRequired) {
                ctrl.BeginInvoke(a);
            }
            else {
                a();
            }
        }

        public static bool IsArrowKey(this Keys k)
        {
            var arrowkeys = new List<Keys> { Keys.Left, Keys.Right, Keys.Up, Keys.Down };
            return arrowkeys.Contains(k);
        }

        //Summarize all the items of the lists to one list and remove double entries
        public static IEnumerable<T> CombineSortDeduplicate<T>(this IEnumerable<IEnumerable<T>> lists)
        {
            var temp = new List<T>();
            foreach (var list in lists) {
                temp.AddRange(list);
            }

            temp.Sort();
            return temp.Distinct();
        }
    }
}

