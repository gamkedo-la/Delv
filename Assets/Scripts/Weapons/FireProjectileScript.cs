using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectileScript : AbstractWeapon
{
    public GameObject ProjectilePrefab;
    private GameObject ProjectileManager;
    private GameObject ParticleManager;
    public Vector2 MoveDir;

    public GameObject FireSpray;
    public ParticleSystem FSPS;

    string fire = "event:/Player/Weapons/fire_wand/firewand_spray";
    FMOD.Studio.EventInstance fireEv;

    bool isSprying = false;

    protected override void Start()
    {
        FSPS = FireSpray.GetComponent<ParticleSystem>();
        MainCam = Camera.main;
        ProjectileManager = GameObject.Find("ProjectileManager");
        if (ProjectileManager == null)
        {
            Debug.Log("ProjectileManager is not present - spawning shots in heirarchy");
        }

        ParticleManager = GameObject.Find("ParticleManager");
        if (ParticleManager == null)
        {
            Debug.Log("ParticleManager is not present - spawning shots in heirarchy");
        }

        base.Start();
    }

    public override void Fire1()
    {
        Debug.Log("Shooting");
        GunCD = fireDelay;
        Vector3 offset = transform.rotation * bulletOffset;
        GameObject TempProjectile = Instantiate(ProjectilePrefab, transform.position + offset, transform.rotation);
        TempProjectile.transform.SetParent(ProjectileManager.transform);

        CameraShake shaker = MainCam.GetComponent<CameraShake>();

        if (GameManagerScript.instance.Screenshake)
        {
            shaker.Shake(.1f, 3, 5);
        }

        if ((ShotParticle != null) && (GameManagerScript.instance.ParticleIntensity > 1))
        {
            GameObject TempParticle = Instantiate(ShotParticle, transform.position + offset, transform.rotation);
            TempParticle.transform.SetParent(ParticleManager.transform);

            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/Weapons/fire_wand/firewand_oneshot", transform.position);
        }

        StopCD1();
        CDzeroed = false;
    }

    public override void Fire2()
    {
        FireSpray.SetActive(true);
        Playerrb.velocity = Playerrb.velocity + new Vector2((CA.RightStickVInput * .5f), -(CA.RightStickHInput * .5f));
        if(!isSprying)
        {
            fireEv = FMODUnity.RuntimeManager.CreateInstance(fire);
            fireEv.start();
            isSprying = true;
        }
    }

    public override void CancelWeapon2()
    {
        isSprying = false;
        fireEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        fireEv.release();
        FireSpray.SetActive(false);
    }
}
