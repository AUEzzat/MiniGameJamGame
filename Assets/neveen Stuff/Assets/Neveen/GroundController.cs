using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GroundController : MonoBehaviour {

    public Text ScoreText;
    public Text GameoverText;
    public Text RestartText;
    private float nextActionTime = 0.0f;
    public float period = 0.1f;
    private int cntr;
    public int secToDie;
    bool gameOver;
    bool restart;
    public GameObject MyRegion;
    public GameObject[] regions;
    Vector3 []arr;
    List<int> listNumbers = new List<int>{0,1,2,3,4,5,6,7};

    private Vector3 startMarker;
    private Vector3 endMarker;
    void Start () {
        nextActionTime = Time.time;
        gameOver = false;
        restart = false;
        GameoverText.text = "";
        RestartText.text = "";
        cntr = 0;
        arr = new Vector3[listNumbers.Count];
        for (int i = 0; i < listNumbers.Count; i++)
        {
            arr[i] = regions[i].transform.position;
        } 
    }
	
	void Update () {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            cntr++;
            if(cntr > secToDie)
            {
                cntr = 0;
                ScoreText.text = (cntr / 10).ToString();               
                startMarker = MyRegion.transform.position;
                endMarker = arr[randomIntExcept()];
                MyRegion.transform.position = Vector3.Lerp(startMarker, endMarker, 1f);
                if (MyRegion.transform.localScale.x >= 0.4 && MyRegion.transform.localScale.y >= 0.4)
                    MyRegion.transform.localScale -= new Vector3(0.4f, 0.4f, 0.4f);
                else
                {
                    Application.Quit();
                    gameOver = true;
                    GameoverText.text = "Game Over :(";
                    RestartText.text = "Restart!!";
                }
            }
            else if (cntr % 10 == 0)
            {
                ScoreText.text = (cntr/10).ToString();
            }
        }
        if (gameOver)
        {
            SceneManager.LoadScene("StartUI", LoadSceneMode.Single);
        }
        
    }
    
    private int randomIntExcept()
    {
        int n = listNumbers.Count;
        return listNumbers[Random.Range(0, n) % n];
    }

}
