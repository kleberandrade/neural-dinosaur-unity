using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float m_JumpForce = 40.0f;
    public float m_GroundDistance = 0.2f;
    public LayerMask m_GroundMask;

    public bool Jump { get; set; }

    private Rigidbody m_Body;
    private bool m_IsGrounded;

    public void Awake()
    {
        m_Body = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        m_IsGrounded = Physics.CheckSphere(transform.position, m_GroundDistance, m_GroundMask);
        if (m_IsGrounded)
        {
            m_Body.velocity = Vector3.zero;
        }


        if (Jump && m_IsGrounded)
        {
            var force = Vector3.up * m_JumpForce;
            m_Body.AddForce(force, ForceMode.Impulse);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, m_GroundDistance);
    }
}