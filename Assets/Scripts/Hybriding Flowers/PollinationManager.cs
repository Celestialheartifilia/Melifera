using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollinationManager : MonoBehaviour
{
    //reference for the pollination rule guide
    //e.g. normal flower 1 + normal flower 2 = hybrid flower 1
    public HybridRulesSOScript hybridRulesSOScript;

    //Temporarily stores the two normal flowers the player pollinated into a list with a capacity of 2 -> players can only pollinate 2 different flower
    private readonly List<ItemsSOScript> pickedFlowers = new List<ItemsSOScript>(2);

    public BeeController beeController;

    //Visual Indicators
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

    //Stores the final hybrid result -> Acts as the “output” of pollination
    public ItemsSOScript ReadyHybrid { get; private set; }

    //method to clears partial input/output -> used for Invalid combo/After planting
    public void ResetPollination()
    {
        pickedFlowers.Clear();
        ReadyHybrid = null;

        ResetAllFlowers();
    }


    public bool TryAddPollinatedFlower(NormalFlower flower)
    {
        //already have a hybrid ready
        if (ReadyHybrid != null) 
        { 
            return false;
        }
        //already picked 2 normal flowers
        if (pickedFlowers.Count >= 2) 
        {
            return false;
        }
        //prevent picking the same flower twice
        if (pickedFlowers.Contains(flower.flowerData))
        {
            Debug.Log("Same flower cannot be picked twice.");
            return false;
        }

        //Adds a normal flower to the list -> store data
        pickedFlowers.Add(flower.flowerData);
        //modify visual state
        flower.SetPollinated(true);

        //once 2 flower is picked
        if (pickedFlowers.Count == 2)
        {
            //get the hybrid result of the pollination
            ItemsSOScript result = hybridRulesSOScript.GetHybridResult(pickedFlowers[0], pickedFlowers[1]);
            //if there are no results -> the combo is wrong
            if (result == null)
            {
                Debug.Log("Invalid combo. Resetting.");
                StartCoroutine(ShowForSeconds(WrongPollinationTryAgain, 1f));
                //reset
                ResetPollination();
                return false;
            }

            ////if have results -> store the result in ReadyHybrid
            //ReadyHybrid = result;
            //Debug.Log($"[POLLINATION] Hybrid ready: {ReadyHybrid.itemName}");
            //StartCoroutine(ShowForSeconds(HybridIsReady, 0.5f));

            // CHECK IF FULL FIRST
            if (InventoryManager.Instance.IsFull(result))
            {
                Debug.Log("Hybrid is full, cannot create more.");

                // optional visual
                StartCoroutine(ShowForSeconds(MaxHybridReachedUI, 1f));

                OnClearPollination(); // VERY IMPORTANT
                return false;
            }

            // ONLY SET if not full
            ReadyHybrid = result;
            Debug.Log($"[POLLINATION] Hybrid ready: {ReadyHybrid.itemName}");
            StartCoroutine(ShowForSeconds(HybridIsReady, 0.5f));
        }

        return true;
    }

    //passes the data of the hybrid flower to the pot
    public bool TryPlantInto(Pot pot)
    {
        //checks if there is the data stored
        if (ReadyHybrid == null) 
        {
            return false;
        }

        // Let the pot decide if planting is valid
        bool planted = pot.Plant(ReadyHybrid);

        if (!planted)
        {
            return false;
        }
            
        //pollination is resetted
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
