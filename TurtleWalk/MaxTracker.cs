namespace TurtleWalks;

public struct MaxTracker 
{
    float val;
    public float Val { get => val; set => Update(value); }
    public MaxTracker()
    {
        val = float.MinValue;
    }

    public void Update(float newVal) 
    {
        if (val > newVal)
            return;
        val = newVal;
    }

}
