using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Snerble.VRC.TouchControls
{
    public enum Alignment { Left = default, Right, Center }

    public static class StringUtils
    {
        private static readonly Regex ArgumentSplitter = new Regex(@"([^""\s]+)|""((?:[^""]|\s)*)""", RegexOptions.Compiled | RegexOptions.Multiline);
     
        public static string Table(
            string[] headers,
            object[][] items,
            params Alignment[] alignments)
        {
            var lines = Justify(items.Prepend(headers).ToArray().To2D().Transpose(), alignments: alignments).ToList();
            lines.Insert(1, new string('-', lines[0].Length));
            return string.Join("\n", lines);
        }

        public static IEnumerable<string> Justify(
            object[][] items,
            string paddingRight = " | ",
            string paddingLeft = "",
            string padding = null,
            params Alignment[] alignments)
        {
            return Justify(items.To2D(), paddingRight, paddingLeft, padding, alignments);
        }

        public static IEnumerable<string> Justify(
            object[,] items,
            string paddingRight = " | ",
            string paddingLeft = "",
            string padding = null,
            params Alignment[] alignments)
        {
            int colums = items.GetLength(0);
            int rows = items.GetLength(1);

            // Convert to string
            string[,] strings = new string[colums, rows];
            for (int x = 0; x < colums; x++)
                for (int y = 0; y < rows; y++)
                    strings[x, y] = items[x, y]?.ToString() ?? "";

            // Populate alignments array
            if (alignments.Length == 1)
                alignments = Enumerable.Repeat(alignments[0], colums).ToArray();
            else if (alignments.Length < colums)
                alignments = alignments.Concat(Enumerable.Repeat(default(Alignment), colums - alignments.Length)).ToArray();

            // Overwrite padding if not null
            if (padding != null)
            {
                paddingLeft = padding;
                paddingRight = padding;
            }

            // Calculate final column widths
            int[] columnSizes = Enumerable.Range(0, colums)
                .Select(x => Enumerable.Range(0, rows)
                    .Select(y => strings[x, y].Length)
                    .Max())
                .ToArray();

            // Iterate through rows first
            for (int y = 0; y < rows; y++)
            {
                var sb = new StringBuilder();

                // Build each column based on item size and alignment type
                for (int x = 0; x < colums; x++)
                {
                    var item = strings[x, y];
                    int maxSize = columnSizes[x];
                    var alignment = alignments[x];

                    int padLeft, padRight;
                    switch (alignment)
                    {
                        default:
                        case Alignment.Left:
                            padLeft = 0;
                            padRight = maxSize - item.Length;
                            break;
                        case Alignment.Right:
                            padLeft = maxSize - item.Length;
                            padRight = 0;
                            break;
                        case Alignment.Center:
                            padLeft = maxSize - item.Length;
                            (padLeft, padRight) = (padLeft / 2, (int)Math.Ceiling(padLeft / 2f));
                            break;
                    }

                    if (x > 0)
                        sb.Append(paddingLeft);
                    sb.Append(' ', padLeft);
                    sb.Append(item);
                    sb.Append(' ', padRight);
                    if (x < colums - 1)
                        sb.Append(paddingRight);
                }

                yield return sb.ToString();
            }
        }


        public static string[] Split(string s)
        {
            if (s is null)
                return Array.Empty<string>();
            return ArgumentSplitter
                .Matches(s)
                .Cast<Match>()
                .Select(x => x.Groups.Cast<Group>()
                    .Skip(1)
                    .First(y => y.Success).Value)
                .ToArray();
        }
    }

    public static class JaggedArrayExtensions
    {
        public static T[,] To2D<T>(this T[][] source)
        {
            try
            {
                int FirstDim = source.Length;
                int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

                var result = new T[FirstDim, SecondDim];
                for (int i = 0; i < FirstDim; ++i)
                    for (int j = 0; j < SecondDim; ++j)
                        result[i, j] = source[i][j];

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }
    }

    public static class MatrixExtensions
    {
        public static T[,] Transpose<T>(this T[,] source)
        {
            int xDim = source.GetLength(0), yDim = source.GetLength(1);

            var result = new T[yDim, xDim];

            for (int x = 0; x < xDim; x++)
                for (int y = 0; y < yDim; y++)
                    result[y, x] = source[x, y];

            return result;
        }

        public static T[][] ToJagged<T>(this T[,] source)
        {
            int xDim = source.GetLength(0), yDim = source.GetLength(1);

            var result = new T[xDim][];

            for (int x = 0; x < xDim; x++)
            {
                result[x] = new T[yDim];
                for (int y = 0; y < yDim; y++)
                    result[x][y] = source[x, y];
            }

            return result;
        }
    }

}
