using UnityEngine;

public class TutorialBinder : MonoBehaviour
{
    public Transform cashierButton;
    public Transform takeOrderButton;
    public Transform hybridButton;
    public Transform clearPollinationButton;
    public Transform bin;
    public Transform orderListButton;
    public Transform hybridBookButton;
    public Transform pot;
    public Transform fertiliser;
    public Transform scissors;
    public Transform flower;
    public Transform packingButton;
    public Transform flowerSlot;
    public Transform orderCompleteButton;

    void Start()
    {
        var t = TutorialManager.Instance;
        if (t == null) return;

        t.cashierButton = cashierButton;
        t.takeOrderButton = takeOrderButton;
        t.hybridButton = hybridButton;
        t.clearPollinationButton = clearPollinationButton;
        t.bin = bin;
        t.orderListButton = orderListButton;
        t.hybridBookButton = hybridBookButton;
        t.pot = pot;
        t.fertiliser = fertiliser;
        t.scissors = scissors;
        t.flower = flower;
        t.packingButton = packingButton;
        t.flowerSlot = flowerSlot;
        t.orderCompleteButton = orderCompleteButton;
    }
}
