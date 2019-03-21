using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AICompanion : MonoBehaviour
{
    [Header("Debug")]
    public bool DEBUG_AI = true; // if true, spam the debug console
    [Space]
    [Header("Main AI Components")]
    public PlayerController AI;
    public GameObject BotGO;
    [Space]
    [Header("Main Player Components")]
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
    private float StopFollowDist = 1.3f;
    private int step;
    private float stepDistanceRange = 0.2f;
    [Space]
    [Header("Controller Axis Input")]
    public float vertNow;
    public float hortNow;
    [Space]
    [Header("Cursor Position")]
    public float aimCursorX;
    public float aimCursorY;
    [Space]
    [Header("AI States")]
    [SerializeField]
    public bool following;
    public bool meandering;
    private int idleTimerFull = 45;
    public int idleTimer = 45;
    private bool meanderingInputSet;
    public bool inCutScene;
    public bool targetAquired;
    [Space]
    [Header("Level Targets")]
    public GameObject[] TargetGOs;
    private GameObject closestTarget;
    [Space]
    public GameObject meanderDestination;

    private Collider2D[] AIColliders;
    private Collider2D AICollider;

    private float AimDistance = 4.5f;

    private int containerLayer;
    private int itemLayer;
    private ContactFilter2D itemLayerFilter;
    private float itemSeekRadius = 5.0f;
    private float distanceToResource;

    private int mana = 1;

    // Start is called before the first frame update
    public void Awake()
    {
        AIColliders = BotGO.GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D box in AIColliders) 
        { 
            if (box.isTrigger) 
            {
                AICollider = box;
            }
        }

        closestTarget = null;
        containerLayer = LayerMask.NameToLayer("Container");
        itemLayer = LayerMask.NameToLayer("Items");
        // the item layer is number 16
        // but the function below wants a BITMASK
        // so it wants the 16th bit to be a 1
        // which is two to the power of whatever bit we want to set
        // eg 00000000000000001000000000000000 // Mathf.Pow(2f,16f)
        //itemLayerFilter.SetLayerMask(itemLayer); << How I tried to make this work ~~ vv solution
        itemLayerFilter.layerMask.value = 1<<itemLayer; //65536; WORKS!!! // 2^16 (16==items layer)
        itemLayerFilter.useLayerMask = true;
        itemLayerFilter.useTriggers = true;
    }

    // Update is called once per frame
    void Update()
    {
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
        meanderingInputSet = false;
        following = false;
        targetAquired = false;
        ZeroOutInput();
        closestTarget = null;
    }

    public bool FollowPlayerCheck()
    {
        distanceFromPlayer = Vector2.Distance(PlayerGO.transform.position, BotGO.transform.position);
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

    private float shotTimer = 0;

    public bool Shoot() // TODO
    {
        if (closestTarget == null || closestTarget.layer == containerLayer) 
        {
            return false;
        }

        shotTimer += 1;

        if (Mathf.Approximately(Mathf.Sin(shotTimer * Mathf.PI / 180), -1))
        {
            return true;
        }
        return false;
    }

    // so we don't create a new array every frame
    private Collider2D[] itemArray = new Collider2D[10];
    [Space]
    [Header("Resource States")]
    public bool foundHealth;
    public bool foundMana;
    public Collider2D nearestResource;

    public void AIMoveBasedOnState()
    {
        if (inCutScene)
        {
            AIReset();
            return;
        }

        if (Player.isDead)
        {
            meandering = false;
            following = false;
            targetAquired = false;
            AimBasedOnAtan2(PlayerGO);
            getToPlayerToRevive();
            return;
        }

        ResourceManager();

        if (nearestResource != null && (foundHealth || foundMana))
        {
            meandering = false;
            following = false;
            GoTowardNeededResource();
            if (DEBUG_AI)
            {
                if (foundMana && foundHealth)
                {
                    Debug.Log("AI: 'Need mana/health and found both types of potions'");
                }
                else if (foundMana)
                {
                    Debug.Log("AI: 'Need mana and found mana'");
                }
                else if (foundHealth)
                {
                    Debug.Log("AI: 'Need health and found health'");
                }
            }
        }

        if (closestTarget != null && closestTarget.layer != containerLayer) 
        {
            // MORTAL KOMBAT!!!
            meandering = false;
            following = false; 
            combatManeuvers();
            return;
        }

        if (FollowPlayerCheck() || following)
        {
            meandering = false;
            FollowPlayer();
            return;
        }

        if (!meandering)
        {
            SetMeanderingDestination();
        }
        else
        {
            Meander();
        }
    } // end of AIMoveBasedOnState():

    public void getToPlayerToRevive()
    {
        if (DEBUG_AI) Debug.Log("AI going to player to revive");
        if (Mathf.Abs(Vector3.Distance(BotGO.transform.position,
                PlayerGO.transform.position)) > 0.6f)
        {
            hortDistance = PlayerGO.transform.position.x - BotGO.transform.position.x;
            vertDistance = PlayerGO.transform.position.y - BotGO.transform.position.y;
            hortNow = SetAxisInput(hortDistance, 1.0f);
            vertNow = SetAxisInput(vertDistance, 1.0f);
            return;
        }
        ZeroOutInput();
    }

    public void ResourceManager()
    {
        // should we bother looking for potions?
        if ((AI.Health + AI.MaxHealth / 2) < Player.Health)
        {
            if (FindNearestResource("HealthPotion"))
            {
                foundHealth = true;
                return;
            }
        }

        if (AI.EnergyType == mana && (AI.Energy + AI.MaxEnergy / 2) < Player.Energy)
        {
            if (FindNearestResource("ManaPotion"))
            {
                foundMana = true;
                return;
            }
        }
        foundHealth = false;
        foundMana = false;
        // not found because AI doesn't need it
    }

    public bool FindNearestResource(string potionName)
    {
        // search for nearby potions - spritey way
        // idea: what if we just do Vector2.Distance(a,b)
        // on an array of potion sprites?
        // nah that sounds slow and old fashioned

        // search for nearby potions - physicsy way:
        Collider2D item;

        //if (DEBUG_AI) Debug.Log("AI Companion itemLayerFilter.layerMask is: " + itemLayerFilter.layerMask.value);
        int count = Physics2D.OverlapCircle(BotGO.transform.position, itemSeekRadius, itemLayerFilter, itemArray);
        if (count == 0)
        {
            if (DEBUG_AI) Debug.Log("AI: 'no " + potionName + " near me'");
            return false;
        }
        if (DEBUG_AI) Debug.Log(count + " item colliders near " + BotGO.name + " at " + BotGO.transform.position);

        for (int num = 0; num < count; num++)
        {
            item = itemArray[num];
            if (DEBUG_AI) Debug.Log(item); // always null? FIXME
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
                return true;
            }
            else
            {
                if (DEBUG_AI) Debug.Log("no sprite on a collider named " + item.transform.parent.gameObject.name + "! That seems wrong!");
            }

            if (DEBUG_AI) Debug.Log("no items near " + BotGO.name + " at " + BotGO.transform.position);

        }
        if (DEBUG_AI) Debug.Log("AI did not bother looking for potions");
        return false;
    }

    public void combatManeuvers()
    {
        if (Vector2.Distance(BotGO.transform.position, closestTarget.transform.position) 
            < (AimDistance - 0.5))
        {
            hortDistance = BotGO.transform.position.x - closestTarget.transform.position.x;
            vertDistance = BotGO.transform.position.y - closestTarget.transform.position.y;

        }
        if (Vector2.Distance(BotGO.transform.position, closestTarget.transform.position)
            > (AimDistance + 0.5))
        {
            hortDistance = closestTarget.transform.position.x - BotGO.transform.position.x;
            vertDistance = closestTarget.transform.position.y - BotGO.transform.position.y;
        }

        hortNow = SetAxisInput(hortDistance, 0.50f);
        vertNow = SetAxisInput(vertDistance, 0.50f);
    }

    private RaycastHit2D[] hitArray = new RaycastHit2D[10];
    float meanderDistanceCheck = 1f;

    public void SetMeanderingDestination()
    {
        Vector2 randomPointInCircle = Random.insideUnitCircle * 2.5f;
        Vector3 meanderingPoint = new Vector3(PlayerGO.transform.position.x + randomPointInCircle.x,
                                              PlayerGO.transform.position.y + randomPointInCircle.y);
        if (PlayerGO.gameObject.GetComponent<Collider2D>().OverlapPoint(meanderingPoint))
        {
            if (DEBUG_AI) Debug.Log("AI: Meandering Destination inside player, waiting before trying again");
            meanderDestination.transform.position = BotGO.transform.position;
            meandering = true;
            return;
        }

        Vector2 distanceToMeanderPoint = BotGO.transform.position - meanderingPoint;
        if (distanceToMeanderPoint.magnitude < meanderDistanceCheck)
        {
            meanderDestination.transform.position = meanderingPoint;
            if (DEBUG_AI) Debug.Log("AI: Meandering Destination is too close, waiting before trying again");
            meanderDestination.transform.position = BotGO.transform.position;
            meandering = true;
            return;
        }

        Vector3 direction = meanderingPoint - BotGO.transform.position;
        int count = AICollider.Cast(direction, hitArray, direction.magnitude + 0.1f);
        for (int i = 0; i < count; i++)
        {
            if (hitArray[i].collider != null)
            {
                if (DEBUG_AI) Debug.Log(hitArray[i].collider.name);
                if (DEBUG_AI) Debug.Log("AI: Meandering path goes through object, waiting before trying again");
                meanderDestination.transform.position = BotGO.transform.position;
                meandering = true;
                return;
            }
        }
        meanderDestination.transform.position = meanderingPoint;
        meandering = true;
    }

    public void Meander()
    {
        float Distance = 0.0f;
        Vector2 Heading = meanderDestination.transform.position - BotGO.transform.position;
        Distance = Heading.magnitude;
        //if (DEBUG_AI) Debug.Log("AI: I am " + Distance + " away from meander point");
        if (Distance < 0.3f)
        {
            ZeroOutInput();
            idleTimer--;
            if (idleTimer <= 0)
            {
                int randomOffset = DiceRoll();
                idleTimer = idleTimerFull + Mathf.CeilToInt(idleTimerFull * (randomOffset / 100));
                meandering = false;
                meanderingInputSet = false;
                return;
            }
        }

        if (!meanderingInputSet) 
        {
            Vector2 Direction = Heading.normalized;
            hortNow = Direction.x * 0.12f;
            vertNow = Direction.y * 0.12f;
            if (DEBUG_AI) Debug.Log("AI: Meandering input set, moving to target");
            meanderingInputSet = true;
        }
    }

    public void GoTowardNeededResource()
    {
        hortDistance = nearestResource.transform.position.x - BotGO.transform.position.x;
        vertDistance = nearestResource.transform.position.y - BotGO.transform.position.y;
        hortNow = SetAxisInput(hortDistance, 1.0f);
        vertNow = SetAxisInput(vertDistance, 1.0f);
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
            if (Mathf.Abs(Vector3.Distance(BotGO.transform.position, 
                Player.PlayerSteps[step])) < 0.3f)
            {
                Player.PlayerSteps.RemoveAt(step);
                if (Player.PlayerSteps.Count == step)
                {
                    ZeroOutInput();
                    return;
                }
            }

            hortDistance = Player.PlayerSteps[step].x - BotGO.transform.position.x;
            vertDistance = Player.PlayerSteps[step].y - BotGO.transform.position.y;

            hortNow = SetAxisInput(hortDistance, 1.0f);
            vertNow = SetAxisInput(vertDistance, 1.0f);
        }
    }

    public float SetAxisInput(float AxisDistance, float AxisIntensity)
    {
        if (AxisDistance > stepDistanceRange)
        {
            return AxisIntensity;
        }
        if (AxisDistance < -stepDistanceRange)
        {
            return -AxisIntensity;
        }
        // AxisDistance is > -0.2 and < 0.2 so set to 0;
        return 0.0f;
    }

    public void ZeroOutInput() // perhaps lerp/something to a stop?
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
            float DistBetween = Mathf.Abs(Vector2.Distance(BotGO.transform.position,
                                            closestTarget.transform.position));
            if (DistBetween < AimDistance && TargetGOs.Length > 0)
            {
                AimBasedOnAtan2(closestTarget);
            }
            return;
        }
        // TODO idle aiming
    }

    public void AquireTarget()
    {
        if (!targetAquired)
        {
            FindClosestTargetEnemy();
        }

        if (closestTarget == null)
        {
            targetAquired = false;
            return;
        }

        float DistBetween = Mathf.Abs(Vector2.Distance(BotGO.transform.position,
                                            closestTarget.transform.position));
        if (DistBetween > AimDistance)
        {
            targetAquired = false; 
            closestTarget = null;
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
