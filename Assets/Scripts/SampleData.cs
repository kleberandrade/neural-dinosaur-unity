
[System.Serializable]
public class SampleData
{
    public SampleData(float[] inputs, float output)
    {
        this.inputs = inputs;
        this.output = output;
    }

    public float[] inputs;
    public float output;
}
