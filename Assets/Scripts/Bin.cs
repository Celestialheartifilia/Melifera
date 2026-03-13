using UnityEngine;

public class Bin : MonoBehaviour
{
    public Animator binAnimator;
    private GameObject currentDisposable;

    void Awake()
    {
        if (binAnimator != null)
            binAnimator.SetBool("Open", false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Disposable"))
            return;

        currentDisposable = other.gameObject;

        if (binAnimator != null)
            binAnimator.SetBool("Open", true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Disposable"))
            return;

        if (currentDisposable == other.gameObject)
        {
            currentDisposable = null;

            if (binAnimator != null)
                binAnimator.SetBool("Open", false);
        }
    }


    public void TryDispose()
    {
        if (currentDisposable == null)
            return;

        Pot pot = currentDisposable.GetComponent<Pot>();

        if (pot != null)
            pot.DisposeContents();
        else
            Destroy(currentDisposable);

        currentDisposable = null;
    }
}