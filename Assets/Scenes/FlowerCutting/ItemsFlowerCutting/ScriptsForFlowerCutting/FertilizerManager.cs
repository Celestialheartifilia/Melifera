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

    void Start()
    {
        emptyShovel.SetActive(false);
        soilShovel.SetActive(false);
    }

    // Hover fertiliser → show empty shovel
    void OnMouseEnter()
    {
        emptyShovel.SetActive(true);
        fertiliserRenderer.sprite = fertiliserWithoutShovelSprite;
    }

    void OnMouseExit()
    {
        // Only hide if not dragging
        if (!soilShovel.activeSelf)
        {
            emptyShovel.SetActive(false);
            fertiliserRenderer.sprite = fertiliserWithShovelSprite;
        }

    }

    // Click fertiliser,activate shovel drag
    void OnMouseDown()
    {
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
