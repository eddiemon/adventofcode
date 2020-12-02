using System;
using System.IO;
using System.Linq;

public class D1
{
    public D1()
    {
        var input = File.ReadAllLines("../../../1.in").Select(l => int.Parse(l)).ToList();

        for (int i = 0; i < input.Count - 2; i++)
        {
            for (int j = i + 1; j < input.Count - 1; j++)
            {
                for (int k = j + 1; k < input.Count; k++)
                {
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
