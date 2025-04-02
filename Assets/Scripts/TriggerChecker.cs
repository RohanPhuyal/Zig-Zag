using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    private PlatformSpawner spawner;
    private Rigidbody rb;

    public void SetSpawner(PlatformSpawner spawnerRef)
    {
        spawner = spawnerRef;
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Ball"))
        {
            Invoke(nameof(FallDown), 0.5f);
        }
    }

    void FallDown()
    {
        rb = GetComponentInParent<Rigidbody>();
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        rb.useGravity = true;
        rb.isKinematic = false;

        // Release platform instead of destroying it
        if (spawner != null)
        {
            Invoke(nameof(DelayedRelease), 1f);
        }
    }

    private void DelayedRelease()
    {
        if (spawner != null)
        {
            spawner.ReleasePlatform(transform.parent.gameObject);
        }
    }

    void OnDisable()
    {
        spawner = null; // Reset spawner reference when platform is deactivated

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // Cancel any pending invoke calls to prevent releasing the platform after deactivation
        CancelInvoke(nameof(DelayedRelease));
    }


}