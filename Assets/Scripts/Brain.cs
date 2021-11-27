using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Brain : MonoBehaviour
{
    public LayerMask m_ObstacleMask;

    [Header("Sensors")]
    public float m_MaxDistance = 6.0f;
    public float m_MaxSize = 27.0f;
    private float m_Distance;
    private float m_Size;

    private PlayerMovement m_Player;
    private NeuralNetwork m_Net;

    [Header("Train")]
    public bool m_UseNet = true;
    public float m_Accuracy = 0.000001f;
    public int m_Epochs = 10;
    public List<SampleData> m_Samples = new List<SampleData>();

    public void Awake()
    {
        m_Net = new NeuralNetwork(3);
        m_Player = GetComponent<PlayerMovement>();
    }

    public void UpdateInputMap()
    {
        if (Input.GetKeyDown(KeyCode.T)) m_UseNet = false;
        if (Input.GetKeyDown(KeyCode.P)) m_UseNet = true;
        if (Input.GetKeyDown(KeyCode.N)) m_Net = new NeuralNetwork(3);
        if (Input.GetKeyDown(KeyCode.R)) m_Samples.Clear();
    }

    public void Update()
    {
        UpdateSensorsData();
        UpdateInputMap();

        if (m_UseNet)
        {
            var inputs = new float[3] { m_Distance, m_Size, -1.0f };
            var output = m_Net.Calculate(inputs);
            m_Player.Jump = output == 1;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                m_Player.Jump = true;
                SaveSample(1.0f);
            }

            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                SaveSample(0.0f);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                StartCoroutine(m_Net.Train(m_Samples, m_Accuracy, m_Epochs));
            }
        }
    }

    public void SaveSample(float output)
    {
        var inputs = new float[3] { m_Distance, m_Size, -1.0f };
        m_Samples.Add(new SampleData(inputs, output));
    }

    public void ClearSensorsData()
    {
        m_Distance = m_MaxDistance;
        m_Size = 0.0f;
    }

    public void UpdateSensorsData()
    {
        ClearSensorsData();
        var obstacle = DetectObstacle();
        if (obstacle != null)
        {
            m_Distance = Vector3.Distance(m_Player.transform.position, obstacle.transform.position);
            var size = obstacle.bounds.size;
            m_Size = size.x * size.y * size.z;
        } 
    }

    public Collider DetectObstacle()
    {
        if (Physics.Raycast(transform.position, Vector3.right, out RaycastHit hit, m_MaxDistance, m_ObstacleMask))
        {
            return hit.collider;
        }

        return null;
    }
}
