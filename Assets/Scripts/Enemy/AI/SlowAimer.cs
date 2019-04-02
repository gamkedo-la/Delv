using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowAimer : MonoBehaviour
{


    public Transform Target;
    public float RotationSpeed = 5;
    private Rigidbody2D rb;
    public bool alert;
    public GameObject Projectile;
    [SerializeField]
    private GameObject ProjectileManager;
    private Transform ProjectileCollector;
    public Vector3 offset;
    [Space]
    public float TimeBetweenShots = 1;
    public float TimeBetweenBursts = 3;
    public float staggerTimer = 3;

    private bool staggered = false;
    private Coroutine fireSequence;


    // Use this for initialization
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        fireSequence = StartCoroutine(FireSequence());

        ProjectileManager = GameObject.Find("ProjectileManager");
        if (ProjectileManager == null)
        {
            Debug.Log("Projectile Collector is not present - spawning shots in heirarchy");
            ProjectileCollector = null;
        } else {
            Debug.Log("Projectile Collector present");
            ProjectileCollector = ProjectileManager.transform;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((Target) && (alert))
        {
            Vector2 direction = (Vector2)Target.position - rb.position;
            direction.Normalize();
            float RotateAmount = Vector3.Cross(direction, transform.right).z;
            rb.angularVelocity = RotateAmount * RotationSpeed;
        }
    }

    private void Update()
    {
        if (!Target && !staggered)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public void EnableStagger()
    {
        staggered = true;
        StartCoroutine(StaggerCountdown());
        StopCoroutine(fireSequence);
        Debug.LogWarning("Stagger enabled for " + staggerTimer + "!");
    }

    IEnumerator StaggerCountdown()
    {
        Target = null;
        yield return new WaitForSeconds(staggerTimer);
        staggered = false;
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.LogWarning("Stagger done restarting firing");
        fireSequence = StartCoroutine(FireSequence());
    }

    IEnumerator FireSequence()
    {
        GameObject TempGO = Instantiate(Projectile, transform.position + offset, transform.rotation);
        TempGO.transform.SetParent(ProjectileCollector);
        yield return new WaitForSeconds(TimeBetweenShots);
        TempGO = Instantiate(Projectile, transform.position + offset, transform.rotation);
        TempGO.transform.SetParent(ProjectileCollector);
        yield return new WaitForSeconds(TimeBetweenShots);
        TempGO = Instantiate(Projectile, transform.position + offset, transform.rotation);
        TempGO.transform.SetParent(ProjectileCollector);
        yield return new WaitForSeconds(TimeBetweenBursts);
        fireSequence = StartCoroutine(FireSequence());
    }
}
