using System;
using System.Collections.Generic;

public class Calc
{
	public double evaluate(String expr)
	{
		var stack = new Stack<double>();

		// Iterate over all tokens
		foreach(var token in expr.Split(" "))
		{
			// If token is a number then add it to the stack
			if(double.TryParse(token, out _))
				stack.Push(double.Parse(token));
			
			// Else, then the token is an operation which require previous numbers which we get from stack
			// then add the result to the stack
			switch(token)
			{
				case "*":
					stack.Push(stack.Pop() * stack.Pop());
					break;

				case "/":
					// Use a temp variable to do the division in a different order than
					// popping stack would do
					var divisor = stack.Pop();
					stack.Push(stack.Pop() / divisor);
					break;
				
				case "+":
					stack.Push(stack.Pop() + stack.Pop());
					break;

				case "-":
					// Use a temp variable to do the substraction in a different order
					// than popping stack would do
					var substrahend = stack.Pop();
					stack.Push(stack.Pop() - substrahend);
					break;
			}
		}
		// The final result should be the only item left in stack
		stack.TryPop(out double returnValue);
		return returnValue;
	}
}