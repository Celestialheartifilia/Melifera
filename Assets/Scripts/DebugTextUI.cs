using UnityEngine;
using TMPro;

public class DebugTextUI : MonoBehaviour
{
    public static DebugTextUI Instance;
    public TextMeshProUGUI debugText;

    void Awake()
    {
        Instance = this;
    }

    public void Log(string msg)
    {
        debugText.text += "\n" + msg;
        Debug.Log(msg);
    }
}