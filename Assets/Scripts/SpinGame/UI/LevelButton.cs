using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public SongType songType;
    [Range(1,4)]
    public int songNumber;
    public bool clickable = false;
    public GameObject lockSprite;
    public GameObject fillDot;

    private void Awake()
    {
        DetermineLevelStatus();
    }

    public void DetermineLevelStatus()
    {
        //Figure out if this level is open, closed, or beaten
        switch (songType)
        {
            case SongType.Dance:
                int beatenSongsDance = PlayerPrefs.GetInt("Dance", 0);
                CheckLevel(beatenSongsDance);
                break;
            case SongType.Hiphop:
                int beatenDanceCheck = PlayerPrefs.GetInt("Dance", 0);
                if (beatenDanceCheck < 2) return;
                int beatenSongsHiphop = PlayerPrefs.GetInt("Hiphop", 0);
                CheckLevel(beatenSongsHiphop);
                break;
        }
    }

    private void CheckLevel(int beatenSongs)
    {
        switch(songNumber)
        {
            case 1:
                if (beatenSongs == 0)
                    ActivateCircle(false);
                else
                    ActivateCircle(true);
                break;
            case 2:
                if (beatenSongs == 1)
                    ActivateCircle(false);
                else if (beatenSongs > 1)
                    ActivateCircle(true);
                break;
            case 3:
                if (beatenSongs == 2)
                    ActivateCircle(false);
                else if (beatenSongs > 2)
                    ActivateCircle(true);
                break;
            case 4:
                if (beatenSongs == 3)
                    ActivateCircle(false);
                else if (beatenSongs > 3)
                    ActivateCircle(true);
                break;
            default:
                Debug.LogError("Incorrect Song Number entered somehow!");
                break;

        }
    }

    private void ActivateCircle(bool completed)
    {
        clickable = true;
        //disable the lock sprite on top
        lockSprite.SetActive(false);
        if (completed)
            fillDot.SetActive(true);
    }


    private void OnMouseDown()
    {
        if (!clickable) return;

        //Stop the music
        clickable = false;
        Loopcontroller.instance.SongStop(songType, songNumber);
        //Load the song up into the main scenes
        if (songNumber == 1 && songType == SongType.Dance)
        {
            //Check if it's the first time. If so, play tutorial
            int firstTime = PlayerPrefs.GetInt("FirstTime", 0);
            if (firstTime == 0)
            {
                Debug.Log("Loading Tutorial");
                SongLoader.instance.LoadScene(5, SongType.Hiphop);
                SceneManager.LoadScene(2);
            } else
            {
                Debug.Log("Loading Main Scene");
                SongLoader.instance.LoadScene(songNumber, songType);
                SceneManager.LoadScene(1);
            }
        } else
        {
            Debug.Log("Loading Main Scene");
            SongLoader.instance.LoadScene(songNumber, songType);
            SceneManager.LoadScene(1);
        }
    }

    private void OnMouseOver()
    {
        if (!clickable) return;
        //Debug.Log("Hovering");
        //Set the volume up on this track
        Loopcontroller.instance.TrackActive(songType, songNumber, true);
    }

    private void OnMouseExit()
    {
        if (!clickable) return;
        //Set the volume back down
        Loopcontroller.instance.TrackActive(songType, songNumber, false);
    }
}

public enum SongType { Dance, Hiphop }
