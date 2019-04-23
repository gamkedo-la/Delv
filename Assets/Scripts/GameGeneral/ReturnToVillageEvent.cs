using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToVillageEvent : MonoBehaviour
{
    [SerializeField] GameObject toSpawn;
    [SerializeField] Transform where;


    public void Trigger()
    {
        Instantiate(toSpawn, where.position, where.rotation);
    }
}
