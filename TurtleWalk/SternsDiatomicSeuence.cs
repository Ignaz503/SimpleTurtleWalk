using System.Collections;
using TurtleWalks;

public class SternsDiatomicSeuence : IWalkSequence
{
    public int Count { get; init; }

    public IEnumerator<(float value, Turtle.PenState penState)> GetEnumerator()
    {
        Queue<int> numbers = new(Count);
        numbers.Enqueue(1);
        numbers.Enqueue(2);

        yield return (0, Turtle.PenState.Down);
        yield return (1, Turtle.PenState.Down);
        int first, second;
        for (int i = 0; i < Count; i++)
        {
            first = numbers.Dequeue();
            second = numbers.Peek();
            numbers.Enqueue(first); 
            var newNum = first+ second;
            numbers.Enqueue(newNum);
            yield return (first, Turtle.PenState.Down);
                
        }


    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
