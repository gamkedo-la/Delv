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
    // Use this for initialization
    void Start ()
    {
	}

    private void FixedUpdate()
    {

         RightStickHInput = Input.GetAxis("RightStickHInput"+PC.PlayerIndex);
         RightStickVInput = Input.GetAxis("RightStickVInput"+PC.PlayerIndex);
        if (RightStickHInput != 0.0f || RightStickVInput != 0.0f)
        {
            angle = Mathf.Atan2(RightStickVInput, RightStickHInput) * Mathf.Rad2Deg;
        }
        //Var works with any variable I think? So I dont have to specify. This is getting my mouse position and rotating the forward vector to mouse position.
        //var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //var mousePosition = new Vector3(0,0,angle);
        //The autocode thing in VS did most of this after I tried to turn it normalyl
        //This is the declaration of rot and then a lock so I don't make my ship look up and down the wrong axiseseseseseseesssZ.
        transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
        transform.eulerAngles = new Vector3(0, 0, angle);
        //Debug.Log(string.Format("RightStickHInput = {0}", RightStickHInput)); ;
        //Debug.Log(string.Format("RightStickVInput = {0}", RightStickVInput)); ;
    }


        void Update ()
    {
		
	}
}
