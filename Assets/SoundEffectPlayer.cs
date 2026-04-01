using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectPlayer : MonoBehaviour
{
    public static SoundEffectPlayer Instance;

    public AudioSource src;
    public AudioClip trashBinSFX, 
        orderTakingSFX, 
        SprinkleFertilizerSFX, 
        buzzingSFX, scissorsCuttingSFX,
        buttonClickSFX, wrappingPaperSFX, 
        wrongOrderSFX, 
        customerArrivedSFX, 
        addAccessoriesSFX, 
        pluckOutLeavesSFX,
        correctOrderSFX,
        pollinateFlowerSFX,
        flowerGrowingSFX;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip clip)
    {
        float volume = 1f;

        if (SoundManager.instance != null)
            volume = SoundManager.instance.GetSFXVolume();

        src.PlayOneShot(clip, volume);
    }

    public void TrashBin()
    {
        src.clip = trashBinSFX;
        src.Play();
    }

    public void OrderTaking() 
    {
        src.clip = orderTakingSFX;
        src.Play();
    }

    public void SprinkleFertilizer()
    {
        src.clip = SprinkleFertilizerSFX;
        src.Play();
    }

    public void BuzzingSound()
    {
        src.clip = buzzingSFX;
        src.Play();
    }

    public void ScissorsCutting()
    {
        src.clip = scissorsCuttingSFX;
        src.Play();
    }

    public void ButtonClicking()
    {
        src.clip = buttonClickSFX;
        src.Play();
    }

    public void WrappingPaper()
    {
        src.clip = wrappingPaperSFX;
        src.Play();
    }

    public void WrongOrder()
    {
        src.clip = wrongOrderSFX;
        src.Play();
    }


    public void CustomerArrived()
    {
        src.clip = customerArrivedSFX;
        src.Play();
    }

    public void AddAccessories()
    {
        src.clip = addAccessoriesSFX;
        src.Play();
    }

    public void PluckOutLeaves()
    {
        src.clip = pluckOutLeavesSFX;
        src.Play();
    }

    public void CorrectOrder()
    {
        src.clip = correctOrderSFX;
        src.Play();
    }

    public void PollinateFlower()
    {
        src.clip = pollinateFlowerSFX;
        src.Play();
    }


    public void FlowerGrowing()
    {
        src.clip = flowerGrowingSFX;
        src.Play();
    }
}
