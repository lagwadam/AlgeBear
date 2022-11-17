namespace UtilityLibraries;

public static class Util
{
    public static bool StartsWithUpper(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return false;

        char ch = str[0];
        return char.IsUpper(ch);
    }

    public static string FormatParens(IExpression expression)
    { 
        string result = expression.ToString();

        // 1. If expression not primative, it's composite (complicated), so wrap with parens.
        // 2. Primatives don't need parens; for example, (a)^(b) can be formatted as a^b.
        if (expression is not IPrimative)
        {
            // If the expression is not primative, it's composite and likely more complicated, so wrap with parens. 
            result = $"({result})";
        }

        return result;
    }

    public static string FormatCoeff(Double coefficient)
    { 
        if (coefficient == 1)
        {
            return string.Empty;
        }

        return coefficient.ToString();
    }
}