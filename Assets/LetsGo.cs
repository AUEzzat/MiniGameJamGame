using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LetsGo : MonoBehaviour
{
    List<bool> enabledPlayers = new List<bool>() { true, true, true, true };
    public List<Toggle> toggles;
    public List<GameObject> players;
    private List<Animator> animators = new List<Animator>();
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            animators.Add(players[i].transform.GetChild(1).GetComponent<Animator>());
        }
    }

    public void SetAIPlayer(int number)
    {
        animators[number].SetBool("isRunning", toggles[number].isOn);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick2Button2) || Input.GetKey(KeyCode.Joystick5Button2))
        {
            if (toggles[0].isOn)
            {
                toggles[0].isOn = false;
                enabledPlayers[0] = true;
            }
            else
            {
                toggles[0].isOn = true;
                enabledPlayers[0] = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Joystick3Button2))
        {
            if (toggles[1].isOn)
            {
                toggles[1].isOn = false;
                enabledPlayers[1] = true;
            }
            else
            {
                toggles[1].isOn = true;
                enabledPlayers[1] = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Joystick4Button2))
        {
            if (toggles[2].isOn)
            {
                toggles[2].isOn = false;
                enabledPlayers[2] = true;
            }
            else
            {
                toggles[2].isOn = true;
                enabledPlayers[2] = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            if (toggles[3].isOn)
            {
                toggles[3].isOn = false;
                enabledPlayers[3] = true;
            }
            else
            {
                toggles[3].isOn = true;
                enabledPlayers[3] = false;
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
        for (int i = 0; i < 4; i++)
        {
            PlayerPrefs.SetInt("player" + i, Convert.ToInt32(enabledPlayers[i]));
        }
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("JumpMode", LoadSceneMode.Single);
    }
}
