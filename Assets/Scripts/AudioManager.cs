using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class AudioManager : MonoBehaviour
{
    public AudioClip[] songs;  // Array to hold the three songs
    public Slider volumeSlider; // Reference to the volume slider UI
    private AudioSource audioSource;
    private int currentSongIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayNextSong();

        // Set initial volume based on slider value
        audioSource.volume = volumeSlider.value;

        // Add listener to detect changes in the slider value
        volumeSlider.onValueChanged.AddListener(delegate { SetVolume(); });
    }

    void Update()
    {
        // Check if the current song has finished playing
        if (!audioSource.isPlaying)
        {
            PlayNextSong();
        }
    }

    // Function to play the next song in the array
    void PlayNextSong()
    {
        audioSource.clip = songs[currentSongIndex];
        audioSource.Play();

        // Increment the song index and loop back to the first song if needed
        currentSongIndex = (currentSongIndex + 1) % songs.Length;
    }

    // Function to set the volume based on slider value
    void SetVolume()
    {
        audioSource.volume = volumeSlider.value; // Set the volume to match the slider
    }
}
