using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject popup;
    public TextMeshProUGUI popupText;
    public float displayTime = 2f;

    Coroutine currentRoutine;

    void Awake()
    {
        Instance = this;
        popup.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(message));
    }

    IEnumerator ShowRoutine(string message)
    {
        popup.SetActive(true);
        popupText.text = message;

        yield return new WaitForSeconds(displayTime);

        popup.SetActive(false);
    }
}
