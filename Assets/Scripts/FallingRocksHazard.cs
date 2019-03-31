using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRocksHazard : MonoBehaviour
{
	[Range(1, 5)]
	[SerializeField] int minTimeout = 1;
	[Range(5, 20)]
	[SerializeField] int maxTimeout = 10;
	[SerializeField] float radius = 4f;
	[SerializeField] GameObject fallingRockPrefab;

	CameraShake camShake;

	void Start()
	{
		GameObject CamParent = GameObject.Find("CamParent");
		Camera cam = CamParent.transform.GetChild(0).gameObject.GetComponent<Camera>();
		camShake = CamParent.transform.GetChild(0).gameObject.GetComponent<CameraShake>();

		StartCoroutine(spawnRocks());
	}

	IEnumerator spawnRocks()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(minTimeout, maxTimeout));

			Vector3 position = Random.insideUnitCircle * radius;
			GameObject rock = Instantiate(fallingRockPrefab, position, Quaternion.Euler(0f, 0f, 0f));
			rock.transform.SetParent(gameObject.transform);
		}
	}

	public void DoneFalling()
	{
		camShake.Shake(0.5f, 2, 5f);
		// @todo do damage to players
	}

	void OnDrawGizmosSelected()
	{
		UnityEditor.Handles.color = Color.green;
		UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
	}
}
