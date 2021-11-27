using UnityEngine;

public class NeuralNetwork
{
    private float[] m_Weights;

    public NeuralNetwork(int inputNumber)
    {
        m_Weights = new float[inputNumber];
        for (int i = 0; i < inputNumber; i++)
        {
            m_Weights[i] = Random.Range(-1.0f, 1.0f);
        }
    }

    public int Calculate(float[] inputs)
    {
        var u = 0.0f;
        for (int i = 0; i < inputs.Length; i++)
        {
            u += inputs[i] * m_Weights[i];
        }

        return ActivationFunction(u);
    }

    private int ActivationFunction(float u)
    {
        return u < 0 ? 0 : 1;
    }
}


