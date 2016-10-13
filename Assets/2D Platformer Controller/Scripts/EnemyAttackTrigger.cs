using UnityEngine;
using System.Collections;

public class EnemyAttackTrigger : MonoBehaviour
{

    public int dmg = 20;

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Enemy Slashes Player");
        if (col.isTrigger != true && col.CompareTag("Player"))
        {
            col.SendMessageUpwards("DamagePlayer", dmg);
            transform.parent.SendMessageUpwards("attackPlayer");
        }
            
    }
}
