using System.Collections.Generic;

namespace NolekSymbols.Helpers
{
    public static class MathHelper
    {
        /// <summary>
        ///     Gets available combinations based on
        /// </summary>
        /// <param name="k">Amount of numbers</param>
        /// <param name="n">Total amount to itterate over</param>
        /// <returns>An array with each different combination</returns>
        public static IEnumerable<int[]> GetCombinationsWithoutRepetition(int k, int n)
        {
            if (k <= 0)
                yield break;
            var result = new int[k];
            var stack = new Stack<int>();
            stack.Push(1);

            while (stack.Count > 0)
            {
                var index = stack.Count - 1;
                var value = stack.Pop();

                while (value <= n)
                {
                    result[index++] = value++;
                    stack.Push(value);
                    if (index != k) continue;
                    yield return result;
                    break;
                }
            }
        }

        public static int CalculatePermutations(int n, int r)
        {
            return Factorial(n) / (Factorial(r) * Factorial(n - r));
        }

        public static int Factorial(int i)
        {
            if (i <= 1)
                return 1;
            return i * Factorial(i - 1);
        }
    }
}