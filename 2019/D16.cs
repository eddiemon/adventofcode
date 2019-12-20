﻿using System;
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
            var originalCode = File.ReadAllText("16.in").Select(c => int.Parse(c.ToString())).ToArray();

            var sb = new StringBuilder();
            foreach (var n in originalCode.AsSpan().Slice(0, 7))
                sb.Append(n.ToString());

            var wat = int.Parse(sb.ToString());
            var c = Enumerable.Repeat(originalCode, 10000).SelectMany(x => x).ToArray();
            var code = c.AsSpan().Slice(wat);
            var state = code.Slice(0);
            var nextState = new int[code.Length];

            for (int p = 0; p < phases; p++)
            {
                var count = 0;
                for (int location = state.Length - 1; location >= 0; location--)
                {
                    count = (count + state[location]) % 10;
                    nextState[location] = count;
                }
                for (int location = 0; location < state.Length; location++)
                {
                    state[location] = nextState[location];
                }
                //    Console.WriteLine($"Applying phase {p + 1}");
                //    ApplyPhase(originalCode);
            }

            sb.Clear();
            foreach (var n in state.Slice(0, 8))
                sb.Append(n.ToString());
            return sb.ToString();
        }

        private unsafe void ApplyPhase(Span<int> numbers)
        {
            var length = numbers.Length;
            var sw = new Stopwatch();
            sw.Start();
            // Process first half of all the numbers
            for (int i = 0; i < length / 2; i++)
            {
                var sum = 0;
                var processed = i; // Skip i characters, since they are all 0
                var repetitions = i + 1;
                while (processed < length)
                {
                    unchecked
                    {
                        Span<int> slice;
                        if (processed + repetitions < length)
                            slice = numbers.Slice(processed, repetitions);
                        else if (processed >= length) break;
                        else
                            slice = numbers.Slice(processed, length - processed);

                        // add
                        int l = slice.Length;
                        for (int j = 0; j < l; j++)
                        {
                            sum += slice[j];
                        }
                        processed += l + repetitions;

                        if (processed + repetitions < length)
                            slice = numbers.Slice(processed, repetitions);
                        else if (processed >= length) break;
                        else
                            slice = numbers.Slice(processed, length - processed);

                        // subtract
                        l = slice.Length;
                        for (int j = 0; j < l; j++)
                        {
                            sum -= slice[j];
                        }
                        processed += l + repetitions;
                    }
                }

                if (i % 10000 == 0) Console.WriteLine(i);

                //while (processed < length - i)
                //{
                //    var repetitions = i + 1;

                //    var slice = numbers.Slice(processed, repetitions);
                //    switch (ret)
                //    {
                //        case 1:
                //            for (int j = 0; j < repetitions; j++)
                //            {
                //                sum += slice[j];
                //            }
                //            processed += repetitions;
                //            break;
                //        case -1:
                //            for (int j = 0; j < repetitions; j++)
                //            {
                //                sum -= slice[j];
                //            }
                //            processed += repetitions;
                //            break;
                //    }

                //    basePatternIdx += basePatternIdx < basePatternLength - 1 ? 1 : -3;
                //    ret = basePattern[basePatternIdx];

                //    if (ret == 0)
                //    {
                //        processed += repetitions;
                //        basePatternIdx += basePatternIdx < basePatternLength - 1 ? 1 : -3;
                //        ret = basePattern[basePatternIdx];
                //    }
                //}

                //if (ret == 1)
                //{
                //    for (int j = 0; j < i + 1 && processed < length; j++)
                //    {
                //        sum += numbers[processed++];
                //    }
                //}
                //else if (ret == -1)
                //{
                //    for (int j = 0; j < i + 1 && processed < length; j++)
                //    {
                //        sum -= numbers[processed++];
                //    }
                //}

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