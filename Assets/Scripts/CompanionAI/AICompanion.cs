using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AICompanion : MonoBehaviour
{
    [Header("Debug")]
    public bool DEBUG_AI = true; // if true, spam the debug console
    [Space]
    [Header("AI Script")]
    public PlayerController AI;
    [Space]
    [Header("Player Main Components")]
    public PlayerController Player;
    public GameObject PlayerGO;
    [Space]
    [Header("Distance From Player")]
    public float distanceFromPlayer;
    [Space]
    [Header("Distance From Player Step")]
    public float vertDistance;
    public float hortDistance;
    private float BeginFollowDist = 3.0f;
    private float StopFollowDist = 1.0f;
    private int step = 0;
    [Space]
    [Header("Controller Axis Input")]
    public float vertNow;
    public float hortNow;
    [Space]
    [Header("Aiming Manager")]
    public float aimCursorX;
    public float aimCursorY;
    Vector2 randomAimingPoint;
    Vector2 randomDirection;
    public float currentAngle;
    public float goalAngle;
    public float angleDiff;
    public float lerpAimingAngleDiff;
    private const float oneDegreeInRadians = Mathf.PI / 180;
    private const float quarterCircleInRadians = Mathf.PI / 2;
    private const float halfCircleInRadians = Mathf.PI;
    private const float fullCircleInRadians = 2 * Mathf.PI;
    private const float idleAngleCheckDifference = 30.0f;
    [Space]
    [Header("AI States")]
    public bool following;
    public bool combatFollowing;
    public bool meandering;
    private readonly int idleTimerFull = 60;
    public int idleTimer;
    public bool arrivedAtMeanderDest;
    private bool meanderingInputSet;
    public bool idleAiming;
    private Coroutine idleAimingWait;
    public bool inCutScene;
    public bool targetAquired;
    public bool targetInRange;
    [Space]
    [Header("Shooting Manager")]
    public GameObject[] TargetGOs;
    public GameObject closestTarget;
    public float shotTimer = 0;
    private float AimDistance = 5.0f;
    [Space]
    [Header("Meandering Destination")]
    public GameObject meanderDestination;
    private int castCount;
    private RaycastHit2D[] hitArray = new RaycastHit2D[10];
    readonly private float meanderDistanceCheck = 1f;
    [Space]
    [Header("Resource Manager")]
    public int itemCount;
    public Collider2D[] itemArray = new Collider2D[10];
    public Collider2D nearestResource;
    private int mana = 1;
    private float itemSeekRadius = 5.0f;
    private float distanceToResource;
    private float PlayerDistToResource;
    private float BotDistToResource;

    private Collider2D[] AIColliders;
    private Collider2D AICollider;

    private int containerLayer;
    private int itemLayer;
    private ContactFilter2D itemLayerFilter;

    // Start is called before the first frame update
    public void Awake()
    {
        AI = GetComponent<PlayerController>();

        PlayerGO = GameObject.FindGameObjectWithTag("Player");
        Player = PlayerGO.GetComponentInParent<PlayerController>();

        AIColliders = GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D box in AIColliders)
        {
            if (box.isTrigger)
            {
                AICollider = box;
            }
        }

        StartCoroutine("checkSurroundingsForPotions");

        closestTarget = null;
        currentAngle = quarterCircleInRadians;

        idleTimer = idleTimerFull;

        containerLayer = LayerMask.NameToLayer("Container");
        itemLayer = LayerMask.NameToLayer("Items");
        // the item layer is number 16
        // but the function below wants a BITMASK
        // so it wants the 16th bit to be a 1
        // which is two to the power of whatever bit we want to set
        // eg 00000000000000001000000000000000 // Mathf.Pow(2f,16f)
        //itemLayerFilter.SetLayerMask(itemLayer); << How I tried to make this work ~~ vv solution
        itemLayerFilter.layerMask.value = 1 << itemLayer; //65536; WORKS!!! // 2^16 (16==items layer)
        //if (DEBUG_AI) Debug.Log("AI Companion itemLayerFilter.layerMask is: " + itemLayerFilter.layerMask.value);
        itemLayerFilter.useLayerMask = true;
        itemLayerFilter.useTriggers = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManagerScript.instance.isAIBot)
        {
            StopCoroutine("checkSurroundingsForPotions");
            enabled = false;
            return;
        }

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            return;
        }

        if (AI.isDead)
        {
            AIReset();
            return;
        }

        TargetGOs = GameObject.FindGameObjectsWithTag("Enemy");

        CursorAim();
        AIMoveBasedOnState();
    }

    public void AIReset()
    {
        meandering = false;
        setIdleTimer();
        meanderingInputSet = false;
        arrivedAtMeanderDest = false;
        combatFollowing = false;
        following = false;
        targetAquired = false;
        targetInRange = false;
        ZeroOutInput();
        closestTarget = null;
        nearestResource = null;
        idleAimingWait = null;
        StopCoroutine("checkSurroundingsForPotions");
        StartCoroutine("checkSurroundingsForPotions");
    }

    public bool FollowPlayerCheck()
    {
        distanceFromPlayer = Vector2.Distance(PlayerGO.transform.position, transform.position);
        if (Mathf.Abs(distanceFromPlayer) > BeginFollowDist && !following)
        {
            following = true;
            return true;
        }

        if (Mathf.Abs(distanceFromPlayer) <= StopFollowDist)
        {
            following = false;
            Player.PlayerSteps.Clear();
            return false;
        }
        return false;
    }

    public bool Shoot()
    {
        if (closestTarget == null || closestTarget.layer == containerLayer)
        {
            return false;
        }

        shotTimer += 1;

        if (Mathf.Approximately(Mathf.Sin(shotTimer * oneDegreeInRadians), -1))
        {
            return true;
        }
        return false;
    }

    public void AIMoveBasedOnState()
    {
        if (inCutScene)
        {
            AIReset();
            return;
        }

        if (Player != null)
        {
            if (Player.isDead)
            {
                meandering = false;
                setIdleTimer();
                meanderingInputSet = false;
                arrivedAtMeanderDest = false;
                following = false;
                combatFollowing = false;
                targetAquired = false;
                idleAimingWait = null;
                goalAngle = AimBasedOnAtan2(PlayerGO);
                lerpAimingAngle(5);
                getToPlayerToRevive();
                return;
            }

            if (nearestResource == null)
            {
                ResourceManager();
            }

            if (nearestResource != null)
            {
                meandering = false;
                arrivedAtMeanderDest = false;
                meanderingInputSet = false;
                setIdleTimer();
                following = false;
                combatFollowing = false;
                idleAimingWait = null;
                GoTowardNeededResource();
                return;
            }

            if (closestTarget != null)
            {
                if (closestTarget.layer == containerLayer)
                {
                    meandering = false;
                    arrivedAtMeanderDest = false;
                }
                else
                {
                    // MORTAL KOMBAT!!!
                    meandering = false;
                    arrivedAtMeanderDest = false;
                    meanderingInputSet = false;
                    following = false;
                    idleAimingWait = null;
                    combatManeuvers();
                    return;
                }
            }

            if (following || combatFollowing || FollowPlayerCheck())
            {
                meandering = false;
                arrivedAtMeanderDest = false;
                meanderingInputSet = false;
                idleAimingWait = null;
                setIdleTimer();
                FollowPlayer();
                return;
            }

            if (!meandering)
            {
                if (closestTarget == null)
                {
                    setIdleTimer();
                    SetMeanderingDestination();
                }
            }
            else
            {
                Meander();
            }
        }
    } // end of AIMoveBasedOnState():

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerGO)
        {
            if (DEBUG_AI) Debug.Log("AI: Bumped by Player");
            meanderDestination.transform.position = transform.position;
            AIReset();
        }

        if (nearestResource != null && collision.gameObject == nearestResource.gameObject)
        {
            itemArray = new Collider2D[10];
            nearestResource = null;
            StartCoroutine("checkSurroundingsForPotions");
            itemCount = 0;
            if (DEBUG_AI) Debug.Log("AI: Got Resource");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

    }

    public void getToPlayerToRevive()
    {
        if (DEBUG_AI) Debug.Log("AI going to player to revive");
        if (Mathf.Abs(Vector3.Distance(transform.position,
                PlayerGO.transform.position)) > 0.6f)
        {
            Vector2 direction = ReturnNormalizedVector(PlayerGO.transform.position);
            SetInputAmount(direction, "fast");
            return;
        }
        ZeroOutInput();
    }

    public void ResourceManager()
    {
        //if (DEBUG_AI)
        //{
        //    if (nearestResource != null)
        //    {
        //        Debug.Log("nearestResource is: " + nearestResource.name, nearestResource);
        //    }
        //    else if (nearestResource == null)
        //    {
        //        Debug.Log("nearestResource is: null");
        //    }
        //}

        if (itemCount == 0)
        {
            //if (DEBUG_AI) Debug.Log("AI: No potions near me");
            return;
        }

        if ((AI.Health + AI.MaxHealth / 2) < Player.Health)
        {
            FindNearestResource("HealthPotion");
            return;
        }

        if (AI.EnergyType == mana && (AI.Energy + AI.MaxEnergy / 2) < Player.Energy)
        {
            FindNearestResource("ManaPotion");
            return;
        } // end of mana type/amount check
    } // end of ResourceManager();

    IEnumerator checkSurroundingsForPotions()
    {
        while (nearestResource == null)
        {
            itemCount = Physics2D.OverlapCircle(transform.position, itemSeekRadius, itemLayerFilter, itemArray);
            if (DEBUG_AI) Debug.Log("Looking for potions");
            yield return new WaitForSeconds(0.75f);
        }
    }

    public void FindNearestResource(string potionName)
    {
        // search for nearby potions - spritey way
        // idea: what if we just do Vector2.Distance(a,b)
        // on an array of potion sprites?
        // nah that sounds slow and old fashioned

        // search for nearby potions - physicsy way:

        Collider2D item;

        //if (DEBUG_AI) Debug.Log(itemCount + " item colliders near " + name + " at " + transform.position);

        for (int num = 0; num < itemCount; num++)
        {
            item = itemArray[num];
            if (item == null)
            {
                if (DEBUG_AI) Debug.Log("Nearby item is null! That seems wrong.");
                continue;
            }
            //itemGO = item.GetComponent<GameObject>(); // not the collider, the parent sprite
            //itemGO = item.transform.parent.gameObject; // no need to traverse hierarchy
            //SpriteRenderer itemSprite = itemGO.GetComponent<SpriteRenderer>(); // errors out
            SpriteRenderer itemSprite = item.GetComponent<SpriteRenderer>();
            if (itemSprite && (itemSprite.sprite.name == potionName))
            {
                nearestResource = item;
            }
        }
    }

    public float distFromTarget;
    public float targetDistFromPlayer;
    private float tangentDirection = quarterCircleInRadians;

    public void combatManeuvers()
    {
        Vector2 direction = Vector2.zero;

        distFromTarget = Vector2.Distance(transform.position, closestTarget.transform.position);
        targetDistFromPlayer = Vector2.Distance(closestTarget.transform.position, PlayerGO.transform.position);

        if (distFromTarget > (AimDistance - 1f) && !targetInRange)
        {
            direction = ReturnNormalizedVector(closestTarget.transform.position);
        } else if (distFromTarget < 1f && !targetInRange)
        {
            direction = ReturnNormalizedVector(-closestTarget.transform.position);
        }
        else
        {
            targetInRange = true;
        }

        if (Random.value > 0.99f)
        {
            tangentDirection = -tangentDirection;
        }

        if (targetInRange)
        {
            direction.x = Mathf.Cos(-goalAngle + tangentDirection);
            direction.y = Mathf.Sin(-goalAngle + tangentDirection);
        }

        if (targetDistFromPlayer > BeginFollowDist + 2.0f)
        {
            combatFollowing = true;
            targetDistFromPlayer = 0;
            targetAquired = false;
            targetInRange = false;
            closestTarget = null;
            Player.PlayerSteps.Clear();
            return;
        }

        SetInputAmount(direction, "combat");
    }

    public void SetMeanderingDestination()
    {
        Vector2 randomPointInCircle = Random.insideUnitCircle * 2.5f;
        Vector3 meanderingPoint = new Vector3(PlayerGO.transform.position.x + randomPointInCircle.x,
                                              PlayerGO.transform.position.y + randomPointInCircle.y,
                                              PlayerGO.transform.position.z);
        if (PlayerGO.gameObject.GetComponent<Collider2D>().OverlapPoint(meanderingPoint))
        {
            if (DEBUG_AI) Debug.Log("AI: Meandering Destination inside player, waiting before trying again");
            meanderDestination.transform.position = transform.position;
            meanderingInputSet = true;
            meandering = true;
            return;
        }

        Vector2 distanceToMeanderPoint = transform.position - meanderingPoint;
        if (distanceToMeanderPoint.magnitude < meanderDistanceCheck)
        {
            if (DEBUG_AI) Debug.Log("AI: Meandering Destination is too close, waiting before trying again");
            meanderDestination.transform.position = transform.position;
            meanderingInputSet = true;
            meandering = true;
            return;
        }

        Vector3 direction = meanderingPoint - transform.position;
        castCount = AICollider.Cast(direction, hitArray, direction.magnitude + 0.1f);
        for (int i = 0; i < castCount; i++)
        {
            if (hitArray[i].collider.isTrigger)
            {
                continue;
            }

            if (hitArray[i].collider != null)
            {
                if (DEBUG_AI) Debug.Log("AI: Meandering path goes through " + hitArray[i].collider.name + ", waiting before trying again", hitArray[i].collider);
                meanderDestination.transform.position = transform.position;
                meanderingInputSet = true;
                meandering = true;
                return;
            }
        }
        meanderDestination.transform.position = meanderingPoint;
        meandering = true;
    }

    public void Meander()
    {
        Vector2 heading = meanderDestination.transform.position - transform.position;
        float Distance = heading.magnitude;
        //if (DEBUG_AI) Debug.Log("AI: I am " + Distance + " away from meander point");
        if (Distance < 0.3f || arrivedAtMeanderDest)
        {
            if (!arrivedAtMeanderDest)
            {
                arrivedAtMeanderDest = true;
            }

            if (idleTimer > 0)
            {
                ZeroOutInput();
                idleTimer--;
                return;
            }

            if (idleTimer <= 0 && !idleAiming)
            {
                setIdleTimer();
                meandering = false;
                meanderingInputSet = false;
                arrivedAtMeanderDest = false;
                return;
            }
        }

        if (!meanderingInputSet)
        {
            Vector2 direction = heading.normalized;
            SetInputAmount(direction, "slow");
            if (DEBUG_AI) Debug.Log("AI: Meandering input set, moving to target");
            meanderingInputSet = true;
        }
    }

    private void setIdleTimer()
    {
        if (idleTimer < idleTimerFull)
        {
            int additionalTime = Random.Range(0, 2500) / 100;
            idleTimer = idleTimerFull + additionalTime;
        }
    }

    public void GoTowardNeededResource()
    {
        if (BotDistToResource > itemSeekRadius)
        {
            if (DEBUG_AI) Debug.Log("AI: I have moved away from the resource");
            AIReset();
            return;
        }

        PlayerDistToResource = Vector2.Distance(Player.transform.position, nearestResource.transform.position);
        BotDistToResource = Vector2.Distance(transform.position, nearestResource.transform.position);
        if (PlayerDistToResource < BotDistToResource)
        {
            if (DEBUG_AI) Debug.Log("AI: Player is closer to resource, wait to see what happens");
            ZeroOutInput();
            return;
        }

        Vector2 direction = ReturnNormalizedVector(nearestResource.transform.position);
        SetInputAmount(direction, "medium");
    }

    private Vector3 ReturnNormalizedVector(Vector3 target)
    {
        Vector2 heading = target - transform.position;
        Vector2 direction = heading.normalized;
        return direction;
    }

    public void FollowPlayer()
    {
        if (Player.PlayerSteps.Count == step)
        {
            ZeroOutInput();
            return;
        }

        if (step < Player.PlayerSteps.Count)
        {
            if (Mathf.Abs(Vector3.Distance(transform.position,
                Player.PlayerSteps[step])) < 0.3f)
            {
                Player.PlayerSteps.RemoveAt(step);
                if (Player.PlayerSteps.Count == step)
                {
                    combatFollowing = false;
                    ZeroOutInput();
                    return;
                }
            }

            Vector2 direction = ReturnNormalizedVector(Player.PlayerSteps[step]);
            SetInputAmount(direction, "fast");
        }
    }

    public void SetInputAmount(Vector2 direction, string tempo)
    {
        float inputIntensity = 0f;
        switch (tempo)
        {
            case "slow":
                inputIntensity = Random.Range(0.10f, 0.15f);
                break;
            case "medium":
                inputIntensity = Random.Range(0.50f, 0.60f);
                break;
            case "combat":
                inputIntensity = Random.Range(0.80f, 0.90f);
                break;
            case "fast":
                inputIntensity = 1f;
                break;
            default:
                ZeroOutInput();
                return;
        }

        hortNow = direction.x * inputIntensity;
        vertNow = direction.y * inputIntensity;
    }

    public void ZeroOutInput()
    {
        hortNow = 0;
        vertNow = 0;
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
            idleAimingWait = null;
            idleAiming = false;
            float DistBetween = Mathf.Abs(Vector2.Distance(transform.position,
                                            closestTarget.transform.position));
            if (DistBetween < AimDistance)
            {
                goalAngle = AimBasedOnAtan2(closestTarget);
                currentAngle = makeAnglePositiveAndInt(goalAngle);
                goalAngle = makeAnglePositiveAndInt(goalAngle);
                lerpAimingAngleDiff = 0f;
                lerpAimingAngle(5);
            }
            return;
        }

        if (!Mathf.Approximately(hortNow, 0))
        {
            aimCursorY = 0;
            angleDiff = 0;
            idleAimingWait = null;
            lerpAimingAngleDiff = 0f;
            if (hortNow > 0)
            {
                goalAngle = halfCircleInRadians;
            }
            else
            {
                goalAngle = 0;
            }

            lerpAimingAngle(5);
        }

        if (idleAimingWait == null)
        {
            idleAiming = false;
            idleAimingWait = StartCoroutine("WaitBeforeIdleAim");
            return;
        }

        if (Mathf.Approximately(hortNow, 0))
        {
            idleAim();
        }
    }

    private void lerpAimingAngle(float smoothing = 1.0f)
    {
        lerpAimingAngleDiff = goalAngle - currentAngle;

        if (lerpAimingAngleDiff > halfCircleInRadians)
        {
            goalAngle = goalAngle - fullCircleInRadians;
        }
        else if (lerpAimingAngleDiff < -halfCircleInRadians)
        {
            currentAngle = currentAngle - fullCircleInRadians;
        }

        currentAngle = Mathf.Lerp(currentAngle, goalAngle, Time.deltaTime * smoothing);

        aimCursorX = Mathf.Cos(currentAngle);
        aimCursorY = Mathf.Sin(currentAngle);
    }

    private void idleAim()
    {
        if (idleAiming)
        {
            angleDiff = goalAngle - currentAngle;
            lerpAimingAngle(2.5f);
            if (angleDiff <= oneDegreeInRadians && angleDiff >= -oneDegreeInRadians)
            {
                idleAiming = false;
                setIdleAimAngle();
            }
        } // end of if idleAiming
    } // end of idleAim()

    private void setIdleAimAngle()
    {
        currentAngle = goalAngle;

        randomAimingPoint = Random.insideUnitCircle;
        randomDirection = ReturnNormalizedVector(randomAimingPoint);
        goalAngle = Mathf.Atan2(
                    randomDirection.y - transform.position.y,
                    transform.position.x - randomDirection.x);

        currentAngle = makeAnglePositiveAndInt(currentAngle);
        goalAngle = makeAnglePositiveAndInt(goalAngle);

        angleDiff = currentAngle - goalAngle;

        if (angleDiff < oneDegreeInRadians * idleAngleCheckDifference &&
            angleDiff > -oneDegreeInRadians * idleAngleCheckDifference)
        {
            //if (DEBUG_AI) 
            //Debug.Log("AI: new goalAngle too close, no change to aim");
            //Debug.Break();
            //idleAimingWait = null;
            return;
        }

        idleAimingWait = null;
        lerpAimingAngleDiff = 0f;
    }


    IEnumerator WaitBeforeIdleAim() 
    {
        if (idleAiming) 
        {
            //Debug.Log("idleAiming is true!");
            yield return null;
        }

        while (!idleAiming)
        {
            yield return new WaitForSecondsRealtime(3.0f);
            idleAiming = true;
        }
    }

    private float makeAnglePositiveAndInt(float angle)
    {
        if (angle <= 0)
        {
            angle += fullCircleInRadians;
        }

        // converts to degrees
        angle /= oneDegreeInRadians;

        angle = Mathf.RoundToInt(angle);

        // converts back to radians
        angle *= oneDegreeInRadians;
        return angle;
    }

    public void AquireTarget()
    {
        if (combatFollowing)
        {
            return;
        }

        if (!targetAquired)
        {
            FindClosestTargetEnemy();
        }

        if (closestTarget == null)
        {
            targetAquired = false;
            targetInRange = false;
            return;
        }

        float DistBetween = Mathf.Abs(Vector2.Distance(transform.position,
                                            closestTarget.transform.position));
        if (DistBetween > AimDistance)
        {
            targetAquired = false;
            targetInRange = false;
            closestTarget = null;
        }
    }

    public float AimBasedOnAtan2(GameObject Target)
    {
        float angle = Mathf.Atan2(
                    Target.transform.position.y - transform.position.y,
                    transform.position.x - Target.transform.position.x);
        return angle;
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
            Vector2 distDiff = target.transform.position - transform.position;
            float diagonalDistBetween = distDiff.sqrMagnitude;

            if (target.layer == containerLayer)
            {
                diagonalDistBetween += 10.0f;
            }

            if (diagonalDistBetween < distance)
            {
                distance = diagonalDistBetween;
                closestTarget = target;
                if (closestTarget.tag == "Enemy")
                {
                    targetAquired = true;
                }
            }
        } // end of foreach
    } // end of FindClosestTargetEnemy()

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
