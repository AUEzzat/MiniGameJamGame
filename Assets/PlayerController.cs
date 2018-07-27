using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    JUMPOVERKILL,
    KINGOFTHEHILL
}
public class PlayerController : MonoBehaviour
{
    public GameObject currentPlatform;
    public GameMode gameMode;
    public int HP = 3;
    public bool grounded = false, canDash = true;
    float width;
    float height;
    public string horizontalLeft;
    public string verticalLeft;
    public float moveSpeed;
    public float jumpSpeed, dashSpeed, dashPushSpeed, jumpDelay, landingDelay;
    public float maxVelocity;
    public float fallSpeed;
    Rigidbody2D playerRigidBody;
    float xMoveDeadZone;
    float yMoveDeadZone;
    float jumpingSpeedTime = 1;
    public float dashCD, dashHitCD, hookHitCD, stunTime;
    public bool noobnessGuardActive = false, stunned = false;
    private float nGuardDeactiveCountdown = 0;
    private float lastDash;
    public int joystickNum;
    private string lastCol;
    Transform hook;
    public GameObject myHook;
    private ParticleSystem runDust, dashLeft, dashRight, jump, stomp;
    public ParticleSystem hookSplash, death;

    public AudioClip jumpSound, dashSound;
    private AudioSource mySource;

    private Animator myAnim;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        hook = transform.GetChild(0);
        xMoveDeadZone = 0.25f;
        yMoveDeadZone = 1f;
        playerRigidBody = GetComponent<Rigidbody2D>();
        Vector2 colliderMin = GetComponent<Collider2D>().bounds.min;
        Vector2 colliderMax = GetComponent<Collider2D>().bounds.max;
        width = colliderMax.x - colliderMin.x;
        height = colliderMax.y - colliderMin.y;

        runDust = transform.GetChild(6).GetChild(0).GetComponent<ParticleSystem>();
        dashLeft = transform.GetChild(6).GetChild(1).GetComponent<ParticleSystem>();
        dashRight = transform.GetChild(6).GetChild(2).GetComponent<ParticleSystem>();
        death = transform.GetChild(6).GetChild(3).GetComponent<ParticleSystem>();
        hookSplash = transform.GetChild(6).GetChild(4).GetComponent<ParticleSystem>();
        jump = transform.GetChild(6).GetChild(5).GetComponent<ParticleSystem>();
        stomp = transform.GetChild(6).GetChild(6).GetComponent<ParticleSystem>();
        

        mySource = GameObject.Find("SoundManager").GetComponent<AudioSource>();
        myAnim = transform.GetChild(4).GetComponent<Animator>();

        

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        float lasthooktime = myHook.GetComponent<HookScript>().lastHookHit;
        lastCol = col.gameObject.tag;
        if (col.gameObject.tag == "Plya" && lastDash + dashHitCD > Time.time && lasthooktime + hookHitCD > Time.time)
        {
            if (gameMode == GameMode.JUMPOVERKILL)
            {
                StartCoroutine(col.gameObject.GetComponent<PlayerController>().waitforStun());
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(col.gameObject.transform.up * jumpSpeed / 3, ForceMode2D.Impulse);

            }
            else if (gameMode == GameMode.KINGOFTHEHILL)
            {
                var dir = col.gameObject.transform.position - transform.position;
                col.gameObject.GetComponent<Rigidbody2D>().AddForce(((Vector3.Normalize(dir) / 1.5f) + (Vector3.Normalize(col.gameObject.transform.up)) / 2f) * dashPushSpeed, ForceMode2D.Impulse);
            }
        }
        else if (col.gameObject.tag == "Wall" && lastDash + dashHitCD > Time.time)
        {
            
            playerRigidBody.AddForce((transform.right + transform.up) * dashSpeed, ForceMode2D.Impulse);
            //playerRigidBody.AddForce(transform.up * dashSpeed, ForceMode2D.Impulse);
        }
        else if (col.gameObject.tag == "Wall")
        {
            playerRigidBody.AddForce((transform.right) * dashSpeed/2, ForceMode2D.Impulse);
        }

    }

    void Update()
    {
        
        if (playerRigidBody.velocity.x != 0 && grounded)
        {
            if (!runDust.isPlaying)
                runDust.Play();
            myAnim.SetBool("isRunning", true);

        }
        else
        {
            if (runDust.isPlaying)
                runDust.Stop();
            myAnim.SetBool("isRunning", false);

        }


        if (gameMode == GameMode.JUMPOVERKILL)
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(true);

            if (nGuardDeactiveCountdown > 0.5)
            {
                noobnessGuardActive = false;
                nGuardDeactiveCountdown = 0;
            }
            else if (noobnessGuardActive)
            {
                nGuardDeactiveCountdown += Time.deltaTime;
            }
        }
        else if (gameMode == GameMode.KINGOFTHEHILL)
        {
            //gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }

        // Time.timeScale = 0.5F;
        //Time.fixedDeltaTime = 0.02F * Time.timeScale;

        KeyCode dashKey;
        switch (joystickNum)
        {
            case 1:
                dashKey = KeyCode.Joystick1Button4;
                break;
            case 0:
            case 2:
            default:
                dashKey = KeyCode.Joystick2Button4;
                break;
            case 3:
                dashKey = KeyCode.Joystick3Button4;
                break;
            case 4:
                dashKey = KeyCode.Joystick4Button4;
                break;
            case 5:
                dashKey = KeyCode.Joystick5Button4;
                break;
        }

        KeyCode jumpKey;
        switch (joystickNum)
        {
            case 1:
                jumpKey = KeyCode.Joystick1Button2;
                break;
            case 0:
            case 2:
            default:
                jumpKey = KeyCode.Joystick2Button2;
                break;
            case 3:
                jumpKey = KeyCode.Joystick3Button2;
                break;
            case 4:
                jumpKey = KeyCode.Joystick4Button2;
                break;
            case 5:
                jumpKey = KeyCode.Joystick5Button2;
                break;
        }

        if (Input.GetKeyDown(dashKey))
        {
            Dash();
        }

        if (lastDash + dashCD < Time.time)
        {
            canDash = true;
        }


        jumpingSpeedTime += Time.deltaTime;
        if (jumpingSpeedTime > landingDelay || playerRigidBody.velocity.y < 0)
        {
            playerRigidBody.velocity -= new Vector2(0, fallSpeed);
            myAnim.SetBool("isJumping", false);

        }
        else if (playerRigidBody.velocity.y > 0)
        {
            playerRigidBody.velocity += new Vector2(0, jumpDelay / jumpingSpeedTime);
        }
        float xMove = Input.GetAxis(horizontalLeft);
        //float yMove = Input.GetAxis(verticalLeft);
        if (Math.Abs(xMove) < xMoveDeadZone && !stunned)
            xMove = 0;
        else if(Math.Abs(xMove) > xMoveDeadZone) {
            Move(xMove);
        }

        if (Input.GetKeyDown(jumpKey)) {
            Jump();
        }

        //------------------------------------------------------------------------------------------
        GameObject hittedObj = null;
        List<RaycastHit2D> hitList = new List<RaycastHit2D>();
        for (int i = -1; i < 2; i++)
        {
            hitList.Add(Physics2D.Raycast(new Vector3(width / 2f * i, -height / 1.6f, 0) + transform.position, -Vector2.up, 0.1f));
            Debug.DrawRay(new Vector3(width / 2f * i, -height / 1.6f, 0) + transform.position, -Vector2.up * 5f);
        }

        for (int i = 0; i < 3; i++)// (RaycastHit2D hit in hitList)
        {
            if (hitList[i].collider != null && (hitList[i].collider.gameObject.tag == "Ground" || hitList[i].collider.gameObject.tag == "Plya"))
            {
                jumpingSpeedTime = 1;
                if (hitList[i].collider.gameObject.tag == "Plya" && hitList[i].collider.gameObject != gameObject)
                {
                    hittedObj = hitList[i].collider.gameObject;
                }
                grounded = true;
                currentPlatform = hitList[i].collider.gameObject;
                myAnim.SetBool("isGrounded", true);

            }
        }
        if (hittedObj)
        {
            playerRigidBody.AddForce( ( (Vector3.Normalize(transform.up) / 3 ) + (-Vector3.Normalize(transform.right) / 6 ) ) * jumpSpeed, ForceMode2D.Impulse);
            if (!hittedObj.GetComponent<PlayerController>().noobnessGuardActive)
            {
                if (gameMode == GameMode.JUMPOVERKILL)
                {
                    stomp.Play();
                    hittedObj.GetComponent<PlayerController>().noobnessGuardActive = true;
                    hittedObj.GetComponent<PlayerController>().HP--;
                    hittedObj.transform.GetChild(1).GetChild(hittedObj.GetComponent<PlayerController>().HP).GetComponent<SpriteRenderer>().enabled = false;
                    if (hittedObj.GetComponent<PlayerController>().HP == 0)
                    {
                        //hittedObj.SetActive(false);
                        hittedObj.GetComponent<PlayerController>().death.Play();
                        hittedObj.transform.GetChild(3).gameObject.SetActive(false);
                        hittedObj.transform.GetChild(4).gameObject.SetActive(false);
                        GameObject.Destroy(hittedObj, 0.25f);
                    }
                }
                else if (gameMode == GameMode.KINGOFTHEHILL && !stunned)
                {
                    //hittedObj.GetComponent<PlayerController>().noobnessGuardActive = true;
                    StartCoroutine(hittedObj.GetComponent<PlayerController>().waitforStun());
                }
            }
        }
        //------------------------------------------------------------------------------------------


    }

    IEnumerator waitforStun()
    {
        stunned = true;
        myAnim.SetBool("isStunned", true);
        stomp.Play();
        yield return new WaitForSeconds(stunTime);
        stunned = false;
        myAnim.SetBool("isStunned", false);


    }

    public void Jump()
    {
        if (grounded && !stunned)
        {
            grounded = false;
            jump.Play();
            mySource.PlayOneShot(jumpSound);
            myAnim.SetBool("isJumping", true);
            myAnim.SetBool("isGrounded", false);
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
            playerRigidBody.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);

        }
    }

    public void Dash()
    {
        if (canDash && !stunned && !myHook.GetComponent<HookScript>().hooked)
        {
            playerRigidBody.AddForce(-transform.right * dashSpeed, ForceMode2D.Impulse);

            mySource.PlayOneShot(dashSound);
            myAnim.SetTrigger("Dashed");
            if (playerRigidBody.velocity.x > 0)
                dashRight.Play();
            else
            {
                dashLeft.Play();
            }
            canDash = false;
            lastDash = Time.time;
        }
    }

    public void Move(float xMove)
    {
        if (!stunned)
        {
            playerRigidBody.velocity += new Vector2(xMove * moveSpeed, 0);
            playerRigidBody.velocity = playerRigidBody.velocity.x > maxVelocity && playerRigidBody.velocity.x < maxVelocity - 10
                ? new Vector2(maxVelocity, 0) : playerRigidBody.velocity;
            if (playerRigidBody.velocity.x > 0 && !myHook.GetComponent<HookScript>().hooked)
            {
                playerRigidBody.transform.right = new Vector3(-1, 0, 0);
            }
            else if ((playerRigidBody.velocity.x <= 0 && !myHook.GetComponent<HookScript>().hooked))
            {
                playerRigidBody.transform.right = new Vector3(1, 0, 0);
            }
        }
    }
}
