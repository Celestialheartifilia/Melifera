using UnityEngine;

public class Bin : MonoBehaviour
{
    [Header("Animation")]
    public Animator binAnimator;

    [Header("Detection")]
    private GameObject currentDisposable;

    void Awake()
    {
        if (binAnimator != null)
            binAnimator.SetBool("Open", false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if object is disposable
        if (!other.CompareTag("Disposable"))
            return;

        currentDisposable = other.gameObject;

        //OpenBin();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (currentDisposable == other.gameObject)
        {
            currentDisposable = null;
            //CloseBin();
        }
    }

    public void TryDispose()
    {
        if (currentDisposable == null)
            return;

        Debug.Log("[BIN] Disposing: " + currentDisposable.name);

        Pot pot = currentDisposable.GetComponent<Pot>();

        if (pot != null)
        {
            pot.DisposeContents();
        }
        else
        {
            Destroy(currentDisposable);
        }

        currentDisposable = null;

        //CloseBin();
    }

    void OpenBin()
    {
        if (binAnimator != null)
            binAnimator.SetBool("Open", true);
    }

    void CloseBin()
    {
        if (binAnimator != null)
            binAnimator.SetBool("Open", false);
    }
}
