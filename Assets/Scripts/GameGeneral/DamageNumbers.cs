using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DamageNumbers : MonoBehaviour
{

    public Animator Anima;
    private Text damageText;

	// Use this for initialization
	void OnEnable ()
    {
        AnimatorClipInfo[] clipInfo = Anima.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject, clipInfo[0].clip.length);
        damageText = Anima.GetComponent<Text>();


	}

    public void SetText (string text)
    {
        Anima.GetComponent<Text>().text = text;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
