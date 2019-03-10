using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisableWhenScene : MonoBehaviour
{
	public string[] sceneNames;
	public GameObject[] otherObjects;
	
    void Start()
    {
        
    }

    void Update()
    {
		for(int i = 0; i < sceneNames.Length; i++)
		{
			if(SceneManager.GetActiveScene().name == sceneNames[i])
			{
				for(int o = 0; o < otherObjects.Length; o++)
				{
					otherObjects[o].SetActive(false);
				}
				gameObject.SetActive(false);
			}
		}
    }
}
