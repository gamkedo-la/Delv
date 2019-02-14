using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerAimer : MonoBehaviour {



    public float RightStickHInput;
    public float RightStickVInput;
    private float angle;
    public PlayerController PC;
    public float ParentPlayerIndex;
    public GameObject PlayerGO;

    private void Awake()
    {
        PC = GetComponentInParent<PlayerController>();
        ParentPlayerIndex = PC.PlayerIndex;
        Transform PlayerTF = this.transform.parent;
        PlayerGO = PlayerTF.gameObject;
    }
    private void OnEnable()
    {
        PC = GetComponentInParent<PlayerController>();
        ParentPlayerIndex = PC.PlayerIndex;
        Transform PlayerTF = this.transform.parent;
        PlayerGO = PlayerTF.gameObject;
    }
    // Use this for initialization
    void Start ()
    {
	}

    private void FixedUpdate()
    {
        if (PC.isBot)
        {
            RightStickVInput = PC.AICompanion.AimCursorX();
            RightStickHInput = PC.AICompanion.AimCursorY();
        } 
        else
        {
            RightStickHInput = Input.GetAxis("RightStickHInput" + PC.ControllerSlot);
            RightStickVInput = Input.GetAxis("RightStickVInput" + PC.ControllerSlot);
        }

        if (RightStickHInput != 0.0f || RightStickVInput != 0.0f)
        {
            angle = Mathf.Atan2(RightStickVInput, RightStickHInput) * Mathf.Rad2Deg;
        }
        transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

}
