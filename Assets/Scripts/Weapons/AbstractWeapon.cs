using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AbstractWeapon : MonoBehaviour
{
    protected float GunCD = 0.0f;
    public float fireDelay = 0.25f;
    public Vector3 bulletOffset = new Vector3(0, 0.3f, 0);
    public float EnergyCost1 = 10;
    public float EnergyCost2 = .1f;
    public float CastRange = 5;
    public float ShockDamage = 15;

    private Collider2D pickupCollider;

    //Directors
    protected ControllerAimer CA;
    protected GameObject Player;
    protected Rigidbody2D Playerrb;
    protected PlayerController PC;
    protected Camera MainCam;

    //Failsafe bools
    protected bool CDzeroed;
    protected bool PLDisconnected;
    protected bool Grounded;

    // This is for the particles when the weapon fires (if it needs them)
    [SerializeField]
    protected GameObject ShotParticle;

    [SerializeField]
    protected ParticleSystem ShotParticleSystem;

    protected virtual void Start()
    {
        if (this.transform.parent != null)
        {
            CA = this.GetComponentInParent<ControllerAimer>();
            Player = CA.PlayerGO;
            PC = Player.GetComponent<PlayerController>();
            pickupCollider = GetComponent<Collider2D>();
            PlayerConnect();
        }
    }

    public void PlayerConnect()
    {
        if (this.transform.parent != null)
        {
            CA = this.GetComponentInParent<ControllerAimer>();
            Player = CA.PlayerGO;
            PC = Player.GetComponent<PlayerController>();
            Playerrb = Player.GetComponent<Rigidbody2D>();
            ManaPing();

            if (pickupCollider)
            {
                pickupCollider.enabled = false;
            }

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

            if (pickupCollider)
            {
                pickupCollider.enabled = true;
            }
        }

        PLDisconnected = true;
    }

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

    public virtual void Fire1() { }

    public virtual void CancelWeapon1() { }

    public virtual void Fire2() { }

    public virtual void CancelWeapon2() { }

    //Mana costs pings
    protected void ManaPing()
    {
        PC.EnergyCost1 = EnergyCost1;
        PC.EnergyCost2 = EnergyCost2;
    }

    //Cooldown pings
    protected void ClearCD1()
    {
        PC.CD1 = true;
    }
    protected void ClearCD2()
    {
        PC.CD1 = true;
    }
    protected void StopCD1()
    {
        PC.CD1 = false;
    }
    protected void StopCD2()
    {
        PC.CD1 = false;
    }
}
