using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MoveToNextLvl : MonoBehaviour
{

    public GameObject player;
    public string nextscene;
    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("You inside me ( ͡° ͜ʖ ͡°)");
        if (col.isTrigger != true && col.CompareTag("Player"))
        {
            //activate.SetActive(true);
            int i = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(i + 1);
        }
    }

}