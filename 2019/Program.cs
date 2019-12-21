using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace aoc
{
    public interface ISolution
    {
        string Answer { get; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new D17().Answer());
        }
    }
}
