using UnityEngine;

public class WallDamageTrigger : MonoBehaviour
{
    [SerializeField] private float damage = 10f;


    private void OnTriggerEnter(Collider other)
    {
    
        BreakableWall wall = other.GetComponent<BreakableWall>();
        if (wall != null)
        {
            wall.TakeDamage(damage);
            Debug.Log("壁にダメージ: " + other.name);
            return;
        }

    
        GasCanObject gasCan = other.GetComponent<GasCanObject>();
        if (gasCan != null)
        {
            gasCan.TakeDamage(damage);
            Debug.Log("ガス缶にダメージ: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}