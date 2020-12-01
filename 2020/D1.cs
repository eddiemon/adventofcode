using System;
using System.IO;
using System.Linq;

public class D1
{
    public D1()
    {
        var input = File.ReadAllLines("../../../1.in").Select(l => int.Parse(l)).ToList();

        for (int i = 0; i < input.Count; i++)
        {
            for (int j = 0; j < input.Count; j++)
            {
                if (i == j) continue;

                for (int k = 0; k < input.Count; k++)
                {
                    if (i == k || j == k) continue;

                    var s = input[i] + input[j] + input[k];
                    if (s == 2020)
                    {
                        Console.WriteLine(input[i] * input[j] * input[k]);
                        return;
                    }
                }
            }
        }
    }
}
