using System.IO;
using System.Linq;

namespace aoc
{
    public class d22
    {
        public string Answer { get {
            
            var instructions = File.ReadAllText("d2.txt").Split(',').Select(x => int.Parse(x)).ToArray();

            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    var workingMemory = instructions.ToArray();
                    workingMemory[1] = noun;
                    workingMemory[2] = verb;
                    try
                    {
                        int result = RunIntCode(workingMemory);
                        if (result == 19690720)
                        {
                            result = 100 * noun + verb;
                            return result.ToString();;
                        }
                    }
                    catch
                    {
                        continue;
                    }

                }
            }

            return string.Empty;
        }}

        int RunIntCode(int[] memory)
        {

            int iptr = 0;
            while (true)
            {
                if (memory[iptr] == 1)
                {
                    var a = memory[iptr + 1];
                    var b = memory[iptr + 2];
                    var outLocation = memory[iptr + 3];
                    memory[outLocation] = memory[a] + memory[b];
                }
                else if (memory[iptr] == 2)
                {
                    var a = memory[iptr + 1];
                    var b = memory[iptr + 2];
                    var outLocation = memory[iptr + 3];
                    memory[outLocation] = memory[a] * memory[b];
                }
                else if (memory[iptr] == 99)
                {
                    break;
                }
                else
                {
                    throw new System.Exception();
                }
                iptr += 4;
            }
            return memory[0];
        }
    }
}