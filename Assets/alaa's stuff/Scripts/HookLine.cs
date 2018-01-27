using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookLine : MonoBehaviour {

	public GameObject character;
	public GameObject character2;
	public Vector2 s;
	public LineRenderer lr;

	void Start () {
		s= new Vector2 (-1.0f,0);
	}
	
	void Update () {
		lr.material.SetTextureScale("_MainTex",new Vector2(1.0f, 1.0f));
		lr.enabled = true;
		lr.SetPosition(0, character.transform.position);
		lr.SetPosition(1, character2.transform.position);
		character.GetComponent<Rigidbody2D> ().AddForce (s);
	}
}
