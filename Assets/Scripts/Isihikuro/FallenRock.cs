using UnityEngine;

public class FalleRock : MonoBehaviour
{
    bool falled = false;
    public GameObject rockFake;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rockFake.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FallActing()
    {
        rockFake.SetActive(true);
        falled = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (falled) return;
            FallActing();
        }
    }
}
