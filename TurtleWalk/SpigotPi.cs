using TurtleWalks;
using System.Collections;

public class SpigotPi : IWalkSequence
{

    uint[]? x;
    uint[]? r;
    uint carry = 0;
    int prevDigit = 0;
    public int Count { get; init; }

    public IEnumerator<(float value, Turtle.PenState penState)> GetEnumerator()
    {

        for (int i = 1; i <= Count; i++)
        {
            var res = Spigot(i);

            if (i == Count) //make it reuseable
                Reset();

            yield return (res, Turtle.PenState.Down);
        }
    }

    void Reset() 
    {
        x = null;
        r = null;
        carry = 0;
        prevDigit = 0;
    }

    uint Spigot(int digit) 
        {
            if (this.x == null) 
            {
                x = new uint[(Count+1) * 10 / 3 + 2];
                for (int j = 0; j < x.Length; j++)
                    x[j] = 20;
            }
            if (this.r == null) 
            {
                r = new uint[(Count + 1) * 10 / 3 + 2];
            }

            digit++;

            uint pDigit = 0;
            for (int i = prevDigit; i < digit; i++)
            {
                
                for (int j = 0; j < x.Length; j++)
                {
                    uint num = (uint)(x.Length - j - 1);
                    uint dem = num * 2 + 1;

                    x[j] += carry;

                    uint q = x[j] / dem;
                    r[j] = x[j] % dem;

                    carry = q * num;
                }


                pDigit = (x[x.Length - 1] / 10);


                r[x.Length - 1] = x[x.Length - 1] % 10; ;

                for (int j = 0; j < x.Length; j++)
                    x[j] = r[j] * 10;
            }
            prevDigit = digit;
            return pDigit;
        }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
