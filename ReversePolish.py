

def calc(s: str):
    stack = []
    for t in s.split(" "):
        try:
            stack.append(float(t))
        except:
            pass
        match t:
            case "*":
                a = float(stack.pop())
                stack[-1] *= a
            case "/":
                a = float(stack.pop())
                stack[-1] /= a
            case "+":
                a = float(stack.pop())
                stack[-1] += a
            case "-":
                a = float(stack.pop())
                stack[-1] -= a
    
    try:
        return stack.pop()
    except:
        return 0


if __name__ == "__main__":
    print(calc("5 1 2 + 4 * + 3 -"))
    print(calc("1 2 3 4 + * +"))
    print(calc("1 2 + 3 * 4 +"))
    print(calc("3.5"))