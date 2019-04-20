using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaWormTester : MonoBehaviour
{
    void Start()
    {
		GetComponent<MegaWormBrain>().BrainStart();
    }
	
    void Update()
    {
		GetComponent<MegaWormBrain>().BrainUpdate();
    }
}
