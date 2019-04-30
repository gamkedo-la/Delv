using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadFmodBanks : MonoBehaviour
{
    [FMODUnity.BankRef]
    public List<string> banks;
    bool HasBankLoaded = false;

    public GameObject loadingScreen;
    public GameObject buttonStart;
    public GameObject loadingText;
    public string sceneName = "MainMenu";
    public Slider slider;
    public Text progressText;


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

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public IEnumerator LoadAsynchronously(int sceneIndex)
    {
        loadingScreen.SetActive(true);
        loadingText.SetActive(true);
        buttonStart.SetActive(false);

        yield return new WaitForSeconds(2);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressText.text = progress * 100f + "%";
            yield return null;
        }
    }
}
