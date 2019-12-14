using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace aoc2019
{
    public interface ISolution
    {
        string Answer { get; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new D14().Answer());
        }
    }
}
