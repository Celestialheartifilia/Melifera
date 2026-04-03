using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollinationManager : MonoBehaviour
{
    public HybridRulesSOScript hybridRulesSOScript;
    private readonly List<ItemsSOScript> pickedFlowers = new List<ItemsSOScript>(2);

    public BeeController beeController;

    [Header("Visual Indicators")]
    public GameObject WrongPollinationTryAgain;
    public GameObject HybridIsReady;
    public GameObject MaxHybridReachedUI;

    public int PollinationCount => pickedFlowers.Count;

    IEnumerator ShowForSeconds(GameObject obj, float seconds)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(seconds);
        obj.SetActive(false);
    }

    void Awake()
    {
        WrongPollinationTryAgain.SetActive(false);
        HybridIsReady.SetActive(false);
        MaxHybridReachedUI.SetActive(false);
    }

    public ItemsSOScript ReadyHybrid { get; private set; }

    public void ResetPollination()
    {
        pickedFlowers.Clear();
        ReadyHybrid = null;

        ResetAllFlowers();
        if (beeController != null)
        {
            beeController.StopPollinateEffect();
        }
    }

    public bool TryAddPollinatedFlower(NormalFlower flower)
    {
        if (ReadyHybrid != null)
        {
            return false;
        }

        if (pickedFlowers.Count >= 2)
        {
            return false;
        }

        if (pickedFlowers.Contains(flower.flowerData))
        {
            Debug.Log("Same flower cannot be picked twice.");
            return false;
        }

        pickedFlowers.Add(flower.flowerData);
        flower.SetPollinated(true);

        // pollination success SFX
        if (SoundEffectPlayer.Instance != null)
            SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.pollinateFlowerSFX);

        if (pickedFlowers.Count == 2)
        {
            ItemsSOScript result = hybridRulesSOScript.GetHybridResult(pickedFlowers[0], pickedFlowers[1]);

            if (result == null)
            {
                Debug.Log("Invalid combo. Resetting.");
                StartCoroutine(ShowForSeconds(WrongPollinationTryAgain, 1f));
                ResetPollination();
                return false;
            }

            if (InventoryManager.Instance.IsFull(result))
            {
                Debug.Log("Hybrid is full, cannot create more.");
                StartCoroutine(ShowForSeconds(MaxHybridReachedUI, 1f));
                OnClearPollination();
                return false;
            }

            ReadyHybrid = result;
            Debug.Log($"[POLLINATION] Hybrid ready: {ReadyHybrid.itemName}");
            StartCoroutine(ShowForSeconds(HybridIsReady, 0.5f));
        }

        return true;
    }

    public bool TryPlantInto(Pot pot)
    {
        if (ReadyHybrid == null)
        {
            return false;
        }

        bool planted = pot.Plant(ReadyHybrid);

        if (!planted)
        {
            return false;
        }

        ResetPollination();
        return true;
    }

    public void ResetAllFlowers()
    {
        NormalFlower[] flowers = FindObjectsOfType<NormalFlower>();

        foreach (NormalFlower flower in flowers)
        {
            flower.SetPollinated(false);
        }

        Debug.Log("[POLLINATION] All flowers reset");
    }

    public void OnClearPollination()
    {
        ResetPollination();
        beeController.ReturnToStart();
    }
}