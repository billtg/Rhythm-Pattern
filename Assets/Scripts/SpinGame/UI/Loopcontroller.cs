using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loopcontroller : MonoBehaviour
{
    public float volumeIncrement;
    public bool loopsPlaying = false;

    public AudioSource[] danceLoops;
    public bool[] danceActive;

    public AudioSource[] hiphopLoops;
    public bool[] hiphopActive;

    public static Loopcontroller instance;

    private void Awake()
    {
        instance = this;
        danceActive = new bool[danceLoops.Length];
        hiphopActive = new bool[hiphopLoops.Length];
        //Make sure every track is set to inactive
        for (int i =0; i < danceActive.Length; i++)
        {
            danceActive[i] = false;
        }

        for (int i = 0; i < hiphopActive.Length; i++)
        {
            hiphopActive[i] = false;
        }
        PlayLoops();
        loopsPlaying = true;
    }

    private void OnEnable()
    {
        //Debug.Log("LoopController enabled");
        if (AudioListener.pause == true)
        {
            //Debug.Log("Unpausing audiolistener");
            AudioListener.pause = false;
        }
        PlayLoops();
    }

    private void PlayLoops()
    {
        //Debug.Log("Playing Loops");
        for (int i = 0; i < danceLoops.Length; i++)
        {
            danceLoops[i].Play();
        }
        for (int i = 0; i <hiphopLoops.Length; i++)
        {
            hiphopLoops[i].Play();
        }
    }

    private void Update()
    {
        //Turn up the active track volumes
        for (int i = 0; i < danceLoops.Length; i++)
        {
            if (danceActive[i])
            {
                //Turn the volume up more
                if (danceLoops[i].volume < 1.0)
                {
                    danceLoops[i].volume += volumeIncrement;
                }
            }
            if (hiphopActive[i])
            {
                if (hiphopLoops[i].volume < 1.0)
                {
                    hiphopLoops[i].volume += volumeIncrement;
                }
            }
        }

        //Turn down the inactive track volumes
        for (int i = 0; i < danceLoops.Length; i++)
        {
            if (!danceActive[i])
            {
                //Turn the volume down
                if (danceLoops[i].volume > 0)
                {
                    danceLoops[i].volume -= volumeIncrement;
                }
            }

            if (!hiphopActive[i])
            {
                if (hiphopLoops[i].volume > 0)
                {
                    hiphopLoops[i].volume -= volumeIncrement;
                }
            }
        }
    }

    public void TrackActive(SongType songType, int songNumber, bool active)
    {
        switch(songType)
        {
            case SongType.Dance:
                danceActive[songNumber - 1] = active;
                break;
            case SongType.Hiphop:
                hiphopActive[songNumber - 1] = active;
                break;
        }
    }

    public void SongStop(SongType songType, int songNumber)
    {
        switch (songType)
        {
            case SongType.Dance:
                danceLoops[songNumber - 1].volume = 0;
                break;
            case SongType.Hiphop:
                hiphopLoops[songNumber - 1].volume = 0;
                break;
        }
    }
}
