using UnityEngine;

public class GasCanObject : MonoBehaviour
{
    [Header("連動する設定")]
    [SerializeField] private Rigidbody ceilingRb; // 天井のRigidbody
    [SerializeField] private GameObject explosionEffect; // 爆発エフェクト（任意）

    [Header("ステータス")]
    [SerializeField] private float hp = 10f;
    private bool isDestroyed = false;

    // 前述のWallDamageTriggerから呼ばれるメソッド
    public void TakeDamage(float damage)
    {
        if (isDestroyed) return;

        hp -= damage;
        Debug.Log($"ガス缶にダメージ！ 残りHP: {hp}");

        if (hp <= 0)
        {
            TriggerExplosion();
        }
    }

    private void TriggerExplosion()
    {
        isDestroyed = true;

        // 1. 爆発演出（もしあれば）
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 2. 天井を落下させる！
        if (ceilingRb != null)
        {
            // 固定を解除して物理演算を有効にする
            ceilingRb.isKinematic = false;
            // 勢いよく落とすために下向きの力を加える
            ceilingRb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
        }

        // 3. ガス缶自体を消す（または壊れた見た目にする）
        // 演出のために少し遅らせて消しても良いです
        Destroy(gameObject, 0.1f);

        Debug.Log("ガス缶が爆発し、天井が崩落しました！");
    }
}