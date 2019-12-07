using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace aoc2019
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(await new D72().Answer());
        }
    }
}
