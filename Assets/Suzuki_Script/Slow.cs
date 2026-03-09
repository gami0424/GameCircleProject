using UnityEngine;

public class Slow : MonoBehaviour
{
    [SerializeField] private float slowMultiplier = 0.2f;
    [SerializeField] private Collider triggerCollider;

    private bool isSlow;
    private PlayerCharacterControllerFps playerController;

    private void Awake()
    {
        if (triggerCollider == null)
        {
            triggerCollider = GetComponent<Collider>();
        }

        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSlow)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        playerController = other.GetComponentInParent<PlayerCharacterControllerFps>();

        if (playerController == null)
        {
            Debug.LogWarning("PlayerCharacterControllerFps が見つかりませんでした。");
            return;
        }

        Debug.Log("減速しました。");
        isSlow = true;
        playerController.ApplySlow(slowMultiplier);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (playerController == null)
        {
            return;
        }

        playerController.ResetSpeed();
        isSlow = false;
        playerController = null;
    }
}