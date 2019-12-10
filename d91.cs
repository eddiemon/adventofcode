using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace aoc2019
{
    public class D91
    {

        public string Answer
        {
            get
            {
                var instructions = File.ReadAllText("d9test.txt").Split(',');

                var inputs = new Queue<string>();
                //inputs.Enqueue("0");

                var computer = new IntCodeComputer(instructions);
                computer.RunWithInput(inputs);
                return computer.Outputs.Last();
            }
        }

        private class IntCodeComputer
        {
            private const string OpCodeAdd = "01";
            private const string OpCodeMultiply = "02";
            private const string OpCodeInput = "03";
            private const string OpCodeOutput = "04";
            private const string OpCodeJumpIfNotZero = "05";
            private const string OpCodeJumpIfZero = "06";
            private const string OpCodeLessThan = "07";
            private const string OpCodeEquals = "08";
            private const string OpCodeRelativeBaseOffset = "09";
            private const string OpCodeBreak = "99";

            private readonly string[] Memory;
            private readonly Dictionary<string, int> BigMemory = new Dictionary<string, int>();
            public readonly List<string> Outputs = new List<string>();
            private int RelativeBaseAddress = 0;

            public IntCodeComputer(string[] memory)
            {
                Memory = memory;
            }

            public enum ParameterMode
            {
                Positional,
                Immediate,
                Relative
            }

            private List<ParameterMode> ParseParameterModes(string parameterModes, int numModes)
            {
                var modes = new List<ParameterMode>();
                for (var i = 0; i < numModes; i++)
                {
                    var offset = parameterModes.Length - 1 - i;
                    if (offset < 0) modes.Add(ParameterMode.Positional);
                    else if (parameterModes[offset] == '0') modes.Add(ParameterMode.Positional);
                    else if (parameterModes[offset] == '1') modes.Add(ParameterMode.Immediate);
                    else if (parameterModes[offset] == '2') modes.Add(ParameterMode.Relative);
                    else Debug.Assert(false);
                }
                return modes;
            }

            private void SetValueInMemory(int theValue, int address, ParameterMode mode)
            {
                int absoluteAddress = 0;
                switch (mode)
                {
                    case ParameterMode.Relative:
                        var offset = GetValueFromMemory(address, ParameterMode.Immediate);
                        absoluteAddress = RelativeBaseAddress + offset;
                        break;
                    case ParameterMode.Positional:
                    case ParameterMode.Immediate:
                        //var relative = GetValueFromMemory(address, ParameterMode.Immediate);
                        absoluteAddress = GetValueFromMemory(address, ParameterMode.Immediate);
                        break;
                }
                Debug.Assert(absoluteAddress >= 0);
                if (absoluteAddress >= Memory.Length) BigMemory[absoluteAddress.ToString()] = theValue;
                else Memory[absoluteAddress] = theValue.ToString();
            }

            private int GetValueFromMemory(int address, ParameterMode mode)
            {
                int absoluteAddress = 0;
                switch (mode)
                {
                    case ParameterMode.Relative:
                        var offset = GetValueFromMemory(address, ParameterMode.Immediate);
                        absoluteAddress = RelativeBaseAddress + offset;
                        break;

                    case ParameterMode.Positional:
                        absoluteAddress = GetValueFromMemory(address, ParameterMode.Immediate);
                        break;

                    case ParameterMode.Immediate:
                        absoluteAddress = address;
                        break;
                }
                Debug.Assert(absoluteAddress >= 0);
                if (absoluteAddress >= Memory.Length)
                    return BigMemory.ContainsKey(absoluteAddress.ToString()) ? BigMemory[absoluteAddress.ToString()] : 0;
                else
                    return Memory[absoluteAddress].AsInt();
            }

            public void RunWithInput(Queue<string> inputs)
            {
                int iptr = 0;
                while (true)
                {
                    var opCode = Memory[iptr];

                    var parameterModes = opCode.Length > 2 ? opCode.Substring(0, opCode.Length - 2) : "";

                    if (opCode.EndsWith(OpCodeAdd))
                    {
                        var modes = ParseParameterModes(parameterModes, 3);
                        var a = GetValueFromMemory(iptr + 1, modes[0]);
                        var b = GetValueFromMemory(iptr + 2, modes[1]);
                        checked
                        {
                            SetValueInMemory(a + b, iptr + 3, modes[2]);
                        }
                        iptr += 4;
                    }
                    else if (opCode.EndsWith(OpCodeMultiply))
                    {
                        var modes = ParseParameterModes(parameterModes, 3);
                        var a = GetValueFromMemory(iptr + 1, modes[0]);
                        var b = GetValueFromMemory(iptr + 2, modes[1]);
                        checked
                        {
                            SetValueInMemory(a * b, iptr + 3, modes[2]);
                        }
                        iptr += 4;
                    }
                    else if (opCode.EndsWith(OpCodeInput))
                    {
                        var input = inputs.Dequeue();
                        var modes = ParseParameterModes(parameterModes, 1);
                        SetValueInMemory(input.AsInt(), iptr + 3, modes[0]);
                        iptr += 2;
                    }
                    else if (opCode.EndsWith(OpCodeOutput))
                    {
                        var modes = ParseParameterModes(parameterModes, 1);
                        var output = GetValueFromMemory(iptr + 1, modes[0]);
                        Outputs.Add(output.ToString());
                        iptr += 2;
                    }
                    else if (opCode.EndsWith(OpCodeJumpIfNotZero))
                    {
                        var modes = ParseParameterModes(parameterModes, 2);
                        var value = GetValueFromMemory(iptr + 1, modes[0]);
                        if (value != 0)
                        {
                            var jumpAddress = GetValueFromMemory(iptr + 2, modes[1]);
                            iptr = jumpAddress;
                        }
                        else iptr += 3;
                    }
                    else if (opCode.EndsWith(OpCodeJumpIfZero))
                    {
                        var modes = ParseParameterModes(parameterModes, 2);
                        var value = GetValueFromMemory(iptr + 1, modes[0]);
                        if (value == 0)
                        {
                            var jumpAddress = GetValueFromMemory(iptr + 2, modes[1]);
                            iptr = jumpAddress;
                        }
                        else iptr += 3;
                    }
                    else if (opCode.EndsWith(OpCodeLessThan))
                    {
                        var modes = ParseParameterModes(parameterModes, 3);
                        var a = GetValueFromMemory(iptr + 1, modes[0]);
                        var b = GetValueFromMemory(iptr + 2, modes[1]);
                        if (a < b) SetValueInMemory(1, iptr + 3, modes[2]);
                        else SetValueInMemory(0, iptr + 3, modes[2]);
                        iptr += 4;
                    }
                    else if (opCode.EndsWith(OpCodeEquals))
                    {
                        var modes = ParseParameterModes(parameterModes, 3);
                        var a = GetValueFromMemory(iptr + 1, modes[0]);
                        var b = GetValueFromMemory(iptr + 2, modes[1]);
                        if (a == b) SetValueInMemory(1, iptr + 3, modes[2]);
                        else SetValueInMemory(0, iptr + 3, modes[2]);
                        iptr += 4;
                    }
                    else if (opCode.EndsWith(OpCodeRelativeBaseOffset))
                    {
                        var modes = ParseParameterModes(parameterModes, 1);
                        var offset = GetValueFromMemory(iptr + 1, modes[0]);
                        RelativeBaseAddress += offset;
                        iptr += 2;
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
            }
        }
    }
}