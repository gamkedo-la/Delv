using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageOnActivate : MonoBehaviour
{

    public GameObject Target;
    public string Message;
    [Space]
    public bool OneShot;
    public bool Activated;

    void Activate()
    {
        if ((OneShot)&&(!Activated))
        {
            Target.SendMessage(Message);
            Activated = true;

        }

        if (!OneShot)
        {
            Target.SendMessage(Message);
        }
    }
}
