using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Conductor : MonoBehaviour {

    //Static song information
    public float beatTempo;
    public float secPerBeat;
    public float offestInBeats = 4;
    private float offsetToFirstBeat;
    public float startingPosition;
    public AudioSource musicSource;
    public AudioSource[] loops;
    public AudioListener audioListener;

    //Dynamic song information
    public float songPosition;
    public float songPosInBeats;
    public float dspSongTime;
    public float loopPosInBeats;
    public float loopPosInAnalog;
    public int completedLoops;
    public bool musicStarted = false;
    public int currentSequence;
    public int totalSequences = 10;

    //Pause information
    public static bool paused = false;
    public static float pauseTimeStamp = -1f;
    public static float pausedTime = 0;
    public GameObject PauseCanvas;

    //Tappable object list
    private List<TappableObject> tappableObjects;

    //UI
    public Text loopText;
    public TextMeshPro announcementText;
    public Animator announcementAnimator;

    //static note information
    public float timeSig;
    public float beatThreshold;

    //Instance
    public static Conductor instance;
    public bool isTutorial = false;

    private void Awake()
    {
        instance = this;

        //Load song parameters from SongLoader
        LoadParameters();
    }

    private void LoadParameters()
    {
        //Debug.Log("Loading conductor parameters");
        beatTempo = SongLoader.instance.activeSong.bpm;
        musicSource.clip = SongLoader.instance.activeSong.metronomeClip;
        timeSig = SongLoader.instance.activeSong.timeSig;
    }

    // Use this for initialization
    void Start ()
    {
        paused = false;
        pauseTimeStamp = -1f;
        //Calculate the number of seconds per beat
        secPerBeat = 60f / beatTempo;
        //Figure out 1 measure lead in
        //Set the song to n-1 measures into the loop
        offsetToFirstBeat = offestInBeats * secPerBeat;
        startingPosition = timeSig * secPerBeat - offsetToFirstBeat;
        songPosition = startingPosition;
        songPosInBeats = songPosition / secPerBeat;
        loopPosInBeats = songPosInBeats + 1;
        loopPosInAnalog = (loopPosInBeats - 1) / timeSig;
        //Run the countdown preparing to start
        StartCoroutine(CountDown());
    }

	// Update is called once per frame
	void Update () {

        //Only do things if the music has started
        if (!musicStarted) return;

        if (paused)
        {
            if (pauseTimeStamp < 0f) //not managed
            {
                pauseTimeStamp = (float)AudioSettings.dspTime;
                AudioListener.pause = true;
                //Activate some UI here
                PauseCanvas.SetActive(true);
            }

            return;
        }
        else if (pauseTimeStamp > 0f) //resume not managed
        {
            AudioListener.pause = false;
            pauseTimeStamp = -1f;
        }

        //calculate the position of the song in seconds from dsp space
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - pausedTime);

        //calculate the position in beats
        songPosInBeats = songPosition / secPerBeat;

        //calculate loop position in beats
        if (songPosInBeats >= (completedLoops + 1) * timeSig)
            completedLoops++;
        loopPosInBeats = songPosInBeats - completedLoops * timeSig + 1;
        loopText.text = loopPosInBeats.ToString("#.0");
        loopPosInAnalog = (loopPosInBeats-1) / timeSig;

    }

    public void Resume()
    {
        PauseCanvas.SetActive(false);
        paused = false;
    }

    void StartMusic()
    {
        //Debug.Log("Starting Music");
        //Record the time when the audio starts
        dspSongTime = (float)AudioSettings.dspTime - startingPosition;

        //start the song
        musicSource.Play();
        //Start the track loops
        for (int i=0; i<loops.Length; i++)
        {
            loops[i].volume = 0;
            //loops[i].Play();
            //Debug.Log("Track starting");
            loops[i].PlayScheduled(dspSongTime + startingPosition + offsetToFirstBeat);
        }

        musicStarted = true;
    }
    
    public bool InThreshold(float beat)
    {
        //Check if the beat sent falls within beatThreshold
        //Written to handle the looping
        if (beat <= beatThreshold + 1)
        {
            //Debug.Log("Case 1, beat is close to 1");
            if (loopPosInBeats > beat + beatThreshold)
            {
                if (loopPosInBeats >= (beat + timeSig - 1) + beatThreshold)
                {
                    //Debug.Log("LoopPos just below loop point");
                    return true;
                } else
                {
                    //Debug.Log("LoopPos not within beat threshold");
                }
            } else
            {
                //Debug.Log("Case 1, loopPos between loop point and beat threshold");
            }
        }
        else if (beat < (timeSig + 1 - beatThreshold))
        {
            //Debug.Log("Case 2, beat is far from loop point.");
            if (loopPosInBeats >= beat-beatThreshold && loopPosInBeats <= beat + beatThreshold)
            {
                //Debug.Log("LoopPos within threshold");
                return true;
            }
        }
        else if (beat >= (timeSig + 1 - beatThreshold))
        {
            //Debug.Log("Case 3, beat is close to loop point");
            if (loopPosInBeats < beat)
            {
                if (loopPosInBeats >= beat - beatThreshold)
                {
                    //Debug.Log("LoopPos just below beat");
                    return true;
                } else if (loopPosInBeats < (beat-timeSig + 1) - beatThreshold)
                {
                    //Debug.Log("LoopPos just above loop point");
                    return true;
                }
            } else
            {
                //Debug.Log("LoopPos just above beat");
                return true;
            }

        }
        else
        {
            Debug.LogError("Strange Case. Where is this beat? This should never happen");
        }
        return false;
    }

    IEnumerator CountDown ()
    {
        yield return new WaitForSeconds(1f);
        announcementAnimator.SetTrigger("getReady");
        for (int i=0; i<1; i++)
        {
            //Debug.Log("Countdown " + i.ToString());
            yield return new WaitForSeconds(1f);
        }
        
        StartMusic();

    }

    public void SequenceComplete()
    {
        //Change the middle button to a back button
        //Pop up a 'Complete' announcement
        announcementText.text = "Complete";
        announcementAnimator.SetTrigger("Complete");
    }

    public void StopMetronome()
    {
        musicSource.volume = 0;
    }
    
}
