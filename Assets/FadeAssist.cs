using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAssist : MonoBehaviour
{

    public void OnFadeComplete()
    {
        GameManagerScript.instance.SendMessage("FadeComplete");

    }


}
