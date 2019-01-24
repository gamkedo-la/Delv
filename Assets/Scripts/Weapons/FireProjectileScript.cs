using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectileScript : MonoBehaviour
{
    // I really should have done all this with inheritance, possibly do that rewrite later.
    //GeneralValues
    public GameObject ProjectilePrefab;
    private float GunCD = 0.0f;
    public float fireDelay = 0.25f;
    public Vector3 bulletOffset = new Vector3(0, 0.3f, 0);
    public float EnergyCost1 = 20;
    public float EnergyCost2 = .1f;
    public Vector2 MoveDir;
    //Directors
    private ControllerAimer CA;
    private GameObject Player;
    private Rigidbody2D Playerrb;
    private PlayerController PC;
    private GameObject GameManagerGO;
    private GameManagerScript GameManager;
    private Camera MainCam;
    //Failsafe bools
    private bool CDzeroed;
    private bool PLDisconnected;
    private bool Grounded;


    // This is for the particles when the weapon fires (if it needs them)
    [SerializeField]
    private GameObject ShotParticle;

    //altfire
    public GameObject FireSpray;
    public ParticleSystem FSPS;

    // Use this for initialization
    void Start()
    {
        GameManagerGO = GameObject.FindGameObjectWithTag("GameManager");
        GameManager = GameManagerGO.GetComponent<GameManagerScript>();
        FSPS = FireSpray.GetComponent<ParticleSystem>();
        MainCam = Camera.main;
        if (this.transform.parent != null)
        {
            CA = this.GetComponentInParent<ControllerAimer>();
            Player = CA.PlayerGO;
            PC = Player.GetComponent<PlayerController>();
            PlayerConnect();
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((GunCD > 0) && CA != null)
        {
            GunCD -= Time.deltaTime;
        }
        if (((GunCD <= 0) && CDzeroed == false) && CA != null)
        {
            ClearCD1();
            CDzeroed = true;
        }
        //Failsafe Disconnect
        if ((this.transform.parent == null) && PLDisconnected == false)
        {
            PlayerDisconnect();
        }
        //Make this float when grounded. TODO

    }


    public void Fire1()
    {
        Debug.Log("Shooting");
        GunCD = fireDelay;
        Vector3 offset = transform.rotation * bulletOffset;
        Instantiate(ProjectilePrefab, transform.position + offset, transform.rotation);
        CameraShake shaker = MainCam.GetComponent<CameraShake>();

        if (GameManagerScript.Screenshake)
        {
            shaker.Shake(.1f, 3, 5);
        }

        if ((ShotParticle != null) && (GameManagerScript.ParticleIntensity > 1))
        {
            Instantiate(ShotParticle, transform.position + offset, transform.rotation);
        }

        StopCD1();
        CDzeroed = false;
    }
    public void CancelWeapon1()
    {
        //Unused in this slot.
        //Debug.Log("Cancel Weapon 1 is not set on" + this.gameObject);
    }

    public void Fire2()
    {
        FireSpray.SetActive(true);
        Playerrb.velocity = Playerrb.velocity + new Vector2((CA.RightStickVInput * .5f), -(CA.RightStickHInput * .5f));
    }

    public void CancelWeapon2()
    {
        FireSpray.SetActive(false);
    }

    public void PlayerConnect()
    {
        if (this.transform.parent != null)
        {
            CA = this.GetComponentInParent<ControllerAimer>();
            Player = CA.PlayerGO;
            PC = Player.GetComponent<PlayerController>();
            ManaPing();
            Playerrb = Player.GetComponent<Rigidbody2D>();
            PLDisconnected = false;
        }

    }
    public void PlayerDisconnect()
    {
        if (CA != null)
        {
            ClearCD1();
            ClearCD2();
            CA = null;
            Player = null;
            PC = null;
            Playerrb = null;
        }

        PLDisconnected = true;

    }


    //Mana costs pings
    void ManaPing()
    {
        PC.EnergyCost1 = EnergyCost1;
        PC.EnergyCost2 = EnergyCost2;
    }

    //Cooldown pings

    void ClearCD1()
    {
        PC.CD1 = true;
    }
    void ClearCD2()
    {
        PC.CD1 = true;
    }
    void StopCD1()
    {
        PC.CD1 = false;
    }
    void StopCD2()
    {
        PC.CD1 = false;
    }



}
