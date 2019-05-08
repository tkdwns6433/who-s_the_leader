using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScene_ButtonListener : MonoBehaviour
{
    public Image fade;
    public float fades=1f;

    public void gaemstart()
    {
        SceneManager.LoadScene(1);
    }
    public void Option()
    {
        SceneManager.LoadScene(2);
    }
    public void Credit()
    {
        SceneManager.LoadScene(3);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
    public void back()
    {
        SceneManager.LoadScene(0);
    }
}
