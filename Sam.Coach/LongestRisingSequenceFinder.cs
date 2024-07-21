using Sam.Coach.Exceptions.Forbidden403;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sam.Coach
{
    public class LongestRisingSequenceFinder : ILongestRisingSequenceFinder
    {
        public Task<IEnumerable<int>> Find(IEnumerable<int> first_numbers, IEnumerable<int> second_numbers) => Task.Run(async () =>
        {
            IEnumerable<int> result = null;
            var firstLongestIncreasingSubsequence = LongestIncreasingSubsequence(first_numbers.ToList());
            var secondLongestIncreasingSubsequence = LongestIncreasingSubsequence(second_numbers.ToList());
            if(firstLongestIncreasingSubsequence.Count == secondLongestIncreasingSubsequence.Count)
            {
                throw new LongestIncrementEqualException();
            }
            result = firstLongestIncreasingSubsequence.Count > secondLongestIncreasingSubsequence.Count ?
                    first_numbers : second_numbers;

            return result;
        });

        private List<int> LongestIncreasingSubsequence(List<int> numbers)
        {
            if (numbers.Count == 0)
            {
                return numbers;
            }
            List<int> longestIncreasing = new List<int>();
            List<int>[] paths = new List<int>[numbers.Count];

            for (int i = 0; i < numbers.Count; i++)
            {
                paths[i] = new List<int>();
            }

            longestIncreasing.Add(numbers[0]);
            paths[0].Add(numbers[0]);

            return ProcessNumbers(numbers, longestIncreasing, paths);
        }

        private List<int> ProcessNumbers(List<int> numbers, List<int> longestIncreasing, List<int>[] paths)
        {
            for (int i = 1; i < numbers.Count; i++)
            {
                if (numbers[i] > longestIncreasing.Last())
                {
                    longestIncreasing.Add(numbers[i]);
                    paths[longestIncreasing.Count - 1] = new List<int>(paths[longestIncreasing.Count - 2]) { numbers[i] };
                }
                else
                {
                    int position = longestIncreasing.BinarySearch(numbers[i]);
                    if (position < 0)
                    {
                        position = ~position;
                    }
                    longestIncreasing[position] = numbers[i];
                    paths[position] = position > 0 ? 
                        new List<int>(paths[position - 1]) { numbers[i] } : new List<int> { numbers[i] };
                }
            }

            return paths[longestIncreasing.Count - 1];
        }

    }
}
