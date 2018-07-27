using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ZoneScript : MonoBehaviour
{
    public List<GameObject> AllPlayers;
    public List<GameObject> PlayersInZone;
    private float LastTimer;
    public Text CountDownTimeText, RoundEndText;
    public float CountDownTime, TimerCD;
    public int fontSize, maxFontSize, fontSizeIncrementSpeed;
    public ParticleSystem ZonePoison, ZonePoison2;
    public bool endTime = false;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Plya")
        {
            PlayersInZone.Add(col.gameObject);
        }

        if(PlayersInZone.Count > 0)
        {
            PlayersInZone[0].transform.GetChild(6).GetChild(7).gameObject.SetActive(true);
            PlayersInZone[0].transform.GetChild(6).GetChild(7).GetComponent<ParticleSystem>().Play();
        }
        
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Plya")
        {
            col.transform.GetChild(6).GetChild(7).gameObject.SetActive(false);
            PlayersInZone.Remove(col.gameObject);
        }

        if (PlayersInZone.Count > 0)
        {
            PlayersInZone[0].transform.GetChild(6).GetChild(7).gameObject.SetActive(true);
            PlayersInZone[0].transform.GetChild(6).GetChild(7).GetComponent<ParticleSystem>().Play();
        }


    }

    void Start ()
    {
        PlayersInZone = new List<GameObject>();
        LastTimer = 0;
        CountDownTimeText.fontSize = fontSize;
        CountDownTimeText.text = CountDownTime.ToString();

    }

	void Update ()
    {
        if(CountDownTime != 0)
        {
            if (LastTimer + TimerCD < Time.time)
            {
                LastTimer = Time.time;
                CountDownTime -= TimerCD;
                CountDownTimeText.text = (Mathf.Round(CountDownTime*100)/100).ToString();
                CountDownTimeText.fontSize = fontSize;
            }

            if (CountDownTime <= 3)
            {
                if(!ZonePoison.isPlaying)
                {
                    ZonePoison.Play();
                    ZonePoison2.Play();
                }
                
                if (CountDownTimeText.fontSize < maxFontSize)
                    CountDownTimeText.fontSize += fontSizeIncrementSpeed;
            }

        }
        else
        {

            if(!endTime)
            {

                CountDownTimeText.text = "";

                if (PlayersInZone.Count == 0)
                {
                    RoundEndText.text = "Draw";
                    for (int i = 0; i < AllPlayers.Count; i++)
                    {
                            AllPlayers[i].GetComponent<PlayerController>().death.Play();
                            AllPlayers[i].transform.GetChild(3).gameObject.SetActive(false);
                            AllPlayers[i].transform.GetChild(4).gameObject.SetActive(false);
                    }
                }
                else
                {
                    RoundEndText.text = PlayersInZone[0].name + " Wins !";
                    for (int i = 0; i < AllPlayers.Count; i++)
                    {
                        if (AllPlayers[i] != PlayersInZone[0])
                        {
                            AllPlayers[i].GetComponent<PlayerController>().death.Play();
                            AllPlayers[i].transform.GetChild(3).gameObject.SetActive(false);
                            AllPlayers[i].transform.GetChild(4).gameObject.SetActive(false);

                        }
                    }
                }
                endTime = true;
                StartCoroutine(waitforRestart());
            }
        }

        
    }

    IEnumerator waitforRestart()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("JumpMode");



    }

}
