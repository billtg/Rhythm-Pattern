using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingParent : MonoBehaviour {
    [Header("Sequence Controller Info")]
    public SequenceController sequenceController;

    [Header("Ring information")]
    private Animator animator;
    public Animator expandRing;
    public int currentCircle;
    public int notesToTelegraph;
    public bool isDone = false;
    public AudioClip ringTrack;
    public bool isMelody = false;

    [Header("Circle Information")]
    public CircleScript[] circleScripts;
    public List<float> beats;
    public List<AudioClip> melodyNotes;
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

            //Give it a measure
            circleScript.measure = GiveMeasure(beats[i]);

            if (isMelody)
            {
                circleScript.SetClip(melodyNotes[i]);
            }
            circleScripts[i] = circleScript;
        }
    }

    private int GiveMeasure(float beat)
    {
        //Take in the beat and give it a measure between 1 and 8
        if (beat < 5)
        {
            return 1;
        } else if (beat < 9)
        {
            return 2;
        } else if (beat < 13)
        {
            return 3;
        } else if (beat < 17)
        {
            return 4;
        } else if (beat < 21)
        {
            return 5;
        } else if (beat < 25)
        {
            return 6;
        } else if (beat < 29)
        {
            return 7;
        } else if (beat < 33)
        {
            return 8;
        } else
        {
            Debug.LogError("Error, beat is not between 1 and 32. Did you make a longer game and forget to reprogram it? Or did you forget to assign a beat?");
            return 0;
        }
    }

    public void ActivateRing(int ringSpot)
    {
        Debug.Log("Activating Big Ring");
        animator.SetTrigger("Activate");

        //Activate first circles in the next measure
        ResetCircles();
        //int index = FindNextCircleAfterThisMeasure(GiveMeasure(Conductor.instance.loopPosInBeats));
        //for (int i = 0; i < notesToTelegraph; i++)
        //{
        //    circleScripts[i].ActivateCircle();
        //}
        //circleScripts[0].EnableCircle();
        //currentCircle = notesToTelegraph;
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
            //Debug.Log("Ring Complete");
            //Check with sequence controller if every ring is done. It will tell the ring whether to do anything or not.
            sequenceController.CompleteRing();

        } else
        {
            //Debug.Log("Ring incomplete. Activating next circle");
            if (!circleScripts[currentCircle].active)
                circleScripts[currentCircle].ActivateCircle();
            currentCircle++;
            //Check if the next circle is past the loop point
            if (currentCircle >= circleScripts.Length)
                currentCircle = 0;
            ////Activate the next circle if it isn't already active
            //if (!circleScripts[currentCircle].active)
            //    circleScripts[currentCircle].ActivateCircle();
            //Enable the next circle
            if (currentCircle - notesToTelegraph < 0)
                //Do something here
                circleScripts[currentCircle - notesToTelegraph + circleScripts.Length].EnableCircle();
            else
                circleScripts[currentCircle - notesToTelegraph].EnableCircle();
        }

    }

    public void CompleteRing()
    {
        //Debug.Log("Completing Ring");
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

    public void ResetCircles()
    {
        //Reset all the circles and activate the next circles after this measure
        int thisMeasure;
        if (Conductor.instance.musicStarted)
        {
            thisMeasure = GiveMeasure(Conductor.instance.loopPosInBeats);
        } else
        {
            thisMeasure = (int)Conductor.instance.timeSig / 4;
            Debug.Log("Measure used: " + thisMeasure.ToString());
        }
        //index will give the circleScripts index for the next circle after this measure
        int index = FindNextCircleAfterThisMeasure(thisMeasure);

        //Deactivate every active circle.
        for (int i = 0; i < circleScripts.Length; i++)
        {
            if (circleScripts[i].active)
                circleScripts[i].DeactivateCircle();
        }

        //activate the initial telegraphed circles
        for (int i = 0; i < notesToTelegraph; i++)
        {
            if (i + index >= circleScripts.Length)
                circleScripts[index + i - circleScripts.Length].ActivateCircle();
            else
                circleScripts[index + i].ActivateCircle();
        }

        //enable the first circle
        circleScripts[index].EnableCircle();
        currentCircle = index + notesToTelegraph;
        if (currentCircle >= circleScripts.Length)
            currentCircle = index + notesToTelegraph - circleScripts.Length;
        isDone = false;
    }

    public int FindNextCircleAfterThisMeasure(int currentMeasure)
    {
        Debug.Log("Searching for next circle");
        int nextMeasure = currentMeasure + 1;
        if (nextMeasure > Conductor.instance.timeSig / 4)
            nextMeasure = 1;
        
        //Check measure by measure until you find a note with that measure.
        //This only breaks if the next note i the start of this measure
        while (nextMeasure != currentMeasure)
        {
            //Check if any circle is in this measure
            for (int i = 0; i < circleScripts.Length; i++)
            {
                if (circleScripts[i].measure == nextMeasure)
                {
                    Debug.Log("Found next circle with index: " + i.ToString());
                    return i;
                }
            }
            //No circles in measure? bump up the nextMeasure
            Debug.Log("Diddn't find circle. Trying next measure");
            nextMeasure++;
            if (nextMeasure > Conductor.instance.timeSig / 4)
            {
                Debug.Log("Next measure is 1");
                nextMeasure = 1;
            }
        }
        //The only reason you should get here is if the next note is the start of this measure. Check the current measure
        for (int i = 0; i < circleScripts.Length; i++)
        {
            if (circleScripts[i].measure == nextMeasure)
            {
                Debug.Log("Weird case. Next note is the start of this measure");
                return i;
            }
        }
        //You should never get here. Return 0 and give an error
        Debug.LogError("Error, didn't find a circle. This should never happen. Activating the first circle instead.");
        return 0;
    }
}
