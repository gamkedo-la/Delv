using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAssist : MonoBehaviour
{

    public GameManagerScript GM;

    public void OnFadeComplete()
    {
        GM.SendMessage("FadeComplete");

    }


}
