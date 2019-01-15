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
        CursorGO = GameObject.Find("MouseCursor");
    }

    private void OnEnable()
    {
        PC = GetComponentInParent<PlayerController>();
        ParentPlayerIndex = PC.PlayerIndex;
        Transform PlayerTF = this.transform.parent;
        PlayerGO = PlayerTF.gameObject;
        CursorGO = GameObject.Find("MouseCursor");

    }

    private void FixedUpdate()
    {
        // Gets mouse position and turns the object to it.
        //var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Quaternion rot = Quaternion.LookRotation(transform.position - mousePosition, Vector3.forward);
        //transform.rotation = rot;
        //transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);

        ///LESS COMPLICATED VERSION// Keeping the above code as it gives sloppy rotation, which I might use for AI.
        // convert mouse position into world coordinates
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get direction you want to point at
        Vector2 direction = (mouseScreenPosition - (Vector2)transform.position).normalized;

        // set vector of transform directly
        transform.up = direction;
    }
}
