using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [SerializeField] Transform rock;
    [SerializeField] Transform shadow;
    [SerializeField] float shadowSizePercentage = 0.6f;
    [SerializeField] float fallSpeed = 5f;

    Vector3 shadowScale;

    float maxDistance;
    public bool fallen = false;
    Vector3 orgPos;

    void Start()
    {
        shadowScale = shadow.localScale;
        shadow.localScale = shadowScale * shadowSizePercentage;

        maxDistance = Vector3.Distance(rock.position, shadow.position);
        orgPos = rock.position;
    }

    void Update()
    {
        if (fallen) {
            return;
        }

        float distance = Vector3.Distance(rock.position, shadow.position);
        shadow.localScale = shadowScale * Mathf.Lerp(shadowSizePercentage, 1.0f, (maxDistance - distance) / maxDistance);

        rock.position = Vector3.MoveTowards(rock.position, shadow.position, fallSpeed * Time.deltaTime);

        fallSpeed *= 1.01f;

        fallen = (distance < 0.001f);
        if (fallen) {
            // @todo do damage
            Destroy(gameObject, 2f);
        }
    }
}
