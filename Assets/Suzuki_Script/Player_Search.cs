using UnityEngine;
using System.Collections;

public class Player_Search : MonoBehaviour
{
    [SerializeField] public GameObject scannerArea;

    bool isScanning = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isScanning)
        {
            StartCoroutine(Scan());
        }
    }

    IEnumerator Scan()
    {
        isScanning = true;

        scannerArea.SetActive(true);

        yield return new WaitForSeconds(5);

        scannerArea.SetActive(false);

        isScanning = false;
    }
}
