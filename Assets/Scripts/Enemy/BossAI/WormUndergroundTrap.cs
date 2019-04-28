using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormUndergroundTrap : MonoBehaviour
{
	public GameObject attackPrefab;

	public float maxAlpha = 0.6f;
	public float maxSize = 100f;
	public float alphaRate = 0.04f;
	public float sizeRate = 0.1f;
	public float deathDelay = 10f;
	
	private SpriteRenderer sprRend;
	private BoxCollider2D coll;

    void Start()
    {
		sprRend = GetComponent<SpriteRenderer>();
		coll = GetComponent<BoxCollider2D>();
    }
	
    void Update()
    {
		float alpha = sprRend.color.a;
		if (deathDelay <= 0f)
		{
			Destroy(gameObject);
		}
		else if (deathDelay <= 2f && alpha > 0f)
		{
            
            alpha -= Time.deltaTime / 2f;
			sprRend.color = new Color(sprRend.color.r, sprRend.color.g, sprRend.color.b, alpha);
		}
		else if (alpha < maxAlpha)
		{
            
            alpha += alphaRate * Time.deltaTime;
			sprRend.color = new Color(sprRend.color.r, sprRend.color.g, sprRend.color.b, alpha);
		}

		Vector2 sz = sprRend.size;
		sz.x += sizeRate * Time.deltaTime;
		sprRend.size = sz;
		coll.size = sz;
		
		deathDelay -= Time.deltaTime;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (sprRend.color.a >= maxAlpha && deathDelay > 2f)
		{
			if (collision.gameObject.tag == "Player")
			{
                
                Instantiate(attackPrefab, collision.gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f));
				deathDelay = 2f;
			}
		}
	}
}
