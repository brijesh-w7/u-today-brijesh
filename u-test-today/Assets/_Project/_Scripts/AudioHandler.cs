using UnityEngine;

public class AudioHandler : SingletonMono<AudioHandler>
{
    [SerializeField] private AudioSource audioSource;

    public AudioClip clipButtonClicked;
    public AudioClip clipGameOver;
    public AudioClip clipCardMatched;
    public AudioClip clipCardmismatched;
    public AudioClip clipCardFlip;

    public void PlayButtonClickedAudio()
    {
        audioSource.PlayOneShot(clipButtonClicked);
    }

    public void PlayAudioClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
