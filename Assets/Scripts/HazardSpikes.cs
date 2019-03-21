using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardSpikes : MonoBehaviour
{
    [SerializeField] GameObject spikesDown;
    [SerializeField] GameObject spikesUp;
    [SerializeField] float triggerDelayInSeconds = 0.7f;
    [SerializeField] float durationInSeconds = 4f;
    [SerializeField] float damageDelayInSeconds = 0.5f;
    [SerializeField] float damage = 0.5f;

    bool hasTriggered = false;
    bool isUp = false;
    int triggerCounter = 0;

    float damageTick;
    float deactivateTimeout;

    void Awake()
    {
        damageTick = damageDelayInSeconds;
        deactivateTimeout = durationInSeconds;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") {
            return;
        }

        triggerCounter++;
        deactivateTimeout = durationInSeconds;

        if (!hasTriggered) {
            StartCoroutine("ActivateSpikes");
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") {
            return;
        }

        if (isUp && damageTick <= 0) {
            other.gameObject.SendMessage("DamageHealth", damage);
        }
    }

    void FixedUpdate()
    {
        if (!isUp) {
            return;
        }

        if (triggerCounter <= 0) {
            deactivateTimeout -= Time.fixedDeltaTime;

            if (deactivateTimeout <= 0) {
                DeactivateSpikes();
            }
        }

        if (0 < triggerCounter) {
            if (damageTick <= 0) {
                damageTick += damageDelayInSeconds;
            }
            damageTick -= Time.fixedDeltaTime;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player") {
            return;
        }

        triggerCounter--;

        if (triggerCounter <= 0) {
            triggerCounter = 0;
        }
    }

    IEnumerator ActivateSpikes()
    {
        hasTriggered = true;
        spikesDown.SetActive(true);

        yield return new WaitForSeconds(triggerDelayInSeconds);

        isUp = true;
        spikesUp.SetActive(true);
        spikesDown.SetActive(false);
    }

    void DeactivateSpikes()
    {
        isUp = false;
        hasTriggered = false;

        spikesUp.SetActive(false);
        spikesDown.SetActive(true);

        StartCoroutine("HideSpikeDown");
    }

    IEnumerator HideSpikeDown()
    {
        yield return new WaitForSeconds(triggerDelayInSeconds);

        if (!hasTriggered) {
            spikesDown.SetActive(false);
        }
    }
}
