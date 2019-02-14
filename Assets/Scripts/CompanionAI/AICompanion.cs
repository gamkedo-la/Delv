using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICompanion : MonoBehaviour
{
    public float vertDistance;
    public float hortDistance;
    public float vertNow;
    public float hortNow;

    public float aimCursorX;
    public float aimCursorY;

    public float distanceBetween;
    public int distanceToBeginFollow = 4;

    public PlayerController AIController;
    public GameObject BotGO;

    public GameObject PlayerGO;

    public bool following;

    private Transform TargetPos;

    // Start is called before the first frame update
    public void Awake()
    {
        StartCoroutine(DistanceCheck());
    }

    // Update is called once per frame
    void Update()
    {
        aimCursorX = Mathf.Cos(Time.timeSinceLevelLoad);
        aimCursorY = Mathf.Sin(Time.timeSinceLevelLoad);
        distanceBetween = Vector2.Distance(PlayerGO.transform.position, BotGO.transform.position);
        if (Mathf.Abs(distanceBetween) > distanceToBeginFollow)
        {
            StartCoroutine(DistanceCheck());
        }
        else
        {
            StopCoroutine(DistanceCheck());
            following = false;
        }

        if (following)
        {
            FollowPlayer();
            return;
        }
        hortNow = 0.0f;
        vertNow = 0.0f;
    }

    void ShootAtTarget()
    {
        // TODO
    }

    public void FollowPlayer()
    {
        hortDistance = PlayerGO.transform.position.x - BotGO.transform.position.x;
        vertDistance = PlayerGO.transform.position.y - BotGO.transform.position.y;
        hortNow = Mathf.Clamp(hortDistance, -1, 1);
        vertNow = Mathf.Clamp(vertDistance, -1, 1);
        return;
    }

    IEnumerator DistanceCheck()
    {
        float timeToWait = DiceRoll();
        timeToWait = timeToWait/100;
        yield return new WaitForSeconds(timeToWait);
        Debug.Log("This is from the coroutine");
        following = true;
        yield break;
    }

    public float VertAxisNow()
    {
        return vertNow;
    }

    public float HortAxisNow()
    {
        return hortNow;
    }

    public void CursorAim()
    {
        // TODO
    }

    public float AimCursorX()
    {
        return aimCursorX;
    }

    public float AimCursorY()
    {
        return aimCursorY;
    }

    //void OnTriggerEnter(Collider other)
    //{

    //    if (other.tag == "Player")
    //    {
    //        following = false;
    //        Debug.Log("Close to player - don't move");
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player") 
    //    {
    //        following = true;
    //        Debug.Log("Player moving - follow player");
    //    }
    //}

    public int DiceRoll()
    {
        int diceRoll = Random.Range(0, 101); //top of range is exclusive so +1 to desired max
        return diceRoll;
    }
}
