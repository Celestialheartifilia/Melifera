using UnityEngine;

public class StationAppear : MonoBehaviour
{
    [SerializeField] private GameObject button;

    void Start()
    {
        if (button != null)
            button.SetActive(false);

        Debug.Log("Working");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovementScript>() != null)
            button.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovementScript>() != null)
            button.SetActive(false);
    }

}
