﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerCamTarget : MonoBehaviour
{

    public GameManagerScript GM;
    public Transform P1;
    public Transform P2;


    void OnEnable()
    {
        P1 = GM.Player1GO.transform;
        P2 = GM.Player2GO.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float TargetX = (P1.position.x - P2.position.x) / 2;
        float TargetY = (P1.position.y - P2.position.y) / 2;
        transform.position = new Vector2(TargetX, TargetY);

    }
}
