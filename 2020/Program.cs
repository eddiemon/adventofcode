using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using aoc;

var input = File.ReadAllLines("../../../21.in");

var allergenToFoods = new List<(string[] Allergens, string[] Ingredients)>();
foreach (var l in input)
{
    var m = Regex.Match(l, @"^(?<ingredients>[^\(]+)\(contains (?<allergen>[^\)]+)\)$");
    var allergens = m.Groups["allergen"].Value.Split(", ");
    var ingredients = m.Groups["ingredients"].Value.Trim().Split(' ');
    allergenToFoods.Add((allergens, ingredients));
}

var allAllergens = allergenToFoods.SelectMany(kvp => kvp.Allergens).Distinct();
var allergensToCandidateIngredients = new Dictionary<string, List<string>>();
foreach (var allergen in allAllergens)
{
    var matchingFoods = allergenToFoods.Where(x => x.Ingredients.Contains(allergen)).Select(kvp => kvp.Ingredients.ToList()).ToList();
    var x = matchingFoods.Aggregate((x, y) => x.Intersect(y).ToList());
    allergensToCandidateIngredients.Add(allergen, x);
}

var ingredientsWithoutAllergens = allergenToFoods
    .SelectMany(kvp => kvp.Ingredients)
    .Where(ingredient => !allergensToCandidateIngredients.SelectMany(x => x.Value).Distinct().Contains(ingredient))
    .ToList();

var allergensToIngredient = new Dictionary<string, string>();
do
{
    var allergensWithOneCandidate = allergensToCandidateIngredients.Where(kvp => kvp.Value.Count == 1);

    foreach (var (allergen, ingredients) in allergensWithOneCandidate)
    {
        allergensToIngredient.Add(allergen, ingredients[0]);
        allergensToCandidateIngredients.Remove(allergen);
        foreach (var x in allergensToCandidateIngredients.Values.Where(v => v.Contains(ingredients[0])))
        {
            x.Remove(ingredients[0]);
        }
    }
}
while (allergensToCandidateIngredients.Count > 0);

Console.WriteLine(string.Join(',', allergensToIngredient.ToList().OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value)));
