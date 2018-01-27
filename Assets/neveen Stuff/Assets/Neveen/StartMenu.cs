using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {

    public void ShowModes()
    {
        SceneManager.LoadScene("ReadyUI", LoadSceneMode.Single);
    }
    public void Quit()
    {
        Application.Quit();
    }
    private void Update()
    {
       
    }
}
