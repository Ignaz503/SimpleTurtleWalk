using System.Collections;
using System.Numerics;
using TurtleWalks;

public class EnumerableSqrt : IWalkSequence
{
    public long Value { get; init; }
    public int Count { get; init; }

    public IEnumerator<(float value, Turtle.PenState penState)> GetEnumerator()
    {
        List<int> pairs = new();
        long num = Value;
        while (num > 0)
        {
            var remainder = num % 100;
            num = num / 100;
            pairs.Add((int)remainder);
        }
        pairs.Reverse();

        BigInteger combinedLeftSide = pairs[0];
        BigInteger combinedRightSide;
        BigInteger rightSideMulResult;
        BigInteger prevResults = 0;
        BigInteger diff;

        for (int i = 0; i < Count; i++)
        {

            BigInteger target;

            if (i == 0) //todo
                target = pairs[i];
            else 
            {
                target = combinedLeftSide;
            }

            int digit;

            var prevDigitsAsNumbderDoubled = prevResults * 2;
            if (i == 0)
                digit = FindIntegerSqrtLessEqual(target);
            else 
            {
                digit = FindBiggestInteger(target, prevDigitsAsNumbderDoubled);
            }
            var digitsOfDigit = Digits_Log10(digit);
            var multiplier = (int)Math.Pow(10, digitsOfDigit);
            prevResults = prevResults * multiplier + digit;


            combinedRightSide = prevDigitsAsNumbderDoubled * multiplier + digit;
            rightSideMulResult = combinedRightSide * digit;

            diff = combinedLeftSide - rightSideMulResult;

            if (i < pairs.Count - 1)
            {
                combinedLeftSide = diff * 100 + pairs[i + 1];
            }
            else 
            {
                combinedLeftSide = diff * 100;
            }

            yield return (digit, Turtle.PenState.Down);
        }

    }

    private int FindBiggestInteger(BigInteger target, BigInteger combinedrightSide)
    {
        return FindBiggestIntegerRecursion(target, combinedrightSide, 0);
    }

    private int FindBiggestIntegerRecursion(BigInteger target, BigInteger combinedrightSide, int prevI)
    {
        var digits = Digits_Log10(prevI + 1);
        var multipier = (int)Math.Pow(10, digits);
        if ((combinedrightSide * multipier + (prevI + 1)) * (prevI + 1) > target)
            return prevI;
        return FindBiggestIntegerRecursion(target, combinedrightSide, prevI + 1);

    }
    int Digits_Log10(int n)
            => n == 0 ? 1 : (int)Math.Floor(Math.Log10(Math.Abs(n)) + 1);


    int FindIntegerSqrtLessEqual(BigInteger target) 
    {
        return FindIntegerSqrtLessEqualRecursion(target, 0);
    }
    int FindIntegerSqrtLessEqualRecursion(BigInteger target, int prev) 
    {
        if ((prev + 1) * (prev + 1) > target)
            return prev;
        return FindIntegerSqrtLessEqualRecursion(target,prev + 1);
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
