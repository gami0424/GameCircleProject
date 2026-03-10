using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    public PlayerInput playerinput;
    public Vector2 lookinput;
    public Transform Cam;
    public float CamY;
    public float CamP;
    private InputDevice lastlookdevice;

    public float sens;
    public float mouselooksensivity = 0.08f;
    public float padlooksensivity = 2.2f;

    public Transform FollowTarget;
    public Transform FollowTarget2;

    public float targetheight = 0.8f;
    public float dis = 3f;
    private float curdis;
    public float disLerpSpeed = 10f;

    public float sphereradius = 0.2f;
    public LayerMask collisionMask = 0;

    public bool lockCursor = true;

    public bool lockOn;
    public GameObject[] enemies;
    public GameObject targetEnemy;
    public float detectRange = 5f;
    public float lockSpeed = 0.8f;
    public GameObject lockOnCanvas;
    public float lockOnHeight = 0.9f;

    private void Start()
    {
        var e = transform.rotation.eulerAngles;
        CamY = e.y;
        CamP = e.x;
        curdis = dis;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnEnable()
    {
        playerinput.onActionTriggered += OnActionTrigger;
    }

    private void OnDisable()
    {
        playerinput.onActionTriggered -= OnActionTrigger;
    }

    private void OnActionTrigger(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name == "Look")
        {
            lastlookdevice = ctx.control?.device;
        }
    }

    public void OnLook(InputValue value)
    {
        lookinput = value.Get<Vector2>();
    }

    private float GetSens()
    {
        if (lastlookdevice is Mouse) return mouselooksensivity;
        if (lastlookdevice is Gamepad) return padlooksensivity;
        return padlooksensivity;
    }

    private void LateUpdate()
    {
      
            UpdateLook();
        
    }

    private void UpdateLook()
    {
        if (FollowTarget == null || playerinput == null || Cam == null) return;
        sens = GetSens();
        CamY += lookinput.x * sens;
        CamP -= lookinput.y * sens;
        CamP = Mathf.Clamp(CamP, -30, 70);

        Vector3 targetpos = FollowTarget.position + Vector3.up * targetheight;
        transform.position = targetpos;
        transform.rotation = Quaternion.Euler(CamP, CamY, 0f);

        float disdir = dis;
        if (Physics.SphereCast(targetpos, sphereradius, -transform.forward, out var hit, dis, collisionMask, QueryTriggerInteraction.Ignore))
        {
            disdir = Mathf.Max(0.2f, hit.distance - 0.05f);
        }

        float t = 1f - Mathf.Exp(-disLerpSpeed * UnityEngine.Time.unscaledDeltaTime);
        curdis = Mathf.Lerp(curdis, disdir, t);
        Cam.localPosition = new Vector3(0f, 0f, -curdis);
        Cam.localRotation = Quaternion.identity;
    }

    public void OnLockOn(InputValue value)
    {
        if (value.isPressed)
        {
            if (!lockOn)
            {
     
                if (targetEnemy != null)
                {
                    lockOn = true;
                }
            }
            else
            {
                CancelLockOn();
            }
        }
    }

    private void ToggleLockOn()
    {
        /*if (targetEnemy)
        {
            var enemyhp = targetEnemy.GetComponentInParent<Enemy_State>();

       
            if (enemyhp != null && enemyhp.CurHP <= 0)
            {
                CancelLockOn();
                return;
            }

            var agent = targetEnemy.GetComponentInParent<NavMeshAgent>();
            float targetenemyheight = (agent != null ? agent.height / 6 : 0) + lockOnHeight;
            Vector3 targetpos = targetEnemy.transform.position + Vector3.up * targetenemyheight;
            lockOnCanvas.transform.position = targetpos;
            lockOnCanvas.SetActive(true);
        }
        else
        {
            CancelLockOn();
        }*/
    }

   
    private void CancelLockOn()
    {
        lockOn = false;
        targetEnemy = null;
        if (lockOnCanvas != null) lockOnCanvas.SetActive(false);

  
        Vector3 currentEuler = transform.rotation.eulerAngles;
        CamY = currentEuler.y;

        CamP = currentEuler.x;
        if (CamP > 180f) CamP -= 360f;
    }

 

    private void UpdateLockOn()
    {
        if (!targetEnemy) return;
        Vector3 targetpos = FollowTarget.position + Vector3.up * targetheight;
        transform.position = targetpos;

        var targetenemypos = targetEnemy.transform.position;
        Vector3 targetenemydir = (targetenemypos - transform.position).normalized;

    
        Quaternion targetenemyrotation = Quaternion.LookRotation(targetenemydir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetenemyrotation, UnityEngine.Time.unscaledDeltaTime * lockSpeed);

        float disdir = dis;
        if (Physics.SphereCast(targetpos, sphereradius, -transform.forward, out var hit, dis, collisionMask, QueryTriggerInteraction.Ignore))
        {
            disdir = Mathf.Max(0.2f, hit.distance - 0.05f);
        }

        float t = 1f - Mathf.Exp(-disLerpSpeed * UnityEngine.Time.unscaledDeltaTime);
        curdis = Mathf.Lerp(curdis, disdir, t);
        Cam.localPosition = new Vector3(0f, 0f, -curdis);
        Cam.localRotation = Quaternion.identity;
    }
}