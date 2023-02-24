# SharpEval V2
C# Expression parsing and evaluation<br>
How to use
```C#
Parser.parse(expression)
```
Will execute the expression and give you a double.

Using the following function, new operators can be added :
```C#
#Unary Operators
Parser.AddOperator(char op, Func<double, double>, bool primary);
#Binary Operators
Parser.AddOperator(char op, Func<double, double, double>, bool primary);
```
The primary boolean gives the operator a priority equal to the multiplication one.

## New : Custom operators can now be added to the parser<br>

<br>
I reckon there is room for improvement<br>
Up next :<br>
  -Code Quality improvement (especialy for the parsing part)<br>
  -Better use of IExpressions.
  
