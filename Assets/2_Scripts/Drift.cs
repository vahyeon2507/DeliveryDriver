using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Drift : MonoBehaviour
{
    [SerializeField] float accleration = 20f;      // ���� / ���� ���ӵ�
    [SerializeField] float steering = 3f;          // ���� �ӵ�
    [SerializeField] float maxSpeed = 10f;         // �ִ� �ӵ� ����
    [SerializeField] float driftFactor = 0.95f;    // �������� �� �̲�����

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float speed = Vector2.Dot(rb.linearVelocity, transform.up);
        if (speed < maxSpeed)
        {
            rb.AddForce(transform.up * Input.GetAxis("Vertical") * accleration);
        }

        float turnAmount = Input.GetAxis("Horizontal") * steering * Mathf.Clamp(speed/maxSpeed, 0.4f ,1f);
        rb.MoveRotation(rb.rotation - turnAmount);

        //Drift
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 sideVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);
        rb.linearVelocity = forwardVelocity + (sideVelocity * driftFactor);
    }
}