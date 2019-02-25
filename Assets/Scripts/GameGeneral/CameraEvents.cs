using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets._2D;
using UnityEngine;

[System.Serializable]
public class Event
{
    public Transform target;
    public int timer; 
    public bool skippable;
}

public class CameraEvents : MonoBehaviour
{
    public Event[] Events;
    public int _totalEvents;
    public int _eventsLeft;
    public int _currentEvent;
    [Space]
    public Transform currentTarget;
    public int currentTimer;
    public bool currentSkippable;
    [Space]
    public bool activated;
    [Space]
    public GameObject CamParent;
    public Camera2DFollow C2D;

    private void Start()
    {
        _currentEvent = -1;
        _totalEvents = Events.Length;
        CamParent = GameObject.Find("CamParent");
        C2D = CamParent.GetComponent<Camera2DFollow>();
    }


    void StartNextEvent()
    {
        Debug.Log("StartNextEvent Fired");
        activated = true;
        _currentEvent++;

        if (_currentEvent > _totalEvents)
        {
            CutsceneComplete();
            return;
        }
        
        StartCoroutine(RunEvent());
        
    }

    void CutsceneComplete()
    {
        Debug.Log("Cutscene Complete");
        C2D.SendMessage("Unlock");

    }

    IEnumerator RunEvent()
    {
        Debug.Log("Event Running...");
        currentTimer = Events[_currentEvent].timer;
        currentTarget = Events[_currentEvent].target;
        C2D.SendMessage("Cutscene", currentTarget);


        //Add animator activator here later
        yield return new WaitForSeconds(currentTimer);
        Debug.Log("EventComplete");
        StartNextEvent();
    }

    void Activate()
    {
        if (!activated)
        {
            StartNextEvent();

        }
        //Set up skippable here when you get a chance.
    }


}
