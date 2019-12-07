using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace aoc2019
{
    public class D72
    {
        private const string OpCodeAdd = "1";
        private const string OpCodeMultiply = "2";
        private const string OpCodeInput = "3";
        private const string OpCodeOutput = "4";
        private const string OpCodeJumpIfNotZero = "5";
        private const string OpCodeJumpIfZero = "6";
        private const string OpCodeLessThan = "7";
        private const string OpCodeEquals = "8";
        private const string OpCodeBreak = "99";

        public async Task<string> Answer()
        {
            var instructions = File.ReadAllText("d7.txt").Split(',');

            int maxOutput = int.MinValue;
            var maxInputCombo = "";

            foreach (var (i, j, k, l, m) in InputCombinations())
            {
                var aStdIn = new ConcurrentQueue<string>();
                var aStdOut = new ConcurrentQueue<string>();
                var bStdOut = new ConcurrentQueue<string>();
                var cStdOut = new ConcurrentQueue<string>();
                var dStdOut = new ConcurrentQueue<string>();

                aStdIn.Enqueue(i.ToString());
                aStdIn.Enqueue("0");
                aStdOut.Enqueue(j.ToString());
                bStdOut.Enqueue(k.ToString());
                cStdOut.Enqueue(l.ToString());
                dStdOut.Enqueue(m.ToString());

                try
                {
                    var a = RunIntCode(instructions.ToArray(), aStdIn, aStdOut);
                    var b = RunIntCode(instructions.ToArray(), aStdOut, bStdOut);
                    var c = RunIntCode(instructions.ToArray(), bStdOut, cStdOut);
                    var d = RunIntCode(instructions.ToArray(), cStdOut, dStdOut);
                    var e = RunIntCode(instructions.ToArray(), dStdOut, aStdIn);

                    Task.WaitAll(a, b, c, d, e);
                    aStdIn.TryDequeue(out var output);

                    if (output.AsInt() > maxOutput)
                    {
                        maxOutput = output.AsInt();
                        maxInputCombo = $"{i}{j}{k}{l}{m}";
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
            }
            return maxOutput.ToString();
        }

        public IEnumerable<(int, int, int, int, int)> InputCombinations()
        {
            const int minInput = 5;
            const int maxInput = 9;
            for (int i = minInput; i < maxInput + 1; i++)
            {
                for (int j = minInput; j < maxInput + 1; j++)
                {
                    if (j == i) continue;
                    for (int k = minInput; k < maxInput + 1; k++)
                    {
                        if (k == j || k == i) continue;
                        for (int l = minInput; l < maxInput + 1; l++)
                        {
                            if (l == k || l == j || l == i) continue;
                            for (int m = minInput; m < maxInput + 1; m++)
                            {
                                if (m == l || m == k || m == j || m == i) continue;
                                yield return (i, j, k, l, m);
                            }
                        }
                    }
                }
            }
        }

        public enum ParameterMode
        {
            Positional,
            Immediate
        }

        private Task RunIntCode(string[] memory, ConcurrentQueue<string> stdIn, ConcurrentQueue<string> stdOut)
        {
            return Task.Factory.StartNew(() =>
            {
                int iptr = 0;
                while (true)
                {
                    var opCode = memory[iptr];

                    var parameterModes = opCode.Length > 2 ? opCode.Substring(0, opCode.Length - 2) : "";
                    var firstParameterMode = parameterModes.Length > 0 ? parameterModes[parameterModes.Length - 1] == '1' ? ParameterMode.Immediate : ParameterMode.Positional : ParameterMode.Positional;
                    var secondParameterMode = parameterModes.Length > 1 ? parameterModes[parameterModes.Length - 2] == '1' ? ParameterMode.Immediate : ParameterMode.Positional : ParameterMode.Positional;

                    if (opCode.EndsWith(OpCodeAdd))
                    {
                        var a = GetValueFromMemory(memory, iptr + 1, firstParameterMode);
                        var b = GetValueFromMemory(memory, iptr + 2, secondParameterMode);
                        var outLocation = memory[iptr + 3].AsInt();
                        memory[outLocation] = (a + b).ToString();
                        iptr += 4;
                    }
                    else if (opCode.EndsWith(OpCodeMultiply))
                    {
                        var a = GetValueFromMemory(memory, iptr + 1, firstParameterMode);
                        var b = GetValueFromMemory(memory, iptr + 2, secondParameterMode);
                        var outLocation = memory[iptr + 3].AsInt();
                        memory[outLocation] = (a * b).ToString();
                        iptr += 4;
                    }
                    else if (opCode.EndsWith(OpCodeInput))
                    {
                        while (stdIn.Count == 0) Thread.SpinWait(50);
                        if (!stdIn.TryDequeue(out var input)) throw new Exception("Could not read from input");

                        var saveLocation = memory[iptr + 1].AsInt();
                        memory[saveLocation] = input;

                        iptr += 2;
                    }
                    else if (opCode.EndsWith(OpCodeOutput))
                    {
                        var outLocation = memory[iptr + 1].AsInt();
                        var output = memory[outLocation];

                        stdOut.Enqueue(output);

                        iptr += 2;
                    }
                    else if (opCode.EndsWith(OpCodeJumpIfNotZero))
                    {
                        var value = GetValueFromMemory(memory, iptr + 1, firstParameterMode);
                        if (value != 0)
                        {
                            var jumpAddress = GetValueFromMemory(memory, iptr + 2, secondParameterMode);
                            iptr = jumpAddress;
                        }
                        else iptr += 3;
                    }
                    else if (opCode.EndsWith(OpCodeJumpIfZero))
                    {
                        var value = GetValueFromMemory(memory, iptr + 1, firstParameterMode);
                        if (value == 0)
                        {
                            var jumpAddress = GetValueFromMemory(memory, iptr + 2, secondParameterMode);
                            iptr = jumpAddress;
                        }
                        else iptr += 3;
                    }
                    else if (opCode.EndsWith(OpCodeLessThan))
                    {
                        var a = GetValueFromMemory(memory, iptr + 1, firstParameterMode);
                        var b = GetValueFromMemory(memory, iptr + 2, secondParameterMode);
                        var outLocation = memory[iptr + 3].AsInt();
                        if (a < b) memory[outLocation] = "1";
                        else memory[outLocation] = "0";
                        iptr += 4;
                    }
                    else if (opCode.EndsWith(OpCodeEquals))
                    {
                        var a = GetValueFromMemory(memory, iptr + 1, firstParameterMode);
                        var b = GetValueFromMemory(memory, iptr + 2, secondParameterMode);
                        var outLocation = memory[iptr + 3].AsInt();
                        if (a == b) memory[outLocation] = "1";
                        else memory[outLocation] = "0";
                        iptr += 4;
                    }
                    else if (opCode.EndsWith(OpCodeBreak))
                    {
                        return;
                    }
                    else
                    {
                        throw new System.Exception();
                    }
                }
            });
        }

        private int GetValueFromMemory(string[] memory, int offset, ParameterMode parameterMode)
        {
            if (parameterMode == ParameterMode.Positional)
                return memory[memory[offset].AsInt()].AsInt();
            else
                return memory[offset].AsInt();
        }
    }
}