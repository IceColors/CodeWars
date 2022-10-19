using Xunit;
using System;
using System.Collections.Generic;

public class Solution
{
    [Fact]
    public void DescriptionExampleTests()
    {
      Assert.Equal(new Dictionary<string, int> {{"H", 2}, {"O", 1}}, Kata.ParseMolecule("H2O"));
      Assert.Equal(new Dictionary<string, int> {{"Mg", 1}, {"O", 2}, {"H", 2}}, Kata.ParseMolecule("Mg(OH)2"));
      Assert.Equal(new Dictionary<string, int> {{"K", 4}, {"O", 14}, {"N", 2}, {"S", 4}}, Kata.ParseMolecule("K4[ON(SO3)2]2"));
      Assert.Equal(new Dictionary<string, int> {{"O", 48}, {"Co", 24}, {"Be", 16}, {"Cu", 5}, {"C", 44}, {"B", 8}, {"As", 2}}, Kata.ParseMolecule("As2{Be4C5[BCo3(CO2)3]2}4Cu5"));
      Assert.Equal(new Dictionary<string, int> {{"C", 4}, {"O", 4}, {"H", 4}}, Kata.ParseMolecule("C2H2(COOH)2"));
      
    }
}