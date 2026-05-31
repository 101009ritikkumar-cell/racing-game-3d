using UnityEngine;

/// <summary>
/// Audio management for engine sounds, music, and effects
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource engineAudioSource;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("Engine Sounds")]
    [SerializeField] private AudioClip[] engineClips;
    [SerializeField] private float minEnginePitch = 0.5f;
    [SerializeField] private float maxEnginePitch = 2f;

    [Header("Music")]
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip raceMusic;

    [Header("SFX")]
    [SerializeField] private AudioClip collisionSound;
    [SerializeField] private AudioClip nitroSound;
    [SerializeField] private AudioClip lapCompleteSound;

    [Header("Settings")]
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float musicVolume = 0.7f;
    [SerializeField] private float sfxVolume = 0.8f;

    private CarController playerCar;
    private float currentEngineRPM = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerCar = FindObjectOfType<CarController>();
        PlayMenuMusic();
    }

    private void Update()
    {
        if (playerCar != null)
            UpdateEngineSound();
    }

    private void UpdateEngineSound()
    {
        float speedRatio = playerCar.CurrentSpeed / (playerCar.MaxSpeed * 3.6f);
        currentEngineRPM = Mathf.Lerp(0f, 8000f, speedRatio);

        if (engineAudioSource != null && engineClips.Length > 0)
        {
            float pitchRatio = currentEngineRPM / 8000f;
            engineAudioSource.pitch = Mathf.Lerp(minEnginePitch, maxEnginePitch, pitchRatio);

            if (!engineAudioSource.isPlaying)
                engineAudioSource.Play();
        }
    }

    public void PlayMenuMusic()
    {
        if (musicAudioSource != null && menuMusic != null)
        {
            musicAudioSource.clip = menuMusic;
            musicAudioSource.volume = musicVolume * masterVolume;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }
    }

    public void PlayRaceMusic()
    {
        if (musicAudioSource != null && raceMusic != null)
        {
            musicAudioSource.clip = raceMusic;
            musicAudioSource.volume = musicVolume * masterVolume;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }
    }

    public void PlayCollisionSound()
    {
        if (sfxAudioSource != null && collisionSound != null)
            sfxAudioSource.PlayOneShot(collisionSound, sfxVolume * masterVolume);
    }

    public void PlayNitroSound()
    {
        if (sfxAudioSource != null && nitroSound != null)
            sfxAudioSource.PlayOneShot(nitroSound, sfxVolume * masterVolume);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
    }
}