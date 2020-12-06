using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../6.in");

var currentlyAnsweredQuestions = new Dictionary<char, int>();
var numberOfPeopleInGroup = 0;
var totalAnsweredQuestions = 0;

foreach (var l in input)
{
    if (string.IsNullOrEmpty(l))
    {
        foreach (var (answer, count) in currentlyAnsweredQuestions)
        {
            if (count == numberOfPeopleInGroup)
                totalAnsweredQuestions++;
        }

        currentlyAnsweredQuestions.Clear();
        numberOfPeopleInGroup = 0;
        continue;
    }
    else
    {
        numberOfPeopleInGroup++;
    }

    for (int i = 0; i < l.Length; i++)
    {
        currentlyAnsweredQuestions[l[i]] = currentlyAnsweredQuestions.TryGetValue(l[i], out int count) ? count + 1 : 1;
    }
}

foreach (var (answer, count) in currentlyAnsweredQuestions)
{
    if (count == numberOfPeopleInGroup)
        totalAnsweredQuestions++;
}

Console.WriteLine(totalAnsweredQuestions);
