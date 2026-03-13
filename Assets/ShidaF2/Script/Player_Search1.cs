using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class Player_Search1 : MonoBehaviour
{
    [Header("探索機側のカウンター")]
    [SerializeField] private ScanTriggerCounter scanTriggerCounter;

    [Header("表示用UIテキスト")]
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private GameObject infoTextObj;



    [Header("表示時間")]
    [SerializeField] private float displayTime = 5f;


    [SerializeField] public GameObject scannerArea;

   private  bool isScanning = false;

    private bool isShowing = false;



    public void OnSearch(InputValue value)
    {
        if(value.isPressed && !isScanning)
        {
            StartCoroutine(Scan());

        }
    }



    IEnumerator Scan()
    {
        isScanning = true;

        scannerArea.SetActive(true);

        yield return new WaitForSeconds(5);

        if (!isShowing)
        {
            isShowing = true;
            ShowScanResult();
        }

        scannerArea.SetActive(false);

        isScanning = false;
        isShowing = false;

    }

    private void ShowScanResult()
    {
        if (scanTriggerCounter == null || infoText == null) return;

        scanTriggerCounter.RemoveNullParts();

        int count = scanTriggerCounter.PartCount;
        infoText.text = $"近くにパーツが {count} 個あります";
        infoText.gameObject.SetActive(true);
        infoTextObj.SetActive(true);
        StartCoroutine(HideInfoText());

    }


    IEnumerator HideInfoText()
    {
        yield return new WaitForSeconds(3f);
        infoText.gameObject.SetActive(false);
        infoTextObj.SetActive(false);
    }

    private void Update()
    {
        if (!isScanning) return;
     
        
    }
}
