using UnityEngine;
using System.Collections;

public class Raycaster : MonoBehaviour
{
    public float distance = 5;
    private RaycastHit2D hit;
    private RaycastHit2D hitb;
    private Ray ray;
    public Enemy me;
    private Collider2D col;
    public int direction = 1;
    int player;

    void start()
    {
        me = transform.GetComponent<Enemy>();
        player = LayerMask.NameToLayer("Player");
    }

    void Update()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.right);
        Debug.DrawRay(transform.position, direction*distance * fwd);
        //hit = Physics2D.Raycast(transform.position, fwd, distance);
        hit = Physics2D.Raycast(transform.position, direction*fwd, distance, 1 << LayerMask.NameToLayer("Player"));
        if (hit.collider != null)
        {
            if (hit.collider.name == "Player" && hit.collider.name != "Obstacle2")
                transform.SendMessageUpwards("Detected");
        }
        Vector3 bwd = transform.TransformDirection(Vector3.left);
        Debug.DrawRay(transform.position, direction * bwd);
        hitb = Physics2D.Raycast(transform.position, direction * bwd, 1, 1 << LayerMask.NameToLayer("Player"));
        if (hitb.collider != null)
        {
            if (hitb.collider.name == "Player" && hitb.collider.name != "Obstacle2")
                transform.SendMessageUpwards("Detected");
        }


        //hit.transform.SendMessage("HitByRay");

    }
}   