using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSCene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneManager.LoadScene("JumpMode", LoadSceneMode.Single);
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
