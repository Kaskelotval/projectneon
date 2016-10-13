using UnityEngine;
using System.Collections;

public class TriggerActivation : MonoBehaviour {

    public GameObject player;
    public GameObject activate;
    public Animator anim;
    public bool start = false;

    private void Start()
    {
        Debug.Log("SETTING activate to false");
        activate.SetActive(true);
       
        anim.Play("TextPop");

        //anim = activate.GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("You inside me ( ͡° ͜ʖ ͡°)");
        if (col.isTrigger != true && col.CompareTag("Player"))
        {
            //activate.SetActive(true);
            anim.Play("TextFadeIn");
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        Debug.Log("Don't Leave me :( ");
        if (col.isTrigger != true && col.CompareTag("Player"))
        {
            //activate.SetActive(false);
            anim.Play("Textfade");

        }
    }
}