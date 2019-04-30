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
}
