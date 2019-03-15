using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossPhase2 : BossPhase
{
    void Start()
    {
        StartCoroutine(NextPhaseAfterTimeout());
    }

    IEnumerator NextPhaseAfterTimeout()
    {
        yield return new WaitForSeconds(2);

        NextPhase();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * 2);
    }
}
