using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace aoc
{
    internal class D16
    {
        const int phases = 100;

        public object Answer()
        {
            var numbers = Enumerable.Repeat(File.ReadAllText("16.2").Select(c => int.Parse(c.ToString())), 10000).SelectMany(x => x).ToArray().AsSpan();

            unchecked
            {
                for (int p = 0; p < phases; p++)
                {
                    Console.WriteLine($"Applying phase {p + 1}");
                    ApplyPhase(numbers);
                }
            }
            var sb = new StringBuilder();
            foreach (var n in numbers.Slice(0, 8))
                sb.Append(n.ToString());
            var offset = int.Parse(sb.ToString());
            sb.Clear();
            foreach (var n in numbers.Slice(offset, 8))
                sb.Append(n.ToString());
            return sb.ToString();
        }

        private unsafe void ApplyPhase(Span<int> numbers)
        {
            var basePattern = stackalloc[] { 1, 0, -1, 0 };
            var basePatternLength = 4;

            var length = numbers.Length;
            var sw = new Stopwatch();
            sw.Start();
            // Process first half of all the numbers
            for (int i = 0; i < length / 2; i++)
            {
                var sum = 0;
                var processed = i; // Skip i characters, since they are all 0

                var basePatternIdx = 0;
                int ret = basePattern[basePatternIdx];

                while (processed < length - i)
                {
                    var repetitions = i + 1;

                    var slice = numbers.Slice(processed, repetitions);
                    switch (ret)
                    {
                        case 1:
                            for (int j = 0; j < repetitions; j++)
                            {
                                sum += slice[j];
                            }
                            processed += repetitions;
                            break;
                        case -1:
                            for (int j = 0; j < repetitions; j++)
                            {
                                sum -= slice[j];
                            }
                            processed += repetitions;
                            break;
                    }
                    //if (ret == 1)
                    //{
                    //}
                    //else if (ret == -1)
                    //{
                    //}

                    basePatternIdx += basePatternIdx < basePatternLength - 1 ? 1 : -3;
                    ret = basePattern[basePatternIdx];

                    if (ret == 0)
                    {
                        processed += repetitions;
                        basePatternIdx += basePatternIdx < basePatternLength - 1 ? 1 : -3;
                        ret = basePattern[basePatternIdx];
                    }
                }

                if (ret == 1)
                {
                    for (int j = 0; j < i + 1 && processed < length; j++)
                    {
                        sum += numbers[processed++];
                    }
                }
                else if (ret == -1)
                {
                    for (int j = 0; j < i + 1 && processed < length; j++)
                    {
                        sum -= numbers[processed++];
                    }
                }

                var n = sum.ToString();
                numbers[i] = (int)(n[n.Length - 1] - '0');

            }
            sw.Stop();
            Console.WriteLine($"Processed half of numbers in {sw.ElapsedMilliseconds} ms");
            sw.Restart();
            // from half of list the list, the right part is only added since they are all zero
            for (int i = length / 2; i < length; i++)
            {
                var sum = 0;
                for (int j = i; j < length; j++)
                {
                    sum += numbers[j];
                }

                var n = sum.ToString();
                numbers[i] = (int)(n[n.Length - 1] - '0');
            }
            sw.Stop();
            Console.WriteLine($"Processed other half of numbers in {sw.ElapsedMilliseconds} ms");
        }
    }
}