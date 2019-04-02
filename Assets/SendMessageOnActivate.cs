using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageOnActivate : MonoBehaviour
{

    public GameObject Target;
    public string Message;
    [Space]
    public bool Activated;

    void Activate()
    {
        if (!Activated)
        {
            Target.SendMessage(Message);
            Activated = true;

        }
    }
}
