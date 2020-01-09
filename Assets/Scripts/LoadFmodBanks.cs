using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadFmodBanks : MonoBehaviour
{
    bool didLoad = false;
    private void Start()
    {
        FMODUnity.RuntimeManager.LoadBank("Master Bank");
        FMODUnity.RuntimeManager.LoadBank("Master Bank.strings");
        StartCoroutine(TimeOutGo());
    }

    void Update()
    {
        if (didLoad == false && FMODUnity.RuntimeManager.HasBankLoaded("Master Bank")
                && FMODUnity.RuntimeManager.HasBankLoaded("Master Bank.strings"))
        {
            didLoad = true;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    IEnumerator TimeOutGo()
    {
        yield return new WaitForSeconds(15.0f);
        if (didLoad == false)
        {
            didLoad = true;
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
