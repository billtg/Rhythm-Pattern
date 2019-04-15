using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingParent : MonoBehaviour {

    public CircleScript[] circleScripts;
    private Animator animator;
    public Animator expandRing;
    public SequenceController sequenceController;
    public int currentRing;
    public int notesToTelegraph;
    public bool isDone = false;

    [Header("Circle Information")]
    public List<float> beats;
    public KeyCode ringKeycode;
    public AudioClip ringAudio;
    public GameObject circlePrefab;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sequenceController = GetComponentInParent<SequenceController>();
        circleScripts = new CircleScript[beats.Count];
    }

    public void PopulateRing(float ringDistance)
    {
        for (int i = 0; i <beats.Count; i++)
        {
            //Instantiate the circle
            GameObject newCircle = Instantiate(circlePrefab);
            CircleScript circleScript = newCircle.GetComponent<CircleScript>();

            //Initialize the Circle
            circleScript.Initialize(ringKeycode, beats[i], ringAudio);
            newCircle.transform.SetParent(this.transform);
            circleScript.ringParent = this;

            //locate the Circle based on it's beat
            Debug.Log("Locating Circle " + i.ToString());
            circleScript.LocateCircle(ringDistance);

            circleScripts[i] = circleScript;
        }
    }

    public void ActivateRing(int ringSpot)
    {
        Debug.Log("Activating Big Ring");
        ////Expand ring
        //for (int i = 0; i < circleScripts.Length; i++)
        //{
        //    circleScripts[i].gameObject.SetActive(true);
        //}
        animator.SetTrigger("Activate");
        //Activate first circles
        for (int i = 0; i < notesToTelegraph; i++)
        {
            circleScripts[i].ActivateRing();
        }
        circleScripts[0].EnableCircle();
        currentRing = notesToTelegraph;
    }

    public void CheckForCompleteRing()
    {
        //Debug.Log("Checking for completion");
        isDone = true;
        for (int i=0; i<circleScripts.Length; i++)
        {
            //Debug.Log(i.ToString());
            if (!circleScripts[i].didHit)
                isDone = false;
        }
        if (isDone)
        {
            Debug.Log("Ring Complete");
            //Check with sequence controller if every ring is done. It will tell the ring whether to do anything or not.
            sequenceController.CompleteRing();

        } else
        {
            //Debug.Log("Ring incomplete. Activating next circle");
            if (currentRing < circleScripts.Length)
            {
                circleScripts[currentRing].ActivateRing();
            }
            currentRing++;
            circleScripts[currentRing - notesToTelegraph].EnableCircle();
        }

    }

    public void CompleteRing()
    {
        Debug.Log("Completing Ring");
        //Expand the ring
        expandRing.SetTrigger("Expand");
        //Reduce the dots in
        animator.SetInteger("Reduction", sequenceController.activeRing);
    }


    public void MissedNote()
    {
        Debug.Log("MissedNote Activated");
        //Tell sequence controller to reset the other activeRings
        sequenceController.MissedNote();
    }

    public void ResetNotes()
    {
        //Deactivate every active circle after the telegraphThreshold
        for (int i = notesToTelegraph; i < circleScripts.Length; i++)
        {
            if (circleScripts[i].active)
                circleScripts[i].DeactivateCircle();
        }

        //Reset the initial telegraphed circles
        for (int i = 0; i < notesToTelegraph; i++)
        {
            circleScripts[i].ResetCircle();
        }

        //enable the first circles
        circleScripts[0].EnableCircle();
        currentRing = notesToTelegraph;
        isDone = false;
    }
}
