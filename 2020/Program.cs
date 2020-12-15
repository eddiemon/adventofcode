using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

//var input = File.ReadAllLines("../../../14.in");
var spoken = new Dictionary<int, List<int>>();

int turn = 1;
int lastSpoken = 0;
Say(0);
Say(5);
Say(4);
Say(1);
Say(10);
Say(14);
Say(7);

//Say(0);
//Say(3);
//Say(6);

while (turn < 30000000)
{
    Say(WhatToSay());
}

Console.WriteLine(WhatToSay());

int WhatToSay()
{
    if (spoken[lastSpoken].Count == 1)
        return 0;
    var last = spoken[lastSpoken][^1];
    var secondLast = spoken[lastSpoken][^2];
    return last - secondLast;
}

void Say(int n)
{
    if (!spoken.ContainsKey(n))
        spoken[n] = new List<int>();

    spoken[n].Add(turn);
    lastSpoken = n;
    turn++;
}
