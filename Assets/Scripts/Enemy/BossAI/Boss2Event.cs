using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets._2D;
using UnityEngine;

public class Boss2Event : MonoBehaviour
{
	public Transform cutsceneCamTransform;
	public float camSize = 7f;
	public float camBossPlaySizeAddition = 2f;
	private float prevCamSize;

	public GameObject CamParent;
	public Camera2DFollow C2D;

	public GameObject spawnAttack;
	public GameObject hpBar;

	public MegaWormBrain[] brains;
	public GameObject wormHole;
	public Vector3 holeOffset;

	public GameObject deathCutscene;

	private Camera cam;
	private CameraShake camShake;

	private Animator anim;
	private bool releaseCamera = false;
	private bool cutsceneStarted = false; //sync with animator "Cutscene" bool property
	private bool cutsceneDone = false;

	private float deathCutsceneTimer = -10f;

    public WormBossSounds[] wormBossSounds;
    private FMOD.Studio.EventInstance WormBossStartSFX;
    FMOD.Studio.PLAYBACK_STATE WormBossStartSFXPlaybackState;

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

		prevCamSize = cam.orthographicSize + camBossPlaySizeAddition;
        WormBossStartSFX = FMODUnity.RuntimeManager.CreateInstance("event:/Enemies/Worm Boss/WormBossStartCutsceneSFX");
    }

    void Update()
	{
		if (!cutsceneStarted && releaseCamera)
		{
            WormBossStartSFX.getPlaybackState(out WormBossStartSFXPlaybackState);
            if (WormBossStartSFXPlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                WormBossStartSFX.start();
            }

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

		if (deathCutsceneTimer > -1f)
		{
            if (deathCutsceneTimer <= 0f)
            {
                CutsceneEnd();
                
            }
            else
                deathCutsceneTimer -= Time.deltaTime;
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
		anim.SetInteger("AttackPhase", 0);
		
		cutsceneStarted = false;

		C2D.enabled = true;
		releaseCamera = false;
		cutsceneDone = true;

		if (hpBar)
			hpBar.SetActive(true);

		if (anim.GetBool("Dead"))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Enemies/Worm Boss/wormboss_death");
            Destroy(gameObject);
        }
    }

	public void EnableCutsceneSpawn()
	{
		gameObject.transform.GetChild(0).gameObject.SetActive(true);
	}

	public void SpawnAttack()
	{
		spawnAttack.SetActive(true);
	}

	public void SetupDeathCutscene()
	{
        foreach (var wormsounds in wormBossSounds)
        {
            wormsounds.GetComponent<WormBossSounds>().earthquakeSoundEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        cutsceneStarted = false; //now it refers to Death Cutscene
        cutsceneDone = false;

		//Clearing all the worm traps!
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyProjectile");
		for (int i = 0; i < enemies.Length; i++)
			if(enemies[i].name.Contains("WormTrap"))
				Destroy(enemies[i]);

		camSize = prevCamSize;

		releaseCamera = true;
		C2D.enabled = false;

		deathCutscene.SetActive(true);
		deathCutsceneTimer = 3f;
	}
	
	public void WormHoles()
	{
		for(int i = 0; i < brains.Length; i++)
			Instantiate(wormHole, brains[i].gameObject.transform.position + holeOffset, Quaternion.Euler(0f,0f,0f));
	}
}
