using UnityEngine;
using System.Collections;

public class NextOrderPopup : MonoBehaviour
{
    public GameObject popup;

    void Start()
    {
        popup.SetActive(false);   // ensure it starts hidden
        ShowPopupAfterDelay();
    }

    public IEnumerator ShowPopupAfterDelay()
    {
        yield return new WaitForSeconds(5f);
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
