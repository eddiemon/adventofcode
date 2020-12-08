using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

//nop +0
//acc +1
//jmp +4
var input = File.ReadAllLines("../../../8.in");

var originalInstructions = new List<Instruction>();
foreach (var instruction in input)
{
    var op = instruction[0..3];
    var count = int.Parse(instruction[4..]);
    originalInstructions.Add(new Instruction((OpCode)Enum.Parse(typeof(OpCode), op), count));
}

for (int i = 0; i < originalInstructions.Count; i++)
{
    var modifyInstruction = originalInstructions[i];
    if (modifyInstruction.op == OpCode.acc)
        continue;

    var instructions = originalInstructions.ToList();
    instructions[i] = modifyInstruction.op switch
    {
        OpCode.nop => new Instruction(OpCode.jmp, modifyInstruction.count),
        OpCode.jmp => new Instruction(OpCode.nop, modifyInstruction.count),
    };

    var accumulator = 0;
    var visited = new List<int>();
    var iPtr = 0;

    while (true)
    {
        visited.Add(iPtr);
        var instruction = instructions[iPtr];

        switch (instruction.op)
        {
            case OpCode.nop:
                iPtr++;
                break;

            case OpCode.acc:
                accumulator += instruction.count;
                iPtr++;
                break;

            case OpCode.jmp:
                iPtr = iPtr + instruction.count;
                break;
        }

        if (iPtr >= instructions.Count)
        {
            Console.WriteLine(accumulator);
            return;
        }

        if (visited.Contains(iPtr))
            break;
    }
}

internal enum OpCode
{ nop, jmp, acc };

record Instruction(OpCode op, int count);
