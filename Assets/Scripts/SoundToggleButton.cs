using UnityEngine;
using UnityEngine.UI;

public class SoundToggleButton : MonoBehaviour
{
    [Header("UI Elements for Music")]
    public RawImage musicButtonIcon;               // RawImage for music button icon
    public Texture muteMusicTexture;               // Icon for music muted
    public Texture unmuteMusicTexture;             // Icon for music unmuted

    [Header("UI Elements for Effects")]
    public RawImage effectButtonIcon;              // RawImage for effect button icon
    public Texture muteEffectTexture;              // Icon for effects muted
    public Texture unmuteEffectTexture;            // Icon for effects unmuted

    void Start()
    {
        UpdateMusicIcon();
        UpdateEffectIcon();
    }

    public void ToggleMusicSound()
    {
        AudioManager.Instance.ToggleMusicMute();
        UpdateMusicIcon();
    }

    public void ToggleEffectSound()
    {
        AudioManager.Instance.ToggleEffectsMute();
        UpdateEffectIcon();
    }

    private void UpdateMusicIcon()
    {
        bool isMuted = AudioManager.Instance.IsMusicMuted();
        musicButtonIcon.texture = isMuted ? muteMusicTexture : unmuteMusicTexture;
    }

    private void UpdateEffectIcon()
    {
        bool isMuted = AudioManager.Instance.AreEffectsMuted();
        effectButtonIcon.texture = isMuted ? muteEffectTexture : unmuteEffectTexture;
    }
}