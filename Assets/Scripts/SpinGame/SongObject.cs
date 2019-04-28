using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Song Object")]
public class SongObject : ScriptableObject
{
    //This holds everything about a song.
    //It gets loaded into the main scene when a song is selected from the main menu
    [Header("Conductor info")]
    public float bpm;
    public AudioClip metronomeClip;
    public float timeSig;
    public float beatThreshold = 0.45f;

    [Header("Sequence Info")]
    public int simultaneousRings;

    [Header("Rings")]
    public RingInfo[] rings;

    [System.Serializable]
    public class RingInfo
    {
        public int notesToTelegraph;
        public KeyCode ringKeycode;
        public AudioClip ringTrack;
        public AudioClip ringSample;
        public bool useMelody;
        public List<float> beats;
        public List<AudioClip> melodyNotes;
    }

}
