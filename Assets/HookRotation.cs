using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookRotation : MonoBehaviour
{
    public string horizontalRight;
    public string verticalRight;
    public float disFromPlayer;
    Rigidbody2D playerRigidBody;
    Transform hook;
    Vector2 hookMove;
    float deadZone;
    public List<GameObject> Players;
    public Vector3 closestPosition;

    void Start()
    {
        deadZone = 0.1f;
        playerRigidBody = GetComponent<Rigidbody2D>();
        hook = transform.GetChild(0);
        hook.transform.position = playerRigidBody.transform.position + transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 hookMove = new Vector2(Input.GetAxis(horizontalRight), Input.GetAxis(verticalRight));
        if (hookMove.magnitude < deadZone)
        {
            hookMove = Vector2.zero;

            hook.transform.position = transform.position + (Vector3.Normalize(-transform.right));
        }
        else
        {
            float aim = Mathf.Atan2(hookMove.y, hookMove.x) ;
            hook.transform.position = transform.position + new Vector3(Mathf.Cos(aim), Mathf.Sin(aim));
        }
        float AIAim = -1000;
        closestPosition = Vector3.zero;
        for (int i = 0; i < Players.Count; i++)
        {
            if (!Players[i])
                continue;
            var dir = Players[i].transform.position - gameObject.transform.position;
            float val = Vector3.Dot(dir, hook.transform.right);
            if (val < 0 && val > AIAim)
            {
                closestPosition = Players[i].transform.position;
                AIAim = val;
            }
        }

    }
}
