using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class Player_Search1 : MonoBehaviour
{
    [Header("�T���@���̃J�E���^�[")]
    [SerializeField] private ScanTriggerCounter scanTriggerCounter;

    [Header("����pUI�e�L�X�g")]
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private GameObject infoTextObj;



    [Header("�������")]
    [SerializeField] private float displayTime = 5f;


    [SerializeField] public GameObject scannerArea;

    private bool isScanning = false;

    private bool isShowing = false;



    public void OnSearch(InputValue value)
    {
        if (value.isPressed && !isScanning)
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
        infoText.text = $"�߂��Ƀp�[�c�� {count} ����܂�";
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
