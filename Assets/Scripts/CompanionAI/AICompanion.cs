using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AICompanion : MonoBehaviour
{
    public PlayerController AIController;
    public GameObject BotGO;

    public PlayerController Player;
    public GameObject PlayerGO;

    public GameManagerScript GameManager;

    public float vertDistance;
    public float hortDistance;
    public float vertNow;
    public float hortNow;

    public float aimCursorX;
    public float aimCursorY;

    public float distanceBetween;
    public int distanceToBeginFollow = 2;
    public bool following;
    public int step = 0;
    private float stepDistanceRange = 0.2f;

    private bool hasTarget;
    public GameObject[] TargetGOs;
    private GameObject clostestTarget;
    private float AimDistance = 4.0f;

    // Start is called before the first frame update
    public void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isAIBot || SceneManager.GetActiveScene().name == "MainMenu")
        {
            return;
        }

        CursorAim();

        distanceBetween = Vector2.Distance(PlayerGO.transform.position, BotGO.transform.position);
        if (Mathf.Abs(distanceBetween) > distanceToBeginFollow)
        {
            StartCoroutine(DistanceCheck());
            following = true;
        }
        else
        {
            StopCoroutine(DistanceCheck());
            following = false;
            Player.PlayerSteps.Clear();
        }
        if (following)
        {
            FollowPlayer();
            return;
        }
        hortNow = 0.0f;
        vertNow = 0.0f;
    }

    void Shoot()
    {
        // TODO
    }

    public void FollowPlayer()
    {
        if (Player.PlayerSteps.Count == step)
        {
            return;
        }

        if (step < Player.PlayerSteps.Count)
        { 
            if (Mathf.Abs(Vector3.Distance(BotGO.transform.position, 
                Player.PlayerSteps[step])) < 0.3f)
            {
                Player.PlayerSteps.RemoveAt(step);
                if (Player.PlayerSteps.Count == step)
                {
                    return;
                }
            }

            hortDistance = Player.PlayerSteps[step].x - BotGO.transform.position.x;
            vertDistance = Player.PlayerSteps[step].y - BotGO.transform.position.y;

            hortNow = SetAxisInput(hortDistance);
            vertNow = SetAxisInput(vertDistance);
        }
    }

    public float SetAxisInput(float AxisDistance)
    {
        if (AxisDistance > stepDistanceRange)
        {
            return 1.0f;
        }
        if (AxisDistance < -stepDistanceRange)
        {
            return -1.0f;
        }
        // AxisDistance is > -0.1 and < 0.1 so set to 0;
        return 0.0f;
    }

    IEnumerator DistanceCheck()
    {
        float timeToWait = DiceRoll();
        timeToWait = timeToWait/100;
        yield return new WaitForSeconds(timeToWait);
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
        if (!hasTarget)
        {
            AquireTarget();
            return;
        }

        if (Mathf.Abs(Vector2.Distance(BotGO.transform.position, clostestTarget.transform.position)) 
            < AimDistance && clostestTarget != null)
        {
            float XTargetDistDiff = Mathf.Atan2(
                clostestTarget.transform.position.y - BotGO.transform.position.y,
                BotGO.transform.position.x -clostestTarget.transform.position.x);
            //float YTargetDistDiff = BotGO.transform.position.y - clostestTarget.transform.position.y;
            aimCursorX = Mathf.Cos(XTargetDistDiff);
            aimCursorY = Mathf.Sin(XTargetDistDiff);
        }
    }

    public void AquireTarget()
    {
        FindTargetEnemy();

        if (Mathf.Abs(Vector2.Distance(BotGO.transform.position, clostestTarget.transform.position)) > AimDistance)
        {
            clostestTarget = null;
            hasTarget = false;
            return;
        }

        hasTarget = true;
    }

    private void FindTargetEnemy()
    {
        TargetGOs = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = Mathf.Infinity;
        foreach (GameObject target in TargetGOs)
        {
            Vector2 distDiff = target.transform.position - BotGO.transform.position;
            float diagonalDistBetween = distDiff.sqrMagnitude;           
            if (diagonalDistBetween < distance)
            {
                clostestTarget = target;
            }
        }
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
