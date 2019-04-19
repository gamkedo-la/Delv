using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToVillageEvent : MonoBehaviour
{
    [SerializeField] GameObject toEnable;

    void Start()
    {
        toEnable.SetActive(false);
    }

    public void Trigger()
    {
        toEnable.SetActive(true);
    }
}
