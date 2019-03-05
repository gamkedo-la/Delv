using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets._2D;
using UnityEngine;

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
    public bool repeatable;
    public bool timerbased;
    [Space]
    public GameObject CamParent;
    public Camera2DFollow C2D;

    private void Start()
    {
        _currentEvent = -1;
        _totalEvents = Events.Length;
        CamParent = GameObject.Find("CamParent");
        if (CamParent) 
        {
            C2D = CamParent.GetComponent<Camera2DFollow>();
        }
        else
        {
            StartCoroutine(retryCamHookUp());
        }
    }

    IEnumerator retryCamHookUp()
    {
        while(CamParent == null)
        {
            yield return new WaitForSeconds(0.1f);
            CamParent = GameObject.Find("CamParent");
            if (CamParent)
            {
                C2D = CamParent.GetComponent<Camera2DFollow>();
            }
        }
    }

    void StartNextEvent()
    {
        Debug.Log("StartNextEvent Fired");
        activated = true;
        _currentEvent++;

        if (_currentEvent >= _totalEvents)
        {
            CutsceneComplete();
            return;
        }
        if (_currentEvent < _totalEvents)
        {
            StartCoroutine(RunEvent());
        }

        
    }

    void CutsceneComplete()
    {
        Debug.Log("Cutscene Complete");
        C2D.SendMessage("Unlock");
        StopAllCoroutines();

    }

    IEnumerator RunEvent()
    {
        Debug.Log("Event Running...");
        currentTimer = Events[_currentEvent].timer;
        currentTarget = Events[_currentEvent].target;
        currentSkippable = Events[_currentEvent].skippable;
        C2D.SendMessage("Cutscene", currentTarget);
        if (timerbased)
        {
        yield return new WaitForSeconds(currentTimer);
            if (_currentEvent < _totalEvents)
            {
                StartNextEvent();
            }
            if (_currentEvent >= _totalEvents)
            {
                CutsceneComplete();
            }
            Debug.Log("EventComplete");
        }
    }

    void Activate()
    {
        if ((activated) && (currentSkippable))
        {
            Skip();
        }

        if (!activated)
        {
            C2D.SendMessage("SaveTarget");
            StartNextEvent();

        }
        //Set up skippable here when you get a chance.
    }
    void Deactivate()
    {
        CutsceneComplete();
        _currentEvent = -1;
        if (repeatable)
        {
            activated = false;
        }
        
    }

    void Skip()
    {
        StopAllCoroutines();
        StartNextEvent();
    }


}

[System.Serializable]
public class Event
{
    public Transform target;
    public int timer;
    public bool skippable;
}
