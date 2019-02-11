using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICompanion : MonoBehaviour
{
    private PlayerController AIController;

    // Start is called before the first frame update
    void Start()
    {
        AIController = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (AIController.isBot)
        {
            Debug.Log("I am a bot beep boop");
        }
    }
}
