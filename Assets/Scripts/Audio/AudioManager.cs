using UnityEngine;

/// <summary>
/// 
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _clapSound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayClapSound()
    {
        _clapSound.Play();
    }
}