using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour {



    public PlayerController PC;
    public float ParentPlayerIndex;
    public GameObject PlayerGO;
    public GameObject CursorGO;


    // Use this for initialization
    void Start()
    {
        PC = GetComponentInParent<PlayerController>();
        ParentPlayerIndex = PC.PlayerIndex;
        Transform PlayerTF = this.transform.parent;
        PlayerGO = PlayerTF.gameObject;
        CursorGO = GameObject.Find("Cursor");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        // Gets mouse position and turns the object to it.
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);
        transform.rotation = rot;
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
    }
}
