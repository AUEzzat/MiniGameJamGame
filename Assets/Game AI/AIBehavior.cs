using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIState
{
    SeekingPlatform,
    DefendingSanctuary
}

public class AIBehavior : MonoBehaviour
{
    public int playerNum;

    [SerializeField]
    string stateString;

    Platforms platformsData;
    ZoneScript zoneData;
    GameObject nextPlatform;
    HookScript hookScript;
    PlayerController playerController;
    AIState state = AIState.SeekingPlatform;
    bool jumpingToPlatform = false;

    // Use this for initialization
    void Start()
    {
        if(PlayerPrefs.GetInt("player"+ playerNum) == 0)
        {
            enabled = false;
        }
        hookScript = GetComponentInChildren<HookScript>();
        playerController = GetComponent<PlayerController>();
        platformsData = GameObject.Find("PlatformSeeker").GetComponent<Platforms>();
        zoneData = GameObject.Find("Zone").GetComponent<ZoneScript>();
        nextPlatform = platformsData.ground;
    }

    // Update is called once per frame
    void Update()
    {
        if (zoneData.endTime)
        {
            return;
        }

        stateString = state.ToString();


        switch (state)
        {
            case AIState.SeekingPlatform:

                HookOtherPlayers();

                if (!platformsData.CorrectNextPlatform(nextPlatform, playerController.currentPlatform))
                {
                    nextPlatform = platformsData.GetNext(transform, playerController.currentPlatform);
                    if (nextPlatform == null)
                    {
                        state = AIState.DefendingSanctuary;
                    }
                }

                else if (nextPlatform != null && Mathf.Abs(nextPlatform.transform.position.x - transform.position.x) > Random.Range(3, 8))
                {
                    playerController.Move(nextPlatform.transform.position.x - transform.position.x);
                }

                else if (nextPlatform != null && playerController.grounded && !jumpingToPlatform)
                {
                    playerController.Move(nextPlatform.transform.position.x - transform.position.x);
                    playerController.Jump();
                    StartCoroutine(SecondJump());
                    playerController.Move(
                        (nextPlatform.transform.position - transform.position).normalized.x * Random.Range(5, 15));
                    jumpingToPlatform = true;
                }

                break;
            case AIState.DefendingSanctuary:
                HookOtherPlayers();
                if (!zoneData.PlayersInZone.Contains(gameObject))
                {
                    state = AIState.SeekingPlatform;
                }

                if (zoneData.PlayersInZone.Count > 0 && zoneData.PlayersInZone[0] != gameObject)
                {
                    GameObject hillKing = zoneData.PlayersInZone[0];
                    playerController.Move(
                        (hillKing.transform.position - transform.position).normalized.x * 0.1f);
                    if (Vector2.Distance(hillKing.transform.position, transform.position) > Random.Range(7, 11))
                    {
                        playerController.Move(hillKing.transform.position.x - transform.position.x);
                    }
                    else if (Vector2.Distance(hillKing.transform.position, transform.position) < Random.Range(0.5f, 3))
                    {
                        playerController.Move(transform.position.x - hillKing.transform.position.x);
                        float randomJump = Random.Range(1, 10);
                        if (randomJump > 9)
                        {
                            playerController.Jump();
                        }
                    }
                }

                else
                {
                    float randomMovement = Random.Range(1, 10);
                    if (randomMovement > 9)
                    {
                        playerController.Move(
                            platformsData.sanctuary.transform.position.x - transform.position.x +
                            Random.Range(-3, 3));
                    }
                }

                break;
        }
    }

    IEnumerator SecondJump()
    {
        yield return new WaitForSeconds(Random.Range(0.25f, 0.35f));
        playerController.Jump();
        jumpingToPlatform = false;
    }

    void HookOtherPlayers()
    {
        if (!playerController.grounded)
        {
            return;
        }

        RaycastHit2D farRay = Physics2D.Raycast(transform.position - transform.right, -transform.right, 10);
        RaycastHit2D closeRay = Physics2D.Raycast(transform.position - transform.right, -transform.right, 5);

        Debug.DrawRay(transform.position - transform.right, -transform.right * 5, Color.black);
        Debug.DrawRay(transform.position - transform.right, -transform.right * 10, new Color(1, 0, 0, 0.5f));

        bool hookedPlya = false;
        if (farRay.collider != null && farRay.collider.CompareTag("Plya"))
        {
            hookScript.Hook();
            hookedPlya = true;
        }
        if (closeRay.collider != null && closeRay.collider.CompareTag("Plya"))
        {
            if (hookedPlya && playerController.currentPlatform != platformsData.ground)
            {
                playerController.Dash();
            }
            else if (nextPlatform == platformsData.sanctuary)
            {
                hookScript.Hook();
            }
            else
            {
                playerController.Jump();
                playerController.Move(
                    (closeRay.collider.transform.position -
                    transform.position).normalized.x * Random.Range(3, 8));
            }
        }
    }
}
