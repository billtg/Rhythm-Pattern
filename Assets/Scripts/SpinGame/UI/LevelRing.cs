using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRing : MonoBehaviour
{
    public SongType songtype;
    private float hiphopScale = 1.75f;

    private void Awake()
    {
        int score;
        switch (songtype)
        {
            case SongType.Dance:
                score = PlayerPrefs.GetInt("Dance", 0);
                if (score >= 4)
                    this.transform.localScale = Vector3.one;
                break;
            case SongType.Hiphop:
                score = PlayerPrefs.GetInt("Hiphop", 0);
                if (score >= 4)
                    this.transform.localScale = new Vector3(hiphopScale,hiphopScale,1);
                break;
        }
    }
}
