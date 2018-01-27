using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionsController : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player is in "+name);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Player Left " + name);
    }

}
