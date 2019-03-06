using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BigBones : MonoBehaviour
{
    //Boss Stats
    public float MaxHP;
    public float HP;
    public int CurrentPhase;
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
        
    }

    void NextPhase()
    {

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
            if ((DMG > 10) && (GameManagerScript.instance.ParticleIntensity > 0))
            {
                Instantiate(DamagedParticle, transform.position, transform.rotation);
                
            }

        }
        if (isShielding == true)
        {
            FloatingTextController.CreateFloatingText("0", transform, 0);
            //Play Ting sound here
        }


    }

    void Activate()
    {

    }
}
