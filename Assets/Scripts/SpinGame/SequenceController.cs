using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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
    public GameObject ringPrefab;
    public AudioMixerGroup[] ringMixers;

    [Header("Line and UI")]
    public Animator lineAnimator;


    private void Awake()
    {
        LoadParameters();
        ringParents = GetComponentsInChildren<RingParent>();
    }

    private void LoadParameters()
    {
        simultaneousRings = SongLoader.instance.activeSong.simultaneousRings;

        //Instantiate the rings
        for (int i = 0; i < SongLoader.instance.activeSong.rings.Length; i++)
        {
            //Debug.Log("Instantiating Ring " + i.ToString());
            GameObject newRing = Instantiate(ringPrefab);
            RingParent ringParent = newRing.GetComponent<RingParent>();
            newRing.transform.SetParent(this.gameObject.transform);

            ringParent.notesToTelegraph = SongLoader.instance.activeSong.rings[i].notesToTelegraph;
            ringParent.ringKeycode = SongLoader.instance.activeSong.rings[i].ringKeycode;
            ringParent.ringTrack = SongLoader.instance.activeSong.rings[i].ringTrack;
            ringParent.ringAudio = SongLoader.instance.activeSong.rings[i].ringSample;
            ringParent.isMelody = SongLoader.instance.activeSong.rings[i].useMelody;
            ringParent.beats = new List<float>();
            for (int j = 0; j < SongLoader.instance.activeSong.rings[i].beats.Count; j++)
            {
                //Debug.Log("Adding beat " + j.ToString() + " to ring " + i.ToString());
                ringParent.beats.Add(SongLoader.instance.activeSong.rings[i].beats[j]);
            }

            ringParent.melodyNotes = new List<AudioClip>();
            for (int j = 0; j < SongLoader.instance.activeSong.rings[i].melodyNotes.Count; j++)
            {
                ringParent.melodyNotes.Add(SongLoader.instance.activeSong.rings[i].melodyNotes[j]);
            }
        }
    }

    private void Start()
    {
        PopulateTracks();
        PopulateRings();
        lineAnimator.SetTrigger("appear");
    }

    private void PopulateTracks()
    {
        for (int i = 0; i < ringParents.Length; i++)
        {
            //Debug.Log("Populating Track " + i.ToString());
            Conductor.instance.loops[i].clip = ringParents[i].ringTrack;
            Conductor.instance.loops[i].outputAudioMixerGroup = ringMixers[i];
        }
    }

    public void PopulateRings()
    {
        //Populate Rings
        for (int i = 0; i < ringParents.Length; i++)
        {
            //Debug.Log("Populating Ring " + i.ToString());
            //Set the distance according to how many rings there are at one time
            float ringDistance = WhatDistance(i);
            ringParents[i].audioMixerGroup = ringMixers[i];
            ringParents[i].PopulateRing(ringDistance);
        }

        //Debug.Log("Activating intitial rings in sequence");
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
            ringParents[index].ResetCircles();
        }
    }

    public void CompleteRing()
    {
        //Debug.Log("checking for full completion");
        bool ringDone = true;
        foreach (int index in activeRings)
        {
            if (!ringParents[index].isDone)
                ringDone = false;
        }
        if (ringDone)
        {
            //Debug.Log("Ring set complete.");
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
            //Debug.Log("Ring set not complete. waiting to completion");
        }
    }

    public float WhatDistance (int index)
    {
        switch(simultaneousRings)
        {
            case 1:
                //Debug.Log("one ring at a time. Easy");
                return mainDistance;
            case 2:
                //Debug.Log("Two rings at one time.");
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
            case 3:
                if (index == 0 || index == 3 || index == 6)
                {
                    return mainDistance;
                } else if (index == 1 || index == 4 || index == 7)
                {
                    return mainDistance + ringSpacing;
                } else if (index == 2 || index == 5 || index == 8)
                {
                    return mainDistance + ringSpacing * 2;
                }
                else
                {
                    Debug.LogError("Error, erroneous ring index: " + index.ToString());
                    return mainDistance;
                }
            case 4:
                //Debug.Log("Four rings at one time");
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

    public void AssistActivate()
    {
        //Turn the track on for each active ring
        //Set the volume up on each ring's track
        foreach (int index in activeRings)
        {
            Conductor.instance.loops[index].volume = 1;
        }
    }

    public void AssistDeactive()
    {
        //Set the volume down on each ring's track
        foreach (int index in activeRings)
        {
            Conductor.instance.loops[index].volume = 0;
        }
    }

    public void SequenceComplete()
    {
        //Debug.Log("Sequence Complete!");
        lineAnimator.SetTrigger("complete");
        Conductor.instance.SequenceComplete();
        if (!Conductor.instance.isTutorial)
            MiddleButton.instance.SequenceComplete();
        Conductor.instance.StopMetronome();
        UpdateSongsCompleted();
    }

    private void UpdateSongsCompleted()
    {
        switch (SongLoader.instance.currentSongType)
        {
            case SongType.Dance:
                int currentDanceIndex = PlayerPrefs.GetInt("Dance",0);
                if (currentDanceIndex < SongLoader.instance.songIndex + 1)
                    PlayerPrefs.SetInt("Dance", SongLoader.instance.songIndex + 1);
                break;
            case SongType.Hiphop:
                if (Conductor.instance.isTutorial) return;
                int currentHiphopIndex = PlayerPrefs.GetInt("Hiphop", 0);
                if (currentHiphopIndex < SongLoader.instance.songIndex - 3)
                    PlayerPrefs.SetInt("Hiphop", SongLoader.instance.songIndex - 3);
                break;
        }
    }
}
