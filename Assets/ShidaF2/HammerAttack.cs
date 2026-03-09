using UnityEngine;

public class HammerAttack : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private bool canAttack = true;
   private BreakableWall wall;
    private void OnTriggerEnter(Collider other)
    {
        if (!canAttack) return;

   if(wall==null) wall = other.GetComponent<BreakableWall>();
        if (wall != null)
        {
            wall.TakeDamage(damage);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if(wall!=null)  wall = null;
    }
}