using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioClip bgmClip;  // �ְ� ���� BGM ����
    private AudioSource audioSource;

    void Awake()
    {
        // ����� �ҽ� ������Ʈ�� ������ �ڵ� �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = 0.4f;
        }

        // ����
        audioSource.clip = bgmClip;
        audioSource.loop = true;       // �ݺ� ���
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        audioSource.Play();
    }
}
