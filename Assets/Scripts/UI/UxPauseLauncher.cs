using UnityEngine;

public class UxPauseLauncher : UxPanel 
{
    [Header("Prefabs")]
    public GameObject pausePrefab;

    // Update is called once per frame
    void Update()
    {
        // if we are not hidden (e.g.: paused) and pause key is pressed, launch pause menu
        if (!hidden && Input.GetButtonDown("Pause"))
        {
            OnPause();
        }
    }

    public void OnPause() {
        // instantiate the pause panel prefab
        var panelGo = Instantiate(pausePrefab, GetComponentInParent<Canvas>().gameObject.transform);
        // setup a callback, so when the sub menu/panel is done, we display the current panel again
        var uxPanel = panelGo.GetComponent<UxPanel>();
        uxPanel.onDoneEvent.AddListener(Display);
        // now hide the current panel
        Hide();
    }

}