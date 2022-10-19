using System;
using System.Collections.Generic;

public class Kata
{
    public static Dictionary<string, int> ParseMolecule(string formula)
    {
        var atomCount = new Dictionary<string, int>();
        var openBrackets = new List<char> {'[', '{', '('};
        var closedBrackets = new List<char> {']', '}', ')'};

        for (int i = 0; i < formula.Length;)
        {
            // Is an atom
            if (char.IsUpper(formula[i]))
            {
                int multiplier = 1;

                // Atom starts with capital letter followed by any number of lower case letters
                string atom = formula[i++].ToString();
                while (i < formula.Length && char.IsLower(formula[i]))
                {
                    atom += formula[i++];
                }

                // Atom could be followed by a number
                if (i < formula.Length && char.IsNumber(formula[i]))
                {
                    string num = char.ToString(formula[i++]);
                    while (i < formula.Length && char.IsNumber(formula[i]))
                    {
                        num += formula[i++];
                    }
                    multiplier = Int32.Parse(num);
                }

                // Add atom times number behind to the count
                atomCount.TryGetValue(atom, out int prevCount);
                atomCount[atom] = prevCount + multiplier;
            }

            // If not an atom, then it should be an open bracket
            else if(openBrackets.Contains(formula[i]))
            {
                int startInd = ++i;
                int currentLevel = 1; // "bracket level"

                // Increase index until we find matching closing bracket
                while(currentLevel > 0)
                {
                    if(openBrackets.Contains(formula[i]))
                        currentLevel++;

                    else if(closedBrackets.Contains(formula[i]))
                        currentLevel--;
                    i++;
                }

                // Parse the substring between the matching brackets
                var d = ParseMolecule(formula.Substring(startInd, i-startInd-1));

                // Bracket could be followed by number, where we should multiply atom count in bracket
                // by that number
                if(i < formula.Length && char.IsNumber(formula[i]))
                {
                    string numString = formula[i++].ToString();
                    while(i < formula.Length && char.IsNumber(formula[i]))
                    {
                        numString += formula[i++];
                    }
                    int num = Int32.Parse(numString);
                    foreach(var item in d)
                    {
                        d[item.Key] *= num;
                    }
                }

                // Add the number of atoms to the count
                foreach(var item in d)
                {
                    atomCount.TryGetValue(item.Key, out int prevCount);
                    atomCount[item.Key] = prevCount + d[item.Key];
                }
            }
            else
            {
                i++;
            }
        }
        return atomCount;
    }
}