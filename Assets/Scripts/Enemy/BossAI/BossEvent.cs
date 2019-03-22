using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets._2D;
using UnityEngine;

public class BossEvent : MonoBehaviour
{
	public Transform cutsceneCamTransform;
	public float camSize = 7f;
	private float prevCamSize;
	
	public GameObject CamParent;
	public Camera2DFollow C2D;
	private Camera cam;
	private CameraShake camShake;

	private Animator anim;
	private bool releaseCamera = false;
	private bool cutsceneStarted = false; //sync with animator "Cutscene" bool property
	private bool cutsceneDone = false;

	public Fist[] fists;
	public Animator[] fistAnim;
	public GameObject spawnAttack;
	public GameObject hpBar;

	[HideInInspector] public bool stateChanged = false;

	void Start()
    {
		CamParent = GameObject.Find("CamParent");
		if (CamParent)
		{
			C2D = CamParent.GetComponent<Camera2DFollow>();
		}

		cam = CamParent.transform.GetChild(0).gameObject.GetComponent<Camera>();
		camShake = CamParent.transform.GetChild(0).gameObject.GetComponent<CameraShake>();
		anim = GetComponent<Animator>();

		prevCamSize = cam.orthographicSize;
	}
	
    void Update()
    {
		if (!cutsceneStarted && releaseCamera)
		{
			CamParent.transform.position = Vector3.MoveTowards(CamParent.transform.position, cutsceneCamTransform.position, 15f * Time.deltaTime);
			cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, camSize, 5f * Time.deltaTime);

			if (CamParent.transform.position == cutsceneCamTransform.position)
			{
				anim.SetBool("Cutscene", true);
				cutsceneStarted = true;
			}
		}
		else if(cutsceneDone)
		{
			cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, prevCamSize, 2.5f * Time.deltaTime);
		}
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!cutsceneDone && collision.gameObject.tag == "Player")
		{
			releaseCamera = true;
			C2D.enabled = false;
		}
	}

	public void CameraShake()
	{
		camShake.Shake( 1f, 2, 5f );
	}

	public void CutsceneEnd()
	{
		anim.SetBool("Cutscene", false);
		anim.SetBool("Player1", true);

		cutsceneStarted = false;

		C2D.enabled = true;
		releaseCamera = false;
		cutsceneDone = true;

		if(hpBar)
			hpBar.SetActive(true);

		if (anim.GetBool("Dead"))
			Destroy(gameObject);
	}

	public void EnableCutsceneSpawn()
	{
		gameObject.transform.GetChild(0).gameObject.SetActive(true);
	}

	public void EnableFists( int playerIndex = -1 )
	{
		fistAnim[0].enabled = true;
		fistAnim[1].enabled = true;
		fists[0].Active = true;
		fists[1].Active = true;

		fists[0].enabled = true;
		fists[1].enabled = true;

		fists[0].Activate(playerIndex);
		fists[1].Activate(playerIndex);
	}

	public void DisableFists()
	{
		fists[0].Active = false;
		fists[1].Active = false;
	}

	public void SpawnAttack()
	{
		spawnAttack.SetActive(true);
	}

	public void SetupDeathCutscene()
	{
		cutsceneStarted = false; //now it refers to Death Cutscene
		cutsceneDone = false;

		//Clearing all the zombies!
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < enemies.Length; i++)
			if(enemies[i].name.Contains("Skeleton"))
				Destroy(enemies[i]);

		camSize = prevCamSize;

		releaseCamera = true;
		C2D.enabled = false;
	}

	public void EndVulnerableStateOnStateChange()
	{
		if (stateChanged)
			anim.SetBool("VulnerableSwitch", false);
		stateChanged = false;
	}

	public void EndVulnerableState()
	{
		anim.SetBool("VulnerableSwitch", false);
	}
}
