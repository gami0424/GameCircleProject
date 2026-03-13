using UnityEngine;

public class WallDamageTrigger : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    // 変数を特定のクラスに固定せず、一時的に保持する形にします

    private void OnTriggerEnter(Collider other)
    {
        // 1. まず「壁」かどうかチェック
        BreakableWall wall = other.GetComponent<BreakableWall>();
        if (wall != null)
        {
            wall.TakeDamage(damage);
            Debug.Log("壁にダメージ: " + other.name);
            return; // 壁だったらここで終了
        }

        // 2. 壁じゃなければ「ガス缶」かどうかチェック
        GasCanObject gasCan = other.GetComponent<GasCanObject>();
        if (gasCan != null)
        {
            gasCan.TakeDamage(damage);
            Debug.Log("ガス缶にダメージ: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // wall 変数をメンバ変数から外したので、ここは空でOKです
    }
}