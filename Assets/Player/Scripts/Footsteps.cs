using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioClip[] clips;
    public Animator animator;
    private float _lastFootstep;

    private void OnValdate()
    {
        if (!animator) animator = GetComponent<Animator>();
    }

    private void Update()
{
    var footstep = animator.GetFloat("Footstep");
    if (Mathf.Abs(footstep) < .00001f) footstep = 0;

    if ((_lastFootstep > 0 && footstep < 0) || (_lastFootstep < 0 && footstep > 0))
    {

        if (clips.Length > 0)
        {
            AudioClip randomClip = clips[Random.Range(0, clips.Length)];
            AudioSource.PlayClipAtPoint(randomClip, transform.position);
        }
    }

    _lastFootstep = footstep;
}

    public void FootstepSound()
    {
        
    }
}
