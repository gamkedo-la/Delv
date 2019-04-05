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
    public GameObject BlueFlipper;
    public GameObject GreenFlipper;
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
        GameObject.Destroy(BlueFlipper);
        Component.Destroy(BlueTalk);
    }
    void FlipGreen()
    {
        GreenFlipped = true;
        GameObject.Destroy(GreenFlipper);
        Component.Destroy(GreenTalk);

    }

    void GateBlue()
    {
        if (BlueFlipped)
        {
            GameObject.Destroy(ObeliskBlue);
            BlueGate = true;
            if (GreenGate)
            {
                Complete();
            }

        }

    }
    void GateGreen()
    {
        if (GreenFlipped)
        {
            GameObject.Destroy(ObeliskGreen);
            GreenGate = true;
            if (BlueGate)
            {
                Complete();
            }

        }
    }

    void Complete()
    {
        QuestComplete = true;
        //move the gate.
        Destroy(Gate);
    }

}
