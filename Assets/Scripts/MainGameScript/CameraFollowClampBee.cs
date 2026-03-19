using UnityEngine;

// This script makes the camera follow a target (your bee)
// while clamping its movement within the background sprite
public class CameraFollowClampBee : MonoBehaviour
{
    /* Object follow Bee */
    public Transform target;

    /* The background sprite that defines the movement boundaries */
    public SpriteRenderer background;

    /* How smooth the camera movement is */
    // smooth timeee
    public float smoothTime = 0.12f;

    //Smooth to keep track of velocity
    float velocityX;

    bool hasSnapped = false;

    // LateUpdate runs AFTER all movement =
    void LateUpdate()
    {
        if (target == null || background == null) return;

        Camera cam = GetComponent<Camera>();

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        Bounds b = background.bounds;

        float minX = b.min.x + halfWidth;
        float maxX = b.max.x - halfWidth;

        float targetX = Mathf.Clamp(target.position.x, minX, maxX);

        //SNAP ON FIRST FRAME (no smooth)
        if (!hasSnapped)
        {
            transform.position = new Vector3(
                targetX,
                transform.position.y,
                transform.position.z
            );

            hasSnapped = true;
            return; // skip smoothing this frame
        }

        //Normal smooth follow AFTER snap
        float smoothX = Mathf.SmoothDamp(
            transform.position.x,
            targetX,
            ref velocityX,
            smoothTime
        );

        transform.position = new Vector3(
            smoothX,
            transform.position.y,
            transform.position.z
        );
    }
}
