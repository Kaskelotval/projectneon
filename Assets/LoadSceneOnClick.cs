using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{

    public void LoadByIndex()
    {
        SceneManager.LoadScene("Intro");
    }
    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void Quit()
    {
                Application.Quit();
    }
}