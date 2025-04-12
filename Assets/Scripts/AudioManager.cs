using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource effectsSource;
    public AudioSource rollingSource;

    [Header("Music Clips")]
    public AudioClip lobbyMusic;
    public AudioClip inGameMusic;

    [Header("Effect Clips")]
    public AudioClip clickSound;      // click1.ogg
    public AudioClip ballTapSound;    // ball_tap.wav
    public AudioClip rollingLoop;     // rolling_long.wav

    private const string MusicMuteKey = "MusicMuted";
    private const string EffectsMuteKey = "EffectsMuted";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            musicSource.mute = PlayerPrefs.GetInt(MusicMuteKey, 0) == 1;
            effectsSource.mute = PlayerPrefs.GetInt(EffectsMuteKey, 0) == 1;
            rollingSource.mute = effectsSource.mute;

            PreloadAudioClips(); // ✅ Preload clips here
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void PreloadAudioClips()
    {
        AudioClip[] allClips = { lobbyMusic, inGameMusic, clickSound, ballTapSound, rollingLoop };
        foreach (var clip in allClips)
        {
            if (clip != null && !clip.loadState.Equals(AudioDataLoadState.Loaded))
            {
                clip.LoadAudioData();
            }
        }
    }

    // MUSIC
    public void PlayLobbyMusic()
    {
        PlayMusic(lobbyMusic);
    }

    public void PlayInGameMusic()
    {
        PlayMusic(inGameMusic);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip != clip)
        {
            if (clip == inGameMusic)
            {
                musicSource.volume = 0.5f;
            }
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void ToggleMusicMute()
    {
        musicSource.mute = !musicSource.mute;
        PlayerPrefs.SetInt(MusicMuteKey, musicSource.mute ? 1 : 0);
    }

    public bool IsMusicMuted()
    {
        return musicSource.mute;
    }

    // EFFECTS
    public void PlayClick()
    {
        PlayEffect(clickSound);
    }

    public void PlayBallTap()
    {
        PlayEffect(ballTapSound);
    }

    private void PlayEffect(AudioClip clip)
    {
        if (!effectsSource.mute && clip != null)
        {
            effectsSource.PlayOneShot(clip);
        }
    }

    public void ToggleEffectsMute()
    {
        effectsSource.mute = !effectsSource.mute;
        rollingSource.mute = effectsSource.mute;
        PlayerPrefs.SetInt(EffectsMuteKey, effectsSource.mute ? 1 : 0);
    }

    public bool AreEffectsMuted()
    {
        return effectsSource.mute;
    }

    // ROLLING SOUND
    public void StartRolling()
    {
        if (rollingLoop != null && !rollingSource.isPlaying)
        {
            rollingSource.clip = rollingLoop;
            rollingSource.loop = true;
            rollingSource.Play();
        }
    }

    public void StopRolling()
    {
        if (rollingSource.isPlaying)
        {
            rollingSource.Stop();
        }
    }
}
