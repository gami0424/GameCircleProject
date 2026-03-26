using System.Collections;
using UnityEngine;

public class SlowTrapWall : MonoBehaviour
{
    [SerializeField] private Transform wallVisual;
    [SerializeField] private Vector3 loweredLocalPosition;
    [SerializeField] private Transform TargetLocalPosition;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float blockDuration = 4f;

    private Vector3 initialLocalPosition;
    private bool isWorking = false;
    private DrillManChaseController currentDrillMan;

    private void Awake()
    {
        if (wallVisual == null) wallVisual = transform;
        initialLocalPosition = wallVisual.localPosition;
        loweredLocalPosition = TargetLocalPosition.position;
    }

    public void SetPreviewProgress(float t)
    {
        t = Mathf.Clamp01(t);
        wallVisual.localPosition = Vector3.Lerp(initialLocalPosition, loweredLocalPosition, t);
    }

    public void ActivateWall(DrillManChaseController drillMan)
    {
        if (isWorking) return;
        currentDrillMan = drillMan;
        StartCoroutine(CoActivateWall());
    }

    private IEnumerator CoActivateWall()
    {
        isWorking = true;

        wallVisual.localPosition = loweredLocalPosition;

        if (currentDrillMan != null)
        {
            currentDrillMan.BlockByTrap(this);
        }

        yield return new WaitForSeconds(blockDuration);

        if (currentDrillMan != null)
        {
            currentDrillMan.ReleaseFromTrap();
        }

        wallVisual.localPosition = initialLocalPosition;
        isWorking = false;
    }

    public void BreakWall()
    {
        StopAllCoroutines();

        if (currentDrillMan != null)
        {
            currentDrillMan.ReleaseFromTrap();
        }

        wallVisual.localPosition = initialLocalPosition;
        isWorking = false;
    }
}