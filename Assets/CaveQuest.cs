using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveQuest : MonoBehaviour
{
    public GameObject Gate;
    public GameObject ObeliskBlue;
    public NPCTalkTrigger BlueTalk;
    public GameObject ObeliskGreen;
    public NPCTalkTrigger GreenTalk;
    [Space]
    public bool BlueFlipped;
    public bool GreenFlipped;
    [Space]
    public bool BlueGate;
    public bool GreenGate;
    [Space]
    private bool QuestComplete;

    void FlipBlue()
    {
        BlueFlipped = true;
    }
    void FlipGreen()
    {
        GreenFlipped = true;
    }

    void GateBlue()
    {
        BlueGate = true;
        if (GreenGate)
        {
            Complete();
        }

    }
    void GateGreen()
    {
        GreenGate = true;
        if (BlueGate)
        {
            Complete();
        }
    }

    void Complete()
    {
        QuestComplete = true;
        //move the gate.
    }

}
