using UnityEngine;

public class item_parts : MonoBehaviour
{
    [SerializeField] public GameObject outlineObject;

    void Start()
    {
        outlineObject.SetActive(false);
    }

    public void ShowOutline()
    {
        outlineObject.SetActive(true);
    }

    public void HideOutline()
    {
        outlineObject.SetActive(false);
    }
}