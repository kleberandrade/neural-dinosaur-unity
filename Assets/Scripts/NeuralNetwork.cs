using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    public float m_LearnRate = 0.01f;
    private float[] m_Weights;
    public string m_FileName = "net.dat";
    public string FilePath => $"{Application.dataPath}/{m_FileName}";

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

        return StepFunction(u);
    }

    private int StepFunction(float u)
    {
        return u < 0 ? 0 : 1;
    }

    public IEnumerator Train(List<SampleData> samples, float accuracy, int epochs)
    {
        Debug.Log($"[Train] start");
        float totalError = 0.0f;
        int epoch = 0;

        do
        {
            totalError = 0.0f;
            foreach (var sample in samples)
            {
                float output = Calculate(sample.inputs);
                float error = sample.output - output;
                totalError += Mathf.Abs(error);
                for (int i = 0; i < m_Weights.Length; i++)
                {
                    float input = sample.inputs[i];
                    m_Weights[i] = m_Weights[i] + m_LearnRate * error * input;
                }
            }

            epoch++;
            Debug.Log($"[Train] epoch {epoch}/{epochs} with error {totalError}/{accuracy}");

            yield return null;

        } while (totalError > accuracy && epoch < epochs);

        if (totalError <= accuracy)
        {
            Debug.Log($"[Train] finish by accuracy {totalError}/{accuracy}");
        }
        else 
        {
            Debug.Log($"[Train] finish by epochs {epoch}/{epochs}");
        }
    }

    public void Load()
    {
        if (!System.IO.File.Exists(FilePath)) return;
        using (var file = System.IO.File.OpenText(FilePath))
        {
            for (int i = 0; i < m_Weights.Length; i++)
            {
                string line = file.ReadLine();
                m_Weights[i] = float.Parse(line);
            }
        }

    }

    public void Save()
    {
        using (var file = System.IO.File.CreateText(FilePath))
        {
            foreach (var weight in m_Weights)
            {
                file.WriteLine(weight);
            }
        }
    }
}


