using UnityEngine;

public class OptionsMenuBackButton : MonoBehaviour
{
    public void CloseOptions()
    {
        if (OptionsMenuController.Instance != null)
            OptionsMenuController.Instance.CloseOptions();
    }
}
