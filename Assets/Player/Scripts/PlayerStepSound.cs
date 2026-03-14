using UnityEngine;

public class PlayerStepSound : MonoBehaviour
{
    public AudioClip[] stepSounds_AR; //massive sounds

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StepSoundPlay()
    {
        audioSource.PlayOneShot(stepSounds_AR[Random.Range(0, stepSounds_AR.Length)]);
        print("Step sound");
    }
}
