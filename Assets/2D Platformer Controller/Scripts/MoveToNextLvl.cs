using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveToNextLvl : MonoBehaviour
{

    public GameObject player;

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("You inside me ( ͡° ͜ʖ ͡°)");
        if (col.isTrigger != true && col.CompareTag("Player"))
        {
            //activate.SetActive(true);
            SceneManager.LoadScene("demo");
        }
    }

}