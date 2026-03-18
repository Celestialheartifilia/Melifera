using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx1, sfx2, sfx3, sfx4;


    private void OnMouseDown()
    {
        // For example, play Sound 1 when the object is clicked
        Sound4();
    }


    public void Sound1()
    {
        src.clip = sfx1;
        src.Play();
    }

    public void Sound2() 
    {
        src.clip = sfx2;
        src.Play();
    }

    public void Sound3()
    {
        src.clip = sfx3;
        src.Play();
    }

    public void Sound4()
    {
        src.clip = sfx4;
        src.Play();
    }
}
