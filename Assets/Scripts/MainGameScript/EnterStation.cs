using UnityEngine;
using UnityEngine.Events;

public class EnterStation : MonoBehaviour
{
    public UnityEvent onStationEnter;

    void OnMouseDown()
    {
        onStationEnter.Invoke();
    }
}
