using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerAttack : MonoBehaviour {

    private bool attacking = false;

    private float attacktimer = 0;
    private float attackcd = 0.1f;

    public Collider2D attackTrigger;

    private Animator anim;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        attackTrigger.enabled = false;
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && !attacking)
        {
            attacking = true;
            attacktimer = attackcd;

             attackTrigger.enabled = true;

            anim.SetBool("Attack", true);
        }
        if(attacking)
        {
            if(attacktimer > 0)
            {
                attacktimer -= Time.deltaTime;
            }
            else
            {
                attacking = false;
                attackTrigger.enabled = false;
            }

            
        }
        anim.SetBool("Attack", true);

    }
}
