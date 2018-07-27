using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour
{
    public int joystickNum;
    public GameObject player;
    private Rigidbody2D rb;
    public float speedReturn, speedGo, minDist, maxDist, hookCD, returnCD;

    private float distPlayer, distPlayerCenter, lastHookTime, lastReturnTime;
    public float lastHookHit = 0f;
    public bool move = true, hooked = false, canHook = true;
    private Vector3 Direc;
    private GameObject HookedObject;

    private LineRenderer lr;
    private Vector3 closestPos;

    public ParticleSystem  hookshot;
    public AudioSource mySource;
    public AudioClip hookSplash, hookShot;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();

        lastHookTime = 0;
        lastReturnTime = 0;

        //hookshot = transform.parent.GetChild(0).GetChild(1).GetComponent<ParticleSystem>();


    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.transform != gameObject.transform.parent && !hooked && !move && col.gameObject.tag == "Plya")
        {
            HookedObject = col.gameObject;
            col.gameObject.GetComponent<PlayerController>().hookSplash.Play();
            mySource.PlayOneShot(hookSplash);
            HookedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            //HookedObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            HookedObject.transform.parent = transform;
            move = true;
            lastHookTime = Time.time;
            lastHookHit = Time.time;
            hooked = true;
        }
    }

    void Update()
    {
        KeyCode fireKeyCode;
        switch (joystickNum)
        {
            case 1:
                fireKeyCode = KeyCode.Joystick1Button5;
                break;
            case 0:
            case 2:
            default:
                fireKeyCode = KeyCode.Joystick2Button5;
                break;
            case 3:
                fireKeyCode = KeyCode.Joystick3Button5;
                break;
            case 4:
                fireKeyCode = KeyCode.Joystick4Button5;
                break;
            case 5:
                fireKeyCode = KeyCode.Joystick5Button5;
                break;
        }

        if (Input.GetKeyDown(fireKeyCode))
        {
            Hook();
        }
        
        if (!canHook && move && lastHookTime + hookCD < Time.time )
        {
            canHook = true;
        }
        //distPlayer = Vector3.Distance(player.transform.position + Vector3.Normalize(player.transform.right), transform.position);
        distPlayerCenter = Vector3.Distance(player.transform.position, transform.position);

        if (distPlayerCenter >= maxDist || (lastReturnTime + returnCD < Time.time))
        {
            if (!move && !hooked)
            {
                move = true;
                lastHookTime = Time.time;
            }

        }

        if (distPlayerCenter <= minDist && hooked)
        {
            HookedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            HookedObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            HookedObject.transform.parent = null;
            hooked = false;
        }

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, player.transform.parent.position);

        //--------------------------------------------------------------------

        if (move)
        {
            Direc = (player.transform.position) - transform.position;
            rb.velocity = (Direc * speedReturn);
            //transform.position = player.transform.position;
        }
        else
        {
            closestPos = player.transform.parent.GetComponent<HookRotation>().closestPosition;
            Vector3 dir = closestPos - player.transform.position;
            //dir = new Vector3(dir.x, -dir.y);
            rb.velocity = Vector3.Normalize(dir) * speedGo;
            // Vector3 dir = targetPosition - transform.position;
            //rb.velocity = Vector3.Normalize(dir) * speedGo;
        }
        //--------------------------------------------------------------------


    }

    public void Hook()
    {
        bool stunned = player.transform.parent.GetComponent<PlayerController>().stunned;
        if (!hooked && canHook && !stunned)
        {
            hookshot.Play();
            mySource.PlayOneShot(hookShot);
            move = false;
            canHook = false;
            //closestPos = player.transform.parent.GetComponent<HookRotation>().closestPosition;
            lastReturnTime = Time.time;

        }
    }
}