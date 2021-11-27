using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour
{
    public float m_Speed = 2.0f;
    public Vector3 m_Direction = Vector3.left;

    public void Start()
    {
        var body = GetComponent<Rigidbody>();
        body.isKinematic = true;
    }

    public void Update()
    {
        transform.Translate(m_Direction * m_Speed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.CompareTag("Limit"))
        {
            Destroy(gameObject);
        }
    }
}
