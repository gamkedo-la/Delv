using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMOD_Enemy_Sounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void PlaySound(string path)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, GetComponent<Transform>().position);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
