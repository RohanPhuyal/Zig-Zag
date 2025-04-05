using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    private PlatformSpawner spawner;
    private Rigidbody rb;
    private float releaseSpeed;
    private float fallSpeed;

    public void SetSpawner(PlatformSpawner spawnerRef)
    {
        spawner = spawnerRef;
        releaseSpeed = spawner.spawnSpeed + 0.2f;
        fallSpeed = spawner.spawnSpeed - 0.1f;
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Ball"))
        {
            Invoke(nameof(FallDown), 0.2f);
        }
    }

    void FallDown()
    {
        rb = GetComponentInParent<Rigidbody>();
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        rb.useGravity = true;
        rb.isKinematic = false;
        // Apply custom gravity force
        float gravityMultiplier = 10f; // Increase this value to make gravity stronger
        Vector3 customGravity = new Vector3(0, -9.81f * gravityMultiplier, 0); // Assuming global gravity is 9.81m/s²
        rb.AddForce(customGravity, ForceMode.Acceleration);
        // Release platform instead of destroying it
        if (spawner != null)
        {
            Invoke(nameof(DelayedRelease), releaseSpeed);
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