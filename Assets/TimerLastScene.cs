using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TimerLastScene : MonoBehaviour
{

    public float timer;
    private float time;
    private int i;
    private bool fading = false;
    private Animator anim;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

        time = 0f;
        i = SceneManager.GetActiveScene().buildIndex;

    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            if (time > timer)
                SceneManager.LoadScene(0);
            else
                time += Time.deltaTime;
        }
        else
        {
            if (time > timer)
            {
                anim.Play("Fadeout");
                fading = true;
                timer = 1f;
            }
            else
                time += Time.deltaTime;

            if (Input.anyKey)
            {
                time = 0;
                timer = 1f;
                anim.Play("Fadeout");
                fading = true;
            }
        }

        //SceneManager.LoadScene(i + 1);
    }
}
