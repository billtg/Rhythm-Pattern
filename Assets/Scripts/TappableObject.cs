using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TappableObject : MonoBehaviour {

    public int testIndex = 0;
    public List<Tap> taps;
    public List<SequencedList> sequencedTaps;

    private void Awake()
    {
        sequencedTaps = new List<SequencedList>();
        //Populate sequencedTriggers
        PopulateSequencedTriggers();
    }

    private void PopulateSequencedTriggers()
    {
        //Debug.Log("Populating SequencedTriggers");
        //Make a list where the index is the sequence
        for (int i = 0; i < 10; i++)
        {
            //Debug.Log("sequenced List: " + i.ToString());
            //Initialize the list
            sequencedTaps.Add(new SequencedList());
            sequencedTaps[i].tapList = new List<Tap>();
            if (taps.Count > 0)
            {
                for (int j = 0; j < taps.Count; j++)
                {
                    //Check if the sequence coincides with this list. If so, add it.
                    if (taps[j].sequence == i)
                    {
                        //Debug.Log("Adding sequenced tap from sequence " + i.ToString() + " tap index " + j.ToString());
                        sequencedTaps[i].tapList.Add(taps[j]);
                    }
                }
            }
        }
    }

    private void Update()
    {
        //Check for missed beat

        //Check if you're in the current beat threshold for pulsing

        //Check if you're in the click threshold for clicking
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked on " + this.gameObject.name);
        TapTriggered(sequencedTaps[Conductor.instance.currentSequence].tapList[testIndex-1].triggers);
        //animator.SetInteger(indexHash, testIndex);
        AdvanceIndex();
    }
    
    private void AdvanceIndex()
    {
        testIndex++;
        if (testIndex > 4)
        {
            testIndex = 0;
        }
    }

    private void TapTriggered (List<TriggerableObject.Trigger> triggers)
    {
        //For each triggerObject included in that tap, tell the TriggerableObject to trigger with the attached index
        for (int i=0; i< triggers.Count; i++)
        {
            triggers[i].targetObject.ActivateTrigger(triggers[i].triggerIndex);
        }
    }


    [System.Serializable]
    public class Tap
    {
        public int sequence;
        public float beat;
        public List<TriggerableObject.Trigger> triggers;
    }

    [System.Serializable]
    public class SequencedList
    {
        public List<Tap> tapList;
    }

}
