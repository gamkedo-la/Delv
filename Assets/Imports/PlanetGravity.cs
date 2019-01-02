using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{


    //Strength of attraction from the Planet
    public float basePull = 100f;

    GameObject PlayerShip;
    public Rigidbody2D Playerrb;

    //Initialise code:
    void Start()
    {
        //Playerrb = GameObject.Find("PlayerShip").GetComponent<PlayerMovement>().Playerrb;
        PlayerShip = GameObject.FindGameObjectWithTag("Player");
        Playerrb = GameObject.Find("PlayerShip").GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {
        float StrengthOfAttraction = basePull * transform.localScale.x * 0.5f;

        if (PlayerShip)
        {
            var distance = Vector3.Distance(transform.position, PlayerShip.transform.position);
            if (distance < transform.localScale.x * 0.5 + 50)
            {
                //Kyle did half of this.
                //magsqr will be the offset squared between the object and the planet
                float magsqr;
                //offset is distance
                Vector3 offset;
                //get offset between each planet and the player
                offset = transform.position - PlayerShip.transform.position;
                //So shit doesnt move in the Z
                offset.z = 0;
                //Offset Squared: I understood this but Kyle did it.
                magsqr = offset.sqrMagnitude;
                //Check distance is more than 0 to prevent division by 0, THIS TOO
                if (magsqr > 0f)
                {
                    //This too.
                    Playerrb.AddForce((StrengthOfAttraction * offset.normalized / magsqr) * Playerrb.mass);
                }
            }

        }


    }
}
