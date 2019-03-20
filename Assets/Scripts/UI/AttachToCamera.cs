using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToCamera : MonoBehaviour
{
	public Vector2 offset = Vector2.zero;

	private GameObject camObject = null;

    void Start()
    {
		
    }
	
    void Update()
    {
		if(camObject == null) camObject = Camera.main.gameObject;

		transform.position = new Vector3(camObject.transform.position.x + offset.x, camObject.transform.position.y + offset.y, transform.position.z);
	}
}
