using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FloatingTextController : MonoBehaviour {

    private static DamageNumbers popupText;
    private static GameObject canvas;



    public static void Initialize()
    {
        Debug.Log("initializing Floatingtext Controller");
        canvas = GameObject.Find("Canvas");
        if (!popupText)
        {
            popupText = Resources.Load<DamageNumbers>("Prefabs/PopupTextParent");
        }
    }


    public static void CreateFloatingText(string text, Transform location,float DMGAMT)
    {
        DamageNumbers instance = Instantiate(popupText);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector2((location.position.x + Random.Range(-.3f, .3f)),(location.position.y + Random.Range(-.3f, .3f))));

        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPosition;
        if (DMGAMT < 1)
        {
            instance.transform.localScale = new Vector2(.2f, .2f);
        }
        if ((DMGAMT < 10)&& DMGAMT > 1)
        {
            instance.transform.localScale = new Vector2(.5f, .5f);
        }
        if (DMGAMT > 20)
        {
            instance.transform.localScale = new Vector2(1.5f, 1.5f);
        }
        instance.SetText(text);
    }
}
