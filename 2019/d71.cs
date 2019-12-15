using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc
{
    public class D71
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

        public string Answer
        {
            get
            {
                var instructions = File.ReadAllText("d7.txt").Split(',');

                int maxOutput = int.MinValue;
                string maxInputCombo = "";
                foreach (var (i,j,k,l,m) in InputCombinations())
                {
                    var inputs = new Queue<string>();
                    inputs.Enqueue(i.ToString());
                    inputs.Enqueue("0");
                    try
                    {
                        var outputs = RunIntCode(instructions.ToArray(), inputs); // A
                        outputs.Insert(0, j.ToString());
                        outputs = RunIntCode(instructions.ToArray(), new Queue<string>(outputs)); // B
                        outputs.Insert(0, k.ToString());
                        outputs = RunIntCode(instructions.ToArray(), new Queue<string>(outputs)); // C
                        outputs.Insert(0, l.ToString());
                        outputs = RunIntCode(instructions.ToArray(), new Queue<string>(outputs)); // D
                        outputs.Insert(0, m.ToString());
                        outputs = RunIntCode(instructions.ToArray(), new Queue<string>(outputs)); // E
                        var output = outputs[0].AsInt();
                        if (output > maxOutput)
                        {
                            maxOutput = output;
                            maxInputCombo = $"{i}{j}{k}{l}{m}";
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                return maxOutput.ToString();
            }
        }

        public IEnumerable<(int, int, int, int, int)> InputCombinations()
        {
            const int maxInput = 5;
            for (int i = 0; i < maxInput; i++)
            {
                for (int j = 0; j < maxInput; j++)
                {
                    if (j == i) continue;
                    for (int k = 0; k < maxInput; k++)
                    {
                        if (k == j || k == i) continue;
                        for (int l = 0; l < maxInput; l++)
                        {
                            if (l == k || l == j || l == i) continue;
                            for (int m = 0; m < maxInput; m++)
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

        List<string> RunIntCode(string[] memory, Queue<string> inputs)
        {
            var outputs = new List<string>();
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
                    var input = inputs.Dequeue();

                    var saveLocation = memory[iptr + 1].AsInt();
                    memory[saveLocation] = input;

                    iptr += 2;
                }
                else if (opCode.EndsWith(OpCodeOutput))
                {
                    var outLocation = memory[iptr + 1].AsInt();
                    var testCode = memory[outLocation].AsInt();

                    outputs.Add(testCode.ToString());
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
                    break;
                }
                else
                {
                    throw new System.Exception();
                }
            }
            return outputs;
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