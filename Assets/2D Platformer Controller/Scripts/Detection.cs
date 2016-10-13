using UnityEngine;
using System.Collections;

public class Detection : MonoBehaviour
{
    public Enemy me;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            me.Detected();
    }
}
