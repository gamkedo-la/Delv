using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaWormDarkMode : MonoBehaviour
{
	public float alphaRate = 0.04f;
	public float maxAlphaValue = 0.8f;
	public float minAlphaValue = 0.25f;

	public GameObject wormGroup;

	private SpriteRenderer sprRend;

	private MegaWormBrain brain;
	private GameObject[] players;

	static private bool toggleState = false;

	void Start()
    {
		sprRend = GetComponent<SpriteRenderer>();
		sprRend.color = new Color(sprRend.color.r, sprRend.color.g, sprRend.color.b, 0f);
		
		players = GameObject.FindGameObjectsWithTag("Player");
	}
	
    void Update()
    {
		if (wormGroup.activeSelf && toggleState)
		{
			float alpha = sprRend.color.a;
			if (alpha < maxAlphaValue)
			{
				alpha += alphaRate * Time.deltaTime;
				sprRend.color = new Color(sprRend.color.r, sprRend.color.g, sprRend.color.b, alpha);
			}
		}
		else if (wormGroup != null && wormGroup.activeSelf)
		{
			float alpha = sprRend.color.a;
			if (alpha > minAlphaValue)
			{
				alpha -= alphaRate * Time.deltaTime;
				sprRend.color = new Color(sprRend.color.r, sprRend.color.g, sprRend.color.b, alpha);
			}
			else if (alpha < minAlphaValue)
			{
				alpha += alphaRate * Time.deltaTime;
				sprRend.color = new Color(sprRend.color.r, sprRend.color.g, sprRend.color.b, alpha);
			}
		}
		else
		{
			float alpha = sprRend.color.a;
			if (alpha > 0f)
			{
				alpha -= alphaRate * Time.deltaTime;
				sprRend.color = new Color(sprRend.color.r, sprRend.color.g, sprRend.color.b, alpha);
			}
		}
    }

	static public void TurnOn() { toggleState = true; }
	static public void TurnOff() { toggleState = false; }
}
