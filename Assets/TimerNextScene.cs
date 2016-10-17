using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TimerNextScene : MonoBehaviour {

    public float timer;
    private float time;
    private int i;
	// Use this for initialization
	void Start () {
        time = 0f;
        i = SceneManager.GetActiveScene().buildIndex;

    }

    // Update is called once per frame
    void Update () {
        if (time > timer)
            SceneManager.LoadScene(i + 1);
        else
            time += Time.deltaTime;
                  
	}
}
