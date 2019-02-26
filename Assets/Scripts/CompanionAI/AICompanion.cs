using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AICompanion : MonoBehaviour
{
    private bool DEBUGAI = true; // if true, spam the debug console

    public PlayerController AI;
    public GameObject BotGO;

    public PlayerController Player;
    public GameObject PlayerGO;

    public float vertDistance;
    public float hortDistance;
    public float vertNow;
    public float hortNow;

    public float aimCursorX;
    public float aimCursorY;

    public float distanceBetween;
    private int BeginFollowDist = 3;
    private int step;
    private float stepDistanceRange = 0.2f;

    public GameObject[] TargetGOs;
    private GameObject closestTarget;
    private float AimDistance = 4.5f;

    private int containerLayer;
    private int itemLayer;
    private ContactFilter2D itemLayerFilter;
    private float itemSeekRadius = 10.0f;

    // Start is called before the first frame update
    public void Awake()
    {
        containerLayer = LayerMask.NameToLayer("Container");
        itemLayer = LayerMask.NameToLayer("Items");
        // the item layer is number 16
        // but the function below wants a BITMASK
        // so it wants the 16th bit to be a 1
        // which is two to the power of whatever bit we want to set
        // eg 00000000000000001000000000000000 // Mathf.Pow(2f,16f)
        //itemLayerFilter.SetLayerMask(itemLayer);
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

        TargetGOs = GameObject.FindGameObjectsWithTag("Enemy");

        CursorAim();
        AIMoveBasedOnState();
    }

    public bool FollowPlayerCheck()
    {
        distanceBetween = Vector2.Distance(PlayerGO.transform.position, BotGO.transform.position);
        if (Mathf.Abs(distanceBetween) > BeginFollowDist)
        {
            StartCoroutine(WaitBeforeFollow());
            return true;
        }

        if (Mathf.Abs(distanceBetween) < (BeginFollowDist - 2.5f))
        {
            StopCoroutine(WaitBeforeFollow());
            Player.PlayerSteps.Clear();
            return false;
        }

        return false;
    }

    public bool Shoot() // TODO
    {
        //if (closestTarget == null || closestTarget.layer == containerLayer) 
        //{
        //    return false;
        //}

        if (DiceRoll() < 100)
        {
            return true;
        }
        return false;
    }

    // so we don't create a new array every frame
    private Collider2D[] itemArray = new Collider2D[5]; 

    public bool ResourceManager()
    {
        int mana = 1;
        Collider2D item;
        GameObject itemGO;

        // should we bother looking for potions?
        if (AI.EnergyType == mana && (AI.Energy + AI.MaxEnergy/10) < Player.Energy)
        {
            // search for nearby potions - spritey way
            // idea: what if we just do Vector2.Distance(a,b)
            // on an array of potion sprites?
            // nah that sounds slow and old fashioned

            // search for nearby potions - physicsy way:
            if (DEBUGAI) Debug.Log("AI Companion itemLayerFilter.layerMask is: " + itemLayerFilter.layerMask.value);
            int count = Physics2D.OverlapCircle(BotGO.transform.position, itemSeekRadius, itemLayerFilter, itemArray);
            
            if (DEBUGAI) Debug.Log(count + " item colliders near " + BotGO.name + " at " + BotGO.transform.position);
            if (count==0) {
                return false;
            }

            for (int num=0; num<count; num++)
            {
                item = itemArray[num];
                //if (DEBUGAI) Debug.Log(item); // aways null? FIXME
                if (item == null) 
                {
                    if (DEBUGAI) Debug.Log("Nearby item is null! That seems wrong.");
                    continue;
                }
                //itemGO = item.GetComponent<GameObject>(); // not the collider, the parent sprite
                //itemGO = item.transform.parent.gameObject; // no need to traverse hierarchy
                //SpriteRenderer itemSprite = itemGO.GetComponent<SpriteRenderer>(); // errors out
                SpriteRenderer itemSprite = item.GetComponent<SpriteRenderer>();
                if (itemSprite && (itemSprite.sprite.name == "ManaPotion"))
                {
                    if (DEBUGAI) Debug.Log("Mana Potion near me! Woo hoo!");
                    return true;
                }
                else {
                    if (DEBUGAI) Debug.Log("no sprite on a collider named " + item.transform.parent.gameObject.name + "! That seems wrong!");
                }

                if (DEBUGAI) Debug.Log("no items near " + BotGO.name + " at " + BotGO.transform.position);

            }
        }
        if (DEBUGAI) Debug.Log("AI did not bother looking for potions");
        return false;
    }

    public void AIMoveBasedOnState()
    {
        if (ResourceManager())
        {
            if (DEBUGAI) Debug.Log("I need mana and I see mana");
            return;
        }

        if (FollowPlayerCheck())
        {
            FollowPlayer();
            return;
        }
        // TODO Meander
        hortNow = 0.0f;
        vertNow = 0.0f;
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

            if (target.layer == containerLayer)
            {
                diagonalDistBetween += 10.0f;
            }

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
