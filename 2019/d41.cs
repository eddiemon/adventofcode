using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace aoc
{
    public static class StringExtension
    {
        public static int AsInt(this string s)
        {
            return int.Parse(s);
        }
    }

    public class D41
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
                var instructions = File.ReadAllText("d4.txt").Split(',');

                Queue<string> inputs = new Queue<string>();
                inputs.Enqueue("5");
                var result = RunIntCode(instructions, inputs);

                return result.Last().ToString();
            }
        }

        public enum ParameterMode
        {
            Positional,
            Immediate
        }

        List<int> RunIntCode(string[] memory, Queue<string> inputs)
        {
            var outputs = new List<int>();
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

                    outputs.Add(testCode);
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