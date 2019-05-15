using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SongLoader : MonoBehaviour
{
    public int songIndex;
    public SongObject[] songObjects;
    public SongType currentSongType;
    public SongObject activeSong;

    public List<float> test = new List<float>(4);

    public static SongLoader instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Debug.Log("Double songloader instance");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(int songIndexUncategorized, SongType songType)
    {
        Debug.Log("Scene loaded with index: " + songIndexUncategorized.ToString());
        switch (songType)
        {
            case SongType.Dance:
                //Debug.Log("Setting Dance index");
                songIndex = songIndexUncategorized - 1;
                break;
            case SongType.Hiphop:
                songIndex = songIndexUncategorized + 3;
                break;
        }
        //Debug.Log("Does this ever get here?");
        activeSong = songObjects[songIndex];
        currentSongType = songType;
    }

    public bool NextSong()
    {
        //Checks if the next song is the tutorial (last index). If so, returns false, otherwise, puts the next song in memory and returns true
        Debug.Log("Loading Next song");
        if (songIndex == 7)
        {
            Debug.Log("Last Song. Go to Main Menu");
            return false;
        }
        else
        {
            songIndex++;
            activeSong = songObjects[songIndex];
            if (songIndex == 4) currentSongType = SongType.Hiphop;
            return true;
        }
    }
}
