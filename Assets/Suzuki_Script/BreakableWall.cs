using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public float hp = 50;
    public float currenthp;
[SerializeField]private PreDeathSpawner preDeathSpawner;

    void Awake()
    {
        currenthp = hp;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0)
        {
            return;
        }
        currenthp -= damage;

        if (currenthp <= 0)
        {

            if (preDeathSpawner != null)
            {
                preDeathSpawner.SpawnNow();
            }
            Break();
        }
    }

    void Break()
    {
        Destroy(gameObject);
    }
}