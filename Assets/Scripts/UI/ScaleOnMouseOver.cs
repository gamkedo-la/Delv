using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public Vector3 newScale = new Vector3(1.2f, 1.2f, 1.2f);

	public bool usingWorldSpaceCanvas = true;

	public AudioClip hoverSound;

	private AudioSource aud = null;

	private Vector3 previousScale = new Vector3(1f, 1f, 1f);

	private bool doScale = false;

	void Start () {
		aud = GetComponent<AudioSource>();
		if(aud == null)
			aud = FindObjectOfType<AudioSource>();

		previousScale = transform.localScale;
	}

	void Update () {
		
		if(doScale)
		{
			//if(transform.localScale.x < newScale.x && transform.localScale.y < newScale.y)
			//	transform.localScale = Vector3.Lerp(transform.localScale, newScale, Time.unscaledDeltaTime * 10f);
		
			transform.localScale = newScale;
		}
		else
		{
			//if(transform.localScale.x > previousScale.x && transform.localScale.y > previousScale.y)
			//	transform.localScale = Vector3.Lerp(transform.localScale, previousScale, Time.unscaledDeltaTime * 10f);
		
			transform.localScale = previousScale;
		}
		
		doScale = usingWorldSpaceCanvas ? false : doScale;
	}

	void OnMouseOver() {
		if(enabled && transform.localScale != newScale && aud != null)
			aud.PlayOneShot(hoverSound);
		doScale = true;
	}

	private void OnMouseExit( )
	{
		doScale = false;
	}

	public void OnPointerEnter(PointerEventData eventData) {
		if(enabled && transform.localScale != newScale && aud != null)
			aud.PlayOneShot(hoverSound);
		doScale = true;
	}

	public void OnPointerExit(PointerEventData eventData)
    {
		doScale = false;
    }

	public void ScaleBackToNormal()
	{
		transform.localScale = previousScale;
	}
}
