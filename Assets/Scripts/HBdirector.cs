using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HBdirector : MonoBehaviour
{

    public GameObject AttachedPlayer;
    private PlayerController PC;
    private float PlayerIndex;
    public Slider HealthBar;
    public Slider EnergyBar;


    // Use this for initialization
 //   void Awake ()
 //   {
 //       AttachedPlayer = GameObject.FindGameObjectWithTag("Player");
 //       PC = AttachedPlayer.GetComponent<PlayerController>();
 //       PlayerIndex = PC.PlayerIndex;
	//}
 //   private void OnEnable()
 //   {
 //       AttachedPlayer = GameObject.FindGameObjectWithTag("Player");
 //       PC = AttachedPlayer.GetComponent<PlayerController>();
 //       PlayerIndex = PC.PlayerIndex;

 //   }
}
