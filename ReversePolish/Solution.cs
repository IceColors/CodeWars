using System;
using Xunit;

public class CalcTest
{
	private Calc calc = new Calc();

	[Fact]
	public void ShouldWorkWithEmptyString()
	{
		Xunit.Assert.Equal(0, calc.evaluate(""));
	}

	[Fact]
	public void ShouldParseNumbers()
	{
		Assert.Equal(3, calc.evaluate("3"));
	}

	[Fact]
	public void ShouldParseFloatNumbers()
	{
		Assert.Equal(3.5, calc.evaluate("3.5"));
	}

	[Fact]
	public void ShouldSupportAddition()
	{
		Assert.Equal(4, calc.evaluate("1 3 +"));
	}

	[Fact]
	public void ShouldSupportMultiplication()
	{
		Assert.Equal(3, calc.evaluate("1 3 *"));
	}

	[Fact]
	public void ShouldSupportSubstraction()
	{
		Assert.Equal(-2, calc.evaluate("1 3 -"));
	}

	[Fact]
	public void ShouldSupportDivision()
	{
		Assert.Equal(2, calc.evaluate("4 2 /"));
	}
}