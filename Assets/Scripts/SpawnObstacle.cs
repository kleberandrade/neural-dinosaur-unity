using UnityEngine;

public class SpawnObstacle : MonoBehaviour
{
    public ObstacleData[] m_Obstacles;

    public float m_MinTimeToSpawn;
    public float m_MaxTimeToSpawn;

    private float m_TimeToSpawn;
    private float m_ElapsedTime;

    public bool CanSpawn => m_ElapsedTime >= m_TimeToSpawn;

    public void Start()
    {
        Reset();
    }

    public void Update()
    {
        m_ElapsedTime += Time.deltaTime;
        Spawn();
    }

    public void Spawn()
    {
        if (!CanSpawn) return;

        var obstacle = SelectObstacle();
        Instantiate(obstacle, transform.position, Quaternion.identity);
        Reset();
    }

    private GameObject SelectObstacle()
    {
        var number = Random.Range(0.0f, 1.0f);
        var value = 0.0f;
        for (int i = 0; i < m_Obstacles.Length; i++)
        {
            value += m_Obstacles[i].odds;
            if (number <= value)
            {
                return SpawnObstacleData(m_Obstacles[i]);
            }
        }

        return SpawnObstacleData(m_Obstacles[0]);
    }

    private GameObject SpawnObstacleData(ObstacleData data)
    {
        var scale = Random.Range(data.min, data.max);
        var prefab = data.prefab;

        prefab.transform.localScale = Vector3.one * scale;
        return prefab;
    }

    private void Reset()
    {
        m_ElapsedTime = 0.0f;
        m_TimeToSpawn = Random.Range(m_MinTimeToSpawn,
            m_MaxTimeToSpawn);
    }
}

[System.Serializable]
public class ObstacleData
{
    public GameObject prefab;
    [Range(0.01f, 1.0f)]
    public float odds;
    [Range(0.5f, 5.0f)]
    public float min;
    [Range(0.5f, 5.0f)]
    public float max;
}
