using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectileScript : AbstractWeapon
{
    public GameObject ProjectilePrefab;
    public Vector2 MoveDir;

    public GameObject FireSpray;
    public ParticleSystem FSPS;

    protected override void Start()
    {
        FSPS = FireSpray.GetComponent<ParticleSystem>();
        MainCam = Camera.main;

        base.Start();
    }

    public override void Fire1()
    {
        Debug.Log("Shooting");
        GunCD = fireDelay;
        Vector3 offset = transform.rotation * bulletOffset;
        Instantiate(ProjectilePrefab, transform.position + offset, transform.rotation);
        CameraShake shaker = MainCam.GetComponent<CameraShake>();

        if (GameManagerScript.instance.Screenshake)
        {
            shaker.Shake(.1f, 3, 5);
        }

        if ((ShotParticle != null) && (GameManagerScript.instance.ParticleIntensity > 1))
        {
            Instantiate(ShotParticle, transform.position + offset, transform.rotation);
        }

        StopCD1();
        CDzeroed = false;
    }

    public override void Fire2()
    {
        FireSpray.SetActive(true);
        Playerrb.velocity = Playerrb.velocity + new Vector2((CA.RightStickVInput * .5f), -(CA.RightStickHInput * .5f));
    }

    public override void CancelWeapon2()
    {
        FireSpray.SetActive(false);
    }
}
