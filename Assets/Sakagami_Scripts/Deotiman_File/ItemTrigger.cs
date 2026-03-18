using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    public DrilmanAdvancedController robot; // ロボットをアサイン

    private void OnTriggerEnter(Collider other) // 2Dなら OnTriggerEnter2D
    {
        if (other.CompareTag("Player"))
        {
            if (robot != null)
            {
                robot.StartSequence(); // 一連の動きを開始
            }
            Destroy(gameObject);
        }
    }
}
