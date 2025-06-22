using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---------- Audio Source ----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    // All music is set in the Unity Engine interface
    [Header("---------- Audio Clip ----------")]
    public AudioClip background;
    public AudioClip buttonClick;
    public AudioClip explosion;

    // Plays the background music when the game starts
    private void Start()
    {
        musicSource.clip = background;
        musicSource.volume = 0.25f; // Set volume
        musicSource.loop = true; // Loops the audio
        musicSource.Play();
    }

    // Function to play sound effects by passing the clip variable
    // e.g. PlaySFX(buttonClick) - Plays button click sound set above
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}