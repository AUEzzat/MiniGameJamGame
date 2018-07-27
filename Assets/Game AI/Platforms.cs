using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    public List<GameObject> LeftPlatforms;
    public List<GameObject> rightPlatforms;
    public GameObject ground;
    public GameObject sanctuary;

    public GameObject GetNext(Transform playerPos, GameObject currentPlatform)
    {
        if (currentPlatform == ground)
        {
            float leftDis = Vector3.Distance(playerPos.position, LeftPlatforms[0].transform.position);
            float rightDis = Vector3.Distance(playerPos.position, rightPlatforms[0].transform.position);
            return leftDis > rightDis ? rightPlatforms[0] : LeftPlatforms[0];
        }
        else if (currentPlatform == sanctuary)
        {
            return null;
        }

        int leftPlatformIndex = LeftPlatforms.IndexOf(currentPlatform);
        int rightPlatformIndex = rightPlatforms.IndexOf(currentPlatform);

        if (leftPlatformIndex != -1 && leftPlatformIndex < LeftPlatforms.Count - 1)
        {
            return LeftPlatforms[++leftPlatformIndex];
        }
        else if (rightPlatformIndex != -1 && rightPlatformIndex < rightPlatforms.Count - 1)
        {
            return rightPlatforms[++rightPlatformIndex];
        }

        return sanctuary;
    }

    public bool CorrectNextPlatform(GameObject nextPlat, GameObject currentPlat)
    {
        if(nextPlat == null && currentPlat == sanctuary)
        {
            return true;
        }
        else if(currentPlat == ground && (LeftPlatforms.IndexOf(nextPlat) == 0
            || rightPlatforms.IndexOf(nextPlat) == 0))
        {
            return true;
        }
        else if(nextPlat == sanctuary && (LeftPlatforms.IndexOf(currentPlat)==LeftPlatforms.Count-1
            || rightPlatforms.IndexOf(currentPlat) == rightPlatforms.Count - 1))
        {
            return true;
        }
        else if(LeftPlatforms.IndexOf(nextPlat) == LeftPlatforms.IndexOf(currentPlat)+1
            || rightPlatforms.IndexOf(nextPlat) == rightPlatforms.IndexOf(currentPlat) + 1)
        {
            return true;
        }
        return false;
    }
}
