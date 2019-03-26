using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadFmodBanks : MonoBehaviour
{
    [FMODUnity.BankRef]
    public List<string> banks;
    bool HasBankLoaded = false;

    private void Awake()
    {
        foreach (string b in banks)
        {
            FMODUnity.RuntimeManager.LoadBank(b, true);
            Debug.Log("Loaded bank " + b);
        }
    }

    void Update()
    {
        if(!HasBankLoaded)
        {
            if (FMODUnity.RuntimeManager.HasBankLoaded("Master Bank"))
            {
                Debug.Log("Master Bank Loaded");
                HasBankLoaded = true;
            }
        }
    }
}
