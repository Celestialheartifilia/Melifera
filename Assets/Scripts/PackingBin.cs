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

        if (PackingTutorial.Instance != null && PackingTutorial.Instance.IsProtectedFlower(other.gameObject))
        {
            currentDisposable = null;
            return;
        }

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

        if (PackingTutorial.Instance != null && PackingTutorial.Instance.IsProtectedFlower(currentDisposable))
        {
            currentDisposable = null;
            return;
        }

        if (SoundEffectPlayer.Instance != null)
            SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.trashBinSFX);

        Debug.Log(currentDisposable);
        packingManager.HandleDisposal(currentDisposable);
        currentDisposable = null;
    }

    public bool TryDisposeFlower(GameObject flowerObj)
    {
        if (flowerObj == null)
            return false;

        if (PackingTutorial.Instance != null && PackingTutorial.Instance.IsProtectedFlower(flowerObj))
        {
            if (UIManager.Instance != null)
                UIManager.Instance.ShowMessage("Flower is correct, disposal not needed.");
            return false;
        }

        if (SoundEffectPlayer.Instance != null)
            SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.trashBinSFX);

        packingManager.HandleDisposal(flowerObj);
        currentDisposable = null;
        return true;
    }

    void OnMouseDown()
    {
        if (PackingTutorial.Instance != null && PackingTutorial.Instance.ShouldBlockWholeBouquetDispose())
        {
            if (UIManager.Instance != null)
                UIManager.Instance.ShowMessage("Flower is correct, disposal not needed.");
            return;
        }

        packingManager.DisposeWholeBouquet();
    }

    public void OpenBin()
    {
        if (binAnimator != null)
            binAnimator.SetBool("Open", true);
    }
}