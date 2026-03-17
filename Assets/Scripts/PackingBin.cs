using UnityEngine;

public class PackingBin : MonoBehaviour
{
    public PackingManager packingManager;
    public Collider2D binCollider;

    private GameObject currentDisposable;
    public Animator binAnimator;

    void Awake()
    {
        if (binAnimator != null)
            binAnimator.SetBool("Open", false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Disposable"))
            return;

        if (binAnimator != null)
            binAnimator.SetBool("Open", true);

        currentDisposable = other.gameObject;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Disposable"))
            return;

        if (binAnimator != null)
            binAnimator.SetBool("Open", false);

        if (currentDisposable == other.gameObject)
            currentDisposable = null;
    }

    public bool IsObjectInsideBin(GameObject obj)
    {
        if (obj == null || binCollider == null)
            return false;

        Collider2D objCol = obj.GetComponent<Collider2D>();
        if (objCol == null)
            return false;

        return objCol.IsTouching(binCollider);
    }

    public void TryDispose()
    {
        if (currentDisposable == null)
            return;

        Debug.Log(currentDisposable);
        packingManager.HandleDisposal(currentDisposable);
        currentDisposable = null;
    }

    public void TryDisposeFlower(GameObject flowerObj)
    {
        if (flowerObj == null)
            return;

        packingManager.HandleDisposal(flowerObj);
        currentDisposable = null;
    }

    void OnMouseDown()
    {
        packingManager.DisposeWholeBouquet();
    }

    public void OpenBin()
    {
        if (binAnimator != null)
            binAnimator.SetBool("Open", true);
    }
}
