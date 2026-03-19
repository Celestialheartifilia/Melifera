using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public int step = 2;

    [Header("UI")]
    public GameObject popup;
    public TMP_Text text;
    public GameObject overlay;
    public RectTransform arrow;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        ShowStep();
    }

    // =========================
    // MAIN CONTROL
    // =========================
    public void TryNext(int expectedStep)
    {
        if (step != expectedStep) return;

        step++;
        ShowStep();
    }

    public void Next()
    {
        step++;
        ShowStep();
    }

    // =========================
    // DISPLAY
    // =========================
    void ShowStep()
    {
        popup.SetActive(true);

        overlay.SetActive(false);
        arrow.gameObject.SetActive(false);

        switch (step)
        {
            case 0:
                text.text = "You’ve just opened your flower shop! A customer has arrived — let’s take their order.";
                break;

            case 1:
                text.text = "Click around to move to the counter.";
                break;

            case 2:
                text.text = "Click the cashier button.";
                ShowOverlay(cashierButton);
                break;

            case 3:
                text.text = "Click 'Take Order'.";
                ShowOverlay(takeOrderButton);
                break;

            case 4:
                text.text = "This is the customer's order. You can leave anytime.";
                break;

            case 5:
                text.text = "Go to the hybrid station on the right.";
                break;

            case 6:
                text.text = "Click the flower button.";
                ShowOverlay(hybridButton);
                break;

            case 7:
                text.text = "This is the hybrid station. Combine flowers to create new ones.";
                break;

            case 8:
                text.text = "This button clears pollination.";
                ShowOverlay(clearPollinationButton);
                break;

            case 9:
                text.text = "Drag the pot to the bin to dispose it.";
                ShowOverlay(bin);
                break;

            case 10:
                text.text = "Click to view the order.";
                ShowOverlay(orderListButton);
                break;

            case 11:
                text.text = "Open the hybrid guide.";
                ShowOverlay(hybridBookButton);
                break;

            case 12:
                text.text = "Click again to close the guide.";
                ShowOverlay(hybridBookButton);
                break;

            case 13:
                text.text = "Select your first flower.";
                break;

            case 14:
                text.text = "Select your second flower.";
                break;

            case 15:
                text.text = "Click the pot to plant.";
                ShowOverlay(pot);
                break;

            case 16:
                text.text = "Drag fertiliser onto the pot.";
                ShowOverlay(fertiliser);
                break;

            case 17:
                text.text = "Use scissors and follow the cut line.";
                ShowOverlay(scissors);
                break;

            case 18:
                text.text = "Click the flower to collect it.";
                ShowOverlay(flower);
                break;

            case 19:
                text.text = "Nice! Now go to packing.";
                break;

            case 20:
                text.text = "Move to the packing station.";
                break;

            case 21:
                text.text = "Click the wrap button.";
                ShowOverlay(packingButton);
                break;

            case 22:
                text.text = "This is the packing station.";
                break;

            case 23:
                text.text = "Click a flower to spawn it.";
                ShowOverlay(flowerSlot);
                break;

            case 24:
                text.text = "Drag leaves to the bin.";
                ShowOverlay(bin);
                break;

            case 25:
                text.text = "Add wrap and accessory.";
                break;

            case 26:
                text.text = "Click 'Order Complete'.";
                ShowOverlay(orderCompleteButton);
                break;

            case 27:
                text.text = "Great job! Tutorial done. Next customer!";
                break;
        }
    }

    void ShowOverlay(Transform target)
    {
        overlay.SetActive(true);

        if (target != null)
        {
            arrow.gameObject.SetActive(true);
            arrow.position = target.position;
        }
    }

    // =========================
    // REFERENCES (assign per scene)
    // =========================
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
}