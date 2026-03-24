using UnityEngine;

public class BigPillerSetting : MonoBehaviour
{
    public Transform tra;
    Vector3 ori, rot = new Vector3(90, 0, 0);

    public Collider col;

    public float time = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ori = tra.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        time = Mathf.Clamp01(time);

        col.isTrigger = time > 0;
        
        tra.localEulerAngles = ori + rot * time;
    }
}
