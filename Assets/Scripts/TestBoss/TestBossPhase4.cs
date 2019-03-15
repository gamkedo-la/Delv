using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossPhase4 : BossPhase
{
    void Start()
    {
        StartCoroutine(FinishBossAfterTimeout());
    }

    IEnumerator FinishBossAfterTimeout()
    {
        yield return new WaitForSeconds(16);

        FinishFinalPhase();
    }

    void Update()
    {
        transform.position = new Vector3(0, Mathf.Cos(Time.time), 0);
    }
}
