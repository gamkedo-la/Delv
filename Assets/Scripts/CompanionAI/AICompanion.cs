using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICompanion : MonoBehaviour
{
    public float vertNow;
    public float hortNow;
    public PlayerController AIController;

    // Start is called before the first frame update
    public void Awake()
    {
        AIController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        vertNow = Mathf.Sin(Time.timeSinceLevelLoad);
        hortNow = Mathf.Cos(Time.timeSinceLevelLoad);
    }

    void ShootAtTarget()
    {

    }

    public float VertAxisNow()
    {
        return vertNow;
    }

    public float HortAxisNow()
    {
        return hortNow;
    }

    public int DiceRoll()
    {
        int diceRoll = Random.Range(0, 101); //top of range is exclusive so +1 to desired max
        return diceRoll;
    }
}
