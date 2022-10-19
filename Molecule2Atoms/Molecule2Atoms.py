from typing import Dict

open_brackets = ["[", "{", "("]
close_brackets =  [")", "}", "]"]
brackets = [*open_brackets, *close_brackets]


def parse_molecule(s: str) -> Dict[str, int]:
    atom_count: Dict[str, int] = {}
    
    i: int = 0
    while i < len(s):
        # is atom
        if s[i].isupper():
            atom = s[i]
            multiplier = 1
            i+=1
            while i < len(s) and s[i].islower():
                atom += s[i]
                i+=1
            
            if i < len(s) and s[i].isnumeric():
                num = s[i]
                i+=1
                while i < len(s) and s[i].isnumeric():
                    num += s[i]
                    i+=1
                multiplier = int(num)
            
            atom_count[atom] = atom_count.get(atom, 0) + multiplier

        elif s[i] in open_brackets:
            i+=1
            start_ind = i
            current_bracket_level = 1
            
            # increase index until we find matching closing bracket
            while(current_bracket_level > 0):
                if s[i] in close_brackets:
                    current_bracket_level-=1
                elif s[i] in open_brackets:
                    current_bracket_level+=1
                i+=1
            d = parse_molecule(s[start_ind:i-1])

            if i < len(s) and s[i].isnumeric():
                num = s[i]
                i+=1
                while i < len(s) and s[i].isnumeric():
                    num += s[i]
                    i+=1
                for item in d:
                    d[item] *= int(num)
                i+=1

            for item in d:
                atom_count[item] = atom_count.get(item, 0) + d[item]
        else:
            i+=1
    
    return atom_count


if __name__ == "__main__":
    print(parse_molecule("H2O"))
    print(parse_molecule("Mg(OH)2"))
    print(parse_molecule("K4[ON(SO3)2]2"))