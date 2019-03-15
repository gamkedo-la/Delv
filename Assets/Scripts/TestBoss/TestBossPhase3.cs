using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossPhase3 : BossPhase
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
        transform.position = new Vector3(Mathf.Cos(Time.time) * -1, 0, 0);
    }
}
