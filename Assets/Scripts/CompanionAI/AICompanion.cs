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

    public PlayerController AIController;
    public GameObject BotGO;

    public GameObject PlayerGO;
    private Collider playerCollider;
    public bool testing;

    private Transform TargetPos;

    // Start is called before the first frame update
    public void Awake()
    {
        playerCollider = PlayerGO.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (testing)
        {
            hortDistance = PlayerGO.transform.position.x - BotGO.transform.position.x;
            vertDistance = PlayerGO.transform.position.y - BotGO.transform.position.y;
            hortNow = Mathf.Clamp(hortDistance, -1, 1);
            vertNow = Mathf.Clamp(vertDistance, -1, 1);
            return;
        }
        hortNow = 0.0f;
        vertNow = 0.0f;
        aimCursorX = Mathf.Cos(Time.timeSinceLevelLoad);
        aimCursorY = Mathf.Sin(Time.timeSinceLevelLoad);
    }

    void ShootAtTarget()
    {
        // do nothing
    }

    public void FollowPlayer()
    {
        //transform.position = Vector3.MoveTowards(transform.position, TargetPos.position, AIController.speed);
    }

    public float VertAxisNow()
    {
        //OnTriggerEnter(playerCollider);
        return vertNow;
    }

    public float HortAxisNow()
    {
        //OnTriggerEnter(playerCollider);
        return hortNow;
    }

    public float AimCursorX()
    {
        return aimCursorX;
    }
    public float AimCursorY()
    {
        //OnTriggerEnter(playerCollider);
        return aimCursorY;
    }

    public void CursorAim()
    {

    }

    public void OnTriggerEnter(Collider collider2D)
    {
        Debug.Log("Close to player - don't move");
        vertNow = 0.0f;
        hortNow = 0.0f;
    }

    public int DiceRoll()
    {
        int diceRoll = Random.Range(0, 101); //top of range is exclusive so +1 to desired max
        return diceRoll;
    }
}
