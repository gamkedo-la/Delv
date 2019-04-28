using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWormBossSong : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayOneShotWormBossSong()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Music/Worm Boss Song");
    }
}
