using System.Collections;
using UnityEngine;

public class FertilizerManager : MonoBehaviour
{
    [Header("References")]
    public Pot pot;
    public Collider2D potSoilCollider;

    [Header("Shovel Objects")]
    public GameObject emptyShovel;
    public GameObject soilShovel;

    [Header("Fertiliser Sprites")]
    public SpriteRenderer fertiliserRenderer;
    public Sprite fertiliserWithShovelSprite;
    public Sprite fertiliserWithoutShovelSprite;

    bool CanUseFertiliser()
    {
        return pot != null && pot.IsReadyToFertilise();
    }

    void Start()
    {
        emptyShovel.SetActive(false);
        soilShovel.SetActive(false);
    }

    // Hover fertiliser,show empty shovel
    void OnMouseEnter()
    {
        if (!CanUseFertiliser()) return;

        emptyShovel.SetActive(true);
        fertiliserRenderer.sprite = fertiliserWithoutShovelSprite;
    }

    void OnMouseExit()
    {
        if (!CanUseFertiliser()) return;

        if (!soilShovel.activeSelf)
        {
            emptyShovel.SetActive(false);
            fertiliserRenderer.sprite = fertiliserWithShovelSprite;
        }
    }

    // Click fertiliser,activate shovel drag
    void OnMouseDown()
    {
        if (!CanUseFertiliser())
        {
            Debug.Log("[FERTILISER] Not ready yet.");
            return;
        }

        emptyShovel.SetActive(false);
        soilShovel.SetActive(true);

        fertiliserRenderer.sprite = fertiliserWithoutShovelSprite;

        Shovel shovel = soilShovel.GetComponent<Shovel>();
        shovel.ActivateShovel(pot, potSoilCollider);
    }

    public void ResetFertiliserState()
    {
        emptyShovel.SetActive(false);
        soilShovel.SetActive(false);

        fertiliserRenderer.sprite = fertiliserWithShovelSprite;
    }

}
