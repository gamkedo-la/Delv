using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BossPhase : MonoBehaviour
{
    PhasesManager phasesManager;

    void Awake()
    {
        phasesManager = GetComponentInParent<PhasesManager>();
    }

    protected void NextPhase()
    {
        phasesManager.SendMessage("NextPhase");
    }

    protected void FinishFinalPhase()
    {
        phasesManager.SendMessage("FinishFinalPhase");
    }
}
