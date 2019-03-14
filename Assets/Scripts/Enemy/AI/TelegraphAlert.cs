using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelegraphAlert : MonoBehaviour
{
    // Start is called before the first frame update
    public MonoBehaviour TargetBehavior = null;
    Animator m_Animator;
    AnimatorStateInfo animStateInfo;
    void Start()
    {       
       m_Animator = GetComponent<Animator>();
       
    }
    void OnEnable()
    {
    	TargetBehavior.enabled = false; 
    }

    // Update is called once per frame
    void Update()
    {
    	animStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        if(!animStateInfo.IsName("Telegraph-Alert")){
        	Debug.Log("has exited alert telegraph");
        	TargetBehavior.enabled = true; 
        	enabled = false;
        } else {
        	Debug.Log("Still in telegraph state and animation: " +Animator.StringToHash("Base.Telegraph-Alert") + " / "+ Animator.StringToHash("Telegraph-Alert") + " / "+ animStateInfo.nameHash);
        }
    }
}
