using Xunit;
using System;

public class SolutionTest
{
    [Theory]
    [InlineData("this is a string!!", "dGhpcyBpcyBhIHN0cmluZyEh")]
    [InlineData("ee", "ZWU=")]
    [InlineData("e", "ZQ==")]
    [InlineData("", "")]
    public void SampleValueEncodeTest(string value, string expected)
    {
        Assert.Equal(expected, Base64Utils.ToBase64(value));
    }

    [Theory]
    [InlineData("dGhpcyBpcyBhIHN0cmluZyEh", "this is a string!!")]
    [InlineData("ZWU=", "ee")]
    [InlineData("ZQ==", "e")]
    [InlineData("", "")]
    public void SampleValueDecodeTest(string value, string expected)
    {
        Assert.Equal(expected, Base64Utils.FromBase64(value));
    }

}