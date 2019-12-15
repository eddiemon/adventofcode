using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading;

namespace aoc
{
    public class IntCode
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

        private readonly BigInteger[] Memory;
        private readonly Dictionary<BigInteger, BigInteger> BigMemory = new Dictionary<BigInteger, BigInteger>();
        private BigInteger RelativeBaseAddress = 0;

        public readonly ConcurrentQueue<BigInteger> StdIn = new ConcurrentQueue<BigInteger>();
        public readonly ConcurrentQueue<BigInteger> StdOut = new ConcurrentQueue<BigInteger>();

        public event Action Halted;

        public IntCode(BigInteger[] memory)
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

        public void Run()
        {
            BigInteger iptr = 0;
            while (true)
            {
                var opCode = Memory[(int)iptr].ToString("D2");

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
                    while (StdIn.Count == 0) Thread.Sleep(1);

                    StdIn.TryDequeue(out var input);

                    var modes = ParseParameterModes(parameterModes, 1);
                    SetValueInMemory(input, iptr + 1, modes[0]);
                    iptr += 2;
                }
                else if (opCode.EndsWith(OpCodeOutput))
                {
                    var modes = ParseParameterModes(parameterModes, 1);
                    var output = GetValueFromMemory(iptr + 1, modes[0]);
                    StdOut.Enqueue(output);
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
                    Halted?.Invoke();
                    break;
                }
                else
                {
                    throw new System.Exception();
                }
            }
        }

        private void SetValueInMemory(BigInteger theValue, BigInteger address, ParameterMode mode)
        {
            BigInteger absoluteAddress = 0;
            switch (mode)
            {
                case ParameterMode.Relative:
                    var offset = GetValueFromMemory(address, ParameterMode.Immediate);
                    absoluteAddress = RelativeBaseAddress + offset;
                    break;
                case ParameterMode.Positional:
                case ParameterMode.Immediate:
                    absoluteAddress = GetValueFromMemory(address, ParameterMode.Immediate);
                    break;
            }
            Debug.Assert(absoluteAddress >= 0);
            if (absoluteAddress >= Memory.Length) BigMemory[absoluteAddress] = theValue;
            else Memory[(int)absoluteAddress] = theValue;
        }

        private BigInteger GetValueFromMemory(BigInteger address, ParameterMode mode)
        {
            BigInteger absoluteAddress = 0;
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
                return BigMemory.ContainsKey(absoluteAddress) ? BigMemory[absoluteAddress] : 0;
            else
                return Memory[(int)absoluteAddress];
        }
    }
}
