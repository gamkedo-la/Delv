using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    /// <summary>
    ///  This is the all emcompassing player script, basically combining the player indexing, the health/mana/stamina/will
    ///  script, and the player movement script all in one.
    ///
    ///
    ///
    ///
    /// This entire script is held together with duct tape. I am so sorry.
    /// </summary>
    ///

    // These vars are related to figuring out who it's coming from. Directors really.
    public float PlayerIndex = 1;
    public GameObject EquippedWeapon;
    public Transform PotentialWeapon;
    private SpriteRenderer SR;
    private Rigidbody2D _rb;
    public GameObject DeadWeapon;
    public GameObject GameManagerGO;
    public GameManagerScript GameManager;
    public TimeManager TimeManager;
    public Camera MainCam;
    public GameObject healthdirectorGO;
    public HBdirector healthdirector;
    public Slider HealthBar;
    public Slider EnergyBar;
    //Dialogue director
    public GameObject DialogueTarget;
    public float DialogueCD;
    [Space]


    //Controller directors
    public Transform Aimer;
    private Aimer MouseAimer;
    private ControllerAimer ConAimer;
    public GameObject ConCursorGO;
    public int ControllerType = 0; //0 is keyboard (reserved for player 1), 1 is Xinput (Xbox 360 or Xbox 1), and 2 will be DualShock4 when it's remade.
    public int ControllerSlot = 0;
    public bool isBot = false;

    public CircleCollider2D reviveCollider;

    [Space]


    // Player Stats
    public float MaxHealth = 100;
    public float Health = 100;
    private float Armor = 0;
    private float Shields = 0;
    public float MaxEnergy = 100;
    public float Energy = 100;
    public int EnergyType = 1; //1 = Mana, 2 = Stamina, 3 = Will
    public float speed = 25;
    public float EnergyRegenRate;
    [Space]


    //Player Specific Cooldowns
    [SerializeField]
    private float PickupCD;
    [Space]


    // iframes
    public float iFrames = 0f;
    public float iAmount = 0.5f;
    int initLayer;
    [Space]


    //EmittedParticles (Damaged
    public GameObject DeathParticle;
    public GameObject DamagedParticle;
    public GameObject LightlyDamaged;
    [Space]



    // These are for mana/stamina/will requirements for a skill, these are sent up when the player picks a weapon up.
    public float EnergyCost1;
    public float EnergyCost2;
    [Space]
    // This gets the cooldown from the weapon
    public bool CD1;
    public bool CD2;
    [Space]


    //These are for aim orientation
    public float LHinput;
    public float LVinput;
    public float RHinput;
    public float RVinput;
    [Space]
    //Movement Indexing strings
    public string LstickH;
    public string LstickV;
    public string RstickH;
    public string RstickV;
    [Space]
    //Startpoint
    public GameObject SP;
    [Space]
    //Cursor for mouse.
    public GameObject CursorGO;
    [Space]
    public bool isDead = false;


    private void OnEnable()
    {
        GoToStart();
        EquippedWeapon.SendMessage("PlayerConnect");
        Health = MaxHealth;
        HealthBar.value = CalculateHealth();
        EnergyBar.value = CalculateEnergy();
        //CursorGO = GameObject.Find("MouseCursor");
        //ConCursorGO = Aimer.GetChild(0).gameObject;
        MouseAimer = Aimer.GetComponent<Aimer>();
        ConAimer = Aimer.GetComponent<ControllerAimer>();

        if (isDead) {
            if (ControllerType == 0) {
                MouseAimer.enabled = true;
            }
            else if (ControllerType == 1) {
                ConAimer.enabled = true;
            }

            reviveCollider.enabled = false;

            isDead = false;
        }
    }

    void OnDisable()
    {
        if (isDead) {
            MouseAimer.enabled = false;
            ConAimer.enabled = false;
        }
    }

    void Awake()
    {
        initLayer = gameObject.layer;
        Aimer = transform.GetChild(0);
        _rb = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        SP = GameObject.FindGameObjectWithTag("StartZone");
        GameManagerGO = GameObject.FindGameObjectWithTag("GameManager");
        GameManager = GameManagerGO.GetComponent<GameManagerScript>();
        TimeManager = GameManager.GetComponent<TimeManager>();
        MainCam = Camera.main;
        HealthBar = healthdirector.HealthBar;
        EnergyBar = healthdirector.EnergyBar;

    }

    // Use this for initialization
    //void Enable ()
    //{
    //    EquippedWeapon.SendMessage("PlayerConnect");
    //    Health = MaxHealth;
    //    HealthBar.value = CalculateHealth();
    //    EnergyBar.value = CalculateEnergy();
    //    CursorGO = GameObject.Find("MouseCursor");
    //    ConCursorGO = Aimer.GetChild(0).gameObject;
    //    MouseAimer = Aimer.GetComponent<Aimer>();
    //    ConAimer = Aimer.GetComponent<ControllerAimer>();


    //    // DONT FORGET WHEN A PLAYER DIES TO LEAVE A TODO SPOT FOR REVIVAL
    //}




    private void Update()
    {


        /// Failsafe for start points
        if (!SP)
        {
            SP = GameObject.FindGameObjectWithTag("StartZone");
        }
        //ENERGY REGEN AREA - Flesh this out later for multiplyers if we get far enough.

        if (Energy < MaxEnergy)
        {
            Energy += EnergyRegenRate * Time.deltaTime;
        }





        /// INPUT FOR SHOOTING
        if (Input.GetButton("Primary" + ControllerSlot))
        {
            FireWeapon1();
        }
        if (Input.GetButton("Secondary" + ControllerSlot))
        {
            FireWeapon2();
        }
        if ((Input.GetButtonUp("Primary" + ControllerSlot)) || (Energy < EnergyCost1))
        {
            CancelWeapon1();
        }
        if ((Input.GetButtonUp("Secondary" + ControllerSlot)) || (Energy < EnergyCost2))
        {
            CancelWeapon2();
        }

    }


    void FixedUpdate ()
    {

        ///Energy Update
        EnergyBar.value = CalculateEnergy();

        /// Equipment Failsafes

        if (EquippedWeapon != null)
        {
            if (!(EquippedWeapon.transform.IsChildOf(this.transform)))
            {
                EquippedWeapon.transform.parent = this.transform;
            }
        }

        if (PickupCD > 0) //Cooldown for Pickup Inputs
        {
            PickupCD -= Time.deltaTime;
        }
        if (DialogueCD > 0) //Cooldown for Dialogue Inputs
        {
            DialogueCD -= Time.deltaTime;
        }

        /// INVICIBILITY FRAMES AREA
        if (iFrames >= -1)
        {
            iFrames -= Time.deltaTime;
        }
        if (iFrames <= 0)
        {
            gameObject.layer = initLayer;
        }

        /// MOVEMENT CONSTRUCTION AREA
        /// If using a controller////////////////////////////////

        if (ControllerType == 1)
        {
            ConAimer.enabled = true;
            ConCursorGO.SetActive(true);
            LstickH = "LeftStickHInput" + ControllerSlot;
            LstickV = "LeftStickVInput" + ControllerSlot;
            RstickH = "RightStickHInput" + ControllerSlot;
            RstickV = "RightStickVInput" + ControllerSlot;

        }
        if ((ControllerType == 1) && PlayerIndex == 1)
        {
            ConAimer.enabled = true;
            MouseAimer.enabled = false;
            CursorGO.SetActive(false);
            ConCursorGO.SetActive(true);
            LstickH = "LeftStickHInput" + ControllerSlot;
            LstickV = "LeftStickVInput" + ControllerSlot;
            RstickH = "RightStickHInput" + ControllerSlot;
            RstickV = "RightStickVInput" + ControllerSlot;

        }
        //If using the keyboard (1st player only)
        if ((ControllerType == 0) && PlayerIndex == 1)
        {
            ConAimer.enabled = false;
            MouseAimer.enabled = true;
            CursorGO.SetActive(true);
            ConCursorGO.SetActive(false);
            LstickH = ("Horizontal");
            LstickV = ("Vertical");
        }


        ///REMINDER///
        ///TEMPORARY///
        ///MAKE AN OPTIONS MENU TO SWAP BETWEEN CONTROLLER TYPE, MAKING IT AUTOMATIC WOULD ADD IT TO EVERY PLAYERCONTROLLER
        if (Input.GetKeyDown(KeyCode.C))
        {
            if ((ControllerType == 0) && PlayerIndex == 1)
            {
                Debug.Log("Swapped to controller type (Xbox one) for player " + PlayerIndex);
                ControllerType = 1;
                ControllerSlot = 1;
                return;
            }
            if ((ControllerType == 1) && PlayerIndex == 1)
            {
                Debug.Log("Swapped to controller type (Keyboard) for player " + PlayerIndex);
                ControllerType = 0;
                ControllerSlot = 0;
                return;
            }
        }

        //DEBUGGING TOOLS FOR PLAYER INDEX AND CONTROLLER SLOTS FOR PLAYER 1
        if ((Input.GetKeyDown(KeyCode.Keypad0)) && PlayerIndex == 1)
        {
            ControllerSlot = 0;
        }
        if ((Input.GetKeyDown(KeyCode.Keypad1)) && PlayerIndex == 1)
        {
            ControllerSlot = 1;
        }

        float Vinput;
        /// Crude Bot Test
        if (isBot)
        {
            Vinput = 1.0f;
        }
        else
        {
            Vinput = Input.GetAxis(LstickV);
        }

        /// MOVEMENT ITSELF


        _rb.AddForce(gameObject.transform.up * speed * Vinput);

        var Hinput = Input.GetAxis(LstickH);
        _rb.AddForce(gameObject.transform.right * (speed) * Hinput);


        /// SPRITE SIDE FLIPPER (PLAN TO POSSIBLY DEPRECATE FOR A STATE MACHINE AND ANIMATIONS)

        if (Hinput > 0)
        {
            SR.flipX = false;
        }
        if (Hinput < 0)
        {
            SR.flipX = true;
        }










    }


    /// PICKUP COLLIDER LOGIC
    void OnTriggerStay2D(Collider2D coll)
    {
        if ((coll.gameObject.tag == "MageWeapon") && (EnergyType == 1)) //REMEMBER TO SPREAD THIS ONCE FIXED
        {
            PotentialWeapon = coll.transform;
            if ((Input.GetButtonDown("Pickup"+ ControllerSlot)) && PickupCD <=0) //REMEMBER TO CHANGE THIS BUTTON
            {
            PickupWeapon();
            }
        }
        if ((coll.gameObject.tag == "RangedWeapon") && (EnergyType == 2))
        {
            PotentialWeapon = coll.transform;
        }
        if ((coll.gameObject.tag == "MeleeWeapon") && (EnergyType == 3))
        {
            PotentialWeapon = coll.transform;
        }
        if ((coll.gameObject.tag == "DialogueBox"))
        {
            DialogueTarget = coll.gameObject;
            if (Input.GetButtonDown("Pickup" + ControllerSlot)) //REMEMBER TO CHANGE THIS BUTTON
            {

                ActivateDialogue();
            }
        }

        if (coll.gameObject.tag == "Player")
        {
            PlayerController other = coll.gameObject.GetComponent<PlayerController>();
            if (other && other.isDead)
            {
                Debug.Log(gameObject.name + " is touching other dead player: " + coll.gameObject.name);
            }
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if ((coll.gameObject.tag == "MageWeapon") && (EnergyType == 1))
        {
            PotentialWeapon = null;
        }
        if ((coll.gameObject.tag == "RangedWeapon") && (EnergyType == 2))
        {
            PotentialWeapon = null;
        }
        if ((coll.gameObject.tag == "MeleeWeapon") && (EnergyType == 3))
        {
            PotentialWeapon = null;
        }
    }


    //This is the method to pickup weapons. It looks like goo, because it is.
    void PickupWeapon()
    {
        PickupCD = 1;
        EquippedWeapon.transform.position = PotentialWeapon.transform.position;
        PotentialWeapon.parent = Aimer.transform;
        PotentialWeapon.localPosition = new Vector3 (0, 0, 0);
        EquippedWeapon.transform.parent = null;
        Collider2D EWC = EquippedWeapon.GetComponent<Collider2D>();
        EWC.enabled = true;
        Collider2D PWC = PotentialWeapon.GetComponent<Collider2D>();
        PWC.enabled = false;
        EquippedWeapon = null;
        EquippedWeapon = PotentialWeapon.gameObject;
        EquippedWeapon.SendMessage("PlayerConnect");
        EquippedWeapon.transform.rotation = new Quaternion(0, 0, 0, 0);
        PotentialWeapon = null;
    }


    //This is the weapon firing mechanism. Just sends a fire message to the weapon if mana allows.
    void FireWeapon1()
    {
        if ((Energy >= EnergyCost1) && (CD1 == true))
        {
            Energy -= EnergyCost1;
            EquippedWeapon.SendMessage("Fire1");
        }
    }
    void CancelWeapon1()
    {
        EquippedWeapon.SendMessage("CancelWeapon1");
    }
    void FireWeapon2()
    {
        if ((Energy >= EnergyCost2) && (CD1 == true))
        {
            Energy -= EnergyCost2;
            EquippedWeapon.SendMessage("Fire2");

        }
        if ((Energy < EnergyCost2) || (CD1 == false))
        {
            EquippedWeapon.SendMessage("CancelWeapon2");

        }
    }
    void CancelWeapon2()
    {
        EquippedWeapon.SendMessage("CancelWeapon2");
    }


    void GoToStart()
    {
        SP = GameObject.FindGameObjectWithTag("StartZone");
        if (!(SP == null))
        {
        transform.position = SP.transform.position;
        Debug.Log("Player Received GoToStart command");
        }
    }




    //Will handle damage through sendmessage even though its a bit slow resource wise. Small game should be able to handle it.
    //Will also add Damage type for resistences maybe as a stretch // , string DMGtype
    void DamageHealth(float DMG)
    {
        Debug.Log("Player health damaged for " + DMG + " Damage");
        Health -= DMG;
        if ((GameManagerScript.ParticleIntensity == 3)&&(DMG < 10 )&&( DMG > 0))
        {
            Instantiate(LightlyDamaged, transform.position, transform.rotation);
        }
        if (DMG >= 10)
        {
            TimeManager.SlowMo();
            iFrames = iAmount;
            gameObject.layer = 12;
            if (GameManagerScript.ParticleIntensity > 1)
            {
                Instantiate(DamagedParticle, transform.position, transform.rotation);
            }
            CameraShake shaker = MainCam.GetComponent<CameraShake>();

            if (GameManagerScript.Screenshake)
            {
                shaker.Shake(.2f,3,15);
            }

        }
        if (Health <= 0)
        {
            Die();
        }
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        HealthBar.value = CalculateHealth();
        Debug.Log("Player health is now " + Health);
    }

    void DamageEnergy(float DMG)
    {
        Energy -= DMG;
        if (Energy > MaxEnergy)
        {
            Energy = MaxEnergy;
        }
    }

    float CalculateHealth()
    {
        return Health / MaxHealth;
    }

    float CalculateEnergy()
    {
        return Energy / MaxEnergy;
    }

    void ActivateDialogue()
    {
        DialogueTarget.SendMessage("SendDialogue");
    }

    void Die ()
    {
        //TODO: Leave stats in the GM for revival purposes.
        Instantiate(DeathParticle, transform.position, transform.rotation);
        //Destroy(gameObject);

        isDead = true;
        enabled = false;

        reviveCollider.enabled = true;
	}

}
