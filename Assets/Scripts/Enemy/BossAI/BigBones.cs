using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BigBones : MonoBehaviour
{
    //Boss Stats
    public float MaxHP;
    public float HP;
    public int PhaseCount;
    public bool isShielding;
    public bool isVulnerable;
    public bool isActive;
    public bool isDying;
    [Space]
    //General Directors
    public Animator HeadAni;
    public GameObject HealthBar;
    public GameObject DamagedParticle;
    public EnemyManager Spawner1;
    public EnemyManager Spawner2;
    public EnemyManager Spawner3;
    public EnemyManager Spawner4;
    public SpriteRenderer ShieldVisuals;

    [Space]
    //Part Directors
    public GameObject Head;
    public GameObject Fist1;
    public GameObject Fist2;





    // Start is called before the first frame update
    void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		Vector2 barScale = HealthBar.transform.GetChild(0).localScale;
		barScale.x = (HP / MaxHP) * 5f; //5f is max scale of health bar
		HealthBar.transform.GetChild(0).localScale = barScale;
	}

    void NextPhase()
    {
        StopAllCoroutines();
        UnShield();
        PhaseCount++;
        Phase(PhaseCount);
        Debug.Log("Starting Next Phase:" + PhaseCount);
    }
    void Phase(int phasenumber)
    {
        if (phasenumber == 1)
        {
            Debug.Log("Phase 1 active");
            Fist1.SendMessage("Activate");
            Fist2.SendMessage("Activate");
            Spawner1.SendMessage("Activate");

        }
        if (phasenumber == 2)
        {
            Debug.Log("Phase 2 active");
            StartCoroutine(Phase2());
            Spawner2.SendMessage("Activate");

        }
        if (phasenumber == 3)
        {
            Debug.Log("Phase 3 active");
            StartCoroutine(Phase3());
            Spawner3.SendMessage("Activate");

        }
        if (phasenumber == 4)
        {
            Debug.Log("Phase 4 active");
            StartCoroutine(Phase4());
            Spawner4.SendMessage("Activate");

        }
        if (phasenumber == 5)
        {
            Debug.Log("Phase 5 (dying) is active");
        }

    }

    void DamageHealth(float DMG)
    {
        if (isShielding == false)
        {
            FloatingTextController.CreateFloatingText(DMG.ToString(), transform, DMG);
            Debug.Log(gameObject + " health damaged by " + DMG);
            HP -= DMG;
            Debug.Log(gameObject + " health is now " + HP);
            //Play Hurt Sound Here
            if (DMG > 10)
            {
                if ((GameManagerScript.instance.ParticleIntensity > 0))
                {
                    Instantiate(DamagedParticle, transform.position, transform.rotation);

                }
                
            }
            PhaseCheck();

        }
        if (isShielding == true)
        {
            FloatingTextController.CreateFloatingText("0", transform, 0);
            //Play Ting sound here
        }


    }

    void PhaseCheck()
    {


        if ((PhaseCount == 1) && (HP < 800))
        {
            NextPhase();
        }
        if ((PhaseCount == 2) && (HP < 500))
        {
            NextPhase();
        }
        if ((PhaseCount == 3) && (HP < 200))
        {
            NextPhase();
        }
        if ((PhaseCount == 4) && (HP <= 0))
        {
            NextPhase();
        }

    }

    IEnumerator Phase2()
    {
        Shield();
        yield return new WaitForSeconds(3);
        UnShield();
        yield return new WaitForSeconds(5);
        StartCoroutine(Phase2());
    }
    IEnumerator Phase3()
    {
        Shield();
        yield return new WaitForSeconds(3);
        UnShield();
        yield return new WaitForSeconds(3);
        StartCoroutine(Phase2());
    }
    IEnumerator Phase4()
    {
        Shield();
        yield return new WaitForSeconds(3);
        UnShield();
        yield return new WaitForSeconds(1);
        StartCoroutine(Phase2());
    }

    void Shield()
    {
        isShielding = true;
        ShieldVisuals.enabled = true;

    }
    void UnShield()
    {
        isShielding = false;
        ShieldVisuals.enabled = false;


    }

    void Activate()
    {
        NextPhase();
    }
}
