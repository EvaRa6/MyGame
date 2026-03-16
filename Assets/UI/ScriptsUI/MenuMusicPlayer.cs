using UnityEngine;

public class MenuMusicPlayer : MonoBehaviour
{
    public AudioClip menuMusic;
    private AudioSource musicSource;

    void Start()
    {
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = menuMusic;
        musicSource.loop = true;
        musicSource.volume = 0.5f;
        musicSource.Play();
    }

    void OnDisable()
    {
        if(musicSource != null)
        {
            musicSource.Stop();
        }
    }
}
