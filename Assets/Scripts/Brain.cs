using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Brain : MonoBehaviour
{
    public float m_MaxDistance = 6.0f;
    public float m_MaxSize = 27.0f;

    public LayerMask m_ObstacleMask;

    public bool m_UseNet = true;

    [Header("Sensors")]
    public float m_Distance;
    public float m_Size;

    private PlayerMovement m_Player;
    private NeuralNetwork m_Net;

    public void Awake()
    {
        m_Net = new NeuralNetwork(3);
        m_Player = GetComponent<PlayerMovement>();
    }

    public void Update()
    {
        if (m_UseNet)
        {
            UpdateSensorsData();
            Decision();
        }
        else
        {
            m_Player.Jump = Input.GetButton("Jump");
        }
    }

    public void Decision()
    {
        var inputs = new float[3] { m_Distance, m_Size, -1.0f };
        var output = m_Net.Calculate(inputs);
        var jump = output == 1;

        m_Player.Jump = jump;
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
