using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using aoc;

const long cardKey = 15113849;
const long doorKey = 4206373;

var subjNo = 7L;
var cardValue = 1L;
var cardLoopSize = 0L;
for (; cardValue != cardKey; cardLoopSize++)
{
    cardValue *= subjNo;
    cardValue %= 20201227;
}

var doorValue = 1L;
var doorLoopSize = 0L;
for (; doorValue != doorKey; doorLoopSize++)
{
    doorValue *= subjNo;
    doorValue %= 20201227;
}

Console.WriteLine(Transform(doorKey, cardLoopSize));
Console.WriteLine(Transform(cardKey, doorLoopSize));

long Transform(long key, long loops)
{
    var value = 1L;
    for (int i = 0; i < loops; i++)
    {
        value *= key;
        value %= 20201227;
    }
    return value;
}
