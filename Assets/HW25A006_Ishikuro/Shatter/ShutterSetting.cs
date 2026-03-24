using UnityEngine;

public class hutterSetting : MonoBehaviour
{
    public Transform tra;
    Vector3 ori, upp = new Vector3(0, 3, 0);

    public float time = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ori = tra.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        time = Mathf.Clamp01(time);
        tra.localPosition = ori + upp * time;
    }
}
