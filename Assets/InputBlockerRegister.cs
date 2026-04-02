using UnityEngine;

public class InputBlockerRegister : MonoBehaviour
{
    public GameObject inputBlocker;

    void OnEnable()
    {
        if (OptionsMenuController.Instance != null)
            OptionsMenuController.Instance.SetInputBlocker(inputBlocker);
    }
}