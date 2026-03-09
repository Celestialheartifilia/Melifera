using UnityEngine;

public class NextOrderPopup : MonoBehaviour
{
    public GameObject popup;

    void Start()
    {
        CheckForNextOrder();
    }

    public void CheckForNextOrder()
    {
        var manager = OrderTakingManager.Instance;

        if (manager == null) return;

        if (!manager.HasActiveOrder() && manager.HasMoreCustomers())
        {
            popup.SetActive(true);
        }
        else
        {
            popup.SetActive(false);
        }
    }
}
