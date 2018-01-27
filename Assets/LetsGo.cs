using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LetsGo : MonoBehaviour {
    public List<Toggle> toggles;
    public List<GameObject> players;
    private List<Animator> animators = new List<Animator>();
    // Use this for initialization
    void Start () {
		for(int i = 0; i < 4; i++)
        {
            animators.Add(players[i].transform.GetChild(1).GetComponent<Animator>());
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Joystick2Button2) || Input.GetKey(KeyCode.Joystick5Button2)) 
        {
            if (toggles[0].isOn)
            {
                toggles[0].isOn = false;
                animators[0].SetBool("isRunning", false);
            }
            else
            {
                toggles[0].isOn = true;
                animators[0].SetBool("isRunning", true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Joystick3Button2) )
        {
            if (toggles[1].isOn)
            {
                toggles[1].isOn = false;
                animators[1].SetBool("isStunned", false);
            }
            else
            {
                toggles[1].isOn = true;
                animators[1].SetBool("isStunned", true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Joystick4Button2) )
        {
            if (toggles[2].isOn)
            {
                toggles[2].isOn = false;
                animators[2].SetBool("isJumping", false);
            }
            else
            {
                toggles[2].isOn = true;
                animators[2].SetBool("isJumping", true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button2) )
        {
            if (toggles[3].isOn)
            {
                toggles[3].isOn = false;
                animators[3].SetBool("isRunning", false);
            }
            else
            {
                toggles[3].isOn = true;
                animators[3].SetBool("isRunning", true);
            }
        }
        bool ready = true;
        for (int i = 0; i < 4; i++)
        {
            if (!toggles[i].isOn)
            {
                ready = false;
            }
        }
        if (ready)
        {
            StartCoroutine(startGame());
        }
    }
    IEnumerator startGame()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("JumpMode", LoadSceneMode.Single);
    }
}
