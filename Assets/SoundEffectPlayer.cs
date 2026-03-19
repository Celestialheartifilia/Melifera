using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx1, sfx2, sfx3, sfx4, sfx5, sfx6, sfx7, sfx8, sfx9, sfx10, sfx11, sfx12, sfx13, sfx14;


    private void OnMouseDown()
    {
        // Play Sound 1 when the object is clicked
        TrashBin()
        OrderTaking();
        SprinkleFertilizer();
        BuzzingSound();
        ScissorsCutting();
        ButtonClicking();
        WrappingPaper();
        WrongOrder();
        CustomerArrived();
        AccossoriesStick();
        PluckOutLeaves();
        CorrectOrder();
        PollinateFlower();
        FlowerGrowing();
    }

    public void TrashBin()
    {
        src.clip = sfx1;
        src.Play();
    }

    public void OrderTaking() 
    {
        src.clip = sfx2;
        src.Play();
    }

    public void SprinkleFertilizer()
    {
        src.clip = sfx3;
        src.Play();
    }

    public void BuzzingSound()
    {
        src.clip = sfx4;
        src.Play();
    }

    public void ScissorsCutting()
    {
        src.clip = sfx5;
        src.Play();
    }

    public void ButtonClicking()
    {
        src.clip = sfx6;
        src.Play();
    }

    public void WrappingPaper()
    {
        src.clip = sfx7;
        src.Play();
    }

    public void WrongOrder()
    {
        src.clip = sfx8;
        src.Play();
    }


    public void CustomerArrived()
    {
        src.clip = sfx9;
        src.Play();
    }

    public void AccossoriesStick()
    {
        src.clip = sfx10;
        src.Play();
    }

    public void PluckOutLeaves()
    {
        src.clip = sfx11;
        src.Play();
    }

    public void CorrectOrder()
    {
        src.clip = sfx12;
        src.Play();
    }

    public void PollinateFlower()
    {
        src.clip = sfx13;
        src.Play();
    }


    public void FlowerGrowing()
    {
        src.clip = sfx14;
        src.Play();
    }
}
