using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceController : MonoBehaviour {

    [Header("Physical Arrangement")]
    //Right now this has to be 2 because of hard coded animation properties. I probably won't change it
    public float mainDistance = 2;
    public float ringSpacing;
    public List<int> activeRings = new List<int>();

    [Header("Ring Properties")]
    public int simultaneousRings;
    public int activeRing = 0;
    public RingParent[] ringParents;


    private void Awake()
    {
        ringParents = GetComponentsInChildren<RingParent>();
    }

    private void Start()
    {

        //Populate Rings
        for (int i = 0; i < ringParents.Length; i++)
        {
            Debug.Log("Populating Ring " + i.ToString());
            //Set the distance according to how many rings there are at one time
            float ringDistance = WhatDistance(i);
            ringParents[i].PopulateRing(ringDistance);
        }

        Debug.Log("Activating intitial rings in sequence");
        for (int i = 0; i < simultaneousRings; i++)
        {
            //Activate each ring with its ringSpot (0 closest, then 1...)
            ringParents[i].ActivateRing(i);
            activeRings.Add(i);
        }
    }

    public void MissedNote()
    {
        //Tell each active ring to reset its notes
        foreach(int index in activeRings)
        {
            ringParents[index].ResetNotes();
        }
    }

    public void CompleteRing()
    {
        Debug.Log("checking for full completion");
        bool ringDone = true;
        foreach (int index in activeRings)
        {
            if (!ringParents[index].isDone)
                ringDone = false;
        }
        if (ringDone)
        {
            Debug.Log("Ring set complete.");
            //Set the volume up on each ring's track and trigger its complete animations
            foreach (int index in activeRings)
            {
                Conductor.instance.loops[index].volume = 1;
                ringParents[index].CompleteRing();
            }

            //Activate the next set of rings
            activeRing += simultaneousRings;

            //Check if the entire sequence is complete, otherwise activate the next rings
            if (activeRing >= ringParents.Length)
                SequenceComplete();
            else
            {
                //Reset activeRings
                activeRings.Clear();

                for (int i = activeRing; i < activeRing + simultaneousRings; i++)
                {
                    //Activate each ring with its ringSpot (0 closest, then 1...)
                    ringParents[i].ActivateRing(i);
                    activeRings.Add(i);
                }
            }
        } else
        {
            Debug.Log("Ring set not complete. waiting to completion");
        }
    }

    public float WhatDistance (int index)
    {
        switch(simultaneousRings)
        {
            case 1:
                Debug.Log("one ring at a time. Easy");
                return mainDistance;
            case 2:
                Debug.Log("Two rings at one time.");
                if (index == 0 || index == 2 || index == 4 || index == 6)
                {
                    return mainDistance;
                } else if (index == 1 || index == 3 || index == 5 || index == 7)
                {
                    return mainDistance + ringSpacing;
                } else
                {
                    Debug.LogError("Error, erroneous ring index: " + index.ToString());
                    return mainDistance;
                }
            case 4:
                Debug.Log("Four rings at one time");
                if (index == 0 || index == 4)
                {
                    return mainDistance;
                }
                else if (index == 1 || index == 5)
                {
                    return mainDistance + ringSpacing;
                }
                else if (index == 2 || index == 6)
                {
                    return mainDistance + 2 * ringSpacing;
                }
                else if (index == 3 || index == 7)
                {
                    return mainDistance + 3 * ringSpacing;
                }
                else
                {
                    Debug.LogError("Error, erroneous ring index: " + index.ToString());
                    return mainDistance;
                }
            default:
                Debug.LogError("Incorrect simultaneous Rings value on sequence controller. Must be 1, 2, or 4 unless you program it differently.");
                return mainDistance;
        }
    }

    public void SequenceComplete()
    {
        Debug.Log("Sequence Complete!");
    }
}
