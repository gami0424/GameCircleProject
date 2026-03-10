using UnityEngine;

public class WallDamageTrigger : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    private BreakableWall wall;

    private void OnTriggerEnter(Collider other)
    {
      wall = other.GetComponent<BreakableWall>();
        Debug.Log(wall);
        if (wall != null)
        {
            wall.TakeDamage(damage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        wall = null;
    }
}