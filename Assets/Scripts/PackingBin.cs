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

        currentDisposable = other.gameObject;

        if (binAnimator != null)
            binAnimator.SetBool("Open", true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (currentDisposable == other.gameObject)
        {
            currentDisposable = null;

            if (binAnimator != null)
                binAnimator.SetBool("Open", false);
        }
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

    private void OnMouseEnter()
    {
        OpenBin();
    }

    private void OnMouseExit()
    {
        CloseBin();
    }

    public void CloseBin()
    {
        if (binAnimator != null)
            binAnimator.SetBool("Open", false);
    }

    public void OpenBin()
    {
        if (binAnimator != null)
            binAnimator.SetBool("Open", true);
    }
}
