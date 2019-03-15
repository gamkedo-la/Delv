using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhasesManager : MonoBehaviour
{
    [SerializeField] GameObject[] phases;

    int currentPhaseIndex = 0;

    void NextPhase()
    {
        Debug.Log("Next phase!");
        if (phases.Length <= currentPhaseIndex) {
            Debug.Log("No more phases after phase index: " + currentPhaseIndex);

            return;
        }

        phases[currentPhaseIndex].SetActive(false);
        currentPhaseIndex++;
        phases[currentPhaseIndex].SetActive(true);
    }

    void FinishFinalPhase()
    {
        Debug.Log("Remove finished boss");
        Destroy(gameObject);
    }
}
