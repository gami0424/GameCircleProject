using UnityEngine;

public class ScannablePart : MonoBehaviour
{
    public enum PartType
    {
        NormalPart1,
        NormalPart2,
        FinalPart
    }

    [SerializeField] private PartType partType;
    [SerializeField] private int amount = 1;
    [SerializeField] private FinalPartEventController finalPartEventController;

    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;
        if (!other.CompareTag("Player")) return;

        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory == null) return;

        switch (partType)
        {
            case PartType.NormalPart1:
                inventory.AddPart(amount);
                break;

            case PartType.NormalPart2:
                inventory.AddPart2(amount);
                break;

            case PartType.FinalPart:
                inventory.AddFinalPart(amount);
                inventory.ShowMessage("最後のパーツを手に入れた！修理しよう！！");

                if (finalPartEventController != null)
                {
                    finalPartEventController.TriggerFinalPartSequence();
                }
                break;
        }

        isCollected = true;
        gameObject.SetActive(false);
    }
}