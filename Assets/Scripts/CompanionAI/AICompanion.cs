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
    public int BeginFollowDist = 2;
    public bool following;
    public int step = 0;
    private float stepDistanceRange = 0.2f;

    public GameObject[] TargetGOs;
    private GameObject closestTarget;
    private float AimDistance = 4.5f;

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

        TargetGOs = GameObject.FindGameObjectsWithTag("Enemy");

        CursorAim();

        distanceBetween = Vector2.Distance(PlayerGO.transform.position, BotGO.transform.position);
        if (Mathf.Abs(distanceBetween) > BeginFollowDist)
        {
            StartCoroutine(WaitBeforeFollow());
            following = true;
        }
        else
        {
            StopCoroutine(WaitBeforeFollow());
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
        // AxisDistance is > -0.2 and < 0.2 so set to 0;
        return 0.0f;
    }

    IEnumerator WaitBeforeFollow()
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
        AquireTarget();
        if (closestTarget != null)
        {
            float DistBetween = Mathf.Abs(Vector2.Distance(BotGO.transform.position,
                                            closestTarget.transform.position));
            if (DistBetween < AimDistance && TargetGOs.Length > 0)
            {
                AimBasedOnAtan2(closestTarget);
            }
            return;
        }
        // current idle cursor script, may be useful when the player is dead 
        // to show intent to revive
        AimBasedOnAtan2(PlayerGO);
    }

    public void AquireTarget()
    {
        FindClosestTargetEnemy();

        if (closestTarget == null)
        {
            return;
        }
        Debug.Log(closestTarget);
        float DistBetween = Mathf.Abs(Vector2.Distance(BotGO.transform.position,
                                            closestTarget.transform.position));
        if (DistBetween > AimDistance)
        {
            closestTarget = null;
            return;
        }
    }

    public void AimBasedOnAtan2(GameObject Target)
    {
        float angle = Mathf.Atan2(
                    Target.transform.position.y - BotGO.transform.position.y,
                    BotGO.transform.position.x - Target.transform.position.x);
        aimCursorX = Mathf.Cos(angle);
        aimCursorY = Mathf.Sin(angle);
    }

    private void FindClosestTargetEnemy()
    {
        closestTarget = null;
        if (TargetGOs.Length == 0)
        {
            return;
        }
        float distance = Mathf.Infinity;
        foreach (GameObject target in TargetGOs)
        {
            Vector2 distDiff = target.transform.position - BotGO.transform.position;
            float diagonalDistBetween = distDiff.sqrMagnitude;
            if (diagonalDistBetween < distance)
            {
                distance = diagonalDistBetween;
                closestTarget = target;
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

    public int DiceRoll()
    {
        int diceRoll = Random.Range(0, 101); //top of range is exclusive so +1 to desired max
        return diceRoll;
    }
}
