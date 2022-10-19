using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class PrefixDiff // Class name required
{
    // These require two things, others require one
    static List<string> twoSidedOps = new List<string> { "+", "-", "*", "/", "^" };

    // Interface for an expression, i.e. what is between matching parentheses
    // Expression is either a number, x, or a operation which contains one or two additional expressions
    public interface IExpr
    {
        // Derivates the expression
        public IExpr Derivative();

        // Simplifies expression by reordering, evaluates expressions and removes unnecessary stuff
        public IExpr Fix();

        // Prints out expression
        public string Print();

        // Returns true if x or operands contain x
        public bool ContainsX();

        // Returns true if expression is a single number
        public bool IsNumeric() { return false; }

        // Returns true if expression is zero
        public bool IsZero() { return false; }

        // Returns true if expression is one
        public bool IsOne() { return false; }
    }

    public class Plus : IExpr
    {
        // Left and right operands
        IExpr left, right;
        public Plus(IExpr _left, IExpr _right)
        {
            left = _left;
            right = _right;
        }

        public bool ContainsX()
        {
            return left.ContainsX() || right.ContainsX();
        }

        public IExpr Derivative()
        {
            // Derivative of plus is just derivative of operands added together
            return new Plus(left.Derivative(), right.Derivative());
        }

        public IExpr Fix()
        {
            var u = left.Fix();
            var v = right.Fix();

            // Expression can be simplified by adding two numbers together
            if (u.IsNumeric() && v.IsNumeric())
            {
                var x = u as Number ?? throw new Exception("Could not cast as Number");
                var y = v as Number ?? throw new Exception("Could not cast as Number");
                return new Number(x.Value + y.Value);
            }

            // 0s could just be removed
            if (v.IsZero()) return u;
            if (u.IsZero()) return v;

            // Reorder if one operand is another addition
            // e.g. (+ 2 (+ 2 x)) can be simplified to (+ (+ 2 2).Fix() x) and then (+ 4 x)
            if (u.IsNumeric() && v is Plus)
            {
                var vPlus = v as Plus ?? throw new Exception("Could not cast as Plus");
                if (vPlus.left.IsNumeric())
                    return new Plus(new Plus(u, vPlus.left).Fix(), vPlus.right);
                if (vPlus.right.IsNumeric())
                    return new Plus(new Plus(u, vPlus.right).Fix(), vPlus.left);
            }

            return new Plus(u, v);
        }

        public string Print()
        {
            return string.Format("({0} {1} {2})", "+", left.Print(), right.Print());
        }
    }

    public class Minus : IExpr
    {
        // Left and right operands
        IExpr left, right;
        public Minus(IExpr _left, IExpr _right)
        {
            left = _left;
            right = _right;
        }

        public bool ContainsX()
        {
            return left.ContainsX() || right.ContainsX();
        }

        public IExpr Derivative()
        {
            return new Minus(left.Derivative(), right.Derivative());
        }

        public IExpr Fix()
        {
            var u = left.Fix();
            var v = right.Fix();

            if (u.IsNumeric() && v.IsNumeric())
            {
                var x = u as Number ?? throw new Exception("Could not cast as Number");
                var y = v as Number ?? throw new Exception("Could not cast as Number");
                return new Number(x.Value - y.Value);
            }

            return new Minus(u, v);
        }

        public string Print()
        {
            return string.Format("({0} {1} {2})", "-", left.Print(), right.Print());
        }
    }

    public class Multiply : IExpr
    {
        // Left and right operands
        IExpr left, right;
        public Multiply(IExpr _left, IExpr _right)
        {
            left = _left;
            right = _right;
        }

        public bool ContainsX()
        {
            return left.ContainsX() || right.ContainsX();
        }

        public IExpr Derivative()
        {
            // Chain rule
            return new Plus(new Multiply(left.Derivative(), right), new Multiply(left, right.Derivative()));
        }

        public IExpr Fix()
        {
            var u = left.Fix();
            var v = right.Fix();

            if (u.IsNumeric() && v.IsNumeric())
            {
                var x = u as Number ?? throw new Exception("Could not cast as Number");
                var y = v as Number ?? throw new Exception("Could not cast as Number");
                return new Number(x.Value * y.Value);
            }

            // Multiply by 1 does nothing
            if (v.IsOne()) return u;
            if (u.IsOne()) return v;

            // Anything multiplied by 0 is 0
            if ((u.IsZero()) || (v.IsZero()))
                return new Number(0.0);

            
            if (u.IsNumeric() && v is Multiply)
            {
                var vMultiply = v as Multiply ?? throw new Exception("Could not cast as Multiply");
                if (vMultiply.left.IsNumeric())
                    return new Multiply(new Multiply(u, vMultiply.left).Fix(), vMultiply.right);
                if (vMultiply.right.IsNumeric())
                    return new Multiply(new Multiply(u, vMultiply.right).Fix(), vMultiply.left);
            }

            return new Multiply(u, v);
        }

        public string Print()
        {
            return string.Format("({0} {1} {2})", "*", left.Print(), right.Print());
        }
    }

    public class Divide : IExpr
    {
        // Left and right operands
        IExpr left, right;
        public Divide(IExpr _left, IExpr _right)
        {
            left = _left;
            right = _right;
        }

        public bool ContainsX()
        {
            return left.ContainsX() || right.ContainsX();
        }

        public IExpr Derivative()
        {
            // Rules of derivation
            return new Divide(new Minus(new Multiply(left.Derivative(), right), new Multiply(left, right.Derivative())), new Power(right, new Number(2.0)));
        }

        public IExpr Fix()
        {
            var u = left.Fix();
            var v = right.Fix();

            if (u.IsNumeric() && v.IsNumeric())
            {
                var x = u as Number ?? throw new Exception("Could not cast as Number");
                var y = v as Number ?? throw new Exception("Could not cast as Number");
                return new Number(x.Value / y.Value);
            }

            return new Divide(u, v);
        }

        public string Print()
        {
            return string.Format("({0} {1} {2})", "/", left.Print(), right.Print());
        }
    }

    public class Power : IExpr
    {
        // Left and right operands
        IExpr left, right;
        public Power(IExpr _left, IExpr _right)
        {
            left = _left;
            right = _right;
        }

        public bool ContainsX()
        {
            return left.ContainsX() || right.ContainsX();
        }

        public IExpr Derivative()
        {
            if (left.ContainsX())
            {
                Number rightNum = right as Number ?? throw new Exception("Could not cast to Number");
                if (rightNum.Value == 0.0) return new Number(0.0);
                else if (rightNum.Value == 1.0) return left.Derivative();
                else return new Multiply(new Number(rightNum.Value), new Power(left, new Number(rightNum.Value - 1)));
            }
            else
            {
                return new Multiply(new Multiply(new Ln(left), this), right.Derivative());
            }
        }

        public IExpr Fix()
        {
            var u = left.Fix();
            var v = right.Fix();

            if (u.IsNumeric() && v.IsNumeric())
            {
                var x = u as Number ?? throw new Exception("Could not cast as Number");
                var y = v as Number ?? throw new Exception("Could not cast as Number");
                return new Number(Math.Pow(x.Value, y.Value));
            }

            if (v.IsOne())
                return left;
            if (v.IsZero())
                return new Number(1.0);

            return new Power(u, v);
        }

        public string Print()
        {
            return string.Format("({0} {1} {2})", "^", left.Print(), right.Print());
        }
    }

    public class Number : IExpr
    {
        public double Value;
        public Number(string numString)
        {
            Value = double.Parse(numString);
        }

        public Number(double _number)
        {
            Value = _number;
        }

        public bool ContainsX()
        {
            return false;
        }

        public IExpr Derivative()
        {
            return new Number(0.0);
        }

        public IExpr Fix()
        {
            return this;
        }

        public string Print()
        {
            return Value.ToString();
        }

        public bool IsNumeric() { return true; }
        public bool IsZero() { return Value == 0.0; }
        public bool IsOne() { return Value == 1.0; }
    }

    public class Ln : IExpr
    {
        IExpr right;
        public Ln(IExpr _right)
        {
            right = _right;
        }

        public bool ContainsX()
        {
            return right.ContainsX();
        }

        public IExpr Derivative()
        {
            return new Divide(right.Derivative(), right);
        }

        public IExpr Fix()
        {
            return new Ln(right.Fix());
        }

        public string Print()
        {
            return string.Format("({0} {1})", "ln", right.Print());
        }
    }

    public class X : IExpr
    {
        public bool ContainsX()
        {
            return true;
        }

        public string Print()
        {
            return "x";
        }

        IExpr IExpr.Derivative()
        {
            return new Number(1.0);
        }

        IExpr IExpr.Fix()
        {
            return this;
        }
    }


    public class Cos : IExpr
    {
        IExpr right;
        public Cos(IExpr _right)
        {
            right = _right;
        }

        public bool ContainsX()
        {
            return right.ContainsX();
        }

        public IExpr Derivative()
        {
            return new Multiply(new Multiply(new Number(-1.0), right.Derivative()), new Sin(right));
        }

        public IExpr Fix()
        {
            return new Cos(right.Fix());
        }

        public string Print()
        {
            return string.Format("({0} {1})", "cos", right.Print());
        }
    }

    public class Sin : IExpr
    {
        IExpr right;
        public Sin(IExpr _right)
        {
            right = _right;
        }

        public bool ContainsX()
        {
            return right.ContainsX();
        }

        public IExpr Derivative()
        {
            return new Multiply(right.Derivative(), new Cos(right));
        }

        public IExpr Fix()
        {
            return new Sin(right.Fix());
        }

        public string Print()
        {
            return string.Format("({0} {1})", "sin", right.Print());
        }
    }

    public class Tan : IExpr
    {
        IExpr right;
        public Tan(IExpr _right)
        {
            right = _right;
        }

        public bool ContainsX()
        {
            return right.ContainsX();
        }

        public IExpr Derivative()
        {
            return new Multiply(right.Derivative(), new Power(new Cos(right), new Number(-2.0)));
        }

        public IExpr Fix()
        {
            return new Tan(right.Fix());
        }

        public string Print()
        {
            return string.Format("({0} {1})", "tan", right.Print());
        }
    }

    public class Exp : IExpr
    {
        IExpr right;
        public Exp(IExpr _right)
        {
            right = _right;
        }

        public bool ContainsX()
        {
            return right.ContainsX();
        }

        public IExpr Derivative()
        {
            return new Multiply(right.Derivative(), this);
        }

        IExpr IExpr.Fix()
        {
            return new Exp(right.Fix());
        }

        public string Print()
        {
            return string.Format("({0} {1})", "exp", right.Print());
        }
    }

    // Construct all expressions
    public IExpr Parse(IEnumerator<Match> token)
    {
        token.MoveNext();
        if (token.Current.Value == "(")
        {
            token.MoveNext();
            var op = token.Current.Value;

            // Every operation requires at least another expression
            var firstOperand = Parse(token);

            // The operations in twoSidedOps require two operands
            if (twoSidedOps.Contains(op))
            {
                var secondOperand = Parse(token);
                token.MoveNext();
                switch (op)
                {
                    case "+": return new Plus(firstOperand, secondOperand);
                    case "-": return new Minus(firstOperand, secondOperand);
                    case "*": return new Multiply(firstOperand, secondOperand);
                    case "/": return new Divide(firstOperand, secondOperand);
                    case "^": return new Power(firstOperand, secondOperand);
                }
            }

            // Every other operation require only one operand
            else
            {
                token.MoveNext();
                if (double.TryParse(op, out double number))
                    return new Number(number);
                switch (op)
                {
                    case "cos": return new Cos(firstOperand);
                    case "sin": return new Sin(firstOperand);
                    case "tan": return new Tan(firstOperand);
                    case "exp": return new Exp(firstOperand);
                    case "ln": return new Ln(firstOperand);
                }
            }
            throw new Exception("No funciton token matched");
        }
        var _op = token.Current.Value;

        // If not beginning with parenthensis that it has to be x or number
        if (_op == "x") return new X();
        return (new Number(_op));
    }

    public string Diff(string expr)
    {
        // New regex search to match all parenteses, numbers, funcitons and operations
        Regex rx = new Regex(@"-?\d+|\w+|[^ ]", RegexOptions.Compiled);
        MatchCollection rxMatches = rx.Matches(expr);
        // Create an enumerator to iterate over all matches
        var rxMatchesI = (IEnumerator<Match>)(rxMatches.GetEnumerator());
        // Parse the input and get the (nested) expression
        var expressison = Parse(rxMatchesI);
        var derivedExpression = expressison.Derivative();
        var fixedExpression = derivedExpression.Fix();
        return fixedExpression.Print();
    }
}

