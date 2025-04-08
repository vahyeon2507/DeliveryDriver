using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioClip bgmClip;  // 넣고 싶은 BGM 파일
    private AudioSource audioSource;

    void Awake()
    {
        // 오디오 소스 컴포넌트가 없으면 자동 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = 0.4f;
        }

        // 설정
        audioSource.clip = bgmClip;
        audioSource.loop = true;       // 반복 재생
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        audioSource.Play();
    }
}
