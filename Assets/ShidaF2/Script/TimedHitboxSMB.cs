using UnityEngine;

public class TimedHitboxSMB : StateMachineBehaviour
{
    [SerializeField] private string hitboxObjectName = "AttackHitbox";
    [SerializeField] private float activeStartTime = 0.2f;
    [SerializeField] private float activeEndTime = 0.5f;
   // [SerializeField] private GameObject hitboxObj;
   private GameObject hitboxObject;
    private bool isActive;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hitboxObject == null)
        {
            hitboxObject = FindHitbox(animator);
        }

        isActive = false;

        if (hitboxObject != null)
        {
            hitboxObject.SetActive(false);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hitboxObject == null)
        {
            return;
        }

        float normalizedTime = stateInfo.normalizedTime % 1f;
        bool shouldBeActive = normalizedTime >= activeStartTime && normalizedTime <= activeEndTime;

        if (shouldBeActive && !isActive)
        {
            hitboxObject.SetActive(true);
            isActive = true;
        }
        else if (!shouldBeActive && isActive)
        {
            hitboxObject.SetActive(false);
            isActive = false;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hitboxObject != null)
        {
            hitboxObject.SetActive(false);
        }

        isActive = false;
    }

    private GameObject FindHitbox(Animator animator)
    {
        Transform[] children = animator.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child.name == hitboxObjectName)
            {
                return child.gameObject;
            }
        }

        Debug.LogWarning($"TimedHitboxSMB: {hitboxObjectName} Ç™å©Ç¬Ç©ÇËÇ‹ÇπÇÒÅB");
        return null;
    }
}