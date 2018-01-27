using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerHolder : MonoBehaviour {

    Rigidbody2D rb;
    public float speed;
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
	}
	
	
	void FixedUpdate () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 motion = new Vector2(moveHorizontal,moveVertical);
        rb.AddForce(motion * speed);
	}
}
