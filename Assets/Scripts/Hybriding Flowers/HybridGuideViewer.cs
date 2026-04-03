using UnityEngine;

public class HybridGuideViewer : MonoBehaviour
{
    public GameObject guidePanel;
    public HybridGuideUI hybridGuideUI;

    bool isOpen = false;

    void Start()
    {
        guidePanel.SetActive(false);
    }

    public void ToggleGuide()
    {
        SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.buttonClickSFX);
        isOpen = !isOpen;
        guidePanel.SetActive(isOpen);

        if (isOpen)
        {
            hybridGuideUI.DisplayGuide();
        }
    }
}
