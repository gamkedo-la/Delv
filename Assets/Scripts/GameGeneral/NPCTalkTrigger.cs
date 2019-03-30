using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalkTrigger : MonoBehaviour
    
{
    public DialogueManager DM;
    public Dialogue Dialogue;
    public bool IsActive;

    private void Start()
    {
        DM = FindObjectOfType<DialogueManager>();
    }
    void Activate()
    {
        if (DM == null)
        {
            DM = FindObjectOfType<DialogueManager>();

        }
        if (IsActive)
        {
            Debug.Log(this.name + " has received Activate command to go to the next sentence");
            DM.DisplayNextSentence();
        }
        if (!IsActive)
        {
            Debug.Log(this.name + " has received Activate command to start dialogue");
            DM.StartDialogue(Dialogue);
            IsActive = true;
        }
    }
    void Deactivate()
    {
        IsActive = false;
        DM.EndDialogue();
    }
}
