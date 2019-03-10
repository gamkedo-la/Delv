using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResumeButton : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
    {
		transform.parent.parent.gameObject.GetComponent<PauseMenu>().Resume();
    }
}
