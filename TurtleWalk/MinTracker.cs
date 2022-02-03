namespace TurtleWalks;

public struct MinTracker 
{
    float val;
    public float Val { get => val; set => Update(value); }
    public MinTracker()
    {
        val = float.MaxValue;
    }

    public void Update(float newVal)
    {
        if (val < newVal)
            return;
        val = newVal;
    }
}
