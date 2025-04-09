using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Drift : MonoBehaviour
{
    [SerializeField] float accleration = 20f;      // 전진 / 후진 가속도
    [SerializeField] float steering = 3f;          // 조향 속도
    [SerializeField] float maxSpeed = 10f;         // 최대 속도 제한
    [SerializeField] float driftFactor = 0.95f;    // 낮을수록 더 미끄러짐

    [SerializeField] float slowAcclerationRatio = 0.5f;
    [SerializeField] float boostAcclerationRatio = 1.5f;
    [SerializeField] float speedTime = 0.5f;
    [SerializeField] float boostMaxSpeed = 7f;    // 부스트 상태에서 최대 속도
    [SerializeField] float boostDuration = 0.5f;     // 부스트 지속 시간
    [SerializeField] float speedIgnoreDuration = 2f;  // 최대 속도 무시 시간

    [SerializeField] ParticleSystem smokeLeft;
    [SerializeField] ParticleSystem smokeRight;
    [SerializeField] TrailRenderer leftTrail;
    [SerializeField] TrailRenderer rigftTrail;
    [SerializeField] BoxCollider2D offHitBox;

    Rigidbody2D rb;
    AudioSource audioSource;

    float defaultAccleation;
    float slowAccleation;
    float boostAccleation;
    float defaultSteering;

    private int groundContactCount = 0;
    private float driftTimer = 0.0f;
    private float boostTimer = 0.0f;
    private float speedIgnoreTimer = 0.0f;
    private float currentMaxSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = rb.GetComponent<AudioSource>();

        defaultAccleation = accleration;
        slowAccleation = accleration * slowAcclerationRatio;
        boostAccleation = accleration * boostAcclerationRatio;
        defaultSteering = steering;

        currentMaxSpeed = maxSpeed;
    }

    void FixedUpdate()
    {
        float speed = Vector2.Dot(rb.linearVelocity, transform.up);

        if (boostTimer > 0)
        {
            // 부스트 상태
            boostTimer -= Time.fixedDeltaTime;
            speedIgnoreTimer -= Time.fixedDeltaTime;

            // 최대 속도 무시
            if (speedIgnoreTimer > 0)
            {
                currentMaxSpeed = float.MaxValue;
            }
            else
            {
                currentMaxSpeed = boostMaxSpeed;
            }

            // W 키 입력 여부와 관계없이 앞으로 이동
            rb.AddForce(transform.up * boostAccleation);
        }
        else
        {
            // 일반 상태
            currentMaxSpeed = maxSpeed;
            steering = defaultSteering;

            // 전진 또는 후진 가속
            if (speed < currentMaxSpeed)
            {
                rb.AddForce(transform.up * Input.GetAxis("Vertical") * accleration);
            }
        }

        // 조향 및 회전
        float turnAmount = Input.GetAxis("Horizontal") * steering * Mathf.Clamp(speed / currentMaxSpeed, 0.4f, 1f);
        rb.MoveRotation(rb.rotation - turnAmount);

        // Drift
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 sideVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);
        rb.linearVelocity = forwardVelocity + (sideVelocity * driftFactor);
    }

    private void Update()
    {
        float sidewayVelocity = Vector2.Dot(rb.linearVelocity, transform.right);

        bool isDrifting = rb.linearVelocity.magnitude > 2f && Mathf.Abs(sidewayVelocity) > 1f;

        // Check for drifting
        if (isDrifting)
        {
            driftTimer = 0.0f; // Reset the drift timer when drifting
            if (!audioSource.isPlaying) audioSource.Play();
            if (!smokeLeft.isPlaying) smokeLeft.Play();
            if (!smokeRight.isPlaying) smokeRight.Play();
        }
        else
        {
            driftTimer += Time.deltaTime; // Increment the drift timer when not drifting
            if (audioSource.isPlaying) audioSource.Stop();
            if (smokeLeft.isPlaying) smokeLeft.Stop();
            if (smokeRight.isPlaying) smokeRight.Stop();

            if (driftTimer >= 3.0f)
            {
                Debug.Log("Not drifting for too long! Restarting...");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        leftTrail.emitting = isDrifting;
        rigftTrail.emitting = isDrifting;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            groundContactCount++;
            Debug.Log($"Entered Ground. Contact count: {groundContactCount}");
        }
        if (other.gameObject.CompareTag("Boost"))
        {
            boostTimer = boostDuration;
            speedIgnoreTimer = speedIgnoreDuration;
            Debug.Log("Boost Activated!");

            // Tree 콜라이더를 트리거로 설정
            SetTreeColliders(true);

            // 부스트 종료 후 트리거 복구
            Invoke(nameof(ResetTreeColliders), boostDuration);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            groundContactCount--;
            Debug.Log($"Exited Ground. Contact count: {groundContactCount}");

            if (groundContactCount <= 0)
            {
                Debug.Log("Out of bounds! Restarting...");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void ResetAcceleration()
    {
        accleration = defaultAccleation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        accleration = slowAccleation;
        Debug.Log("Collision! Slowing down.");

        Invoke("ResetAcceleration", speedTime);

        accleration = slowAccleation;
        Debug.Log("Collision! Slowing down.");

        Invoke("ResetAcceleration", speedTime);

        // Rock 태그 충돌 처리
        if (collision.gameObject.CompareTag("Rock"))
        {
            Debug.Log("Collision with Rock! Restarting...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void SetTreeColliders(bool isTrigger)
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("tree");
        foreach (GameObject tree in trees)
        {
            CircleCollider2D collider = tree.GetComponent<CircleCollider2D>();
            if (collider != null)
            {
                collider.isTrigger = isTrigger;
            }
        }
    }

    void ResetTreeColliders()
    {
        SetTreeColliders(false);
    }


}
