using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc
{
    internal class D16
    {
        static int[] basePattern = new int[] { 0, 1, 0, -1 };
        const int phases = 100;

        public object Answer()
        {
            var numbers = Enumerable.Repeat(File.ReadAllText("16.in").Select(c => int.Parse(c.ToString())), 100).SelectMany(x => x).ToArray();

            for (int p = 0; p < phases; p++)
            {
                Console.WriteLine($"Phase {p + 1}");
                ApplyPhase(numbers);
            }

            return string.Join("", numbers.Take(8));
        }

        private void ApplyPhase(int[] numbers)
        {
            for (int i = 0; i < numbers.Length / 2; i++)
            {
                var sum = 0;

                var basePatternIdx = 0;
                int ret = basePattern[basePatternIdx];

                int processed = 0;
                for (int j = 0; j < i; j++)
                {
                    if (ret == 1) sum += numbers[processed++];
                    else if (ret == 0) { ++processed; continue; }
                }

                basePatternIdx = (basePatternIdx + 1) % basePattern.Length;
                ret = basePattern[basePatternIdx];

                while (processed < numbers.Length - i)
                {
                    for (int j = 0; j < i + 1; j++)
                    {
                        if (ret == 0) { ++processed; continue; }
                        if (ret == 1) sum += numbers[processed++];
                        else if (ret == -1) sum -= numbers[processed++];
                    }
                    basePatternIdx = (basePatternIdx + 1) % basePattern.Length;
                    ret = basePattern[basePatternIdx];
                }

                if (ret == 1)
                {
                    for (int j = 0; j < i + 1 && processed < numbers.Length; j++)
                    {
                        sum += numbers[processed++];
                    }
                }
                else if (ret == -1)
                {
                    for (int j = 0; j < i + 1 && processed < numbers.Length; j++)
                    {
                        sum -= numbers[processed++];
                    }
                }

                var n = sum.ToString();
                numbers[i] = (int)(n[n.Length - 1] - '0');
                //Console.WriteLine($"Calculated number {i + 1} of {numbers.Length}");
            }

            // from half of list the list, the right part is only added
            for (int i = numbers.Length / 2; i < numbers.Length; i++)
            {
                var sum = 0;
                for (int j = i; j < numbers.Length; j++)
                {
                     sum += numbers[j];
                }

                var n = sum.ToString();
                numbers[i] = (int)(n[n.Length - 1] - '0');
                //Console.WriteLine($"Calculated number {i + 1} of {numbers.Length}");
            }
        }

        private IEnumerable<int> GeneratePattern(int repeats)
        {
            var basePatternIdx = 0;
            int ret = basePattern[basePatternIdx];

            for (int i = 0; i < repeats - 1; i++)
            {
                yield return ret;
            }

            basePatternIdx = (basePatternIdx + 1) % basePattern.Length;
            ret = basePattern[basePatternIdx];

            while (true)
            {
                for (int i = 0; i < repeats; i++)
                {
                    yield return ret;
                }
                basePatternIdx = (basePatternIdx + 1) % basePattern.Length;
                ret = basePattern[basePatternIdx];
            }

        }
    }
}