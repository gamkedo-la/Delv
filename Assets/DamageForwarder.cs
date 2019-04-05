using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageForwarder : MonoBehaviour
{
    public GameObject FwdTarget;

    void DamageHealth(float DMG)
    {
        FwdTarget.SendMessage("DamageHealth", DMG);
    }

}
