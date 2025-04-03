using UnityEngine;

public class Driver : MonoBehaviour
{
    [SerializeField] float turnSpeed = 20f;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float slowSpeedRatio = 0.5f;
    [SerializeField] float boostSpeedRatio = 1.5f;
    float slowSpeed;
    float boostSpeed;

    void Start()
    {
        slowSpeed = moveSpeed * slowSpeedRatio;
        boostSpeed = moveSpeed * boostSpeedRatio;
    }

    void Update()
    {
        float turnaAmount = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        float turnaAmounty = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Rotate(0, 0, -turnaAmount);
        transform.Translate(0, turnaAmounty, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Boost"))
        {
            moveSpeed = boostSpeed;
            Debug.Log("Boost!!!!!!!!!!!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        moveSpeed = slowSpeed;
    }
}
