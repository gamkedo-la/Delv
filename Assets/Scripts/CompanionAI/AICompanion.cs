using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICompanion : MonoBehaviour
{
    public float vertNow;
    public float hortNow;

    // Start is called before the first frame update
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {
        vertNow = Mathf.Sin(Time.timeSinceLevelLoad);
        hortNow = Mathf.Cos(Time.timeSinceLevelLoad);
    }

    public float VertAxisNow()
    {
        return vertNow;
    }

    public float HortAxisNow()
    {
        return hortNow;
    }
}
